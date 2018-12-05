using UnityEngine.Networking;
using UnityEngine;
using System.Collections.Generic;

public class RedZone_Script : NetworkBehaviour
{
    [SerializeField] float radius = 30;

    CircleCollider2D circleCollider;
    DrawCircle drawCircle;
    string playerTag = "Player";

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        drawCircle = GetComponent<DrawCircle>();
        circleCollider.radius = radius;
        drawCircle.SetRadius(radius);
        //drawCircle.SetUpCircle();
    }

    private void Update()
    {
        if (!isServer)
            return;

        if (radius > 1)
            radius -= 1f * Time.deltaTime;
        circleCollider.radius = radius;
        drawCircle.SetRadius(radius);
        drawCircle.SetUpCircle();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            Health health = collision.GetComponent<Health>();
            health.SetOutOfZone(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            Health health = collision.GetComponent<Health>();
            health.SetOutOfZone(true);
        }
    }

    public float GetRadius()
    {
        return radius;
    }

}
