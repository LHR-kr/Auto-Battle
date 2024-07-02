using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPComponent : MonoBehaviour, IGameRestartEventListener, IGameTickEndEventListener
{
    private float hp;
    

    [SerializeField] protected float maxHP;
    public float HP
    {
        get
        {
            return hp;
        }
    }

    public float MaxHP
    {
        get
        {
            return maxHP;
        }
    }
    
   
    private void Start()
    {
        hp = maxHP;
        GameManager.SendGameRestartEvent += HandleGameRestartEvent;
    }
    private void OnEnable()
    {
        GameManager.SendGameTickEndEvent += HandleGameTickEndEvent;
    }

    private void OnDisable()
    {
        GameManager.SendGameTickEndEvent -= HandleGameTickEndEvent;
    }
    public void TakeDamage(float Damage)
    {
        hp = Mathf.Max(0, hp - Damage);
    }
    
 

    public void HandleGameRestartEvent()
    {
        hp = maxHP;
    }
    public void HandleGameTickEndEvent()
    {
        if (hp <= 0)
            this.gameObject.SetActive(false);
    }
    
}
