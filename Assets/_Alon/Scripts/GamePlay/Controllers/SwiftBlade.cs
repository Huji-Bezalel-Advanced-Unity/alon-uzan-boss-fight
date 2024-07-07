using _Alon.Scripts.Core.Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class SwiftBlade : BasePlayerController
    {
        private float _BaseDamageToTake = 20f;
        private float _BaseDamageToGive = 30f;

        public override void TakeDamage(float damage)
        {
            _playersLife = Mathf.Max(0, _playersLife - damage);
            base.TakeDamage(damage);
            StartCoroutine(UpdateLifeBar(_playersLife));
        }

        protected override void GiveDamage()
        {
            GameManager.Instance.DealEnemyDamage(_BaseDamageToGive, nearestEnemy);
        }
        
    }
}