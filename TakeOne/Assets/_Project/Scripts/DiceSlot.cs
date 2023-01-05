using System;
using System.Collections.Generic;
using System.Linq;
using DiceGame.Dice;
using DiceGame.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace DiceGame
{
    public class DiceSlot : MonoBehaviour
    {
        [SerializeField] private List<Transform> diceSlotTransforms = new List<Transform>();
        [SerializeField] private DiceSlotContainerSO diceSlotContainerSo;

        private HashSet<DiceFace> _diceFaces = new HashSet<DiceFace>();

        private Dictionary<Transform, DiceFace> _diceSlotToFaceDictionary = new Dictionary<Transform, DiceFace>();

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
            diceFace.transform.position = emptyDiceSlot.position;
            _diceFaces.Add(diceFace);
            diceFace.CurrentSlot = this;
        }
        
        //TODO: Remove dice from slot
        public void RemoveFromDiceSlot(DiceFace diceFace)
        {
            if (_diceFaces.Contains(diceFace))
            {
                _diceFaces.Remove(diceFace);
            }

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

            foreach (var diceFace in _diceFaces.Reverse())
            {
                diceFace.DestroyDice();
            }
            _diceFaces.Clear();
            foreach (var key in _diceSlotToFaceDictionary.Keys)
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