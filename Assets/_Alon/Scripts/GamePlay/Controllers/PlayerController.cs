using System;
using Spine.Unity;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Serialized Fields
        /// </summary>
        [SerializeField] private float _moveSpeed = 1f;
        [SerializeField] private Vector3 _fakeBossPosition;
        
        /// <summary>
        /// Private Fields
        /// </summary>
        private SkeletonAnimation _skeletonAnimation;
        private bool _isMoving;
        private bool _wasMoving;

        private void Start()
        {
            _skeletonAnimation = GetComponent<SkeletonAnimation>();
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
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (moveDirection.x < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

        private void HandleAnimation()
        {
            if (_isMoving && !_wasMoving)
            {
                _skeletonAnimation.AnimationState.SetAnimation(0, "Run", true);
            }
            else if (!_isMoving && _wasMoving)
            {
                _skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
            }
            _wasMoving = _isMoving;
        }

        private void HandleMovement()
        {
            var moveDirection = (_fakeBossPosition - transform.position).normalized;

            float distanceToMove = _moveSpeed * Time.deltaTime;
            float distanceToTarget = Vector3.Distance(transform.position, _fakeBossPosition);

            if (distanceToTarget > distanceToMove)
            {
                transform.Translate(moveDirection * distanceToMove, Space.World);
                _isMoving = true;
                HandleDirections(moveDirection);
            }
            else
            {
                transform.position = _fakeBossPosition;
                _isMoving = false;
            }
            
        }
    }
}