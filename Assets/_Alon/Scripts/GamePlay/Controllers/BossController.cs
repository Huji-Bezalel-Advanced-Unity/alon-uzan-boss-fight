using System;
using System.Collections;
using _Alon.Scripts.Core.Managers;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class BossController : MonoBehaviour
    {
        private BasePlayerController _playerToAttack = null;

        private bool _isAttacking = false;

        private bool _isTherePlayer = false;

        private bool _isWasPlayer = false;

        private float _maxBossLife = 1000;
        
        private float _bossLife = 1000f;

        [SerializeField] private Image bossLifeBar;

        private void Start()
        {
            bossLifeBar.fillAmount = 100;
        }

        private void Update()
        {
            TryAttack();
            if (!_isAttacking)
            {
                HandleAttack();
                StartCoroutine(DelayAttack());
            }
        }

        private IEnumerator DelayAttack()
        {
            _isAttacking = true;
            yield return new WaitForSeconds(2f);
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
        }

        private void Attack()
        {
            GameManager.Instance.SetBossAnimation(gameObject, "attack", false);
            GameManager.Instance.DealDamage(_playerToAttack);
        }
        
        public void TakeDamage(float damage)
        {
            // take damage
            _bossLife = Mathf.Max(0, _bossLife - damage);
            bossLifeBar.fillAmount = _bossLife / _maxBossLife;
            print(_bossLife);
        }
    }
}