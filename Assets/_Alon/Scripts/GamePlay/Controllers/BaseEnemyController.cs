using System.Collections;
using System.Collections.Generic;
using _Alon.Scripts.Core.Managers;
using _Alon.Scripts.Gameplay.Controllers;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class BaseEnemyController : MonoBehaviour
    {
        /// <summary>
        /// Private Fields
        /// </summary>
        private bool _isAttacking = false;

        private Animator _animator;
        private float _deadZone = 3f;
        private float _minDistanceToAttack = 1.2f;
        private float _moveSpeed = 0.6f;

        private bool _isDead = false;
        
        private Rigidbody2D _rigidbody2D;

        /// <summary>
        /// Public Fields
        /// </summary>
        public float life = 100;

        /// <summary>
        /// Protected Fields
        /// </summary>
        protected BasePlayerController _playerToAttack = null;

        private float expToAdd = 100;

        // End Of Local Variables

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            BasePlayerController.OnPlayerDeath += CheckForTarget;
            CheckForTarget();
        }

        private void Update()
        {
            if (_isDead)
            {
                return;
            }
            
            
            HandleApproachToPlayer();
            CheckForDeath();
        }

        private void HandleApproachToPlayer()
        {
            if (_playerToAttack == null || _isDead)
            {
                return;
            }

            if (!_isAttacking && Vector3.Distance(transform.position, _playerToAttack.transform.position) < _deadZone)
            {
                MoveToPlayer();
            }
            else
            {
                _animator.SetBool("isWalk", false);
            }
        }

        private void CheckForTarget()
        {
            var player = GameManager.Instance.GetNearestPlayerToEnemy(this.gameObject);
            _playerToAttack = player;
            if (!_playerToAttack)
            {
                return;
            }
            HandleDirections();
        }

        private void Die()
        {
            _isDead = true;
            BasePlayerController.OnPlayerDeath -= CheckForTarget;
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            StopAllCoroutines();
            UIManager.Instance.SetExp(expToAdd);
            GameManager.Instance.RemoveEnemy(this);
            _animator.Play("death");
            StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {
            yield return new WaitForSeconds(2);
            Destroy(this.gameObject);
        }

        private IEnumerator AttackCoolDown()
        {
            yield return new WaitForSeconds(2);
            _isAttacking = false;
        }

        protected virtual void Attack()
        {
            _isAttacking = true;
            _animator.SetBool("isAttack", true);
            StartCoroutine(AttackCoolDown());
            StartCoroutine(DelayAttack());
        }

        public virtual void TakeDamage(float damage)
        {
            life -= damage;
        }

        private void CheckForDeath()
        {
            if (life <= 0)
            {
                Die();
            }
        }

        private void MoveToPlayer()
        {
            if (Vector3.Distance(transform.position, _playerToAttack.transform.position) > _minDistanceToAttack)
            {
                _animator.SetBool("isWalk", true);
                transform.Translate((_playerToAttack.transform.position - transform.position).normalized * (_moveSpeed * Time.deltaTime));
            }
            else if (!_isAttacking)
            {
                GameManager.Instance.InvokeOnEnemyPosChanged();
                Attack();
            }
            
        }

        private IEnumerator DelayAttack()
        {
            yield return new WaitForSeconds(2);
            _isAttacking = false;
        }

        private void HandleDirections()
        {
            if (_playerToAttack.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y,
                    transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y,
                    transform.localScale.z);
            }
        }
    }
}