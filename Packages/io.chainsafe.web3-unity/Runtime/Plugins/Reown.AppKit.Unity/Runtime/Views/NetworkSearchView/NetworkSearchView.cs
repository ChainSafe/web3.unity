using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity.Components
{
    public class NetworkSearchView : VisualElement
    {
        public const string Name = "network-search-view";
        public static readonly string NameList = $"{Name}__list";
        public static readonly string NameInput = $"{Name}__input";

        public readonly ScrollView scrollView;
        public readonly TextInput searchInput;

        public new class UxmlFactory : UxmlFactory<NetworkSearchView>
        {
        }
        
        public NetworkSearchView() : this(null)
        {
        }

        public NetworkSearchView(string visualTreePath)
        {
            var asset = Resources.Load<VisualTreeAsset>(visualTreePath ?? "Reown/AppKit/Views/NetworkSearchView/NetworkSearchView");
            asset.CloneTree(this);

            name = Name;

            scrollView = this.Q<ScrollView>(NameList);
        }
    }
}