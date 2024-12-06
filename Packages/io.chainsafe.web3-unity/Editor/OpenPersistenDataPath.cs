﻿using UnityEditor;
using UnityEngine;
using System.IO;

public class OpenPersistentDataPath
{
    [MenuItem("Edit/Open Persistent Data Path")]
    private static void OpenPersistentDataPathFolder()
    {
        string path = Application.persistentDataPath;

        // Check if the directory exists
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // Open the folder in the file explorer
        EditorUtility.RevealInFinder(path);
    }
}