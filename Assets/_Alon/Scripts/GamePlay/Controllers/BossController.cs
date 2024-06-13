using System;
using System.Collections;
using _Alon.Scripts.Core.Managers;
using Spine.Unity;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class BossController : MonoBehaviour
    {
        private GameObject _playerToAttack = null;

        private bool _isAttacking = false;

        private bool _isTherePlayer = false;

        private bool _isWasPlayer = false;

        private float _bossLife = 100f;

        private void OnPlayerDeath()
        {
            Debug.Log("Change Player.");
        }

        private void Update()
        {
            TryAttack();
            if (!_isAttacking)
            {
                StartCoroutine(DelayAttack());
                HandleAttack();
            }
        }

        private IEnumerator DelayAttack()
        {
            _isAttacking = true;
            yield return new WaitForSeconds(2);
            _isAttacking = false;
        }

        private void HandleAttack()
        {
            if (_playerToAttack == null)
            {
                GameManager.Instance.SetBossAnimation(gameObject, "idle", true);
                return;
            }
            
            Attack();
            _isAttacking = true;
        }

        private void TryAttack()
        {
            _playerToAttack = GameManager.Instance.GetNearestPlayer();
            if (_playerToAttack != null)
            {
                Debug.Log("attack!");
            }
        }

        private void Attack()
        {
            GameManager.Instance.SetBossAnimation(gameObject, "attack", false);
            GameManager.Instance.DealDamage(_playerToAttack);
        }
    }
}