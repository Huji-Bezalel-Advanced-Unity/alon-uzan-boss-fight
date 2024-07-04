using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProjectMaster : MonoBehaviour
{
    public GameObject[] EnemiesArray;
    public int CurrentEnemy;
    public int totalEnemies;
    private BossAnimator enemyScript;
    public Button flyButton;
    public Button flyAttackButton;

    private void Start()
    {
        CurrentEnemy = 0;
        totalEnemies = EnemiesArray.Count()-1;
        enemyScript = EnemiesArray[CurrentEnemy].GetComponent<BossAnimator>();
    }

    private void Update()
    {
        
    }

    public void NextEnemy()     
    {
        enemyScript.walk = false;
        enemyScript.run = false;
        EnemiesArray[CurrentEnemy].SetActive(false);

        if (CurrentEnemy < totalEnemies) {CurrentEnemy++;}
        else {CurrentEnemy = 0;}
        SetPosition();
        EnemiesArray[CurrentEnemy].SetActive(true);
        enemyScript = EnemiesArray[CurrentEnemy].GetComponent<BossAnimator>();
        CheckButtons();
    }

    public void PrevEnemy()
    {
        enemyScript.walk = false;
        enemyScript.run = false;
        EnemiesArray[CurrentEnemy].SetActive(false);

        if (CurrentEnemy > 0){CurrentEnemy--;}
        else {CurrentEnemy = totalEnemies;}
        SetPosition();
        EnemiesArray[CurrentEnemy].SetActive(true);
        enemyScript = EnemiesArray[CurrentEnemy].GetComponent<BossAnimator>();
        CheckButtons();
    }

    public void SetPosition() 
    {
        EnemiesArray[CurrentEnemy].transform.position = new Vector3(-0.44f, -3.47f, 0);
    }

    public void CheckButtons() 
    {
        if (enemyScript.isFlying == false)
        {
            flyButton.interactable = false;
            flyAttackButton.interactable = false;
        }
        else
        {
            flyButton.interactable = true;
            flyAttackButton.interactable = true;
        }
    }


    public void SetIdle()
    {
        string setAnimation = BossAnimations.PLAYER_IDLE.ToString();
        enemyScript.ChangeAnimationState(setAnimation, false, false);

    }
    public void SetAttack()
    {
        string setAnimation = BossAnimations.PLAYER_ATTACK.ToString();
        enemyScript.ChangeAnimationState(setAnimation, false, false);

    }
    public void SetWalk()
    {
        string setAnimation = BossAnimations.PLAYER_WALK.ToString();
        enemyScript.ChangeAnimationState(setAnimation, true, false);

    }
    public void SetRun() 
    {
        string setAnimation = BossAnimations.PLAYER_RUN.ToString(); 
        enemyScript.ChangeAnimationState(setAnimation, false, true);

    }
    public void SetDeath()
    {
        string setAnimation = BossAnimations.PLAYER_DEATH.ToString();
        enemyScript.ChangeAnimationState(setAnimation, false, false);

    }
    public void SetFly()
    {
        string setAnimation = BossAnimations.PLAYER_FLY.ToString();
        enemyScript.ChangeAnimationState(setAnimation, true, false);

    }
    public void SetFlyAttack()
    {
        string setAnimation = BossAnimations.PLAYER_FLY_ATTACK.ToString();
        enemyScript.ChangeAnimationState(setAnimation, false, false);

    }
}
