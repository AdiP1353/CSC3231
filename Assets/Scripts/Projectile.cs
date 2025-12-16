using System.Collections;
using UnityEngine;

public enum ProjectileType { Apple, Bomb }

public class Projectile : MonoBehaviour
{
    public ProjectileType type;
    public float speed = 10f;
    public Vector2 direction;
    public System.Action<Projectile> onHit;

    private float _deathDelay = 2f;
    

    void Update()
    {
        transform.position += (Vector3)(direction * (speed * Time.deltaTime));
    }
    
    
    public void Bounce(Vector2 segmentNormal)
    {
        direction = Vector2.Reflect(direction, segmentNormal).normalized;
        StartCoroutine(StartDeathTimer());
    }


    IEnumerator StartDeathTimer()
    {
        yield return new WaitForSeconds(_deathDelay);
        Destroy(gameObject);
    }
    
}

