using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] int damage;
    [SerializeField] string playerTag;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == playerTag)
        {
            var hit = collision.gameObject;
            var health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
