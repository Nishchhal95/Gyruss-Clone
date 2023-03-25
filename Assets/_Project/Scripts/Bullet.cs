using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Action backToPoolAction;
    private bool active;
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!active)
        {
            return;
        }
        if (transform.position.x < -3f || transform.position.x > 3f || transform.position.y < -6f ||
            transform.position.y > 6f)
        {
            active = false;
            backToPoolAction?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage();
        }
        
        active = false;
        backToPoolAction?.Invoke();
    }

    public void Init(Vector2 velocity, Action backToPool)
    {
        active = true;
        _rigidbody2D.velocity = velocity;
        backToPoolAction = backToPool;
    }
}
