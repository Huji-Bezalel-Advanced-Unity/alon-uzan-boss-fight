using System;
using _Alon.Scripts.Core.Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class RamSmasher : BasePlayerController
    {
        private float _BaseDamageToTake = 30f;
        private float _BaseDamageToGive = 50f;

        public override void TakeDamage()
        {
            _playersLife = Mathf.Max(0, _playersLife - _BaseDamageToTake);
            base.TakeDamage();
            StartCoroutine(UpdateLifeBar(_playersLife));
        }

        protected override void GiveDamage()
        {
            GameManager.Instance.DealBossDamage(_BaseDamageToGive);
        }
    }
}