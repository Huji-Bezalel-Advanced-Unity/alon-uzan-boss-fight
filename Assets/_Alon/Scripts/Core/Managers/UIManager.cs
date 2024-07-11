using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Alon.Scripts.Core.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        private const float BossMaxLife = 1000f;
        private float _mesos = 2500f;

        [SerializeField] private Image bossLifeBar;
        [SerializeField] private TextMeshProUGUI mesosText;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogException(new Exception("GameManager already exists"));
            }
        }

        private void Start()
        {
            bossLifeBar.fillAmount = 1;
            mesosText.text = _mesos.ToString();
        }

        public IEnumerator UpdateBossLifeBar(float target)
        {
            float startPercentage = bossLifeBar.fillAmount;
            float targetPercentage = target / BossMaxLife; // make constant
            float elapsedTime = 0f;
            float duration = 1f; // Duration in seconds for the fill amount animation

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                bossLifeBar.fillAmount = Mathf.Lerp(startPercentage, targetPercentage, elapsedTime / duration);
                yield return null;
            }

            bossLifeBar.fillAmount = targetPercentage;
        }
        public void SetMesos(float mesos)
        {
            _mesos -= mesos;
            mesosText.text = _mesos.ToString();
        }

        public float GetMesos()
        {
            return _mesos;
        }
    }
}