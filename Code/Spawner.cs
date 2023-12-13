using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;
    //public GameObject enemyCleaner;
    public static Spawner instance;
    public float timer;
    int level;
    int a = 0;

    void Awake()
    {
        instance = this;
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }

    

    void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1); //소숫점 아래 버림

        //if (level == 3 && timer > spawnData[level - 1].spawnTime)
        //{
        //    //enemyCleaner.SetActive(true);
        //    timer = 0;
        //    level = 2;
        //    Spawn();
        //}
        if (timer > spawnData[level].spawnTime && a == 0 && level <= 2)
        {
            
            

            timer = 0;
            
            Spawn();
           

        }
        if (GameManager.instance.boss == 1 && a == 0)
        {
            a = 1;

        }
        if (a==1&& timer > spawnData[level].spawnTime)
        {
            spawnData[level].spawnTime = 10000;
            Spawn();
        }
        
        
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }


}


[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public float spawnTime;
    public int health;
    public float speed;
}