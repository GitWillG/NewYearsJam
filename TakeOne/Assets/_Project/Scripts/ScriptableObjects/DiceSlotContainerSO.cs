using System.Collections.Generic;
using UnityEngine;

namespace DiceGame.ScriptableObjects
{
    [CreateAssetMenu(order = 0, fileName = "New DiceSlot Container", menuName = "Create Dice Slot Container")]
    public class DiceSlotContainerSO : ScriptableObject
    {
        private HashSet<DiceSlotHolder> _diceSlots = new HashSet<DiceSlotHolder>();

        public HashSet<DiceSlotHolder> DiceSlots
        {
            get => _diceSlots;
            set => _diceSlots = value;
        }

        public void AddToDiceList(DiceSlotHolder diceSlotHolder)
        {
            if (!_diceSlots.Contains(diceSlotHolder))
            {
                _diceSlots.Add(diceSlotHolder);
            }
        }

        public void RemoveFromDiceList(DiceSlotHolder diceSlotHolder)
        {
            if(!_diceSlots.Contains(diceSlotHolder)) return;
            _diceSlots.Remove(diceSlotHolder);
        }
    }
}