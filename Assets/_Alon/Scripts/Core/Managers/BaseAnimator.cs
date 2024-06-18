using System;
using UnityEngine;
using Spine.Unity;

namespace _Alon.Scripts.Core.Managers
{
    public class BaseAnimator
    {
        protected BaseAnimator() { }

        public virtual void SetAnimation(GameObject player, String animationName, bool loop)
        {
            var skeletonAnimation = player.GetComponent<SkeletonAnimation>();
            if (skeletonAnimation == null)
            {
                Debug.LogError("Failed to get SkeletonAnimation component.");
                return;
            }

            skeletonAnimation.AnimationState.SetAnimation(0, animationName, loop);
        }
    }
}