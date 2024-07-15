using System;
using System.Collections.Generic;
using _Alon.Scripts.Gameplay.Controllers;
using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    public class GameManager
    {
        /// <summary>
        /// Private Fields
        /// </summary>
        private readonly Action<bool> _onComplete;

        private readonly HashSet<BasePlayerController> _players;
        private readonly HashSet<BaseEnemyController> _enemies;
        
        private const float MinDistanceToAttack = 1.2f;
        private GameObject _currentPlayerToSpawn;

        private readonly PlayerFactory _playerFactory = new PlayerFactory();

        /// <summary>
        /// Public Fields
        /// </summary>
        public static GameManager Instance { get; private set; }

        public Dictionary<string, int> _damagesDict;
        public event Action OnAllEnemiesCleared;
        public event Action OnEnemyPosChanged;
        public GameObject Boss { get; private set; }

        // End Of Local Variables


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
            this._enemies = new HashSet<BaseEnemyController>();
            _onComplete = onComplete;
            _damagesDict = new Dictionary<string, int>
            {
                { "IronGuardian", 20 },
                { "RamSmasher", 30 },
                { "SwiftBlade", 40 }
            };
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
            _players.Add(player);
        }

        public void RemovePlayer(BasePlayerController player)
        {
            _players.Remove(player);
        }

        public BasePlayerController GetNearestPlayerToBoss()
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

        public GameObject GetNearestEnemy(GameObject basePlayer)
        {
            float currentDistance = 1000f;
            BaseEnemyController nearestEnemy = null;
            foreach (var enemy in _enemies)
            {
                float distance = Vector3.Distance(enemy.transform.position, basePlayer.transform.position);
                if (distance < currentDistance)
                {
                    currentDistance = distance;
                    nearestEnemy = enemy;
                }
            }

            return nearestEnemy ? nearestEnemy.gameObject : null;
        }

        public void DealDamage(BasePlayerController playerToAttack)
        {
            playerToAttack.TakeDamage(_damagesDict[playerToAttack.gameObject.name.Replace("(Clone)", "")]);
        }

        public void DealEnemyDamage(float baseDamageToGive, GameObject enemy)
        {
            if (enemy == null)
            {
                Debug.LogError("Boss is not set in GameManager.");
                return;
            }

            var bossController = enemy.GetComponent<BaseEnemyController>();
            if (bossController == null)
            {
                Debug.LogError("BossController component not found on Boss.");
                return;
            }

            bossController.TakeDamage(baseDamageToGive);
        }

        public void SetPlayerToSpawn(string player)
        {
            _currentPlayerToSpawn = _playerFactory.CreatePlayer(player);
        }

        public GameObject GetPlayerToSpawn()
        {
            return _currentPlayerToSpawn;
        }


        public void AddEnemy(GameObject enemyPrefab)
        {
            foreach (Transform child in enemyPrefab.transform)
            {
                _enemies.Add(child.gameObject.GetComponent<BaseEnemyController>());
            }
        }

        public BasePlayerController GetNearestPlayerToEnemy(GameObject gameObject)
        {
            float currentDistance = 1000f;
            BasePlayerController nearestPlayer = null;
            foreach (var player in _players)
            {
                float distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
                if (distance < currentDistance)
                {
                    currentDistance = distance;
                    nearestPlayer = player;
                }
            }

            return nearestPlayer;
        }

        public void RemoveEnemy(BaseEnemyController baseEnemyController)
        {
            _enemies.Remove(baseEnemyController);
            if (_enemies.Count == 0)
            {
                Debug.Log("All enemies cleared");
                UIManager.Instance.StartBossPhase();
                OnAllEnemiesCleared?.Invoke();
            }
            else
            {
                OnEnemyPosChanged?.Invoke();
            }
        }

        public void KillAllEnemies()
        {
            foreach (var enemy in _enemies)
            {
                enemy.TakeDamage(1000);
            }
        }

        public void InvokeOnEnemyPosChanged()
        {
            OnEnemyPosChanged?.Invoke();
        }
    }
}