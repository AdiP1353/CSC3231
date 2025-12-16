using System;
using Unity.VisualScripting;
using UnityEngine;

public class Segment : MonoBehaviour
{
    private void OnEnable()
    {
        ;
    }


    private void OnDisable()
    {
        ;
    }
    
    
    public bool IsActiveSegment()
    {
        return gameObject.activeSelf;
    }

    public Vector2 GetNormal()
    {
        Vector2 center = transform.parent.position;
        return ((Vector2)transform.position - center).normalized;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            Projectile projectile = collision.GetComponent<Projectile>();
            if (projectile != null)
            {
                Vector2 normal = GetNormal(); 
        
                projectile.Bounce(normal);
            }
        }
    }
    
    


    
}
