using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPlayerItems : MonoBehaviour
{
    [SerializeField] ItemSlot weaponSlot;
    [SerializeField] ItemSlot salesSlot;
    [SerializeField] ItemSlot firstAidKitSlot;

    public ItemSlot GetItemSlot(ItemSlot.SlotType slotType)
    {
        switch (slotType)
        {
            case ItemSlot.SlotType.Weapon:
                return weaponSlot;
            case ItemSlot.SlotType.FirstKit:
                return firstAidKitSlot;
            case ItemSlot.SlotType.Bandages:
                return salesSlot;
            default:
                return null;
        }
    } 

}
