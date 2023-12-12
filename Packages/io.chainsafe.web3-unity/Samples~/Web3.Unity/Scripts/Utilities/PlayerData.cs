using System.IO;
using ChainSafe.Gaming.WalletConnect;
using Newtonsoft.Json;
#if UNITY_EDITOR
using System.Diagnostics;
using UnityEditor;
#endif
using UnityEngine;
using Debug = UnityEngine.Debug;

/// <summary>
/// Player Data that can be persisted through runtime and sessions in a PlayerData.json file.
/// </summary>
/// <remarks>Only serialize(Save and Load) fields and properties with the [JsonProperty] attribute.</remarks>
[JsonObject(MemberSerialization.OptIn)]
public class PlayerData
{
    #region Save & Load

    private string Path => $"{System.IO.Path.Combine(Application.persistentDataPath, nameof(PlayerData))}.json";

    /// <summary>
    /// Singleton instance.
    /// </summary>
    public static PlayerData Instance { get; private set; } = new PlayerData();

    private bool FileExists => File.Exists(Path);

    /// <summary>
    /// Loads Data when runtime starts.
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    public static void LoadOnStart()
    {
        Instance.LoadData();
    }

    private void LoadData()
    {
        if (!FileExists)
        {
            SaveData();

            return;
        }

        using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
        using (StreamReader sr = new StreamReader(fs))
        {
            string rawJson = sr.ReadToEnd();

            Instance = JsonConvert.DeserializeObject<PlayerData>(rawJson);

            Debug.Log($"{nameof(PlayerData)} loaded from path {Path}.");
        }
    }

    private void SaveData()
    {
        // create file
        using (FileStream fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write))
        using (StreamWriter sw = new StreamWriter(fs))
        {
            sw.WriteLine(JsonConvert.SerializeObject(Instance));

            Debug.Log($"{nameof(PlayerData)} saved at path {Path}.");
        }
    }

    /// <summary>
    /// Save/Persist data in <see cref="Instance"/>.
    /// </summary>
    public static void Save()
    {
        Instance.SaveData();
    }

    /// <summary>
    /// Load saved/persisted data into <see cref="Instance"/>
    /// </summary>
    public static void Load()
    {
        Instance.LoadData();
    }

    /// <summary>
    /// Clear all data.
    /// </summary>
    public static void Clear()
    {
        Instance = new PlayerData();

        Save();
        
        Debug.Log($"{nameof(PlayerData)} cleared.");
    }

    #endregion

    /// <summary>
    /// Saved Wallet Connect Config used for restoring session (Remember Me) Implementation.
    /// </summary>
    [JsonProperty] public WalletConnectConfig WalletConnectConfig { get; set; }

#if UNITY_EDITOR
    [MenuItem("Tools/Player Data/Remove")]
    public static void RemovePlayerData()
    {
        if (File.Exists(Instance.Path))
        {
            File.Delete(Instance.Path);
        }
        else
        {
            Debug.LogError($"{nameof(PlayerData)} not found at {Instance.Path}.");
        }
        
        Debug.Log($"{nameof(PlayerData)} Removed.");
    }
    
    [MenuItem("Tools/Player Data/Clear")]
    public static void ClearPlayerData()
    {
        Clear();
    }
    
    [MenuItem("Tools/Open Persistent Data Path")]
    public static void OpenPersistentDataPath()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
                Process.Start(Application.persistentDataPath);
                break;
            case RuntimePlatform.LinuxEditor:
                Process.Start("xdg-open", Application.persistentDataPath);
                break;
            case RuntimePlatform.OSXEditor:
                Process.Start("open", Application.persistentDataPath);
                break;
            default:
                Debug.LogError($"Can't open on {Application.platform} Platform.");
                break;
        }
    }
#endif
}
