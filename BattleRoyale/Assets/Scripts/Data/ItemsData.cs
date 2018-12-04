using UnityEngine;

//[CreateAssetMenu(menuName = ("Game/Data/Items"))]
public class ItemsData : ScriptableObject
{
    public enum ItemType { Weapon, FirstKit, Sales };

    [SerializeField] Sprite icon;
    [SerializeField] ItemType item;

    public Sprite GetIcon()
    {
        return icon;
    }

    public virtual void DoSomething(GameObject other) { }
}
