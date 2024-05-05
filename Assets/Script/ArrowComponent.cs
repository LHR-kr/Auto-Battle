using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowComponent : MonoBehaviour
{

    private GameObject owner;
    private float attackDamage;
    [SerializeField] private float speed = 1000.0f;

    
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
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (transform.TransformDirection(Vector3.right)* speed * Time.deltaTime);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("arrow hit");
        //공격 유효성 검사
        CharacterComponent hitCharacter = other.GetComponent<CharacterComponent>();
        if (!hitCharacter)
            return;
        if (hitCharacter.gameObject == owner)
            return;
        if (hitCharacter.Team == owner.GetComponent<CharacterComponent>().Team)
            return;
        hitCharacter.TakeDamage(attackDamage);
        Destroy(this.gameObject);
    }

    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(this.gameObject);
    }
    
}
