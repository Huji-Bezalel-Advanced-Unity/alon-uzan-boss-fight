using System;
using System.Collections;
using _Alon.Scripts.Core.Managers;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Constants
        /// </summary>
        private const float MinDistanceToAttack = 0.5f;

        /// <summary>
        /// Serialized Fields
        /// </summary>
        [SerializeField] private float moveSpeed = 1f;

        /// <summary>
        /// Private Fields
        /// </summary>
        private bool _isMoving;

        private bool _wasMoving;

        private bool _isAttacking;

        private GameObject _boss;

        [SerializeField] private float _playersLife = 100f;

        private float _TimeToTakeDamage = 2f;
        
        private bool _isDead = false;


        /// <summary>
        /// Public Fields
        /// </summary>

        // End Of Local Variables
        private void Start()
        {
            _boss = GameManager.Instance.Boss;
        }

        private void Update()
        {
            HandleAnimation();
            HandleMovement();
            HandleAttack();
            TakeDamageRoutine();
        }

        private void HandleAttack()
        {
            if (_isAttacking && !_isDead)
            {
                StartCoroutine(AttackCooldown());
            }
        }

        private void TakeDamageRoutine()
        {
            _TimeToTakeDamage -= Time.deltaTime;
            if (!(_TimeToTakeDamage <= 0) || GameManager.Instance.GetNearestPlayer() != gameObject || _isDead) return;
            TakeDamage(10f);
            _TimeToTakeDamage = 2f;
        }

        private void Attack()
        {
            int randomAttack = UnityEngine.Random.Range(1, 3); // Corrected the range to include 2
            String attackAnimation = randomAttack == 1 ? "Stab" : "Slash";
            GameManager.Instance.SetPlayerAnimation(gameObject, attackAnimation, false);
            _isAttacking = false;
        }

        private void HandleDirections(Vector3 moveDirection)
        {
            if (moveDirection.x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y,
                    transform.localScale.z);
            }
            else if (moveDirection.x < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y,
                    transform.localScale.z);
            }
        }

        private void HandleAnimation()
        {
            if (_isMoving && !_wasMoving)
            {
                GameManager.Instance.SetPlayerAnimation(gameObject, "Run", true);
                _wasMoving = true;
            }
            else if (!_isMoving && _wasMoving && _isAttacking)
            {
                GameManager.Instance.SetPlayerAnimation(gameObject, "Idle", true);
            }

            _wasMoving = _isMoving;
        }

        private void HandleMovement()
        {
            if (_boss == null)
            {
                _isMoving = false;
                return;
            }

            var moveDirection = (_boss.transform.position - transform.position).normalized;

            float distanceToMove = moveSpeed * Time.deltaTime;
            float distanceToTarget = Vector3.Distance(transform.position, _boss.transform.position);

            if (distanceToTarget - distanceToMove >= MinDistanceToAttack)
            {
                transform.Translate(moveDirection * distanceToMove, Space.World);
                _isMoving = true;
                HandleDirections(moveDirection);
            }
            else if (_isMoving)
            {
                _isMoving = false;
                _isAttacking = true;
            }
        }

        private IEnumerator AttackCooldown()
        {
            Attack();
            yield return new WaitForSeconds(2f);
            _isAttacking = true;
        }

        private void TakeDamage(float damage)
        {
            GameManager.Instance.SetPlayerAnimation(gameObject, "Hurt", false);
            _playersLife -= damage;
            if (_playersLife <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _isDead = true;
            GameManager.Instance.SetPlayerAnimation(gameObject, "Death", false);
            Debug.Log("Diee");
            GameManager.Instance.RemovePlayer(gameObject);
            GameManager.Instance.InvokeOnPLayerDeath();
            StartCoroutine(DelayDeathForAnimation());
        }
        
        private IEnumerator DelayDeathForAnimation()
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }
    }
}