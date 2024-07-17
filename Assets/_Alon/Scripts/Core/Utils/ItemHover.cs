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
        /// <summary>
        /// Private Fields
        /// </summary>
        private const float TargetScale = 1.18f;
        private bool _isHovered = false;

        /// <summary>
        /// Serialized Fields
        /// </summary>
        [SerializeField]
        private new ParticleSystem particleSystem;

        // End Of Local Variables

        private void Update()
        {
            if (_isHovered)
            {
                return;
            }

            if (Camera.main == null) return;
            var mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, Camera.main.nearClipPlane));
            mouseWorldPosition.z =
                0;

            if (!(Vector3.Distance(mouseWorldPosition, gameObject.transform.position) < 0.2f)) return;
            _isHovered = true;
            StartCoroutine(MoveItemToUI());
        }

        private IEnumerator MoveItemToUI()
        {
            Destroy(particleSystem);
            if (gameObject.name.Contains("Coins"))
            {
                foreach (Transform coin in transform)
                {
                    if (coin.gameObject.name.Contains("Coin"))
                    {
                        coin.GetComponent<SpriteRenderer>().sortingLayerID = 25;
                    }
                    coin.DOMove(UIManager.Instance.moneyImage.position, 0.5f)
                        .SetEase(Ease.InQuint).OnComplete(() => Destroy(coin.gameObject));
                    coin.DOScale(Vector3.one * TargetScale, 0.5f)
                        .SetEase(Ease.Linear);
                    yield return new WaitForSeconds(0.1f);
                }

                UIManager.Instance.SetMesos(250);
            }
            else
            {
                foreach (Transform exp in transform)
                {
                    if (exp.gameObject.name.Contains("Exp"))
                    {
                        // set exp order in 
                    }
                    exp.DOMove(UIManager.Instance.expImage.position, 0.5f)
                        .SetEase(Ease.InQuint).OnComplete(() => Destroy(exp.gameObject));
                    exp.DOScale(Vector3.one * TargetScale, 0.5f)
                        .SetEase(Ease.Linear);
                    yield return new WaitForSeconds(0.1f);
                }

                UIManager.Instance.SetExp(50);
            }

            AudioManager.Instance.PlayAudioClip(0);
        }
    }
}