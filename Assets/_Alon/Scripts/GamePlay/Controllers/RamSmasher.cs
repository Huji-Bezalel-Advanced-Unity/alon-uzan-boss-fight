using UnityEngine;
using _Alon.Scripts.Core.Managers;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class RamSmasher : BasePlayerController
    {
        private float _BaseDamageToTake = 20f;
        private float _BaseDamageToGive = 30f;

        public override void TakeDamage()
        {
            _playersLife = Mathf.Max(0, _playersLife - _BaseDamageToTake);
            base.TakeDamage();
            StartCoroutine(UpdateLifeBar(_playersLife));
        }

        protected override void GiveDamage()
        {
            GameManager.Instance.DealEnemyDamage(_BaseDamageToGive, nearestEnemy);
        }
        
    }
}