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
        
        private Vector3 _leftPatrolPoint;
        private Vector3 _rightPatrolPoint;
        private bool _isPatrolling = false;
        private float _lastAttackTime = 0;
        private Vector3 _currentPatrolTarget;
        private readonly float _moveSpeed = 0.6f;

        private void Start()
        {
            _leftPatrolPoint = transform.position + Vector3.left * 2;  // 5 units to the left
            _rightPatrolPoint = transform.position + Vector3.right * 2; // 5 units to the right
            _currentPatrolTarget = _rightPatrolPoint;
        }

        private void Update()
        {
            TryAttack();
            if (!_isAttacking)
            {
                _lastAttackTime += Time.deltaTime;
                HandleAttack();
            }
            else
            {
                
            }

            if (_lastAttackTime > 3 && !_isPatrolling)
            {
                _lastAttackTime = 0;
                StartCoroutine(Patrol());
            }

            if (_bossLife <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            BossAnimator.Instance.ChangeAnimationState("death", false, false);
            StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {
            this.enabled = false;
            GameManager.Instance.IsBossAlive = false;
            yield return new WaitForSeconds(4);
            Destroy(gameObject);
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
            _playerToAttack = GameManager.Instance.GetNearestPlayer();
        }

        private void Attack()
        {
            _isAttacking = true;
            BossAnimator.Instance.ChangeAnimationState("attack", false, false);
            GameManager.Instance.DealDamage(_playerToAttack);
            _isPatrolling = false; // Stop patrolling when attacking
            StartCoroutine(DelayAttack());
        }

        private IEnumerator Patrol()
        {
            Debug.Log("StartPatrol");
            _isPatrolling = true;
            while (_isPatrolling && !_isAttacking)
            {
                float step = Time.deltaTime * _moveSpeed;
                transform.position = Vector3.MoveTowards(transform.position, _currentPatrolTarget, step);
                BossAnimator.Instance.ChangeAnimationState("walk", true, false);
                
                if (Vector3.Distance(transform.position, _currentPatrolTarget) < 0.001f)
                {
                    // BossAnimator.Instance.ChangeAnimationState("idle", false, false);
                    // yield return new WaitForSeconds(1);
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

        public void TakeDamage(float damage)
        {
            _bossLife = Mathf.Max(0, _bossLife - damage);
            StartCoroutine(UIManager.Instance.UpdateBossLifeBar(_bossLife));
            print(_bossLife);
        }
    }
}
