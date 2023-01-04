using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiceGame.ScriptableObjects
{
    public class DiceSlotSO : ScriptableObject
    {
        [SerializeField] private int numberOfSlots;
        [SerializeField] private List<SlotObjectRelation> allSlotSprites;

        public GameObject SlotPrefab => allSlotSprites.First(x => x.numberOfSlots == numberOfSlots).slotPrefab;
    }
}

[System.Serializable]
public class SlotObjectRelation
{
    public GameObject slotPrefab;
    public int numberOfSlots;
}