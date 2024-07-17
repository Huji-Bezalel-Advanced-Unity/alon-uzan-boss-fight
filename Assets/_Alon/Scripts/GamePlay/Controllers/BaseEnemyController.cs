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
        /// Constants
        /// </summary>
        private const float DeadZone = 3f;
        private const float MinDistanceToAttack = 1.2f;
        private const float MoveSpeed = 0.6f;
        private const float AttackCoolDownTime = 2.5f;

        /// <summary>
        /// Private Fields
        /// </summary>
        private Animator _animator;
        private bool _isDead = false;
        private Rigidbody2D _rigidbody2D;
        private float _timeToAttack = 0;
        private GameObject[] _itemPrefabs;

        /// <summary>
        /// Protected Fields
        /// </summary>
        protected BasePlayerController PlayerToAttack = null;
        protected float Life = 100;


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
            _itemPrefabs = new GameObject[2];
            _itemPrefabs[0] = Resources.Load<GameObject>("Coins");
            _itemPrefabs[1] = Resources.Load<GameObject>("Exps");
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
            if (PlayerToAttack == null)
            {
                _animator.SetBool("isAttack", false);
                return;
            }

            if (Vector3.Distance(transform.position, PlayerToAttack.transform.position) < DeadZone)
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
            PlayerToAttack = player;
            if (!PlayerToAttack)
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
            Life -= damage;
            if (Life <= 0)
            {
                Die();
            }
        }

        private void MoveToPlayer()
        {
            if (Vector3.Distance(transform.position, PlayerToAttack.transform.position) > MinDistanceToAttack)
            {
                _animator.SetBool("isWalk", true);
                transform.Translate((PlayerToAttack.transform.position - transform.position).normalized *
                                    (MoveSpeed * Time.deltaTime));
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
            if (PlayerToAttack.transform.position.x > transform.position.x)
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
            var itemPrefab = Random.Range(0, 10) == 0 ? _itemPrefabs[0] : _itemPrefabs[1];

            if (itemPrefab == null) return;
            var position = new Vector3(transform.position.x, transform.position.y, 0);
            Instantiate(itemPrefab, position, Quaternion.identity);
        }
    }
}