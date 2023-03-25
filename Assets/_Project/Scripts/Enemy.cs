using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float currentRotateSpeed = 10f;
    [SerializeField] private float distanceBetweenCenterAndTransform = 0.1f;
    private bool distanceChanged = false;
    [SerializeField] private GameObject blastEffect;

    private Vector2 lastDistance;
    private Vector2 newDistance;
    
    private float time;
    private float distanceChangeDelay = .4f;

    public static event Action<float> OnEnemyDeath;
    public static event Action OnEnemyDeathNoArgs;

    public void Init(float distanceBetweenCenterAndTransform, float rotateSpeed)
    {
        currentRotateSpeed = rotateSpeed;
        this.distanceBetweenCenterAndTransform = distanceBetweenCenterAndTransform;
        InvokeRepeating("IncrementDistance", 0f, 5f);
    }
    
    private void Update()
    {
        HandleRadiusChange();
        Rotate();
    }

    private void HandleRadiusChange()
    {
        if (!distanceChanged)
        {
            return;
        }

        time += Time.deltaTime;
        transform.position = Vector2.Lerp(lastDistance, newDistance, time / distanceChangeDelay);

        if (Vector2.Distance(transform.position, newDistance) <= 0.01f)
        {
            transform.position = newDistance;
            distanceChanged = false;
            time = 0;
        }
    }

    private void Rotate()
    {
        transform.RotateAround(Vector3.zero, Vector3.forward, Time.deltaTime * currentRotateSpeed);
    }

    public void TakeDamage()
    {
        OnEnemyDeath?.Invoke(distanceBetweenCenterAndTransform);
        OnEnemyDeathNoArgs?.Invoke();
        Destroy(Instantiate(blastEffect, transform.position, Quaternion.identity), 1f);
        Destroy(gameObject);
    }

    private void IncrementDistance()
    {
        if (distanceBetweenCenterAndTransform >= 2f)
        {
            CancelInvoke();
            return;
        }
        
        distanceBetweenCenterAndTransform += .2f;
        distanceChanged = true;
        lastDistance = transform.position;
        newDistance = (transform.position - Vector3.zero).normalized * distanceBetweenCenterAndTransform;
    }
}
