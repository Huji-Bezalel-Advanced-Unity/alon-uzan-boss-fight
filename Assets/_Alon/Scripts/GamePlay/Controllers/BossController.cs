using System;
using System.Collections;
using _Alon.Scripts.Core.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class BossController : BaseEnemyController
    {
        /// <summary>
        /// Constants
        /// </summary>
        private const float MaxBossLife = 10000f;
        private const float MoveSpeed = 0.6f;

        /// <summary>
        /// Private Fields
        /// </summary>
        private BasePlayerController _playerToAttack = null;
        private bool _isAttacking = false;
        private Vector3 _leftPatrolPoint;
        private Vector3 _rightPatrolPoint;
        private bool _isPatrolling = false;
        private float _lastAttackTime = 0;
        private Vector3 _currentPatrolTarget;
        private BossAnimator _animator;

        /// <summary>
        /// Events
        /// </summary>
        public event Action OnBossDeath;

        // End Of Local Variables

        private void Start()
        {
            _leftPatrolPoint = transform.position + Vector3.left * 2;
            _rightPatrolPoint = transform.position + Vector3.right * 2;
            _currentPatrolTarget = _rightPatrolPoint;
            Life = MaxBossLife;
            _animator = GetComponent<BossAnimator>();
        }

        private void Update()
        {
            TryAttack();
            if (!_isAttacking)
            {
                _lastAttackTime += Time.deltaTime;
                HandleAttack();
            }

            if (_lastAttackTime > 3 && !_isPatrolling)
            {
                _lastAttackTime = 0;
                StartCoroutine(Patrol());
            }

            if (Life <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            OnBossDeath?.Invoke();
            _animator.ChangeAnimationState("death");
            StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {
            this.enabled = false;
            yield return new WaitForSeconds(4);
            Destroy(gameObject);
            AudioManager.Instance.PlayAudioClip(4);
            SceneManager.LoadScene("Winning");
        }

        private IEnumerator DelayAttack()
        {
            yield return new WaitForSeconds(2f);
            _isAttacking = false;
        }

        private void HandleAttack()
        {
            if (_playerToAttack == null)
            {
                return;
            }

            Attack();
        }

        private void TryAttack()
        {
            _playerToAttack = GameManager.Instance.GetNearestPlayerToBoss();
        }

        protected override void Attack()
        {
            _isAttacking = true;
            _animator.ChangeAnimationState("attack");
            GameManager.Instance.DealDamage(_playerToAttack);
            _isPatrolling = false;
            StartCoroutine(DelayAttack());
        }

        private IEnumerator Patrol()
        {
            Debug.Log("StartPatrol");
            _isPatrolling = true;
            while (_isPatrolling && !_isAttacking)
            {
                var step = Time.deltaTime * MoveSpeed;
                transform.position = Vector3.MoveTowards(transform.position, _currentPatrolTarget, step);
                _animator.ChangeAnimationState("walk");

                if (Vector3.Distance(transform.position, _currentPatrolTarget) < 0.001f)
                {
                    if (_currentPatrolTarget == _rightPatrolPoint)
                    {
                        _currentPatrolTarget = _leftPatrolPoint;
                        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y,
                            transform.localScale.z);
                    }
                    else
                    {
                        _currentPatrolTarget = _rightPatrolPoint;
                        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y,
                            transform.localScale.z);
                    }
                }

                yield return null;
            }
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            Life = Mathf.Max(0, Life - damage);
            StartCoroutine(UIManager.Instance.UpdateBossLifeBar(Life));
        }
    }
}