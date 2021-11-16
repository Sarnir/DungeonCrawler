using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    private Vector2 targetPos;

    [SerializeField]
    private float speed = 3f;
    
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        
        var direction = targetPos - rigidbody2D.position;
        var velocity = direction.normalized * speed;

        rigidbody2D.velocity = velocity;
        
        Vector3 dir = -velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void SetTarget(Vector2 target)
    {
        targetPos = target;
    }
}
