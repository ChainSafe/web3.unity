using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// Search menu functionality
/// </summary>
public class StringListSearchProvider : ScriptableObject, ISearchWindowProvider
{
    #region Fields

    private string[] listItems;
    private Action<string> onSetIndexCallback;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes list items and sets the callback for when items are selected
    /// </summary>
    /// <param name="list"></param>
    /// <param name="callback"></param>
    public void Initialize(string[] list, Action<string> callback)
    {
        listItems = list;
        onSetIndexCallback = callback;
    }

    /// <summary>
    /// Creates, formats and sorts the search list tree
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();
        searchList.Add(new SearchTreeGroupEntry(new GUIContent("Select Option"), 0));
        List<string> sortedSearchList = listItems.ToList();
        sortedSearchList.Sort((a, b) =>
        {
            string[] splits1 = a.Split('/');
            string[] splits2 = b.Split('/');
            for (int i = 0; i < splits2.Length; i++)
            {
                if (i >= splits2.Length)
                {
                    return 1;
                }

                int value = splits1[i].CompareTo(splits2[i]);
                if (value != 0)
                {
                    if (splits1.Length != splits2.Length && (i == splits1.Length || i == splits2.Length - 1))
                        return splits1.Length < splits2.Length ? 1 : -1;
                    return value;
                }
            }

            return 0;
        });
        List<string> groups = new List<string>();
        foreach (var item in sortedSearchList)
        {
            string[] entryTitle = item.Split('/');
            string groupName = "";
            for (int i = 0; i < entryTitle.Length - 1; i++)
            {
                groupName += entryTitle[i];
                if (!groups.Contains(groupName))
                {
                    searchList.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[i]), i + 1));
                    groups.Add(groupName);
                }
                groupName += "/";
            }
            SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(entryTitle.Last()));
            entry.level = entryTitle.Length;
            entry.userData = entryTitle.Last();
            searchList.Add(entry);
        }
        return searchList;
    }

    /// <summary>
    /// Fires when an item is selected, also invokes the onDropDownChange event from Chainsafe server settings to save data
    /// </summary>
    /// <param name="SearchTreeEntry"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        onSetIndexCallback?.Invoke((string)SearchTreeEntry.userData);
        ChainSafeServerSettings instance = EditorWindow.GetWindow<ChainSafeServerSettings>();
        instance.UpdateServerMenuInfo();
        return true;
    }

    #endregion
}
