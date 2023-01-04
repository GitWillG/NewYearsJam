using System.Collections.Generic;
using System.Linq;
using DiceGame.Dice;
using UnityEngine;

namespace DiceGame
{
    public class DiceSlot : MonoBehaviour
    {
        [SerializeField] private List<Transform> diceSlotTransforms = new List<Transform>();

        private HashSet<DiceFace> _diceFaces = new HashSet<DiceFace>();
        private int _diceSlotCount;
        public void AddDieToSlot(DiceFace diceFace)
        {
            if (_diceSlotCount > diceSlotTransforms.Count) return; // No room for the dice do nothing.

            if (_diceFaces.Contains(diceFace)) return;
            
            diceFace.transform.position = diceSlotTransforms[_diceSlotCount++].position;
            _diceFaces.Add(diceFace);
        }
        
        //TODO: Remove dice from slot
        
        public List<int> GetDieResults()
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
            _diceSlotCount = 0;
            return returnList;
        }
    }
}