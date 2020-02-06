using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

    public int playerDmg;
    public int hp = 3;
    public AudioClip enemyAtk;
    public AudioClip enemyHit;
    private Animator animator;
    private Transform target;

	protected override void Start ()
    {
        GameManager.instance.AddEnemyToList(this);
        GameManager.instance.e1 = this;
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPLayer = component as Player;

        animator.SetTrigger("EnemyAtk");

        hitPLayer.LoseHP(playerDmg);

        SoundManager.instance.PlaySingle(enemyAtk);
    }

    public void LoseHP(int loss)
    {
        animator.SetTrigger("EnemyHit");
        hp -= loss;
        if (hp <= 0)
            gameObject.SetActive(false);
        CheckVictory();
    }

    private void CheckVictory()
    {
        if (hp <= 0)
        {
            //SoundManager.instance.PlaySingle(gameOver);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.Victory();
        }
    }
}
