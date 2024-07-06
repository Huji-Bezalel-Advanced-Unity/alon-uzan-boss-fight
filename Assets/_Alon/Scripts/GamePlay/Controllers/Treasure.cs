using System;
using UnityEngine;

namespace _Alon.Scripts.Gameplay.Controllers
{
    public class Treasure : MonoBehaviour
    {
        private Animator _animator;
        private bool isOpen = false;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) // Check if left mouse button was pressed
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject clickedObject = hit.transform.gameObject;
                    Debug.Log("Clicked on " + clickedObject.name);

                    // Perform further actions
                    if (clickedObject.CompareTag("Treasure"))
                    {
                        OnClick();
                    }
                }
            }
        }

        public void OnClick()
        {
            if (isOpen)
            {
                _animator.SetTrigger("PickUp");
            }
            else
            {
                _animator.SetTrigger("Open");
                isOpen = true;
            }
        }
    }
}