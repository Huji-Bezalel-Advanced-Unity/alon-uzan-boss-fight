using Unity.VisualScripting;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class RamSmasher : BasePlayerController
    {
        private float _BaseDamageToTake = 30f;

        public override void TakeDamage()
        {
            base.TakeDamage();
            _playersLife -= _BaseDamageToTake;
        }
        
    }
}