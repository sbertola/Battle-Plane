using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerHP = 3;
    public float turnDelay = .1f;
    public float lvlStartDelay = 2f;
    [HideInInspector] public bool playerTurn = true;

    private Text lvlTxt;
    private GameObject lvlImg;
    private int level = 1;
    private List<Enemy> enemy;
    public Enemy e1;
    private bool enemyMoving;
    private bool doingSetup;
	// Use this for initialization
	void Awake ()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        enemy = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
	}

    private void OnLevelWasLoaded(int index)
    {
        level++;

        InitGame();
    }

    void InitGame()
    {
        doingSetup = true;

        lvlImg = GameObject.Find("StoryImage");
        lvlTxt = GameObject.Find("StoryText").GetComponent<Text>();
        lvlImg.SetActive(true);
        Invoke("HideLvlImg",lvlStartDelay);

        enemy.Clear();
        boardScript.SetupScene(level);
    }

    private void HideLvlImg()
    {
        lvlImg.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        lvlTxt.text = "You have fallen";
        lvlImg.SetActive(true);
        enabled = false;
    }

    public void Victory()
    {
        lvlTxt.text = "Your soul has blackened...";
        lvlImg.SetActive(true);
        enabled = false;
    }

    IEnumerator MoveEnemy()
    {
        enemyMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if(enemy.Count == 0)
            yield return new WaitForSeconds(turnDelay);

        for(int i = 0; i<enemy.Count;i++)
        {
            enemy[i].MoveEnemy();
            yield return new WaitForSeconds(enemy[i].moveTime);
        }
        playerTurn = true;
        enemyMoving = false;
    }
	// Update is called once per frame
	void Update ()
    {
        if (playerTurn || enemyMoving || doingSetup)
            return;
        StartCoroutine(MoveEnemy());
	}

    public void AddEnemyToList(Enemy script)
    {
        enemy.Add(script);
    }
}
