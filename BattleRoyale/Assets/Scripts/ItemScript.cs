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

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag) && collision.gameObject.GetComponent<Player>().isLocalPlayer)
        {
            collision.GetComponentInChildren<GetPickUpText>().GetGameObject().SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                collision.GetComponentInChildren<GetPickUpText>().GetGameObject().SetActive(false);
                ItemSlot slot = collision.GetComponent<AllPlayerItems>().GetItemSlot(data.GetSlotType());
                slot.SetData(data);
                slot.gameObject.SetActive(true);
                CmdDestroyObject();
                //Destroy(gameObject);
                //RpcDestroyObject();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.F) && collision.CompareTag(playerTag) && collision.gameObject.GetComponent<Player>().isLocalPlayer)
        {
            collision.GetComponentInChildren<GetPickUpText>().GetGameObject().SetActive(false);
            ItemSlot slot = collision.GetComponent<AllPlayerItems>().GetItemSlot(data.GetSlotType());
            slot.SetData(data);
            slot.gameObject.SetActive(true);
            CmdDestroyObject();
            //Destroy(gameObject);
            //RpcDestroyObject();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag) && collision.gameObject.GetComponent<Player>().isLocalPlayer)
        collision.GetComponentInChildren<GetPickUpText>().GetGameObject().SetActive(false);
    }

    [Command]
    void CmdDestroyObject()
    {
        Destroy(this.gameObject);
        //NetworkServer.Destroy(this.gameObject);
        //RpcDestroyObject();
    }

    /*[ClientRpc]
    void RpcDestroyObject()
    {
        if (!isServer)
            return;

        NetworkServer.Destroy(this.gameObject);
        //NetworkIdentity.Destroy(gameObject);
    }

}

    }*/

    public ItemsData GetData()
    {
        return data;
    }