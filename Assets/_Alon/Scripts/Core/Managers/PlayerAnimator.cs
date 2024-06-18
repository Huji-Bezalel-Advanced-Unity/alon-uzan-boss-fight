using UnityEngine;
using System;
using System.Collections.Generic;

namespace _Alon.Scripts.Core.Managers
{
    public class PlayerAnimator : BaseAnimator
    {
        private readonly HashSet<String> _animations = new HashSet<String>
        {
            "Death",
            "Hurt",
            "Slash",
            "Stab",
            "Idle",
            "Run",
            "Walk"
        };

        public PlayerAnimator() { }

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