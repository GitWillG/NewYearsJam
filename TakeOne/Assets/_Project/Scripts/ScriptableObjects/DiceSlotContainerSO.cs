using System.Collections.Generic;
using UnityEngine;

namespace DiceGame.ScriptableObjects
{
    [CreateAssetMenu(order = 0, fileName = "New DiceSlot Container", menuName = "Create Dice Slot Container")]
    public class DiceSlotContainerSO : ScriptableObject
    {
        private HashSet<DiceSlot> _diceSlots = new HashSet<DiceSlot>();

        public HashSet<DiceSlot> DiceSlots
        {
            get => _diceSlots;
            set => _diceSlots = value;
        }

        public void AddToDiceList(DiceSlot diceSlot)
        {
            if (!_diceSlots.Contains(diceSlot))
            {
                _diceSlots.Add(diceSlot);
            }
        }

        public void RemoveFromDiceList(DiceSlot diceSlot)
        {
            if(!_diceSlots.Contains(diceSlot)) return;
            _diceSlots.Remove(diceSlot);
        }
    }
}