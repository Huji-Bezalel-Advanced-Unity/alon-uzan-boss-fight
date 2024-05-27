using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Alon.Scripts.Core.Loaders
{
    public class GameLoaderUI : MonoBehaviour
    {
        /// <Header>
        /// Constants
        /// </Header>
        
        /// <Header>
        /// Serialized Fields
        /// </Header>
        [SerializeField] private Image loaderFG;
        [SerializeField] private TextMeshProUGUI accumulatePerc;

        /// <Header>
        /// Public Fields
        /// </Header>
        
        /// <Header>
        /// Private Fields
        /// </Header>
        private int _accumulate;
        private int _accumulateTarget;

        private void Reset()
        {
            loaderFG = GetComponent<Image>();
        }

        public void Init(int target)
        {
            _accumulateTarget = target;
            _accumulate = 0;
            UpdateUI();
        }

        public void AddAccumulate(int amount)
        {
            SetAccumulate(_accumulate + amount);
        }

        private void SetAccumulate(int amount)
        {
            _accumulate = amount;
            UpdateUI();
        }

        private void UpdateUI()
        {
            var percentage = (float)_accumulate / _accumulateTarget;
            var percentageClamp = Mathf.Clamp01(percentage);
            loaderFG.fillAmount = percentageClamp;
            accumulatePerc.text = _accumulate.ToString() + "%";
        }

    }
}