using Unity.VisualScripting;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class LittleEnemy : BaseEnemyController
    {
        /// <summary>
        /// Constants
        /// </summary>
        private const float BaseDamageToGive = 5f;

        // End Of Local Variables

        protected override void GiveDamage()
        {
            PlayerToAttack.TakeDamage(BaseDamageToGive);
        }
    }
}