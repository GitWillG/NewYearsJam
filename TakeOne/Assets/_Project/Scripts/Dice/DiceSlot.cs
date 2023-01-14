using System;
using UnityEngine;
using UnityEngine.Events;

namespace DiceGame.Dice
{
    public class DiceSlot : MonoBehaviour
    {
        public UnityEvent OnAwake, OnAttachToSlot, OnDetachFromSlot;

        private void Awake()
        {
            OnAwake?.Invoke();
        }
    }
}
