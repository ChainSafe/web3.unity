using UnityEditor;
using UnityEditor.Animations;

namespace LootBoxes
{
    public static class EditorUtil
    {
        public const string RootMenu = Menues.Root + "Util/";

        #region Animator

        [MenuItem(RootMenu + "Remove Transition Time #&t")]
        public static void RemoveTransitionTime()
        {
            var transition = (AnimatorStateTransition)Selection.activeObject;
            transition.hasExitTime = false;
            transition.duration = 0f;
        }

        [MenuItem(RootMenu + "Remove Transition Time #&t", true)]
        public static bool ValidateRemoveTransitionTime()
        {
            return Selection.activeObject is AnimatorStateTransition;
        }

        #endregion
    }
}