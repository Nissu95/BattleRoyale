using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool>{}

public class Player : NetworkBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] float speed;
    [SerializeField] Transform characterTransform;

    [SerializeField] ToggleEvent onToggleShared;
    [SerializeField] ToggleEvent onToggleLocal;
    [SerializeField] ToggleEvent onToggleRemote;
    string itemTag = "Item";

    void Start()
    {
        EnablePlayer();
    }
    
    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += move * speed * Time.deltaTime;
        

        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseScreenPosition - (Vector2)characterTransform.position).normalized;
        characterTransform.up = direction;


        if (Input.GetMouseButtonDown(0))
            CmdFire();
    }

    [Command]
    void CmdFire()
    {
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);
        
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * 6;

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 2.0f);
    }

    public void DisablePlayer()
    {
        onToggleShared.Invoke(false);

        if (isLocalPlayer)
            onToggleLocal.Invoke(false);
        else
            onToggleRemote.Invoke(false);
    }

    void EnablePlayer()
    {
        onToggleShared.Invoke(true);

        if (isLocalPlayer)
            onToggleLocal.Invoke(true);
        else
            onToggleRemote.Invoke(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isLocalPlayer)
            return;

        if (collision.CompareTag(itemTag))
        {
            GetComponentInChildren<GetPickUpText>().GetGameObject().SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                GetComponentInChildren<GetPickUpText>().GetGameObject().SetActive(false);

                ItemsData data = collision.gameObject.GetComponent<ItemScript>().GetData();

                ItemSlot slot = GetComponent<AllPlayerItems>().GetItemSlot(data.GetSlotType());
                slot.SetData(data);
                slot.gameObject.SetActive(true);
                
                CmdDestroyObject(collision.gameObject);
            }
        }
    }

    [Command]
    void CmdDestroyObject(GameObject objectToDestroy)
    {
        RpcDestroyObject(objectToDestroy);
        Destroy(objectToDestroy);
    }

    [ClientRpc]
    void RpcDestroyObject(GameObject objectToDestroy)
    {
        Destroy(objectToDestroy);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isLocalPlayer)
            return;

        if (collision.CompareTag(itemTag))
            GetComponentInChildren<GetPickUpText>().GetGameObject().SetActive(false);
    }
}
