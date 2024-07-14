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
            if (nearestEnemy == null || Vector3.Distance(transform.position, nearestEnemy.transform.position) > MinDistanceToAttack)
            {
                return;
            }

            GameManager.Instance.DealEnemyDamage(GameManager.Instance._damagesDict["RamSmasher"], nearestEnemy);
        }

        public override float GetMesosCost()
        {
            return mesosCost;
        }
    }
}