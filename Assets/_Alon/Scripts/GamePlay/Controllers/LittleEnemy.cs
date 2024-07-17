using Unity.VisualScripting;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class LittleEnemy : BaseEnemyController
    {
        /// <summary>
        /// Private Fields
        /// </summary>
        private float baseDamageToGive = 5f;

        // End Of Local Variables

        protected override void GiveDamage()
        {
            _playerToAttack.TakeDamage(baseDamageToGive);
        }
    }
}