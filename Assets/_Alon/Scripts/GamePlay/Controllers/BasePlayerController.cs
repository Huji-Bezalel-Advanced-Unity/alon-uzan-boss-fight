using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using _Alon.Scripts.Core.Managers;
using Spine.Unity;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class BasePlayerController : MonoBehaviour
    {
        /// <summary>
        /// Constants
        /// </summary>
        private const float AttackCoolDownTime = 2.5f;
        private const float MinDistanceToAttack = 1.2f;
        private const float MaxLife = 100;

        /// <summary>
        /// Serialized Fields
        /// </summary>
        [SerializeField]
        private Image lifeBar;

        [SerializeField]
        private GameObject barHolder;
        
        [SerializeField]
        protected float playersLife = 100f;

        /// <summary>
        /// Private Fields
        /// </summary>
        private bool _isMoving;
        private GameObject _boss;
        private NavMeshAgent _navMeshAgent;
        private Vector3 _target;
        private SkeletonAnimation _skeletonAnimation;
        private float _timeToAttack = 0;
        private bool _isSetEnemyOnes = false;
        private bool _allEnemiesDead = false;
        private readonly BaseAnimator _baseAnimator = new BaseAnimator();
        
        /// <summary>
        /// Public Fields
        /// </summary>
        public bool isDead = false;

        /// <summary>
        /// Events
        /// </summary>
        public static event Action OnPlayerDieOrSpawn;

        /// <summary>
        /// Protected Fields
        /// </summary>
        protected GameObject NearestEnemy;

        // End Of Local Variables

        private void Start()
        {
            _boss = GameManager.Instance.Boss;
            SubscribeToAllEvents();
            InitialNavMash();
            OnPlayerDieOrSpawn?.Invoke();
            _skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
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
            UIManager.Instance.OnBossPhaseStart += HandleBossPhase;
        }

        private void HandleBossDie()
        {
            NearestEnemy = null;
            _baseAnimator.SetAnimation(_skeletonAnimation, "Idle", true);
            this.enabled = false;
        }

        private void HandleAllEnemiesCleared()
        {
            NearestEnemy = _boss;
            _allEnemiesDead = true;
            HandleSetTarget();
        }

        private void Update()
        {
            if (isDead) return;
            // HandleSetTarget();
            HandleMovement();
            HandleIncreaseTimers();
        }

        private void HandleIncreaseTimers()
        {
            _timeToAttack += Time.deltaTime;
        }

        private void HandleBossPhase()
        {
            NearestEnemy = _boss;
            StartCoroutine(UpdateBossPosition());
        }

        private IEnumerator UpdateBossPosition()
        {
            while (NearestEnemy == _boss)
            {
                HandleSetTarget();
                yield return new WaitForSeconds(0.5f);
            }
        }


        private void HandleSetTarget()
        {
            if (!NearestEnemy &&
                (_isSetEnemyOnes || _allEnemiesDead))
            {
                return;
            }

            Vector3 destination;
            var tryToGetEnemy = GameManager.Instance.GetNearestEnemy(this.gameObject);
            if (NearestEnemy != _boss && tryToGetEnemy)
            {
                NearestEnemy = tryToGetEnemy;
                OnPlayerDieOrSpawn?.Invoke();
                destination = NearestEnemy.transform.position + _target;
            }
            else
            {
                destination = _boss.transform.position;
            }

            _isSetEnemyOnes = true;

            _navMeshAgent.SetDestination(destination);
            HandleDirections();
        }

        private void Attack()
        {
            var randomAttack = Random.Range(1, 3);
            var attackAnimation = randomAttack == 1 ? "Stab" : "Slash";
            _baseAnimator.SetAnimation(_skeletonAnimation, attackAnimation, false);
            GiveDamage();
        }

        protected virtual void GiveDamage()
        {
        }

        private void HandleDirections()
        {
            if (!NearestEnemy)
            {
                return;
            }

            if (NearestEnemy.transform.position.x > transform.position.x)
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
            if (NearestEnemy == null)
            {
                _baseAnimator.SetAnimation(_skeletonAnimation, "Idle", false);
                return;
            }

            if (Vector2.Distance(transform.position, NearestEnemy.transform.position) <= MinDistanceToAttack)
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
            _baseAnimator.SetAnimation(_skeletonAnimation, "Run", true);
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
            if (playersLife <= 0)
            {
                Die();
            }
            else
            {
                _baseAnimator.AddAnimation(_skeletonAnimation, "Hurt", false);
            }
        }

        private void Die()
        {
            isDead = true;
            OnPlayerDieOrSpawn?.Invoke();
            UnSubscribeFromAllEvents();
            StopAllCoroutines();
            _baseAnimator.SetAnimation(_skeletonAnimation, "Death", false);
            GameManager.Instance.RemovePlayer(this);
            StartCoroutine(DelayForDeathAnimation());
        }

        private void UnSubscribeFromAllEvents()
        {
            GameManager.Instance.OnAllEnemiesCleared -= HandleAllEnemiesCleared;
            GameManager.Instance.OnEnemyPosChanged -= HandleSetTarget;
            _boss.GetComponent<BossController>().OnBossDeath -= HandleBossDie;
            UIManager.Instance.OnBossPhaseStart -= HandleBossPhase;
        }

        private IEnumerator DelayForDeathAnimation()
        {
            barHolder.SetActive(false);
            yield return new WaitForSeconds(4f);
            Destroy(gameObject);
        }

        protected IEnumerator UpdateLifeBar(float target)
        {
            yield return new WaitForSeconds(0.7f);
            lifeBar.fillAmount = Mathf.Max(0, target / MaxLife);
        }

        public virtual float GetMesosCost()
        {
            return 0;
        }
    }
}