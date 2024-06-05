using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Alon.Scripts.Core.Managers
{
    public class BossAnimator : BaseAnimator
    {
        private readonly HashSet<String> _animations = new HashSet<String>
        {
            "skill",
            "attack",
            "idle"
        };
        
        public BossAnimator(){}

        public override void SetAnimation(GameObject player, String animationName, bool loop)
        {
            if (!_animations.Contains(animationName))
            {
                Debug.LogException(new Exception("Invalid animation name."));
            }
            base.SetAnimation(player, animationName, loop);
        }
    }
}