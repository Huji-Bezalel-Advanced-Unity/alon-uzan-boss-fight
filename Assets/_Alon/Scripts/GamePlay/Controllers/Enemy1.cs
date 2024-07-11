using Unity.VisualScripting;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class Enemy1 : BaseEnemyController
    {
        /// <summary>
        /// Private Fields
        /// </summary>
        private float baseDamageToGive = 5f;

        // End Of Local Variables

        protected override void Attack()
        {
            base.Attack();
            _playerToAttack.TakeDamage(baseDamageToGive);
        }
    }
}