using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int score;
    private List<ScoreLookUp> ScoreLookupList = new List<ScoreLookUp>()
    {
        new ScoreLookUp()
        {
            lowerLimit = 0,
            upperLimit = 0.1f,
            score = 1000
        },
        new ScoreLookUp()
        {
            lowerLimit = 0.1f,
            upperLimit = 0.5f,
            score = 800
        },
        new ScoreLookUp()
        {
            lowerLimit = 0.5f,
            upperLimit = 1f,
            score = 500
        },
        new ScoreLookUp()
        {
            lowerLimit = 1f,
            upperLimit = 2f,
            score = 200
        },
    };
    private void OnEnable()
    {
        Enemy.OnEnemyDeath += EnemyOnOnEnemyDeath;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= EnemyOnOnEnemyDeath;
    }

    private void EnemyOnOnEnemyDeath(float obj)
    {
        for (int i = 0; i < ScoreLookupList.Count; i++)
        {
            if (obj > ScoreLookupList[i].lowerLimit && obj <= ScoreLookupList[i].upperLimit)
            {
                score += ScoreLookupList[i].score;
                scoreText.SetText("" + score);
                break;
            }
        }
    }
}

public class ScoreLookUp
{
    public float lowerLimit;
    public float upperLimit;
    public int score;
}
