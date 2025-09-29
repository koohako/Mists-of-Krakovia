using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public float lifeTime = 5f;
    private Vector3 dir;

    public void Init(Vector2 d)
    {
        dir = d.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0, angle - 90f);
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // Lógica de colisão (inimigos, obstáculos, etc.)
        if (other.CompareTag("Enemy") || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
