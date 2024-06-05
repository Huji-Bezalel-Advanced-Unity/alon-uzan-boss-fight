using System;
using System.Collections;
using TMPro;
using UnityEngine;
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
        private Coroutine _currentCoroutine;

        public event Action OnUIFinished;

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
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }
            _currentCoroutine = StartCoroutine(UpdateFillAmount(percentageClamp));
            accumulatePercent.text = Mathf.FloorToInt(percentageClamp * 100).ToString() + "%";
        }

        private IEnumerator UpdateFillAmount(float targetPercentage)
        {
            float startPercentage = loaderFg.fillAmount;
            float elapsedTime = 0f;
            float duration = 1f; // Duration in seconds for the fill amount animation

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                loaderFg.fillAmount = Mathf.Lerp(startPercentage, targetPercentage, elapsedTime / duration);
                yield return null;
            }

            loaderFg.fillAmount = targetPercentage;

            if (Mathf.Approximately(targetPercentage, 1f))
            {
                yield return new WaitForSeconds(1f); // Add delay when reaching 100%
                OnUIFinished?.Invoke();
            }
        }
    }
}
