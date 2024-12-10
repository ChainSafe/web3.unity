using System;
using UnityEngine;
using UnityEngine.UIElements;
using Reown.AppKit.Unity.Utils;

namespace Reown.AppKit.Unity.Components
{
    public class Tag : VisualElement
    {
        public const string Name = "tag";
        public static readonly string NameLabel = $"{Name}__label";

        public static readonly string ClassNameTypeInfo = $"{Name}--info";
        public static readonly string ClassNameTypeAccent = $"{Name}--accent";

        public readonly Label label;

        public new class UxmlFactory : UxmlFactory<Tag>
        {
        }

        public Tag() : this("---", TagType.Info)
        {
        }

        public Tag(string text, TagType type)
        {
            var asset = Resources.Load<VisualTreeAsset>("Reown/AppKit/Components/Tag/Tag");
            asset.CloneTree(this);

            name = Name;

            // --- Label
            label = this.Q<Label>(NameLabel);
            label.text = text.FontWeight700();

            // --- Type Style
            switch (type)
            {
                case TagType.Info:
                    AddToClassList(ClassNameTypeInfo);
                    break;
                case TagType.Accent:
                    AddToClassList(ClassNameTypeAccent);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public enum TagType
        {
            Info,
            Accent
        }
    }
}