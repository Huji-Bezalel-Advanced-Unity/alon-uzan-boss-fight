using Unity.VisualScripting;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class Enemy1 : BaseEnemyController
    {
        private float _moveSpeed = 0.6f;
        private float baseDamageToGive = 10f;

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
            
        }
        
        protected override void Attack()
        {
            _isAttacking = true;
            base.Attack();
            _animator.SetBool("isAttack", true);
            _playerToAttack.TakeDamage(baseDamageToGive);
            StartCoroutine(DelayAttack());
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
                // barHolder.transform.Rotate(0, 180, 0);
            }
        }
        
        private new void HandleApproachToPlayer()
        {
            if (_playerToAttack == null)
            {
                return;
            }
            Debug.Log(_playerToAttack.gameObject.name);
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

        private new void MoveToPlayer()
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
    }
}