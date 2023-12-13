using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // ��������� ������ ����
    public GameObject[] prefabs;

    // Ǯ ����� �� ����Ʈ��
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

        // ������ Ǯ�� ��� �ִ� ���ӿ�����Ʈ ����


        foreach (GameObject item in pools[index]) // �迭, ����Ʈ �����͸� ���������� �����ϴ� �ݺ���
        {
            if(!item.activeSelf) {  
                // �߰��ϸ� select������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }

        }


            
        // ���࿡ ��� ������̸�

        if (select == null) {
            // ���Ӱ� �����ϰ� select������ �Ҵ�
            select = Instantiate(prefabs[index], transform); //���� ������Ʈ�� �����Ͽ� ��鿡 �����ϴ� �Լ�
            pools[index].Add(select);

        }

        return select;
    }


}
