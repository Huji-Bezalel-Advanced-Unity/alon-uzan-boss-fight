using Unity.VisualScripting;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class Enemy1 : BaseEnemyController
    {
        
        private float baseDamageToGive = 5f;
        
        protected override void Attack()
        {
            base.Attack();
            _playerToAttack.TakeDamage(baseDamageToGive);
        }
        
    }
}