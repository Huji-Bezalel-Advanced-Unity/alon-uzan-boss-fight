using System;
using System.Collections.Generic;
using _Alon.Scripts.Gameplay.Controllers;
using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    public class GameManager
    {
        private readonly Action<bool> _onComplete;
        private HashSet<BasePlayerController> _players;
        private static float MinDistanceToAttack = 1.2f;
        private GameObject currentPlayerToSpawn;
        private Dictionary<string, GameObject> playersPrefabs = new Dictionary<string, GameObject>
        {
            {"IronGuardian", Resources.Load<GameObject>("IronGuardian")},
            {"RamSmasher", Resources.Load<GameObject>("RamSmasher")},
            {"SwiftBlade", Resources.Load<GameObject>("SwiftBlade")}
        };

        public static GameManager Instance { get; private set; }
        public GameObject Boss { get; private set; }
        public PlayerAnimator PlayerAnimator { get; private set; }

        public bool IsBossAlive = true;

        public GameManager(Action<bool> onComplete)
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogException(new Exception("GameManager already exists"));
                return;
            }

            this._players = new HashSet<BasePlayerController>();
            _onComplete = onComplete;
            PlayerAnimator = new PlayerAnimator();
            OnLoadSuccess();
        }

        private void OnLoadSuccess()
        {
            _onComplete?.Invoke(true);
        }

        public void OnLoadFail()
        {
            _onComplete?.Invoke(false);
        }

        public void SetBoss(GameObject boss)
        {
            Boss = boss;
        }

        public void AddPlayer(BasePlayerController player)
        {
            Debug.Log("new player added");
            _players.Add(player);
        }

        public void RemovePlayer(BasePlayerController player)
        {
            _players.Remove(player);
        }

        public BasePlayerController GetNearestPlayer()
        {
            foreach (var player in _players)
            {
                float distance = Vector3.Distance(player.transform.position, Boss.transform.position);
                if (distance <= MinDistanceToAttack)
                {
                    return player;
                }
            }
            return null;
        }

        public void DealDamage(BasePlayerController playerToAttack)
        {
            playerToAttack.TakeDamage();
        }

        public void DealBossDamage(float baseDamageToGive)
        {
            if (Boss == null)
            {
                Debug.LogError("Boss is not set in GameManager.");
                return;
            }

            var bossController = Boss.GetComponent<BossController>();
            if (bossController == null)
            {
                Debug.LogError("BossController component not found on Boss.");
                return;
            }

            bossController.TakeDamage(baseDamageToGive);
        }
        
        public void SetPlayerToSpawn(string player)
        {
            currentPlayerToSpawn = playersPrefabs[player];
        }

        public GameObject GetPlayerToSpawn()
        {
            return currentPlayerToSpawn;
        }
    }
}
