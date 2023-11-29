using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace MetaMask.Unity.Utils
{
	/// <summary>
	/// UnityBinder entry class. Use this class to setup any Unity Object that has any
	/// Binder Attributes
	/// </summary>
	public static class UnityBinder
	{
		/// <summary>
		/// Inject an Object's field that have attributes.
		/// </summary>
		/// <param name="obj">The object to inject</param>
		public static void Inject(Object obj)
		{
			var bindingFlags = BindingFlags.Instance |
			                   BindingFlags.NonPublic |
			                   BindingFlags.Public;

			var fields = obj.GetType().GetFields(bindingFlags);

			foreach (var field in fields)
			{
				var injections = (Binder[]) field.GetCustomAttributes(typeof(Binder), true);

				if (injections.Length > 0)
				{
					foreach (var inject in injections)
					{
						inject.InjectInto(obj, field);
					}
				}
			}

			var methods = obj.GetType().GetMethods(bindingFlags);
			foreach (var method in methods)
			{
				var injections = (BindOnClick[]) method.GetCustomAttributes(typeof(BindOnClick), true);

				if (injections.Length > 0)
				{
					foreach (var inject in injections)
					{
						inject.InjectInto(obj, method);
					}
				}
			}
		}

		private static GameObject DeepFind(string name)
		{
			if (name.StartsWith("/"))
			{
				string[] temp = name.Split('/');

				GameObject current = null;
				for (int i = 1; i < temp.Length; i++)
				{
					string n = temp[i];
					if (current == null)
					{
						current = GameObject.Find(n);
						if (current == null)
						{
							current = FindInActiveObjectByName(n);
						}
					}
					else
					{
						current = current.transform.Find(n).gameObject;
					}
				}

				return current;
			}

			return GameObject.Find(name);
		}

		internal static GameObject FindInActiveObjectByName(string name)
		{
			if (name.StartsWith("/"))
				return DeepFind(name);

			Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>();
			for (int i = 0; i < objs.Length; i++)
			{
				if (objs[i].hideFlags == HideFlags.None)
				{
					if (objs[i].name == name)
					{
						return objs[i].gameObject;
					}
				}
			}

			return null;
		}
	}

	/// <summary>
	/// Abstract resource to represent any kind of Bind
	/// </summary>
	public abstract class Binder : Attribute
	{
		public abstract void InjectInto(Object obj, FieldInfo field);
	}

	/// <summary>
	/// Bind a method to an OnClick event that is triggered by a Button.
	/// 
	/// By default, the Button is searched on the gameObject attached to the script being bound
	/// You may specify a GameObject to search in by supplying the Editor path in the constructor
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class BindOnClick : Attribute
	{
		private string buttonPath;

		public BindOnClick(string buttonPath = "")
		{
			this.buttonPath = buttonPath;
		}

		public void InjectInto(Object obj, MethodInfo method)
		{
			GameObject fromObj;
			if (string.IsNullOrEmpty(buttonPath))
			{
				var component = obj as Component;
				if (component != null)
				{
					fromObj = component.gameObject;
				}
				else
				{
					Debug.LogError("fromObject empty for field " + method.Name +
					               ", and no default gameObject could be found!");
					return;
				}
			}
			else
			{
				fromObj = GameObject.Find(buttonPath);

				if (fromObj == null)
				{
					fromObj = UnityBinder.FindInActiveObjectByName(buttonPath);

					if (fromObj == null)
					{
						Debug.LogError(
							"Could not find GameObject with name " + buttonPath + " for field " + method.Name);

						return;
					}
				}
			}

			var button = fromObj.GetComponent<Button>();
			if (button == null)
			{
				Debug.LogError("No Button Component found on GameObject @ " + buttonPath);
				return;
			}

			button.onClick.AddListener(delegate { method.Invoke(obj, new object[0]); });
		}
	}

	/// <summary>
	/// Attribute to bind a resource at runtime
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class BindResource : Binder
	{
		private static MethodInfo _cacheMethod;
		private static MethodInfo _cacheNotGeneric;

		public static MethodInfo GenericResourceLoad
		{
			get
			{
				if (_cacheMethod != null) return _cacheMethod;

				var methods = typeof(Resources).GetMethods();

				foreach (var method in methods)
				{
					if (method.Name != "Load" || !method.IsGenericMethod) continue;

					_cacheMethod = method;
					break;
				}

				return _cacheMethod;
			}
		}

		public static MethodInfo ResourceLoad
		{
			get
			{
				if (_cacheNotGeneric != null) return _cacheNotGeneric;

				_cacheNotGeneric = typeof(Resources).GetMethod("Load", new[] {typeof(string)});

				return _cacheNotGeneric;
			}
		}

		public string path;

		public BindResource(string path)
		{
			this.path = path;
		}


		public override void InjectInto(Object obj, FieldInfo field)
		{
			var injectType = field.FieldType;

			bool bindPrefab = false;
			object rawResult;
			if (injectType == typeof(GameObject))
			{
				bindPrefab = true;

				injectType = typeof(Object);

				rawResult = ResourceLoad.Invoke(null, new object[] {path});
			}
			else
			{
				var genericMethod = GenericResourceLoad.MakeGenericMethod(injectType);
				rawResult = genericMethod.Invoke(null, new object[] {path});
			}


			if (rawResult == null)
			{
				Debug.LogError("Could not find resource of type " + injectType + " for field " + field.Name);
			}
			else if (!injectType.IsInstanceOfType(rawResult))
			{
				Debug.LogError("Could not cast resource of type " + rawResult.GetType() + " to type of " + injectType +
				               " for field " + field.Name);
			}
			else
			{
				if (bindPrefab)
				{
					var objResult = rawResult as Object;
					var instance = Object.Instantiate(objResult) as GameObject;

					field.SetValue(obj, instance);
				}
				else
				{
					field.SetValue(obj, rawResult);
				}
			}
		}
	}

	/// <summary>
	/// Attribute that lets a field be injected from FindObjectOfType at runtime
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class Inject : Binder
	{

		public int index = 0;
		public bool optional = false;

		public Inject(int index = 0, bool optional = false)
		{
			this.index = index;
			this.optional = optional;
		}

		public override void InjectInto(Object obj, FieldInfo field)
		{
			var injectType = field.FieldType;

			var unityCall = typeof(Object).GetMethod("FindObjectsOfType", new Type[0]);
			if (unityCall == null)
			{
				Debug.LogError("Could not find method GetComponents !!");
				return;
			}


			var genericMethod = unityCall.MakeGenericMethod(injectType);
			var rawResult = genericMethod.Invoke(null, null);

			if (rawResult == null)
			{
				if (!optional)
					Debug.LogError("Could not find object of type " + injectType + " for field " + field.Name);
			}
			else if (rawResult is object[])
			{
				var result = rawResult as object[];

				if (result.Length > 0)
				{
					if (index >= result.Length)
					{
						if (!optional)
							Debug.LogError("Could not find object of type " + injectType + " for field " + field.Name +
						               " at index " + index);
					}
					else
					{
						var found = result[index];

						field.SetValue(obj, found);
					}
				}
				else
				{
					if (!optional)
						Debug.LogError("Could not find object of type " + injectType + " for field " + field.Name + " in " +
					               obj.name);
				}
			}
		}
	}

	/// <summary>
	/// Attribute to Bind a field to a component at runtime
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class BindComponent : Binder
	{

		public int index = 0;
		public string fromObject = "";
		public bool errorWhenFail = true;

		public BindComponent(int index = 0, string fromObject = "", bool errorWhenFailed = true)
		{
			this.index = index;
			this.fromObject = fromObject;
			this.errorWhenFail = errorWhenFailed;
		}

		public override void InjectInto(Object obj, FieldInfo field)
		{
			var injectType = field.FieldType;

			var unityCall = typeof(GameObject).GetMethod("GetComponents", new Type[0]);
			if (unityCall == null)
			{
				Debug.LogError("Could not find method GetComponents !!");
				return;
			}

			GameObject fromObj;
			if (string.IsNullOrEmpty(fromObject))
			{
				var component = obj as Component;
				if (component != null)
				{
					fromObj = component.gameObject;
				}
				else
				{
					if (errorWhenFail)
						Debug.LogError("fromObject empty for field " + field.Name +
						               ", and no default gameObject could be found!");
					return;
				}
			}
			else
			{
				fromObj = GameObject.Find(fromObject);

				if (fromObj == null)
				{
					fromObj = UnityBinder.FindInActiveObjectByName(fromObject);

					if (fromObj == null)
					{
						if (errorWhenFail)
							Debug.LogError("Could not find GameObject with name " + fromObject + " for field " +
							               field.Name);

						return;
					}
				}
			}

			if (injectType == typeof(GameObject) && !string.IsNullOrEmpty(fromObject))
			{
				field.SetValue(obj, fromObj);
				return;
			}


			var genericMethod = unityCall.MakeGenericMethod(injectType);
			var rawResult = genericMethod.Invoke(fromObj, null);

			if (rawResult == null)
			{
				if (errorWhenFail)
					Debug.LogError("Could not find component of type " + injectType + " for field " + field.Name);
			}
			else if (rawResult is object[])
			{
				var result = rawResult as object[];

				if (result.Length > 0)
				{
					if (index >= result.Length)
					{
						if (errorWhenFail)
							Debug.LogError("Could not find component of type " + injectType + " for field " +
							               field.Name + " at index " + index);
					}
					else
					{
						var found = result[index];

						field.SetValue(obj, found);
					}
				}
				else
				{
					if (errorWhenFail)
						Debug.LogError("Could not find component of type " + injectType + " for field " + field.Name);
				}
			}
		}
	}

	/// <summary>
	/// Attribute to Bind a field to a component at runtime
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class BindComponentInChildren : Binder
	{

		public int index = 0;
		public string fromObject = "";

		public BindComponentInChildren(int index = 0, string fromObject = "")
		{
			this.index = index;
			this.fromObject = fromObject;
		}

		public override void InjectInto(Object obj, FieldInfo field)
		{
			var injectType = field.FieldType;

			var unityCall = typeof(GameObject).GetMethod("GetComponentsInChildren", new Type[0]);
			if (unityCall == null)
			{
				Debug.LogError("Could not find method GetComponents !!");
				return;
			}

			GameObject fromObj;
			if (string.IsNullOrEmpty(fromObject))
			{
				var component = obj as Component;
				if (component != null)
				{
					fromObj = component.gameObject;
				}
				else
				{
					Debug.LogError("fromObject empty for field " + field.Name +
					               ", and no default gameObject could be found!");
					return;
				}
			}
			else
			{
				fromObj = GameObject.Find(fromObject);

				if (fromObj == null)
				{
					fromObj = UnityBinder.FindInActiveObjectByName(fromObject);

					if (fromObj == null)
					{
						Debug.LogError("Could not find GameObject with name " + fromObject + " for field " +
						               field.Name);

						return;
					}
				}
			}

			if (injectType == typeof(GameObject) && !string.IsNullOrEmpty(fromObject))
			{
				field.SetValue(obj, fromObj);
				return;
			}


			var genericMethod = unityCall.MakeGenericMethod(injectType);
			var rawResult = genericMethod.Invoke(fromObj, null);

			if (rawResult == null)
			{
				Debug.LogError("Could not find component of type " + injectType + " for field " + field.Name);
			}
			else if (rawResult is object[])
			{
				var result = rawResult as object[];

				if (result.Length > 0)
				{
					if (index >= result.Length)
					{
						Debug.LogError("Could not find component of type " + injectType + " for field " + field.Name +
						               " at index " + index);
					}
					else
					{
						var found = result[index];

						field.SetValue(obj, found);
					}
				}
				else
				{
					Debug.LogError("Could not find component of type " + injectType + " for field " + field.Name);
				}
			}
		}
	}

	/// <summary>
	/// Attribute to Bind a field to a component at runtime
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class BindComponentInParent : Binder
	{

		public int index = 0;
		public string fromObject = "";

		public BindComponentInParent(int index = 0, string fromObject = "")
		{
			this.index = index;
			this.fromObject = fromObject;
		}

		public override void InjectInto(Object obj, FieldInfo field)
		{
			var injectType = field.FieldType;

			var unityCall = typeof(GameObject).GetMethod("GetComponentsInParent", new Type[0]);
			if (unityCall == null)
			{
				Debug.LogError("Could not find method GetComponentInParent !!");
				return;
			}

			GameObject fromObj;
			if (string.IsNullOrEmpty(fromObject))
			{
				var component = obj as Component;
				if (component != null)
				{
					fromObj = component.gameObject;
				}
				else
				{
					Debug.LogError("fromObject empty for field " + field.Name +
					               ", and no default gameObject could be found!");
					return;
				}
			}
			else
			{
				fromObj = GameObject.Find(fromObject);

				if (fromObj == null)
				{
					fromObj = UnityBinder.FindInActiveObjectByName(fromObject);

					if (fromObj == null)
					{
						Debug.LogError("Could not find GameObject with name " + fromObject + " for field " +
						               field.Name);

						return;
					}
				}
			}

			if (injectType == typeof(GameObject) && !string.IsNullOrEmpty(fromObject))
			{
				field.SetValue(obj, fromObj);
				return;
			}


			var genericMethod = unityCall.MakeGenericMethod(injectType);
			var rawResult = genericMethod.Invoke(fromObj, null);

			if (rawResult == null)
			{
				Debug.LogError("Could not find component of type " + injectType + " for field " + field.Name);
			}
			else if (rawResult is object[])
			{
				var result = rawResult as object[];

				if (result.Length > 0)
				{
					if (index >= result.Length)
					{
						Debug.LogError("Could not find component of type " + injectType + " for field " + field.Name +
						               " at index " + index);
					}
					else
					{
						var found = result[index];

						field.SetValue(obj, found);
					}
				}
				else
				{
					Debug.LogError("Could not find component of type " + injectType + " for field " + field.Name);
				}
			}
		}
	}


	[AttributeUsage(AttributeTargets.Field)]
	public class BindComponentsInChildren : Binder
	{
		public string fromObject = "";

		public BindComponentsInChildren(string fromObject = "")
		{
			this.fromObject = fromObject;
		}

		private static List<T> ConvertArray<T>(Array input)
		{
			return input.Cast<T>().ToList(); // Using LINQ for simplicity
		}

		public override void InjectInto(Object obj, FieldInfo field)
		{
			var injectArrayType = field.FieldType;

			bool isList = false;
			Type injectType;
			if (injectArrayType.IsArray)
			{
				injectType = injectArrayType.GetElementType();
			}
			else if (injectArrayType.IsGenericType && injectArrayType.GetGenericTypeDefinition() == typeof(List<>))
			{
				injectType = injectArrayType.GetGenericArguments()[0];
				isList = true;
			}
			else
			{
				Debug.LogError("Could not find suitable type " + injectArrayType + " for field " + field.Name +
				               ". Field must either be an Array or a List");
				return;
			}

			var unityCall = typeof(GameObject).GetMethod("GetComponentsInChildren", new Type[0]);
			if (unityCall == null)
			{
				Debug.LogError("Could not find method GetComponents !!");
				return;
			}

			GameObject fromObj;
			if (string.IsNullOrEmpty(fromObject))
			{
				var component = obj as Component;
				if (component != null)
				{
					fromObj = component.gameObject;
				}
				else
				{
					Debug.LogError("fromObject empty for field " + field.Name +
					               ", and no default gameObject could be found!");
					return;
				}
			}
			else
			{
				fromObj = GameObject.Find(fromObject);

				if (fromObj == null)
				{
					fromObj = UnityBinder.FindInActiveObjectByName(fromObject);

					if (fromObj == null)
					{
						Debug.LogError("Could not find GameObject with name " + fromObject + " for field " +
						               field.Name);

						return;
					}
				}
			}

			if (injectType == typeof(GameObject) && !string.IsNullOrEmpty(fromObject))
			{
				field.SetValue(obj, fromObj);
				return;
			}


			var genericMethod = unityCall.MakeGenericMethod(injectType);
			var rawResult = genericMethod.Invoke(fromObj, null);

			if (rawResult == null)
			{
				Debug.LogError("Could not find component of type " + injectType + " for field " + field.Name);
			}
			else if (rawResult is object[])
			{
				var result = rawResult as object[];

				if (!isList)
				{
					field.SetValue(obj, result);
				}
				else
				{
					MethodInfo convertMethod = typeof(BindComponentsInChildren).GetMethod("ConvertArray",
						BindingFlags.NonPublic | BindingFlags.Static);
					if (convertMethod != null)
					{
						MethodInfo generic = convertMethod.MakeGenericMethod(new[] {injectType});

						field.SetValue(obj, generic.Invoke(null, new object[] {rawResult}));
					}
					else
					{
						Debug.LogError(
							"Fatel Error! Cannot make generic method call to BindComponentsInChildren.ConvertArray");
					}
				}
			}
		}
	}

	/// <summary>
	/// A MonoBehavior that injects fields in the Awake() function
	/// </summary>
	public class BindableMonoBehavior : MonoBehaviour
	{

		/// <summary>
		/// The standard Unity Awake() function. This function will invoke UnityBinder.Inject.
		/// 
		/// If you override this Awake() function, be sure to call base.Awake()
		/// </summary>
		protected virtual void Awake()
		{
			UnityBinder.Inject(this);
		}
	}

	public class MonoBehaviourEvents : MonoBehaviour
	{
		private List<Action> delegates = new List<Action>();

		private void Start()
		{
			foreach (Action @delegate in delegates)
			{
				@delegate();
			}
			
			delegates.Clear();
		}

		public async Task<bool> WaitForStart()
		{
			TaskCompletionSource<bool> eventCompleted = new TaskCompletionSource<bool>(TaskCreationOptions.None);
			
			delegates.Add(delegate
			{
				eventCompleted.SetResult(true);
			});

			return await eventCompleted.Task;
		}
	}

	public static class MonoBehaviorAsyncBinders
	{
		public static async Task<bool> WaitForStart(this GameObject gameObject)
		{
			var unityEvents = gameObject.GetComponent<MonoBehaviourEvents>();
			if (unityEvents == null)
				unityEvents = gameObject.AddComponent<MonoBehaviourEvents>();

			return await unityEvents.WaitForStart();
		}
	}
}