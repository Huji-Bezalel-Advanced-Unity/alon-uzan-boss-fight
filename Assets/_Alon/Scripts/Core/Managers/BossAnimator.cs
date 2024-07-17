using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    public class BossAnimator : MonoBehaviour
    {
        /// <summary>
        /// Serialized Fields
        /// </summary>
        [SerializeField] private Animator _animator;

        /// <summary>
        /// Public Fields
        /// </summary>
        public static BossAnimator Instance { get; private set; }

        public bool IsFlying { get; set; }

        // End Of Local Variables

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogWarning("Multiple instances of BossAnimator detected. The newest instance will be destroyed.");
                Destroy(gameObject);
            }
        }

        public void ChangeAnimationState(string newAnimation)
        {
            if (_animator)
            {
                _animator.Play(newAnimation);
            }
            else
            {
                Debug.LogError("Animator component is not assigned or found.");
            }
        }

        public Animator Animator
        {
            get => _animator;
            set => _animator = value;
        }
    }
}