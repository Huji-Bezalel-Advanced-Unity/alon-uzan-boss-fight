using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace _Alon.Scripts.Core.Managers
{
  public class BossAnimator : MonoBehaviour
  {
      public static BossAnimator Instance { get; private set; }
      
      public Animator animator;
      public bool isFlying;
      
      private void Awake()
      {
          if (Instance == null)
          {
              Instance = this;
          }
          else
          {
              Destroy(gameObject);
          }
      }
  
      public void ChangeAnimationState(string newAnimation)
      {
          animator.Play(newAnimation);
      }
  }  
}
