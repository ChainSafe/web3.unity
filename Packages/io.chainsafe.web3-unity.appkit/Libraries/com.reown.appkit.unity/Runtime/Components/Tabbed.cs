using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Reown.AppKit.Unity.Components
{
    public class Tabbed : VisualElement
    {
        public const string Name = "tabbed";

        public static readonly string ClassNameTabsContainer = $"{Name}__tabs-container";
        public static readonly string ClassNameTabsContainerHidden = $"{ClassNameTabsContainer}--hidden";
        public static readonly string ClassNameContentContainer = $"{Name}__content-container";

        public static readonly string ClassNameTab = $"{Name}__tab";
        public static readonly string ClassNameTabActive = $"{ClassNameTab}--active";
        public static readonly string ClassNameTabHidden = $"{ClassNameTab}--hidden";
        public static readonly string ClassNameContentActive = $"{Name}__content--active";
        public static readonly string ClassNameTabDynamicBg = $"{Name}__tab-dynamic-bg";

        private const string TabNameSuffix = "Tab";
        private const string ContentNameSuffix = "Content";

        private VisualElement _dynamicTabBg;

        public event Action<VisualElement> ContentShown;
        public event Action<VisualElement> ContentHidden;

        public new class UxmlFactory : UxmlFactory<Tabbed>
        {
        }

        public Tabbed()
        {
            styleSheets.Add(Resources.Load<StyleSheet>("Reown/AppKit/Components/Tabbed/Tabbed"));
            name = Name;

            RegisterCallback<AttachToPanelEvent>(_ =>
            {
                var tabs = GetAllTabs();
                tabs.ForEach((Label tab) => tab.RegisterCallback<ClickEvent>(OnTabClick));

                CreateDynamicTabBg();
            });
        }

        public void ActivateFirstVisibleTab()
        {
            var firstVisibleTab = GetAllTabs()
                .Where(tab => !tab.ClassListContains(ClassNameTabHidden))
                .First();

            if (firstVisibleTab != null)
            {
                GetAllTabs()
                    .Where(tab => tab != firstVisibleTab && IsTabActive(tab))
                    .ForEach(UnselectTab);

                SelectTab(firstVisibleTab);
            }
        }

        private void CreateDynamicTabBg()
        {
            if (_dynamicTabBg != null)
                return;

            _dynamicTabBg = new VisualElement
            {
                name = ClassNameTabDynamicBg,
                pickingMode = PickingMode.Ignore
            };
            _dynamicTabBg.AddToClassList(ClassNameTabDynamicBg);

            this.Q(className: ClassNameTabsContainer).Add(_dynamicTabBg);

            _dynamicTabBg.SendToBack();
        }

        public static bool IsTabActive(Label tab)
        {
            return tab.ClassListContains(ClassNameTabActive);
        }

        private static string GenerateContentName(VisualElement tab)
        {
            return tab.name.Replace(TabNameSuffix, ContentNameSuffix);
        }

        private void OnTabClick(ClickEvent evt)
        {
            var clickedTab = evt.currentTarget as Label;
            var isTabActive = IsTabActive(clickedTab);
            if (!isTabActive)
                GetAllTabs()
                    .Where(tab => tab != clickedTab && IsTabActive(tab))
                    .ForEach(UnselectTab);

            SelectTab(clickedTab);
        }

        private UQueryBuilder<Label> GetAllTabs()
        {
            return this.Query<Label>(className: ClassNameTab);
        }

        private void UnselectTab(Label tab)
        {
            tab.RemoveFromClassList(ClassNameTabActive);

            var content = FindContent(tab);
            content.RemoveFromClassList(ClassNameContentActive);
            ContentHidden?.Invoke(content);
        }

        private void SelectTab(VisualElement tab)
        {
            tab.AddToClassList(ClassNameTabActive);
            UpdateTabDynamicBg(tab);

            var content = FindContent(tab);
            content.AddToClassList(ClassNameContentActive);
            ContentShown?.Invoke(content);
        }

        private void UpdateTabDynamicBg(VisualElement tab)
        {
            _dynamicTabBg.style.width = tab.resolvedStyle.width;
            _dynamicTabBg.style.height = tab.resolvedStyle.height;
            _dynamicTabBg.style.left = tab.resolvedStyle.left;
        }

        private VisualElement FindContent(VisualElement tab)
        {
            return this.Q(GenerateContentName(tab));
        }
    }
}