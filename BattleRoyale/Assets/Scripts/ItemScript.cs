using UnityEngine.Networking;
using UnityEngine;

public class ItemScript : NetworkBehaviour
{
    [SerializeField] ItemsData data;

    string playerTag = "Player";
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
        if (collision.CompareTag(playerTag))
        {
            collision.GetComponentInChildren<GetPickUpText>().GetGameObject().SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                collision.GetComponentInChildren<GetPickUpText>().GetGameObject().SetActive(false);
                ItemSlot slot = collision.GetComponent<AllPlayerItems>().GetItemSlot(data.GetSlotType());
                slot.SetData(data);
                slot.gameObject.SetActive(true);
                CmdDestroyObject();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.F) && collision.CompareTag(playerTag))
        {
            collision.GetComponentInChildren<GetPickUpText>().GetGameObject().SetActive(false);
            ItemSlot slot = collision.GetComponent<AllPlayerItems>().GetItemSlot(data.GetSlotType());
            slot.SetData(data);
            slot.gameObject.SetActive(true);
            CmdDestroyObject();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        collision.GetComponentInChildren<GetPickUpText>().GetGameObject().SetActive(false);
    }

    [Command]
    void CmdDestroyObject()
    {
        NetworkServer.Destroy(this.gameObject);
        //NetworkIdentity.Destroy(gameObject);
    }

}
