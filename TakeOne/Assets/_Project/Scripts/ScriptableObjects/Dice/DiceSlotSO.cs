using UnityEngine;

namespace DiceGame.ScriptableObjects.Dice
{

    [CreateAssetMenu(fileName = "CreateDiceSlotSO", menuName = "Assets/Create/CreateDiceSlotSO", order = 1)]
    public class DiceSlotSO : ScriptableObject
    {
        [SerializeField] private int numberOfSlots;
        //[SerializeField] private List<SlotObjectRelation> allSlotSprites;
        [SerializeField] private GameObject slotPrefab;

        public GameObject SlotPrefab => slotPrefab;
        public int NumberOfSlots => numberOfSlots;
    }
}