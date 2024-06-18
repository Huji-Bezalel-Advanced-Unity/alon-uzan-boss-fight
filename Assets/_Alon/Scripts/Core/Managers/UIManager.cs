using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _Alon.Scripts.Core.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        private const float BossMaxLife = 1000f;

        [SerializeField] private Image bossLifeBar;

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

    }
}