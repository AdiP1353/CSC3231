using UnityEngine;

public class Core : MonoBehaviour
{
    [SerializeField] public GameManager gameManager;


    private void OnTriggerEnter2D(Collider2D other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile != null)
        {
            if (projectile.type == ProjectileType.Apple)
            {
                gameManager.score++;
            }
            else if (projectile.type == ProjectileType.Bomb)
            {
                gameManager.score--;
            }

            Destroy(other.gameObject);
        }

    }
}