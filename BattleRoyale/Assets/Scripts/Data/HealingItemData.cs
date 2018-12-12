using UnityEngine;

[CreateAssetMenu(menuName = ("Game/Data/Healing_Items"))]
public class HealingItemData : ItemsData
{
    public int healthAmount;

    //Health health;

    public override void DoSomething(GameObject other)
    {
        //health = other.GetComponentInParent<Health>();
        ItemSlot itemSlot = other.GetComponent<ItemSlot>();
        itemSlot.StashSubstraction(1);
        //health.Heal(healthAmount);
        if (itemSlot.GetStash() <= 0)
        {
            other.SetActive(false);
            other.GetComponent<ItemSlot>().SetData(null);
        }
    }
}
