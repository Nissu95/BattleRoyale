using UnityEngine.UI;
using UnityEngine;

public class StashUI : MonoBehaviour
{
    [SerializeField] Text stashText;
    [SerializeField] ItemSlot ItemSlot;

    private void Update()
    {
        stashText.text = "x" + ItemSlot.GetStash();
    }

}
