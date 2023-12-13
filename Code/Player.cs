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
        rigid = GetComponent<Rigidbody2D>(); //오프젝트에서 컴포넌트를 가져오는 함수
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


    void Update() //프레임마다 호출
    {
        if(!GameManager.instance.isLive)
        {
            return;
        }
       // inputVec.x = Input.GetAxisRaw("Horizontal");  //-1~1
       // inputVec.y = Input.GetAxisRaw("Vertical");    //GetAxis 는 보정 들어감
    }
    
    /*
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>(); 
    }
    */


    void FixedUpdate()  //물리연산 프레임마다 호출되는 생명주기 함수
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // normalized 벡타값의 크기가 1이 되도록 좌표가 수정된 값
                                                                             // fixedDeltaTime 물리프레임 하나가 소모한 시간
        // 1. 힘을 준다
        //rigid.AddForce(inputVec);

        // 2. 속도 제어
        //rigid.velocity = inputVec;

        // 3. 위치 이동
        rigid.MovePosition(rigid.position + nextVec);


    }

    void LateUpdate()  //프레임이 종료되기 전 실행되는 생명주기 함수
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        anim.SetFloat("Speed", inputVec.magnitude); //magnitude 크기

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
            for(int i=2; i <transform.childCount; i++)  //childCount 자식 오브젝트의 개수
            {
                transform.GetChild(i).gameObject.SetActive(false); //주어진 인덱스의 자식 오브젝트를 반환하는 함수
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
