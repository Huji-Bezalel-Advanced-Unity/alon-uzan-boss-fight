using System;
using UnityEngine;
using Spine.Unity;

namespace _Alon.Scripts.Core.Managers
{
    public class BaseAnimator
    {
        public BaseAnimator()
        {
        }

        public static void SetAnimation(SkeletonAnimation skeletonAnimation, String animationName, bool loop)
        {
            if (!skeletonAnimation)
            {
                return;
            }
            skeletonAnimation.AnimationState.SetAnimation(0, animationName, loop);
        }
    }
}