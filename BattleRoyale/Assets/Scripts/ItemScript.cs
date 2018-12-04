using UnityEngine.Networking;
using UnityEngine;

public class ItemScript : NetworkBehaviour
{
    [SerializeField] ItemsData data;

    SpriteRenderer sp;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        sp.sprite = data.GetIcon();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponentInChildren<GetPickUpText>().GetGameObject().SetActive(true);
        if (Input.GetKeyDown(KeyCode.F))
        {
            collision.GetComponentInChildren<GetPickUpText>().GetGameObject().SetActive(false);
            ItemSlot slot = collision.GetComponent<AllPlayerItems>().GetItemSlot(data.GetSlotType());
            slot.SetData(data);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            collision.GetComponentInChildren<GetPickUpText>().GetGameObject().SetActive(false);
            ItemSlot slot = collision.GetComponent<AllPlayerItems>().GetItemSlot(data.GetSlotType());
            slot.SetData(data);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    { 
        collision.GetComponentInChildren<GetPickUpText>().GetGameObject().SetActive(false);
    }

}
