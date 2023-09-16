using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using UnityEditor.Android;
using UnityEngine;

public class AndroidBuildPostProcess : IPostGenerateGradleAndroidProject
{
    public int callbackOrder { get { return 1; } }

    public void OnPostGenerateGradleAndroidProject(string path)
    {
        Uri uri = null;

        try
        {
            uri = new Uri(System.IO.File.ReadAllText("Assets/Resources/webauth"));
        }
        catch
        {
            throw new Exception("Deep Link uri is invalid or does not exist. Please generate from \"Window > Web3Auth > Generate Deep Link\" Menu");
        }

        string manifest = path + "\\src\\main\\AndroidManifest.xml";

        var document = new XmlDocument();
        document.Load(manifest);

        var activityNode = document.DocumentElement.SelectSingleNode("application").SelectSingleNode("activity");
        activityNode.AppendChild(activityNode.OwnerDocument.ImportNode(BuildeNode(string.Format(@"
          <intent-filter  xmlns:android=""http://schemas.android.com/apk/res/android"">
            <action android:name=""android.intent.action.VIEW"" />

            <category android:name=""android.intent.category.DEFAULT"" />
            <category android:name= ""android.intent.category.BROWSABLE"" />


            <data android:scheme=""{0}"" android:host=""{1}"" android:pathPrefix=""{2}"" android:pathPattern=""/*"" />
          </intent-filter>
        ", uri.Scheme, uri.Host, uri.LocalPath)), true));

        document.Save(manifest);

    }

    private XmlNode BuildeNode(string text)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(text);

        return doc.DocumentElement;
    }
}
