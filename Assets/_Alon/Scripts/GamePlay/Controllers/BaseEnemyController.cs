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
        [SerializeField]protected Animator _animator;
        protected float _deadZone = 3f;
        private float _minDistanceToAttack = 1.2f;

        private void Start()
        {
            Debug.Log("BaseEnemyController Start");
            // _animator = GetComponentInChildren<Animator>();
        }

        private void HandleApproachToPlayer() {}

        protected void CheckForTarget()
        {
            var player = GameManager.Instance.GetNearestPlayerToEnemy(this.gameObject);
            if (!player)
            {
                Debug.Log("No player to attack");
            }
            _playerToAttack = player;
        }
    
        protected void Die()
        {
            _animator.Play("death");
            StartCoroutine(DieRoutine());
        }
    
        protected IEnumerator DieRoutine()
        {
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }
    
        protected void TryAttack()
        {
            if (_playerToAttack == null)
            {
                return;
            }
    
            if (Vector3.Distance(transform.position, _playerToAttack.transform.position) < _minDistanceToAttack)
            {
                _isAttacking = true;
                StartCoroutine(AttackCoolDown());
                Attack();
            }
            else
            {
                _isAttacking = false;
            }
        }
    
        private IEnumerator AttackCoolDown()
        {
            yield return new WaitForSeconds(1);
            _isAttacking = false;
        }
    
        protected virtual void Attack()
        {
        }
        
        public virtual void TakeDamage(float damage){}
        
        protected void MoveToPlayer(){}

        protected IEnumerator DelayAttack()
        {
            yield return new WaitForSeconds(2);
            _isAttacking = true;
        }
        
    }

}
