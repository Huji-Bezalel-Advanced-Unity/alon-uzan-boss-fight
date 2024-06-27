using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SampleMaster : MonoBehaviour
{
    public GameObject[] EnemiesArray;
    public int animationCounter;
    public int totalEnemies;

    void Start()
    {
        totalEnemies = EnemiesArray.Count() - 1;
        animationCounter = 1;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextAnimation() 
    {
        
       
            switch (animationCounter)
            {
                case 0:
                    foreach (GameObject enemy in EnemiesArray)
                    {
                        Enemy enemyScript = enemy.GetComponent<Enemy>();
                        enemyScript.ChangeAnimationState(enemyScript.PLAYER_IDLE, false, false);
                    }
                    break;
                case 1:
                    foreach (GameObject enemy in EnemiesArray)
                    {
                        Enemy enemyScript = enemy.GetComponent<Enemy>();
                        enemyScript.ChangeAnimationState(enemyScript.PLAYER_ATTACK, false, false);
                    Debug.Log("1");
                    }
                    break;
                case 2:
                    foreach (GameObject enemy in EnemiesArray)
                    {
                        Enemy enemyScript = enemy.GetComponent<Enemy>();
                        enemyScript.ChangeAnimationState(enemyScript.PLAYER_WALK, false, false);

                    }
                    break;
                case 3:
                    foreach (GameObject enemy in EnemiesArray)
                    {
                        Enemy enemyScript = enemy.GetComponent<Enemy>();
                        enemyScript.ChangeAnimationState(enemyScript.PLAYER_RUN, false, false);

                    }
                    break;
                case 4:
                    foreach (GameObject enemy in EnemiesArray)
                    {
                        Enemy enemyScript = enemy.GetComponent<Enemy>();
                        enemyScript.ChangeAnimationState(enemyScript.PLAYER_DEATH, false, false);

                    }
                    break;
                case 5:
                    foreach (GameObject enemy in EnemiesArray)
                    {
                        Enemy enemyScript = enemy.GetComponent<Enemy>();
                        enemyScript.ChangeAnimationState(enemyScript.PLAYER_FLY, false, false);

                    }
                    break;
                case 6:
                    foreach (GameObject enemy in EnemiesArray)
                    {
                        Enemy enemyScript = enemy.GetComponent<Enemy>();
                        enemyScript.ChangeAnimationState(enemyScript.PLAYER_FLY_ATTACK, false, false);
                        
                    }
                    break;


                default:
                    break;
            }
        animationCounter++;
        if (animationCounter == 7) { animationCounter = 0; }

    }

}
