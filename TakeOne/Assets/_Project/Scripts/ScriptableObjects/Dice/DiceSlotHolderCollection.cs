using DiceGame.Dice;
using UnityEngine;

namespace DiceGame.ScriptableObjects.Dice
{
    [CreateAssetMenu(order = 0, fileName = "New DiceSlot Container", menuName = "Create Dice Slot Container")]
    public class DiceSlotHolderCollection : CollectionExposerSO<DiceSlotHolder> { }
}

