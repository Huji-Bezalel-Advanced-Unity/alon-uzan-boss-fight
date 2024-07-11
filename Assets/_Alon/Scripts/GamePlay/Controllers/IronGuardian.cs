using _Alon.Scripts.Core.Managers;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class IronGuardian : BasePlayerController
    {
        public float _BaseDamageToTake = 20f;
        private float _BaseDamageToGive = 30f;
        public float mesosCost = 100f;

        public override void TakeDamage(float damage)
        {
            _playersLife = Mathf.Max(0, _playersLife - damage);
            base.TakeDamage(damage);
            StartCoroutine(UpdateLifeBar(_playersLife));
        }

        protected override void GiveDamage()
        {
            if (nearestEnemy == null)
            {
                return;
            }
            GameManager.Instance.DealEnemyDamage(_BaseDamageToGive, nearestEnemy);
        }

        public override float GetMesosCost()
        {
            return mesosCost;
        }
    }
}