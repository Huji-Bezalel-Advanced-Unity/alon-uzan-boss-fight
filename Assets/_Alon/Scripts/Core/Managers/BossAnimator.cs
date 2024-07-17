using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Alon.Scripts.Core.Managers
{
    public class BossAnimator : MonoBehaviour
    {
        /// <summary>
        /// Serialized Fields
        /// </summary>
        [SerializeField]
        private Animator animator;

        /// <summary>
        /// Public Fields
        /// </summary>
        private static BossAnimator Instance { get; set; }

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
            if (animator)
            {
                animator.Play(newAnimation);
            }
            else
            {
                Debug.LogError("Animator component is not assigned or found.");
            }
        }
    }
}