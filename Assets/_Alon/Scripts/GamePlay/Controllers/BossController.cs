using System;
using _Alon.Scripts.Core.Managers;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class BossController : MonoBehaviour
    {
        private GameObject _boss;
        
        private GameObject playerToAttack = null;
        
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
            }
        }

        private void TryAttack()
        {
            playerToAttack = GameManager.Instance.GetNearestPlayer();
        }

        private void Attack()
        {
            GameManager.Instance.SetBossAnimation(gameObject, "attack", true);
        }
    }
}