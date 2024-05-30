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
        [SerializeField] private Image loaderFg;

        [SerializeField] private TextMeshProUGUI accumulatePercent;

        /// <summary>
        /// Private Fields
        /// </summary>
        private int _accumulate;

        private int _accumulateTarget;


        // End Of Local Variables

        private void Reset()
        {
            loaderFg = GetComponent<Image>();
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
            loaderFg.fillAmount = percentageClamp;
            accumulatePercent.text = _accumulate.ToString() + "%";
        }
    }
}