using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); //������Ʈ���� ������Ʈ�� �������� �Լ�
        speed = 3;
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);

    }

    void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }


    void Update() //�����Ӹ��� ȣ��
    {
        if(!GameManager.instance.isLive)
        {
            return;
        }
       // inputVec.x = Input.GetAxisRaw("Horizontal");  //-1~1
       // inputVec.y = Input.GetAxisRaw("Vertical");    //GetAxis �� ���� ��
    }
    
    /*
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>(); 
    }
    */


    void FixedUpdate()  //�������� �����Ӹ��� ȣ��Ǵ� �����ֱ� �Լ�
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // normalized ��Ÿ���� ũ�Ⱑ 1�� �ǵ��� ��ǥ�� ������ ��
                                                                             // fixedDeltaTime ���������� �ϳ��� �Ҹ��� �ð�
        // 1. ���� �ش�
        //rigid.AddForce(inputVec);

        // 2. �ӵ� ����
        //rigid.velocity = inputVec;

        // 3. ��ġ �̵�
        rigid.MovePosition(rigid.position + nextVec);


    }

    void LateUpdate()  //�������� ����Ǳ� �� ����Ǵ� �����ֱ� �Լ�
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        anim.SetFloat("Speed", inputVec.magnitude); //magnitude ũ��

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime*10;

        if (GameManager.instance.health < 0)
        {
            for(int i=2; i <transform.childCount; i++)  //childCount �ڽ� ������Ʈ�� ����
            {
                transform.GetChild(i).gameObject.SetActive(false); //�־��� �ε����� �ڽ� ������Ʈ�� ��ȯ�ϴ� �Լ�
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
}
