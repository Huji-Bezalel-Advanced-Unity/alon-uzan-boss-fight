using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using _Alon.Scripts.Core.Managers;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class BasePlayerController : MonoBehaviour
    {
        private const float MinDistanceToAttack = 0.5f;
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private Image lifeBar;
        [SerializeField] private GameObject barHolder;
        private bool _isMoving;
        private bool _wasMoving;
        private bool _isAttacking;
        private GameObject _boss;
        [SerializeField] protected float _playersLife = 100f;
        private float _TimeToTakeDamage = 2f;
        private float _TimeToGiveDamage = 2f;
        private bool _isDead = false;
        private NavMeshAgent _navMeshAgent;
        private float _maxLife = 100;

        protected PlayerAnimator _playerAnimator;

        protected void Start()
        {
            _boss = GameManager.Instance.Boss;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.updateUpAxis = false;
            lifeBar.fillAmount = 1;
            HandleDirections();
            _playerAnimator = GameManager.Instance.PlayerAnimator; // inject Player Animator
        }

        private void Update()
        {
            if (_isDead) return;
            HandleAnimation();
            HandleMovement();
            HandleAttack();
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

        private void Attack()
        {
            int randomAttack = Random.Range(1, 3);
            string attackAnimation = randomAttack == 1 ? "Stab" : "Slash";
            _playerAnimator.SetAnimation(gameObject, attackAnimation, false);
            _isAttacking = false;
        }

        protected virtual void GiveDamage()
        {
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
                barHolder.transform.Rotate(0, 180, 0);
            }
        }

        private void HandleAnimation()
        {
            if (_isMoving && !_wasMoving)
            {
                _playerAnimator.SetAnimation(gameObject, "Run", true);
                _wasMoving = true;
            }
            else if (!_isDead && !_isMoving && _wasMoving)
            {
                _playerAnimator.SetAnimation(gameObject, "Idle", true);
                _wasMoving = false;
            }
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
            yield return new WaitForSeconds(2.5f);
            if (!_isDead)
                GiveDamage();
            _isAttacking = true;
        }

        public virtual void TakeDamage()
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
                _playerAnimator.SetAnimation(gameObject, "Hurt", false);
            }
        }

        private void Die()
        {
            _isDead = true;
            _playerAnimator.SetAnimation(gameObject, "Death", false);
            GameManager.Instance.RemovePlayer(this);
            StartCoroutine(DelayDeathForAnimation());
        }

        private IEnumerator DelayDeathForAnimation()
        {
            this.enabled = false;
            yield return new WaitForSeconds(0.5f);
            _playerAnimator.SetAnimation(gameObject, "Death", false);
            yield return new WaitForSeconds(0.7f);
            barHolder.SetActive(false);
            yield return new WaitForSeconds(4f);
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("Collided with " + other.gameObject.name);
        }

        protected IEnumerator UpdateLifeBar(float target)
        {
            yield return new WaitForSeconds(0.7f);
            lifeBar.fillAmount = Mathf.Max(0, target / _maxLife);
        }
    }
}