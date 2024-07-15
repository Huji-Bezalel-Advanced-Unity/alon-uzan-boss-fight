using System;
using System.Collections;
using System.Collections.Generic;
using _Alon.Scripts.Core.Managers;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace _Alon.Scripts.Core.Utils
{
    public class ItemHover : MonoBehaviour
    {

        private const float TargetScale = 1.18f;
        [SerializeField] private ParticleSystem particleSystem;

        private void Update()
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, Camera.main.nearClipPlane));
            mouseWorldPosition.z =
                0; // Set Z to 0 or other appropriate value depending on your game's coordinate system

            if (Vector3.Distance(mouseWorldPosition, gameObject.transform.position) < 0.2f)
            {
                StartCoroutine(MoveItemToUI());
            }
        }

        private IEnumerator MoveItemToUI()
        {
            Destroy(particleSystem);
            if (gameObject.name.Contains("Coins"))
            {
                foreach (Transform coin in transform)
                {
                    coin.DOMove(UIManager.Instance.MoneyImage.position, 1.5f) 
                        .SetEase(Ease.InOutElastic).OnComplete(() => Destroy(coin.gameObject));
                    coin.DOScale(Vector3.one * TargetScale, 1.5f) 
                        .SetEase(Ease.InOutElastic);
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                foreach (Transform exp in transform)
                {
                    exp.DOMove(UIManager.Instance.ExpImage.position, 1.5f) 
                        .SetEase(Ease.InOutElastic).OnComplete(() => Destroy(exp.gameObject));
                    exp.DOScale(Vector3.one * TargetScale, 1.5f) 
                        .SetEase(Ease.InOutElastic);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

}