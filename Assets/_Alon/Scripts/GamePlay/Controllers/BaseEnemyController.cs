using System.Collections;
using System.Collections.Generic;
using _Alon.Scripts.Core.Managers;
using _Alon.Scripts.Gameplay.Controllers;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class BaseEnemyController : MonoBehaviour
    {
        private BasePlayerController _playerToAttack = null;
        private bool _isAttacking = false;
        private Animator _animator;
        private float _deadZone = 3f;
    
        private void Start()
        {
            Debug.Log("BaseEnemyController Start");
            _animator = GetComponent<Animator>();
        }
    
        private void Update()
        {
            CheckForTarget();
            TryAttack();
        }
    
        private void CheckForTarget()
        {
            var player = GameManager.Instance.GetNearestPlayer(this.gameObject);
            if (!player)
            {
                return;
            }
            _playerToAttack = player;
        }
    
        private void Die()
        {
            _animator.Play("death");
            StartCoroutine(DieRoutine());
        }
    
        private IEnumerator DieRoutine()
        {
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }
    
        private void TryAttack()
        {
            if (_playerToAttack == null)
            {
                return;
            }
    
            if (Vector3.Distance(transform.position, _playerToAttack.transform.position) < _deadZone)
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
            _animator.Play("attack");
        }
        
        public virtual void TakeDamage(float damage){}
        
        
    }

}
