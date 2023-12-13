using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리펩들을 보관할 변수
    public GameObject[] prefabs;

    // 풀 담당을 할 리스트들
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject> ();
        }

        Debug.Log(pools.Length);
        
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // 선택한 풀의 놀고 있는 게임오브젝트 접근


        foreach (GameObject item in pools[index]) // 배열, 리스트 데이터를 순차적으로 접근하는 반복문
        {
            if(!item.activeSelf) {  
                // 발견하면 select변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }

        }


            
        // 만약에 모두 사용중이면

        if (select == null) {
            // 새롭게 생성하고 select변수에 할당
            select = Instantiate(prefabs[index], transform); //원본 오브젝트를 복제하여 장면에 생성하는 함수
            pools[index].Add(select);

        }

        return select;
    }


}
