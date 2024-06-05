using System;
using _Alon.Scripts.Core.Managers;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Serialized Fields
        /// </summary>
        [SerializeField] private float moveSpeed = 1f;

        /// <summary>
        /// Private Fields
        /// </summary>
        private bool _isMoving;

        private bool _wasMoving;

        private GameObject _boss;


        // End Of Local Variables

        private void Start()
        {
            _boss = GameManager.Instance.Boss;
        }

        private void Update()
        {
            HandleMovement();
            HandleAnimation();
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
            }
            else if (!_isMoving && _wasMoving)
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

            if (distanceToTarget > distanceToMove)
            {
                transform.Translate(moveDirection * distanceToMove, Space.World);
                _isMoving = true;
                HandleDirections(moveDirection);
            }
            else
            {
                transform.position = _boss.transform.position;
                _isMoving = false;
            }
        }
    }
}