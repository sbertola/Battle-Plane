using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject {

    public int enemyDmg = 1;
    public float playerAcc = 1.0f;
    public float playerDodge = 0.1f;
    public float restartLevelDelay = 1f;
    public int actions = 1;//used to con't number of remaining actions
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip shootSound;
    public AudioClip hitSound;
    public AudioClip gameOver;
    public Text hpText;
    public Text actionTxt;


    private Animator animator;
    private int hp = 3;
	// Use this for initialization
	protected override void Start () {
        animator = GetComponent<Animator>();
        hpText.text = "HP: " + hp;
        actionTxt.text = "Actions: " + actions;
        base.Start();
	}

    // Update is called once per frame
    void Update ()
    {
        if (!GameManager.instance.playerTurn)
        {
            //actions = 1;
            return;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Enemy hitEnemy = GameManager.instance.e1;

            animator.SetTrigger("Player_Fire");

            hitEnemy.LoseHP(enemyDmg);

            SoundManager.instance.PlaySingle(shootSound);
        }

        int hor = 0;
        int vert = 0;

        hor = (int)Input.GetAxisRaw("Horizontal");
        vert = (int)Input.GetAxisRaw("Vertical");

        if (hor != 0)
            vert = 0;

        if (hor != 0 || vert != 0)
        {
            AttemptMove<Player>(hor, vert);
          //  actions--;
        }
            
        
        //GameManager.instance.playerTurn = false;
        //actions = 3;
    }

    protected override void OnCantMove<T>(T component)
    {///////incomplete function
        //FireWeapon(component);
         Enemy hitEnemy = component as Enemy;

         animator.SetTrigger("Player_Fire");

         hitEnemy.LoseHP(enemyDmg);

         SoundManager.instance.PlaySingle(shootSound);
    }

    public void Completed()
    {/////////make load end scene card
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseHP(int loss)
    {
        animator.SetTrigger("Player_Hit");
        hp -= loss;
        SoundManager.instance.PlaySingle(hitSound);
        hpText.text = "HP: " + hp;
        CheckGameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Completed", restartLevelDelay);
            enabled = false;
        }
        else if(other.tag == "Terrain")/////add a montain tag and a forest tag instead
        {
            //affect accuracy and dodge
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);
        

        RaycastHit2D hit;
        if(Move(xDir,yDir,out hit))
        {
            SoundManager.instance.RandomizeSFX(moveSound1, moveSound2);
        }

        actionTxt.text = "Actions: " + actions;
      //  if (actions == 0)
            GameManager.instance.playerTurn = false;

        //actions--;
        
        //actionText.text = "Remaining actions: " +actions;
        
    }

    private void CheckGameOver()
    {
        if (hp <= 0)
        {
            SoundManager.instance.PlaySingle(gameOver);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }

    public void FireWeapon<T>(T component)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
             Enemy hitEnemy = component as Enemy;

             animator.SetTrigger("Player_Fire");

             hitEnemy.LoseHP(enemyDmg);

             SoundManager.instance.PlaySingle(shootSound);
        }
    }
}
