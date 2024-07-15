using UnityEngine;
using DG.Tweening;  // Import DOTween
using TMPro;  // Import TextMeshPro

public class Notification : MonoBehaviour
{
    [SerializeField]private RectTransform notificationPanel;
    [SerializeField]private float moveDistance = 100f;
    [SerializeField]private float duration = 2.0f;
    private Vector3 _initPos;

    [SerializeField] private TextMeshProUGUI notificationText;

    private void Awake()
    {
        notificationPanel.gameObject.SetActive(false);
        _initPos = notificationPanel.localPosition;
    }

    public void ShowNotification(string message)
    {
        notificationText.text = message;
        notificationPanel.gameObject.SetActive(true);

        notificationPanel.localPosition = new Vector3(notificationPanel.localPosition.x, notificationPanel.localPosition.y - moveDistance, notificationPanel.localPosition.z);
        

        Sequence notificationSequence = DOTween.Sequence();

        notificationSequence.Append(notificationPanel.DOLocalMoveY(notificationPanel.localPosition.y + moveDistance, duration).SetEase(Ease.OutQuad));
        notificationSequence.Join(notificationText.DOFade(0, duration).From(1));

        notificationSequence.OnComplete(() => {
            notificationPanel.gameObject.SetActive(false);
            notificationPanel.localPosition = _initPos;
            notificationText.alpha = 1;
        });
    }
}