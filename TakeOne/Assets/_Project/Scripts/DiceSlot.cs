using System.Collections.Generic;
using System.Linq;
using DiceGame.Dice;
using DiceGame.ScriptableObjects;
using UnityEngine;

namespace DiceGame
{
    public class DiceSlot : MonoBehaviour
    {
        [SerializeField] private List<Transform> diceSlotTransforms = new List<Transform>();
        [SerializeField] private DiceSlotContainerSO diceSlotContainerSo;

        private List<DiceFace> _diceFaces = new List<DiceFace>();

        private Dictionary<Transform, DiceFace> _diceSlotToFaceDictionary = new Dictionary<Transform, DiceFace>();
        
        public bool HasSlotAvailable => _diceSlotToFaceDictionary.Any(x => x.Value == null);

        private void Awake()
        {
            foreach (var slotTransform in diceSlotTransforms)
            {
                if (!_diceSlotToFaceDictionary.ContainsKey(slotTransform))
                {
                    _diceSlotToFaceDictionary.Add(slotTransform, null);
                }
            }
            
            diceSlotContainerSo.AddToDiceList(this);
        }

        public void AddDiceToSlot(DiceFace diceFace)
        {
            var hasFreeSlot = _diceSlotToFaceDictionary.Any(x => x.Value == null);

            if (!hasFreeSlot)
            {
                //No Free Slot Fuck off.
                return;
            }
            
            if (_diceFaces.Contains(diceFace)) return; // Already have this dice? Wth?

            var emptyDiceSlot = _diceSlotToFaceDictionary.First(x => x.Value == null).Key;
            if (emptyDiceSlot == null) return;

            _diceSlotToFaceDictionary[emptyDiceSlot] = diceFace;
            //diceFace.transform.position = emptyDiceSlot.position;
            _diceFaces.Add(diceFace);
            diceFace.CurrentSlot = this;
            diceFace.SetAnchor(emptyDiceSlot);
        }
        
        public void RemoveFromDiceSlot(DiceFace diceFace)
        {
            if (!_diceFaces.Contains(diceFace)) return;
            
            _diceFaces.Remove(diceFace);

            var slotForDice = _diceSlotToFaceDictionary.First(x => x.Value == diceFace).Key;
            _diceSlotToFaceDictionary[slotForDice] = null;
        }
        
        public List<int> GetDiceResults()
        {
            var returnList = new List<int>();
            foreach (var diceFace in _diceFaces)
            {
                returnList.Add(diceFace.FaceValue);
            }


            for (var i = 0; i < _diceFaces.Count; i++)
            {
                var diceFace = _diceFaces[i];
                diceFace.DestroyDice();
            }

            _diceFaces = new List<DiceFace>();
            foreach (var key in _diceSlotToFaceDictionary.Keys.ToList())
            {
                _diceSlotToFaceDictionary[key] = null;
            }
            return returnList;
        }

        private void OnDestroy()
        {
            diceSlotContainerSo.RemoveFromDiceList(this);
        }
    }
}