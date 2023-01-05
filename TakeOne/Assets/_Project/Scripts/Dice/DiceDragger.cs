using System;
using System.Collections.Generic;
using System.Linq;
using DiceGame.Dice;
using DiceGame.ScriptableObjects;
using UnityEngine;

namespace DiceGame
{ 
    public class DiceDragger : MonoBehaviour
    {
        [SerializeField] private DiceFace currentDice;
        [SerializeField] private float releaseThreshold;
        [SerializeField] private Camera diceCam;
        [SerializeField] private DiceSlotContainerSO diceSlotCSO;


        private List<DiceSlot> _diceSlots => diceSlotCSO.DiceSlots.ToList();

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
          
            if (Input.GetKey(KeyCode.Mouse0))
            {
                mouseTracking();
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                OnDrop();
            }
           
            
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
        public void mouseTracking()
        {
            Debug.Log("Raycasting for dice in Dice dragger");
            var ray = diceCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (!hit.collider.gameObject.CompareTag("Dice")) return;
                currentDice = hit.collider.gameObject.GetComponent<DiceFace>();
                OnPickUp();
                currentDice.transform.position = new Vector3(hit.transform.position.x, currentDice.transform.position.y, hit.transform.position.z);
            }
        }
    }
}
