using ChainSafe.Gaming;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DefaultAssetValueAttribute))]
public class DefaultAssetValuePropertyDrawer : PropertyDrawer
{
    private DefaultAssetValueAttribute _attribute;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label, true);

        if (property.propertyType != SerializedPropertyType.ObjectReference || property.objectReferenceValue != null)
        {
            return;
        }

        _attribute ??= (DefaultAssetValueAttribute)attribute;

        var asset = AssetDatabase.LoadAssetAtPath(_attribute.Path, fieldInfo.FieldType);

        if (asset != null)
        {
            property.objectReferenceValue = asset;
        }
    }
}
