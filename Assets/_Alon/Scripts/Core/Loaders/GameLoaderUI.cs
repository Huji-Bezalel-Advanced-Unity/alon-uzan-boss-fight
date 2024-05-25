using System;
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

        public void SetAccumulate(int amount)
        {
            _accumulate = amount;
            UpdateUI();
        }

        private void UpdateUI()
        {
            var percentage = (float)_accumulate / _accumulateTarget;
            var percentageClamp = Mathf.Clamp01(percentage);
            loaderFG.fillAmount = percentageClamp;
        }

    }
}