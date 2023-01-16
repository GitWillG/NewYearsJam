using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiceGame.ScriptableObjects
{

    [CreateAssetMenu(fileName = "CreateDiceSlotSO", menuName = "Assets/Create/CreateDiceSlotSO", order = 1)]
    public class DiceSlotSO : ScriptableObject
    {
        [SerializeField] private int numberOfSlots;
        //[SerializeField] private List<SlotObjectRelation> allSlotSprites;
        [SerializeField] private GameObject slotPrefab;

        public GameObject SlotPrefab => slotPrefab;
    }
}

[System.Serializable]
public class SlotObjectRelation
{
    public GameObject slotPrefab;
    public int numberOfSlots;
}