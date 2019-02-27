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
    
    private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.

    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();

        EnablePlayer();
    }
    
    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //Add velocity to our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        rb2d.velocity = (movement * speed);
        

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
                if (!loseCanvas.activeInHierarchy)
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
