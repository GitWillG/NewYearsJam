using System;
using System.Collections.Generic;
using System.Linq;
using DiceGame.Dice;
using DiceGame.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace DiceGame
{ 
    public class DiceDragger : MonoBehaviour
    {
        [SerializeField] private float releaseThreshold;
        [SerializeField] private Camera diceCam;
        [FormerlySerializedAs("diceSlotCSO")] [SerializeField] private DiceSlotContainerSO diceSlotCollectionSO;
        
        private List<DiceSlot> DiceSlots => diceSlotCollectionSO.DiceSlots.ToList();

        private DiceManager _diceManager;
        private DiceFace _currentDice;


        private void Awake()
        {
            _diceManager = FindObjectOfType<DiceManager>();
        }

        public void OnPickUp()
        {
            if(_currentDice == null) return;
            _diceManager.RemoveFromDiceTray(_currentDice);

            if (_currentDice.IsInSlot)
            {
                //remove from that slot
                _currentDice.DetachFromSlot();
            }

        }

        private void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                SetCurrentDice();
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                MouseTracking();
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                OnDrop();
                _currentDice = null;
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
            if(_currentDice == null) return;

            var any = DiceSlots.Any(x =>
                Vector3.Distance(_currentDice.transform.position, x.transform.position) < releaseThreshold);

            if (any)
            {
                var closestDiceSlot = DiceSlots.First(x =>
                    Vector3.Distance(_currentDice.transform.position, x.transform.position) < releaseThreshold);
                if (closestDiceSlot != null && closestDiceSlot.HasSlotAvailable)
                {
                    _currentDice.SetAnchor(closestDiceSlot.transform, true);
                    closestDiceSlot.AddDiceToSlot(_currentDice);
                    return;
                }
            }
            
            _diceManager.AddDiceToTraySlot(_currentDice);
        }
        
        public void MouseTracking()
        {
            if(_currentDice == null) return;
            
            var ray = diceCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                var currentDiceTransform = _currentDice.transform;
                
                var trackPoint = new Vector3(hit.point.x, currentDiceTransform.position.y, hit.point.z);
                
                currentDiceTransform.position = trackPoint;
            }
        }

        public void SetCurrentDice()
        {
            var ray = diceCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (!hit.collider.gameObject.CompareTag("Dice")) return;
                _currentDice = hit.collider.gameObject.GetComponent<DiceFace>();
                OnPickUp();
            }
            else
            {
                _currentDice = null;
            }
        }
    }
}
