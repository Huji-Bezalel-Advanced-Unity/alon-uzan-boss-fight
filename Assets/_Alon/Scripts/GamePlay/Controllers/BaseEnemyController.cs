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
        private Animator _animator;

        private float _deadZone = 3f;
        private float _minDistanceToAttack = 1.2f;
        private float _moveSpeed = 0.6f;
        private bool _isDead = false;
        private Rigidbody2D _rigidbody2D;
        private const float AttackCoolDownTime = 2.5f;
        private float _timeToAttack = 0;
        private float expToAdd = 100;
        private GameObject[] itemPrefabs;

        /// <summary>
        /// Protected Fields
        /// </summary>
        protected BasePlayerController _playerToAttack = null;

        protected float life = 100;


        // End Of Local Variables

        private void Start()
        {
            InitFields();
            SubscribeForAllEvents();
            CheckForTarget();
        }

        private void SubscribeForAllEvents()
        {
            BasePlayerController.OnPlayerDieOrSpawn += CheckForTarget;
        }

        private void InitFields()
        {
            _animator = GetComponentInChildren<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            itemPrefabs = new GameObject[2];
            itemPrefabs[0] = Resources.Load<GameObject>("Coins");
            itemPrefabs[1] = Resources.Load<GameObject>("Exps");
        }

        private void Update()
        {
            if (_isDead)
            {
                return;
            }

            HandleApproachToPlayer();
            HandleIncreaseTimers();
        }

        private void HandleIncreaseTimers()
        {
            _timeToAttack += Time.deltaTime;
        }

        private void HandleApproachToPlayer()
        {
            if (_playerToAttack == null)
            {
                _animator.SetBool("isAttack", false);
                return;
            }

            if (Vector3.Distance(transform.position, _playerToAttack.transform.position) < _deadZone)
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
                _animator.SetBool("isAttack", false);
                return;
            }

            HandleDirections();
        }

        private void Die()
        {
            _isDead = true;
            UnSubscribeFromAllEvents();
            StopAllCoroutines();
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            GameManager.Instance.RemoveEnemy(this);
            _animator.Play("death");
            DropItem();
            StartCoroutine(DelayForDeathAnimation());
        }

        private void UnSubscribeFromAllEvents()
        {
            BasePlayerController.OnPlayerDieOrSpawn -= CheckForTarget;
        }

        private IEnumerator DelayForDeathAnimation()
        {
            yield return new WaitForSeconds(2);
            Destroy(this.gameObject);
        }


        protected virtual void Attack()
        {
            _animator.SetBool("isAttack", true);
            GiveDamage();
        }

        protected virtual void GiveDamage()
        {
        }

        public virtual void TakeDamage(float damage)
        {
            life -= damage;
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
                transform.Translate((_playerToAttack.transform.position - transform.position).normalized *
                                    (_moveSpeed * Time.deltaTime));
            }
            else
            {
                GameManager.Instance.InvokeOnEnemyPosChanged();
                _animator.SetBool("isWalk", false);
                TryAttack();
            }
        }

        private void TryAttack()
        {
            if (_timeToAttack < AttackCoolDownTime)
            {
                return;
            }

            _timeToAttack = 0;
            Attack();
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

        private void DropItem()
        {
            var itemPrefab = Random.Range(0, 10) == 0 ? itemPrefabs[0] : itemPrefabs[1];

            if (itemPrefab == null) return;
            var position = new Vector3(transform.position.x, transform.position.y, 0);
            Instantiate(itemPrefab, position, Quaternion.identity);
        }
    }
}