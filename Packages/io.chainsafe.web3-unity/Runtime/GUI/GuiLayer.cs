using UnityEngine;

[CreateAssetMenu(menuName = "ChainSafe/Temp/GUI Layer", fileName = "New GUI Layer", order = 1000)]
public class GuiLayer : ScriptableObject
{
    [field:SerializeField] public int SortOrder { get; private set; }
    [field:SerializeField] public bool Transparent { get; private set; }
}