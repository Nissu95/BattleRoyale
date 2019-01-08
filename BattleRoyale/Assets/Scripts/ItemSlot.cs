using UnityEngine.UI;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public enum SlotType { Weapon, FirstKit, Bandages };

    [SerializeField] ItemsData data;
    //[SerializeField] SlotType slot;

    Image itmImage;
    int stash = 0;

    void Awake()
    {
        itmImage = GetComponent<Image>();
    }

    void Start()
    {
        if (data)
            itmImage.sprite = data.GetIcon();
        else
        {
            itmImage.enabled = false;
            stash = 0;
        }
    }

    public void SlotClick()
    {
        if (data)
        {
            GetComponentInParent<Player>().CmdHeal((HealingItemData)data);
            data.DoSomething(gameObject);
        }
    }

    public void SetData(ItemsData _data)
    {
        data = _data;
        if (itmImage)
        {
            itmImage.enabled = true;
            if (_data)
            {
                itmImage.sprite = _data.GetIcon();
                stash++;
            }
            else
            {
                itmImage.sprite = null;
                itmImage.enabled = false;
                stash = 0;
            }
        }
    }

    public ItemsData GetData()
    {
        return data;
    }

    public int GetStash()
    {
        return stash;
    }

    public void StashSubstraction(int amount)
    {
        stash -= amount;
    }

}
