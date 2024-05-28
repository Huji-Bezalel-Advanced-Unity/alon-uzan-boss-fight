using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Alon.Scripts.Core.Loaders
{
    public class GameLoaderUI : MonoBehaviour
    {
        /// <summary>
        /// Serialized Fields
        /// </summary>
        [SerializeField] private Image loaderFG;
        [SerializeField] private TextMeshProUGUI accumulatePercent;
        
        /// <summary>
        /// Private Fields
        /// </summary>
        private int accumulate;
        private int accumulateTarget;

        private void Reset()
        {
            loaderFG = GetComponent<Image>();
        }

        public void Init(int target)
        {
            accumulateTarget = target;
            accumulate = 0;
            UpdateUI();
        }

        public void AddAccumulate(int amount)
        {
            SetAccumulate(accumulate + amount);
        }

        private void SetAccumulate(int amount)
        {
            accumulate = amount;
            UpdateUI();
        }

        private void UpdateUI()
        {
            var percentage = (float)accumulate / accumulateTarget;
            var percentageClamp = Mathf.Clamp01(percentage);
            loaderFG.fillAmount = percentageClamp;
            accumulatePercent.text = accumulate.ToString() + "%";
        }

    }
}