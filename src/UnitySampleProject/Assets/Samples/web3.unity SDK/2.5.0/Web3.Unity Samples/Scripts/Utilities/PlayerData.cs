using System.Collections;
using System.Collections.Generic;
using System.IO;
using ChainSafe.Gaming.WalletConnect;
using Newtonsoft.Json;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
public class PlayerData
{
    #region Save & Load

    private static string _path = $"{Path.Combine(Application.persistentDataPath, nameof(PlayerData))}.json";

    public static PlayerData Instance { get; private set; } = new PlayerData();

    private bool FileExists => File.Exists(_path);

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

        using (FileStream fs = new FileStream(_path, FileMode.Open, FileAccess.Read))
        using (StreamReader sr = new StreamReader(fs))
        {
            string rawJson = sr.ReadToEnd();

            Instance = JsonConvert.DeserializeObject<PlayerData>(rawJson);
            
            Debug.Log($"{nameof(PlayerData)} loaded from path {_path}.");
        }
    }

    private void SaveData()
    {
        // create file
        using (FileStream fs = new FileStream(_path, FileMode.OpenOrCreate, FileAccess.Write))
        using (StreamWriter sw = new StreamWriter(fs))
        {
            sw.WriteLine(JsonConvert.SerializeObject(Instance));
            
            Debug.Log($"{nameof(PlayerData)} saved at path {_path}.");
        }
    }

    public static void Save()
    {
        Instance.SaveData();
    }
    
    public static void Load()
    {
        Instance.LoadData();
    }

    public static void Clear()
    {
        Instance = new PlayerData();
        
        Save();
    }
    
    #endregion

    [JsonProperty] public WalletConnectConfig WalletConnectConfig { get; set; }
}
