using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool islive;

    Collider2D coll;
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    
        spriter = GetComponent<SpriteRenderer>();  
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
        coll = GetComponent<Collider2D>();
    }

   
    void FixedUpdate()
    {
        if (!islive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") )
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!islive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        islive = true;
        health = maxHealth;
        
        coll.enabled = true; //�浹 Ȱ��ȭ
        rigid.simulated = true; // ���� Ȱ��ȭ
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !islive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack()); //���� ���¸� �������� �Լ�

        if(health > 0)
        {
            anim.SetTrigger("Hit");

            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }

        else
        {
            islive = false;
            coll.enabled = false; //�浹 ��Ȱ��ȭ
            rigid.simulated = false; // ���� ��Ȱ��ȭ
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            
            if (GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);

            if (GameManager.instance.boss == 1 && Spawner.instance.timer > 10)
            {
                
                
                GameManager.instance.GameVictory();
            }

            //Dead();  �ִϸ��̼��� ��������ٰž�
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait; //�ϳ��� ���� ������ ������

        //yield return null; // 1������ ����
        //yield return new WaitForSeconds(2f); //2�� ����

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized*3, ForceMode2D.Impulse);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
