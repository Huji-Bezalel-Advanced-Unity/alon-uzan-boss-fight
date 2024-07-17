using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Alon.Scripts.Core.Utils
{
    public class Notification : MonoBehaviour
    {
        /// <summary>
        /// Serialized Fields
        /// </summary>
        [SerializeField]
        private RectTransform notificationPanel;

        [SerializeField]
        private float moveDistance = 100f;
        
        [SerializeField]
        private float duration = 2.0f;
        
        [SerializeField]
        private TextMeshProUGUI notificationText;

        /// <summary>
        /// Private Fields
        /// </summary>
        private Vector3 _initPos;

        // End Of Local Variables

        private void Awake()
        {
            notificationPanel.gameObject.SetActive(false);
            _initPos = notificationPanel.localPosition;
        }

        public void ShowNotification(string message)
        {
            notificationText.text = message;
            notificationPanel.gameObject.SetActive(true);

            notificationPanel.localPosition = new Vector3(notificationPanel.localPosition.x,
                notificationPanel.localPosition.y - moveDistance, notificationPanel.localPosition.z);


            var notificationSequence = DOTween.Sequence();

            notificationSequence.Append(notificationPanel
                .DOLocalMoveY(notificationPanel.localPosition.y + moveDistance, duration).SetEase(Ease.OutQuad));
            notificationSequence.Join(notificationText.DOFade(0, duration).From(1));

            notificationSequence.OnComplete(() =>
            {
                notificationPanel.gameObject.SetActive(false);
                notificationPanel.localPosition = _initPos;
                notificationText.alpha = 1;
            });
        }
    }
}