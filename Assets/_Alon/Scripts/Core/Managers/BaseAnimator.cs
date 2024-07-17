using System;
using UnityEngine;
using Spine.Unity;

namespace _Alon.Scripts.Core.Managers
{
    public class BaseAnimator
    {
        public void SetAnimation(SkeletonAnimation skeletonAnimation, string animationName, bool loop)
        {
            if (!skeletonAnimation)
                return;

            skeletonAnimation.AnimationState.SetAnimation(0, animationName, loop);
        }
        
        public void AddAnimation(SkeletonAnimation skeletonAnimation, string animationName, bool loop)
        {
            if (!skeletonAnimation)
                return;

            skeletonAnimation.AnimationState.AddAnimation(0, animationName, loop, 0);
        }
    }
}