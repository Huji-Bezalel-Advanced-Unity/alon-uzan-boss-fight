using System.Collections;
using _Alon.Scripts.Core.Managers;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class BossController : MonoBehaviour
    {
        private BasePlayerController _playerToAttack = null;
        private bool _isAttacking = false;
        private float _maxBossLife = 1000;
        private float _bossLife = 1000f;

        private BossAnimator _bossAnimator;

        private void Start()
        {
            _bossAnimator = GameManager.Instance.BossAnimator; // inject Boss Animator
            _bossAnimator.SetAnimation(gameObject, "idle", true);
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
                _bossAnimator.SetAnimation(gameObject, "idle", true);
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
            _bossAnimator.SetAnimation(gameObject, "attack", false);
            GameManager.Instance.DealDamage(_playerToAttack);
        }

        public void TakeDamage(float damage)
        {
            _bossLife = Mathf.Max(0, _bossLife - damage);
            StartCoroutine(UIManager.Instance.UpdateBossLifeBar(_bossLife));
            print(_bossLife);
        }
    }
}