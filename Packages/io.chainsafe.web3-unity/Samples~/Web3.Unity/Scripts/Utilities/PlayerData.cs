using System.Collections;
using System.Collections.Generic;
using System.IO;
using ChainSafe.Gaming.WalletConnect;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// Player Data that can be persisted through runtime and sessions in a PlayerData.json file.
/// </summary>
/// <remarks>Only serialize(Save and Load) fields and properties with the [JsonProperty] attribute.</remarks>
[JsonObject(MemberSerialization.OptIn)]
public class PlayerData
{
    #region Save & Load

    private string Path
    {
        get
        {
            // save closer to assets when in editor
            // more accessible
            string directory = Application.isEditor ? Application.dataPath : Application.persistentDataPath;
            
            return $"{System.IO.Path.Combine(directory, nameof(PlayerData))}.json";
        }
    }

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
    }
    
    #endregion

    /// <summary>
    /// Saved Wallet Connect Config used for restoring session (Remember Me) Implementation.
    /// </summary>
    [JsonProperty] public WalletConnectConfig WalletConnectConfig { get; set; }
}
