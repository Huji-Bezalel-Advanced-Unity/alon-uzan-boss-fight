// using UnityEngine;
// using System;
// using System.Collections.Generic;
//
// namespace _Alon.Scripts.Core.Managers
// {
//     public class PlayerAnimator : BaseAnimator
//     {
//         /// <summary>
//         /// Private Fields
//         /// </summary>
//         private readonly HashSet<String> _animations = new HashSet<String>
//         {
//             "Death",
//             "Hurt",
//             "Slash",
//             "Stab",
//             "Idle",
//             "Run",
//             "Walk"
//         };
//
//         // End Of Local Variables
//
//         public PlayerAnimator()
//         {
//         }
//
//         public override void SetAnimation(GameObject player, String animationName, bool loop)
//         {
//             if (!_animations.Contains(animationName))
//             {
//                 return;
//             }
//
//             base.SetAnimation(player, animationName, loop);
//         }
//     }
// }