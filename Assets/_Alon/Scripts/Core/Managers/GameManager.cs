using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private BossAnimator _bossAnimator;

        private PlayerAnimator _playerAnimator;
        
        private HashSet<BasePlayerController> _players;
        
        private static float MinDistanceToAttack = 1f;
        
        
        /// <summary>
        /// Public Fields
        /// </summary>
        public static GameManager Instance { get; private set; }

        
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

            this._bossAnimator = new BossAnimator();
            this._playerAnimator = new PlayerAnimator();
            
            this._players = new HashSet<BasePlayerController>();
            
            _onComplete = onComplete;
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
        
        public void SetBossAnimation(GameObject boss, string animationName, bool loop)
        {
            _bossAnimator.SetAnimation(boss, animationName, loop);
        }
        
        public void SetPlayerAnimation(GameObject player, string animationName, bool loop)
        {
            _playerAnimator.SetAnimation(player, animationName, loop);
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
            Boss.GetComponent<BossController>().TakeDamage(baseDamageToGive);
        }
    }
}