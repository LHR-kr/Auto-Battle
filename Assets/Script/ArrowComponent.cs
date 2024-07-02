using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowComponent : MonoBehaviour
{

    private GameObject owner;
    private float attackDamage;
    [SerializeField] private float speed = 10.0f;
    private Rigidbody2D rigid2D; 
    
    public float AttackDamage
    {
        set
        {
            attackDamage = value;
        }
        
    }

    public GameObject Owner
    {
        set
        {
            owner = value;
        }
        
    }


    private void Start()
    {
        StartCoroutine(DestroyGameObject());
        rigid2D = GetComponent<Rigidbody2D>();
    }

  private void FixedUpdate()
    {
        Vector2 velocityVector = transform.TransformDirection(Vector3.right) * Time.fixedDeltaTime * speed;
        rigid2D.MovePosition(rigid2D.position + velocityVector);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //공격 유효성 검사
        CharacterComponent hitCharacter = other.GetComponent<CharacterComponent>();
        if (!hitCharacter)
            return;
        if (hitCharacter.gameObject == owner)
            return;
        if (hitCharacter.Team == owner.GetComponent<CharacterComponent>().Team)
            return;
        hitCharacter.hpComponent.TakeDamage(attackDamage);
        Destroy(this.gameObject);
    }

    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}

