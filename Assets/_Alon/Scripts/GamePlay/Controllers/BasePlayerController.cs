using System;
using System.Collections;
using System.Collections.Generic;
using _Alon.Scripts.Core.Managers;
using Spine.Unity;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class BasePlayerController : MonoBehaviour
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

        [SerializeField] protected float _playersLife = 100f;

        private float _TimeToTakeDamage = 2f;
        
        private float _TimeToGiveDamage = 2f;

        private bool _isDead = false;

        private readonly float _baseDamageToTake = 10f;
        
        private readonly float _baseDamageToGive = 10f;

        private NavMeshAgent _navMeshAgent;

        /// <summary>
        /// Public Fields
        /// </summary>

        // End Of Local Variables
        private void Start()
        {
            _boss = GameManager.Instance.Boss;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.updateUpAxis = false;
            
        }

        private void Update()
        {
            HandleDirections();
            HandleAnimation();
            HandleMovement();
            HandleAttack();
            // TakeDamageRoutine();
            GiveDamageRoutine();
        }

        private void GiveDamageRoutine()
        {
            _TimeToGiveDamage -= Time.deltaTime;
            if (!(Vector3.Distance(transform.position, _boss.transform.position) <= MinDistanceToAttack) ||
                !(_TimeToGiveDamage <= 0)) return;
            _TimeToGiveDamage = 2f;
        }

        private void HandleAttack()
        {
            if (_isAttacking && !_isDead)
            {
                StartCoroutine(AttackCooldown());
            }
        }

        // private void TakeDamageRoutine()
        // {
        //     _TimeToTakeDamage -= Time.deltaTime;
        //     if (!(_TimeToTakeDamage <= 0) || GameManager.Instance.GetNearestPlayer() != gameObject || _isDead) return;
        //     TakeDamage();
        //     _TimeToTakeDamage = 2f;
        // }

        private void Attack()
        {
            int randomAttack = UnityEngine.Random.Range(1, 3); // Corrected the range to include 2
            String attackAnimation = randomAttack == 1 ? "Stab" : "Slash";
            GameManager.Instance.SetPlayerAnimation(gameObject, attackAnimation, false);
            _isAttacking = false;
        }

        private void HandleDirections()
        {
            if (_boss.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y,
                    transform.localScale.z);
            }
            else if (_boss.transform.position.x < transform.position.x)
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
            
            if (Vector2.Distance(transform.position, _boss.transform.position) >= MinDistanceToAttack)
            {
                _navMeshAgent.SetDestination(_boss.transform.position);
                _isMoving = true;
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

        public virtual void TakeDamage()
        {
            GameManager.Instance.SetPlayerAnimation(gameObject, "Hurt", false);
            if (_playersLife <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _isDead = true;
            GameManager.Instance.SetPlayerAnimation(gameObject, "Death", false);
            GameManager.Instance.RemovePlayer(gameObject);
            StartCoroutine(DelayDeathForAnimation());
        }

        private IEnumerator DelayDeathForAnimation()
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("Collided with " + other.gameObject.name);
        }
    }
}