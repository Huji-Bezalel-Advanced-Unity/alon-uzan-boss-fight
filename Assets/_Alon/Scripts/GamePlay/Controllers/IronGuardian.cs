using _Alon.Scripts.Core.Managers;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class IronGuardian : BasePlayerController
    {
        /// <summary>
        /// Public Fields
        /// </summary>
        public float mesosCost = 100f;

        // End Of Local Variables

        public override void TakeDamage(float damage)
        {
            playersLife = Mathf.Max(0, playersLife - damage);
            base.TakeDamage(damage);
            StartCoroutine(UpdateLifeBar(playersLife));
        }

        protected override void GiveDamage()
        {
            NearestEnemy.GetComponent<BaseEnemyController>()
                .TakeDamage(GameManager.Instance.DamagesDict["IronGuardian"]);
        }

        public override float GetMesosCost()
        {
            return -mesosCost;
        }
    }
}