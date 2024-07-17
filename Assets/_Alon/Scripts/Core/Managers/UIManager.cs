using System;
using System.Collections;
using _Alon.Scripts.Core.Utils;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Alon.Scripts.Core.Managers
{
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// Private Serialized Fields
        /// </summary>
        [SerializeField]
        private Image bossLifeBar;

        [SerializeField]
        private TextMeshProUGUI mesosText;

        [SerializeField]
        private TextMeshProUGUI expText;

        [SerializeField]
        private GameObject barHolder;

        [SerializeField]
        private GameObject dangerImage;

        [SerializeField]
        private Notification notification;

        /// <summary>
        /// Public Serialized Fields
        /// </summary>
        [SerializeField]
        public Transform moneyImage;

        [SerializeField]
        public Transform expImage;

        /// <summary>
        /// Private Fields
        /// </summary>
        private const float BossMaxLife = 10000f;
        private float _mesos = 2500f;
        private float _exp = 0f;

        /// <summary>
        /// Public Fields
        /// </summary>
        public static UIManager Instance { get; private set; }
        public event Action OnBossPhaseStart;

        // End Of Local Variables

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
            expText.text = _exp.ToString();
        }

        public IEnumerator UpdateBossLifeBar(float target)
        {
            var startPercentage = bossLifeBar.fillAmount;
            var targetPercentage = target / BossMaxLife;
            var elapsedTime = 0f;
            var duration = 1f;

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
            AddMesos(mesos);
        }

        private void AddMesos(float mesos)
        {
            _mesos += mesos;
            mesosText.text = _mesos.ToString();
        }

        public float GetMesos()
        {
            return _mesos;
        }

        public void SetExp(float exp)
        {
            AddExp(exp);
        }

        private void AddExp(float exp)
        {
            _exp += exp;
            expText.text = _exp.ToString();
        }

        public float GetExp()
        {
            return _exp;
        }

        public void StartBossPhase()
        {
            StartCoroutine(DangerAnimation());
        }

        private IEnumerator DangerAnimation()
        {
            dangerImage.SetActive(true);
            yield return new WaitForSeconds(1f);
            dangerImage.SetActive(false);
            yield return new WaitForSeconds(1f);
            dangerImage.SetActive(true);
            yield return new WaitForSeconds(1f);
            dangerImage.SetActive(false);
            yield return new WaitForSeconds(1f);
            dangerImage.SetActive(true);
            yield return new WaitForSeconds(3f);
            dangerImage.SetActive(false);
            barHolder.SetActive(true);
            AudioManager.Instance.PlayAudioClip(3);
            OnBossPhaseStart?.Invoke();
        }

        public void Notify(string message)
        {
            notification.ShowNotification(message);
        }
    }
}