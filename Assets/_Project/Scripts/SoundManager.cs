using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource shootAudioSource;
    [SerializeField] private AudioSource deathAudioSource;
    private void OnEnable()
    {
        PlayerController.PlayerShoot += PlayerControllerOnPlayerShoot;
        Enemy.OnEnemyDeathNoArgs += EnemyOnOnEnemyDeathNoArgs;
    }

    private void OnDisable()
    {
        PlayerController.PlayerShoot -= PlayerControllerOnPlayerShoot;
        Enemy.OnEnemyDeathNoArgs -= EnemyOnOnEnemyDeathNoArgs;
    }

    private void EnemyOnOnEnemyDeathNoArgs()
    {
        deathAudioSource.Play();
    }

    private void PlayerControllerOnPlayerShoot()
    {
        shootAudioSource.Play();
    }
}
