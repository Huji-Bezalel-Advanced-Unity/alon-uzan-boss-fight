using System.Collections;
using System.Collections.Generic;
using _Alon.Scripts.Core.Managers;
using _Alon.Scripts.Gameplay.Controllers;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class BaseEnemyController : MonoBehaviour
    {
        protected BasePlayerController _playerToAttack = null;
        protected bool _isAttacking = false;
        protected Animator _animator;
        protected float _deadZone = 3f;
        private float _minDistanceToAttack = 1.2f;
        private float _moveSpeed = 0.6f;
        public float life = 100;

        private void Start()
        {
            Debug.Log("BaseEnemyController Start");
            _animator = GetComponentInChildren<Animator>();
        }
        
        private void Update()
        {
            CheckForTarget();
            HandleDirections();
            if (!_isAttacking)
            {
                TryAttack();
            }
            HandleApproachToPlayer();
            if (_playerToAttack == null || (_playerToAttack != null && _playerToAttack._isDead))
            {
                _animator.SetBool("isAttack", false);
            }
            CheckForDeath();
        }

        private void HandleApproachToPlayer()
        {
            if (_playerToAttack == null)
            {
                return;
            }
            
            if (!_isAttacking && Vector3.Distance(transform.position, _playerToAttack.transform.position) < _deadZone)
            {
                Debug.Log("Approaching player");
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
        }
    
        protected void Die()
        {
            StopAllCoroutines();
            GameManager.Instance.RemoveEnemy(this);
            _animator.Play("death");
            StartCoroutine(DieRoutine());
        }
    
        private IEnumerator DieRoutine()
        {
            Debug.Log("start die routine");
            this.enabled = false;
            yield return new WaitForSeconds(2);
            Destroy(this.gameObject);
        }
    
        private void TryAttack()
        {
            if (_playerToAttack == null)
            {
                _animator.SetBool("isAttack", false);
                _animator.SetBool("isWalk", false);
                return;
            }
    
            if (Vector3.Distance(transform.position, _playerToAttack.transform.position) < _minDistanceToAttack)
            {
                StartCoroutine(AttackCoolDown());
                Attack();
            }
            else
            {
                _animator.SetBool("isAttack", false);
                _isAttacking = false;
            }
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
            if (_playerToAttack == null)
            {
                Debug.Log("No player to attack");
                return;
            }
            Debug.Log("Moving to player");
            // move to the player in _moveSpeed
            _animator.SetBool("isWalk", true);
            transform.position = Vector3.MoveTowards(transform.position, _playerToAttack.transform.position, _moveSpeed * Time.deltaTime);
        }

        private IEnumerator DelayAttack()
        {
            yield return new WaitForSeconds(2);
            _isAttacking = false;
        }
        
        private void HandleDirections()
        {
            if (_playerToAttack == null)
            {
                return;
            }
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
