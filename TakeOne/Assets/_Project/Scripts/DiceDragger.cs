using System;
using System.Collections.Generic;
using System.Linq;
using DiceGame.Dice;
using UnityEngine;

namespace DiceGame
{ 
    public class DiceDragger : MonoBehaviour
    {
        [SerializeField] private DiceFace currentDice;
        [SerializeField] private float releaseThreshold;
        [SerializeField] private Camera diceCam;
        
        private List<DiceSlot> _diceSlots = new List<DiceSlot>();

        private DiceManager _diceManager;

        private void Awake()
        {
            _diceManager = FindObjectOfType<DiceManager>();
        }

        public void OnPickUp()
        {
            if(currentDice == null) return;
            _diceManager.RemoveFromDiceTray(currentDice);

            if (currentDice.IsInSlot)
            {
                //remove from that slot
                currentDice.DetachFromSlot();
            }

        }

        private void Update()
        {
            //Cast ray from cam
            
            //If ray hits dice and current dice is not there
            
            //Pick up the dice
            
            //Track the dice with the ray
            
            //When mouse is let go of
            
            //Drop the dice
        }

        public void OnDrop()
        {
            if(currentDice == null) return;

            var closestDiceSlot = _diceSlots.First(x =>
                Vector3.Distance(currentDice.transform.position, x.transform.position) < releaseThreshold);
            
            if (closestDiceSlot != null)
            {
                currentDice.SetAnchor(closestDiceSlot.transform, true);
                closestDiceSlot.AddDiceToSlot(currentDice);
                return;
            }
            
            _diceManager.AddDiceToTraySlot(currentDice);
        }
    }
}
