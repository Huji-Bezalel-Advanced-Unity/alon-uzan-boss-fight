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
        
        private void Awake()
        {
            _boss = GameManager.Instance.Boss;
        }

        private void Update()
        {
            TryAttack();
            if (playerToAttack != null)
            {
                Attack();
                _isAttacking = true;
            }
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