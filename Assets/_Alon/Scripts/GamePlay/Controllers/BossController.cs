using System;
using _Alon.Scripts.Core.Managers;
using Spine.Unity;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class BossController : MonoBehaviour
    {
        private GameObject _boss;
        
        private GameObject playerToAttack = null;

        private bool _isAttacking = false;
        
        private PlayerController _playerController;
        
        private bool _isTherePlayer = false;
        
        private bool _isWasPlayer = false;
        
        private void Awake()
        {
            GameManager.Instance.OnPlayerDeath += OnPlayerDeath;
            _boss = GameManager.Instance.Boss;
        }

        private void OnPlayerDeath()
        {
            Debug.Log("Change Player.");
            TryAttack();
        }

        private void Update()
        {
            HandleAttack();
        }

        private void HandleAttack()
        {
            if (playerToAttack == null) return;
            Attack();
            _isAttacking = true;
        }

        private void TryAttack()
        {
            playerToAttack = GameManager.Instance.GetNearestPlayer();
        }

        private void Attack()
        {
            if (!_isAttacking)
            {
                GameManager.Instance.SetBossAnimation(gameObject, "attack", true);
            }
        }
    }
}