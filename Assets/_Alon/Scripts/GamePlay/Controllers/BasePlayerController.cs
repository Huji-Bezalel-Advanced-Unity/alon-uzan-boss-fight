using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using _Alon.Scripts.Core.Managers;
using Spine.Unity;
using Random = UnityEngine.Random;

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

        private GameObject _boss;
        private float _TimeToTakeDamage = 2f;
        private float _TimeToGiveDamage = 2f;
        public bool _isDead = false;
        private NavMeshAgent _navMeshAgent;
        private float _maxLife = 100;
        private Vector3 _target;

        private SkeletonAnimation _skeletonAnimation;
        
        private const float timeLapsToSetBossTarget = 0.5f;
        private float _timerForBossPhase;

        private const float AttackCoolDownTime = 2.5f;

        /// <summary>
        /// Public Fields
        /// </summary>
        protected BaseAnimator _playerAnimator;

        protected GameObject nearestEnemy;
        private float _timeToAttack = 0;

        protected const float MinDistanceToAttack = 1;
        
        public static event Action OnPlayerDeath;

        // End Of Local Variables

        protected void Start()
        {
            _boss = GameManager.Instance.Boss;
            SubscribeToAllEvents();
            InitialNavMash();
            OnPlayerDeath?.Invoke();
            _skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>(); // inject Player Animator
        }

        private void InitialNavMash()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.updateUpAxis = false;
            HandleSetTarget();
        }

        private void SubscribeToAllEvents()
        {
            _boss.GetComponent<BossController>().OnBossDeath += HandleBossDie;
            GameManager.Instance.OnAllEnemiesCleared += HandleAllEnemiesCleared;
            GameManager.Instance.OnEnemyPosChanged += HandleSetTarget;
        }

        private void HandleBossDie()
        {
            nearestEnemy = null;
            BaseAnimator.SetAnimation(_skeletonAnimation, "Idle", true);
        }

        private void HandleAllEnemiesCleared()
        {
            nearestEnemy = _boss;
            HandleSetTarget();
        }

        private void Update()
        {
            if (_isDead) return;
            HandleBossPhase();
            HandleMovement();
            HandleIncreaseTimers();
        }

        private void HandleIncreaseTimers()
        {
            _timeToAttack += Time.deltaTime;
            _timerForBossPhase += Time.deltaTime;
        }

        private void HandleBossPhase()
        {
            if (nearestEnemy == _boss && _timerForBossPhase >= timeLapsToSetBossTarget)
            {
                HandleSetTarget();
                _timerForBossPhase = 0;
            }
        }


        private void HandleSetTarget()
        {
            Vector3 destination;
            if (nearestEnemy != _boss)
            {
                nearestEnemy = GameManager.Instance.GetNearestEnemy(this.gameObject);
                destination = nearestEnemy.transform.position + _target;
            }
            else
            {
                destination = _boss.transform.position;
            }

            if (!nearestEnemy) return;
            _navMeshAgent.SetDestination(destination);
            HandleDirections();
        }

        private void Attack()
        {
            int randomAttack = Random.Range(1, 3);
            string attackAnimation = randomAttack == 1 ? "Stab" : "Slash";
            BaseAnimator.SetAnimation(_skeletonAnimation, attackAnimation, false);
            GiveDamage();
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
        

        private void HandleMovement()
        {
            if (nearestEnemy == null)
            {
                BaseAnimator.SetAnimation(_skeletonAnimation, "Idle", false);
                return;
            }

            if (Vector2.Distance(transform.position, nearestEnemy.transform.position) <= MinDistanceToAttack)
            {
                TryAttack();
            }
            else if (!_isMoving)
            {
                StartCoroutine(SetRunAnimation());
            }
            
        }

        private IEnumerator SetRunAnimation()
        {
            _isMoving = true;
            BaseAnimator.SetAnimation(_skeletonAnimation, "Run", true);
            yield return new WaitForSeconds(1f);
            _isMoving = false;
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


        public virtual void TakeDamage(float damage)
        {
            if (_playersLife <= 0)
            {
                Die();
            }
            else
            {
                BaseAnimator.AddAnimation(_skeletonAnimation, "Hurt", false);
            }
        }

        private void Die()
        {
            _isDead = true;
            OnPlayerDeath?.Invoke();
            UnSubscribeFromAllEvents();
            StopAllCoroutines();
            BaseAnimator.SetAnimation(_skeletonAnimation, "Death", false);
            GameManager.Instance.RemovePlayer(this);
            StartCoroutine(DelayAnimationForDeath());
        }

        private void UnSubscribeFromAllEvents()
        {
            GameManager.Instance.OnAllEnemiesCleared -= HandleAllEnemiesCleared;
            GameManager.Instance.OnEnemyPosChanged -= HandleSetTarget;
            _boss.GetComponent<BossController>().OnBossDeath -= HandleBossDie;
        }

        private IEnumerator DelayAnimationForDeath()
        {
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