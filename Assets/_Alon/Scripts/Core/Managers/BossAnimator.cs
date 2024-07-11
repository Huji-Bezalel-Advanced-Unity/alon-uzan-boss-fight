using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace _Alon.Scripts.Core.Managers
{
    public class BossAnimator : MonoBehaviour
    {
        /// <summary>
        /// Public Fields
        /// </summary>
        public static BossAnimator Instance { get; private set; }

        public Animator animator;
        public bool isFlying;

        // End Of Local Variables

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ChangeAnimationState(string newAnimation)
        {
            animator.Play(newAnimation);
        }
    }
}