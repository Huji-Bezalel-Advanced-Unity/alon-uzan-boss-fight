using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using _Alon.Scripts.Core.Managers;
using Spine.Unity;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class BasePlayerController : MonoBehaviour
    {
        /// <summary>
        /// Serialized Fields
        /// </summary>
        [SerializeField] private Image lifeBar;

        [SerializeField] private GameObject barHolder;
        [SerializeField] protected float _playersLife = 100f;

        /// <summary>
        /// Private Fields
        /// </summary>
        private bool _isMoving;

        private bool _wasMoving;
        private bool _isAttacking;
        private GameObject _boss;
        private float _TimeToTakeDamage = 2f;
        private float _TimeToGiveDamage = 2f;
        public bool _isDead = false;
        private NavMeshAgent _navMeshAgent;
        private float _maxLife = 100;
        private Vector3 _target;

        private SkeletonAnimation _skeletonAnimation;

        /// <summary>
        /// Public Fields
        /// </summary>
        protected BaseAnimator _playerAnimator;

        protected GameObject nearestEnemy;

        protected const float MinDistanceToAttack = 1.5f;

        // End Of Local Variables

        protected void Start()
        {
            _boss = GameManager.Instance.Boss;
            GameManager.Instance.OnAllEnemiesCleared += HandleAllEnemiesCleared;
            GameManager.Instance.OnEnemyDeath += HandleSetTarget;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.updateUpAxis = false;
            lifeBar.fillAmount = 1;
            HandleSetTarget();
            _skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>(); // inject Player Animator
        }

        private void HandleAllEnemiesCleared()
        {
            nearestEnemy = _boss;
            HandleSetTarget();
        }

        private void Update()
        {
            if (_isDead) return;
            HandleAnimation();
            HandleMovement();
            HandleAttack();
            GiveDamageRoutine();
        }

        private void HandleSetTarget()
        {
            if (nearestEnemy != _boss)
                nearestEnemy = GameManager.Instance.GetNearestEnemy(this.gameObject);
            if (nearestEnemy)
            {
                Vector3 destination = nearestEnemy.transform.position + _target;
                _navMeshAgent.SetDestination(destination);
                HandleDirections();
            }
        }

        private void GiveDamageRoutine()
        {
            _TimeToGiveDamage -= Time.deltaTime;
            if (!(Vector3.Distance(transform.position, nearestEnemy.transform.position) <= MinDistanceToAttack) ||
                !(_TimeToGiveDamage <= 0)) return;
            _TimeToGiveDamage = 2f;
        }

        private void HandleAttack()
        {
            if (!nearestEnemy) return;
            if (Vector3.Distance(transform.position, nearestEnemy.transform.position) > MinDistanceToAttack)
            {
                return;
            }

            if (_isAttacking && !_isDead)
            {
                StartCoroutine(AttackCooldown());
            }
        }

        private void Attack()
        {
            int randomAttack = Random.Range(1, 3);
            string attackAnimation = randomAttack == 1 ? "Stab" : "Slash";
            BaseAnimator.SetAnimation(_skeletonAnimation, attackAnimation, false);
            _isAttacking = false;
        }

        protected virtual void GiveDamage()
        {
        }

        private void HandleDirections()
        {
            if (nearestEnemy.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y,
                    transform.localScale.z);
                _target = Vector3.left;
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y,
                    transform.localScale.z);
                barHolder.transform.Rotate(0, -180, 0);
                _target = Vector3.right;
            }
        }

        private void HandleAnimation()
        {
            if (_isMoving && !_wasMoving)
            {
                BaseAnimator.SetAnimation(_skeletonAnimation, "Run", true);
                _wasMoving = true;
            }
            else if (nearestEnemy == null || (!_isDead && !_isMoving && _wasMoving))
            {
                BaseAnimator.SetAnimation(_skeletonAnimation, "Idle", true);
                _wasMoving = false;
            }
        }

        private void HandleMovement()
        {
            if (nearestEnemy == null)
            {
                _isMoving = false;
                return;
            }

            if (Vector2.Distance(transform.position, nearestEnemy.transform.position) >= MinDistanceToAttack)
            {
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
            yield return new WaitForSeconds(2.5f);
            if (!_isDead)
                GiveDamage();
            _isAttacking = true;
        }

        public virtual void TakeDamage(float damage)
        {
            if (_playersLife <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(DelayHurtAnimation());
            }
        }

        private IEnumerator DelayHurtAnimation()
        {
            yield return new WaitForSeconds(1f);
            if (!_isDead)
            {
                BaseAnimator.SetAnimation(_skeletonAnimation, "Hurt", false);
            }
        }

        private void Die()
        {
            this.enabled = false;
            UnSubscribeFromAllEvents();
            _isDead = true;
            StopAllCoroutines();
            BaseAnimator.SetAnimation(_skeletonAnimation, "Death", false);
            GameManager.Instance.RemovePlayer(this);
            StartCoroutine(DelayDeathForAnimation());
        }

        private void UnSubscribeFromAllEvents()
        {
            GameManager.Instance.OnAllEnemiesCleared -= HandleAllEnemiesCleared;
            GameManager.Instance.OnEnemyDeath -= HandleSetTarget;
        }

        private IEnumerator DelayDeathForAnimation()
        {
            yield return new WaitForSeconds(0.5f);
            BaseAnimator.SetAnimation(_skeletonAnimation, "Death", false);
            yield return new WaitForSeconds(0.7f);
            barHolder.SetActive(false);
            yield return new WaitForSeconds(4f);
            Destroy(gameObject);
        }


        protected IEnumerator UpdateLifeBar(float target)
        {
            yield return new WaitForSeconds(0.7f);
            lifeBar.fillAmount = Mathf.Max(0, target / _maxLife);
        }

        public virtual float GetMesosCost()
        {
            return 0;
        }
    }
}