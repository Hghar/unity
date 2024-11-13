using System;
using UnityEngine;

namespace Events
{
  public class RangeEventHandler : MonoBehaviour
  {
      public event Action ReadyToAttack;
  
      public void OnReadyToAttack()
      {
          ReadyToAttack?.Invoke();
      }
  }  
}
