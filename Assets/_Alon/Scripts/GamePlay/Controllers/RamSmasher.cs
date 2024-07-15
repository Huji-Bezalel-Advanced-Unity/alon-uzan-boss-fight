using UnityEngine;
using _Alon.Scripts.Core.Managers;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class RamSmasher : BasePlayerController
    {
        /// <summary>
        /// Private Fields
        /// </summary>

        /// <summary>
        /// Public Fields
        /// </summary>
        public float mesosCost = 150f;

        // End Of Local Variables

        public override void TakeDamage(float damage)
        {
            _playersLife = Mathf.Max(0, _playersLife - damage);
            base.TakeDamage(damage);
            StartCoroutine(UpdateLifeBar(_playersLife));
        }

        protected override void GiveDamage()
        {
            nearestEnemy.GetComponent<BaseEnemyController>().TakeDamage(GameManager.Instance._damagesDict["IronGuardian"]);
        }

        public override float GetMesosCost()
        {
            return -mesosCost;
        }
    }
}