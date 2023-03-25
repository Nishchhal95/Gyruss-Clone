using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 10f;
    private float currentRotateSpeed = 10f;
    [SerializeField] private KeyCode counterClockwiseRotationKey = KeyCode.D;
    [SerializeField] private KeyCode clockwiseRotationKey = KeyCode.A;
    [SerializeField] private KeyCode shootKey = KeyCode.Space;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float bulletShootForce = 20f;
    [SerializeField] private float shootInterval = 0.5f;
    private float currentShootInterval = 0.5f;
    private bool canShoot = true;

    private Transform _transform;
    
    public static event Action PlayerShoot;

    private void Awake()
    {
        _transform = transform;
        currentRotateSpeed = rotateSpeed;
        currentShootInterval = shootInterval;
        
        ObjectPooler.CreatePool(bulletPrefab, null, 5);
    }

    private void Update()
    {
        ShootTimerCheck();
        HandleInput();
        RotatePlayer();
    }

    private void ShootTimerCheck()
    {
        if (canShoot)
        {
            return;
        }

        currentShootInterval -= Time.deltaTime;
        if (!(currentShootInterval <= 0))
        {
            return;
        }
        canShoot = true;
        currentShootInterval = shootInterval;
    }

    private void HandleInput()
    {
        if (Input.GetKey(clockwiseRotationKey))
        {
            currentRotateSpeed = -rotateSpeed;
        }
        
        else if (Input.GetKey(counterClockwiseRotationKey))
        {
            currentRotateSpeed = rotateSpeed;
        }

        else
        {
            currentRotateSpeed = 0;
        }

        if (canShoot && Input.GetKey(shootKey))
        {
            Shoot();
        }
    }

    private void RotatePlayer()
    {
        _transform.RotateAround(Vector3.zero, Vector3.forward, Time.deltaTime * currentRotateSpeed);
    }
    
    private void Shoot()
    {
        canShoot = false;

        PlayerShoot?.Invoke();
        GameObject bullet = ObjectPooler.InstantiateFromPool(bulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(shootPoint.up * bulletShootForce, 
            () => ObjectPooler.BackToPool(bulletPrefab, bullet));
    }
}
