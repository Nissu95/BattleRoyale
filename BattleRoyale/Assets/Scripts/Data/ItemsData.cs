using UnityEngine;

//[CreateAssetMenu(menuName = ("Game/Data/Items"))]
public class ItemsData : ScriptableObject
{
    [SerializeField] Sprite icon;
    [SerializeField] ItemSlot.SlotType item;

    public Sprite GetIcon()
    {
        return icon;
    }

    public ItemSlot.SlotType GetSlotType()
    {
        return item;
    }

    public virtual void DoSomething(GameObject other) { }
}
