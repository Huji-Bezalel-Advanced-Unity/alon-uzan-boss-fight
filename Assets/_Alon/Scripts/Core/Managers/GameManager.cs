using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Alon.Scripts.Gameplay.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Alon.Scripts.Core.Managers
{
    public class GameManager
    {
        /// <summary>
        /// Constants
        /// </summary>
        private const float MinDistanceToAttack = 1.2f;

        /// <summary>
        /// Private Fields
        /// </summary>
        private HashSet<BasePlayerController> _players;
        private HashSet<BaseEnemyController> _enemies;
        private GameObject _currentPlayerToSpawn;
        private readonly PlayerFactory _playerFactory = new PlayerFactory();

        /// <summary>
        /// Public Fields
        /// </summary>
        public static GameManager Instance { get; private set; }
        public Dictionary<string, int> DamagesDict;
        public GameObject Boss { get; private set; }

        /// <summary>
        /// Events
        /// </summary>
        private Action<bool> _onComplete;
        public event Action OnAllEnemiesCleared;
        public event Action OnEnemyPosChanged;

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

            Init(onComplete);
        }

        private void Init(Action<bool> onComplete)
        {
            this._players = new HashSet<BasePlayerController>();
            this._enemies = new HashSet<BaseEnemyController>();
            _onComplete = onComplete;
            DamagesDict = new Dictionary<string, int>
            {
                { "IronGuardian", 10 },
                { "RamSmasher", 15 },
                { "SwiftBlade", 20 }
            };
            AudioManager.Instance.PlayAudioClip(2);
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
            if (_players.Count != 0) return;
            AudioManager.Instance.PlayAudioClip(5);
            SceneManager.LoadScene("Deafet");
        }

        public BasePlayerController GetNearestPlayerToBoss()
        {
            // return the closest player to the boss using LINQ
            return (from player in _players
                let distance = Vector3.Distance(player.transform.position,
                    Boss.transform.position)
                where distance <= MinDistanceToAttack
                select player).FirstOrDefault();
        }

        public GameObject GetNearestEnemy(GameObject basePlayer)
        {
            var currentDistance = 1000f;
            BaseEnemyController nearestEnemy = null;
            foreach (var enemy in _enemies)
            {
                var distance = Vector3.Distance(enemy.transform.position, basePlayer.transform.position);
                if (!(distance < currentDistance)) continue;
                currentDistance = distance;
                nearestEnemy = enemy;
            }

            return nearestEnemy ? nearestEnemy.gameObject : null;
        }

        public void DealDamage(BasePlayerController playerToAttack)
        {
            playerToAttack.TakeDamage(DamagesDict[playerToAttack.gameObject.name.Replace("(Clone)", "")]);
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
            var currentDistance = 1000f;
            BasePlayerController nearestPlayer = null;
            foreach (var player in _players)
            {
                var distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
                if (!(distance < currentDistance)) continue;
                currentDistance = distance;
                nearestPlayer = player;
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

        public void UpgradePlayer(string playerName)
        {
            DamagesDict[playerName] *= 5;
        }
    }
}