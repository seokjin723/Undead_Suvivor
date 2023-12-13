using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //static으로 선언된 변수는 inspector에 나타나지 않는다.
    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10;
    public int boss=0;
    [Header("# Player Info")]
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };
    [Header("# GameObject")]
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public Transform uiJoy;
    public GameObject enemyCleaner;

    void Awake()
    {
        instance = this;  //Awake 생명주기에서 인스턴스 변수를 자기자신 this로 초기화
        Application.targetFrameRate = 60;
        
    }

    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;
        //isLive = true;

        player.gameObject.SetActive(true);
        
        uiLevelUp.Select(playerId % 2);
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }



    public void GameVictory()
    { 

        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        //enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }


    public void GameOver()
    {

        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    void Update()
    {
        if (!isLive)
        {
            return;
        }
        gameTime += Time.deltaTime;

        if (gameTime >= maxGameTime)
        {
            //gameTime = 0;
            gameTime = maxGameTime;
            boss = 1;
            //GameVictory();

        }


    }

    public void GetExp()
    {
        if (!isLive)
            return;

        exp++;
        
        if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0; //유니티의 시간속도
        uiJoy.localScale = Vector3.zero;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1; //유니티의 시간속도
        uiJoy.localScale = Vector3.one;
    }

}


