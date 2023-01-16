using DiceGame.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace DiceGame.Dice
{
    public class DiceSlot : MonoBehaviour , INotificationReceiver
    {
        public UnityEvent onAwake, onAttachToSlot, onDetachFromSlot;

        private void Awake()
        {
            onAwake?.Invoke();
        }

        public UnityEvent OnNotify => onAttachToSlot;
    }
}
