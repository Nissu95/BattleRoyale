using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool>{}

public class Player : NetworkBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject deadCharacter;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] float speed;
    [SerializeField] Transform characterTransform;
    [SerializeField] GameObject loseCanvas;
    [SerializeField] GameObject winCanvas;
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

        if (GameManager.instance.cantPlayers == 1)
        {
            onToggleShared.Invoke(false);
            if (isLocalPlayer)
            {
                winCanvas.SetActive(true);
                Camera.main.transform.parent = null;
                onToggleLocal.Invoke(false);
            }
        }
    }

    [Command]
    void CmdFire()
    {
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);
        
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * 10;

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 2.0f);
    }

    [Command]
    public void CmdHeal(HealingItemData data)
    {
        Health health = GetComponent<Health>();
        health.Heal(data.healthAmount);
    }

    public void DisablePlayer()
    {
        onToggleShared.Invoke(false);
        Instantiate<GameObject>(deadCharacter, transform.position, characterTransform.rotation);
        GetComponentInChildren<SpriteRenderer>().enabled = false;

        if (isLocalPlayer)
        {
            loseCanvas.SetActive(true);
            Camera.main.transform.parent = null;
            onToggleLocal.Invoke(false);
        }
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
