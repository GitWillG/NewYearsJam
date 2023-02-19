using System.Collections.Generic;
using System.Linq;
using DiceGame.ScriptableObjects;
using DiceGame.ScriptableObjects.Dice;
using UnityEngine;

namespace DiceGame.Dice
{ 
    /// <summary>
    /// Used for dragging the dice from the tray to slots.
    /// <seealso cref="DiceGame.Dice.DiceSlotHolder"/> <seealso cref="DiceGame.Dice.DiceController"/>
    /// </summary>
    public class DiceDragger : MonoBehaviour
    {
        [SerializeField] private float releaseThreshold;
        [SerializeField] private Camera diceCam;
        [SerializeField] private DiceSlotHolderCollection diceSlotCollectionCollection;
        
        private List<DiceSlotHolder> DiceSlots => diceSlotCollectionCollection.CollectionHashset.ToList();
        private DiceManager _diceManager;
        private DiceController _currentDice;
        
        private void Awake()
        {
            AssignReferences();
        }

        private void AssignReferences()
        {
            _diceManager = FindObjectOfType<DiceManager>();
        }

        private void OnPickUp()
        {
            if(_currentDice == null) return;
            
            _diceManager.RemoveFromDiceTray(_currentDice);

            _currentDice.DetachFromSlot();
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
            }
        }

        private void OnDrop()
        {
            if(_currentDice == null) return;
            
            if (_currentDice.IsInSlot)
            {
                _currentDice.DetachFromSlot();
            }
            
            var position = _currentDice.transform.position;
            Vector2 currentDiceVector2Pos = new Vector2(position.x, position.z);
            
            //Checks if there are any slots within range
            var any = DiceSlots.Any(x => Vector2.Distance(currentDiceVector2Pos, new Vector2(x.transform.position.x, x.transform.position.z)) < releaseThreshold);

            if (any)
            {
                var closestDiceSlot = DiceSlots.First(x => Vector2.Distance(currentDiceVector2Pos, new Vector2(x.transform.position.x, x.transform.position.z)) < releaseThreshold);
                
                if (closestDiceSlot != null && closestDiceSlot.HasSlotAvailable)
                {
                    closestDiceSlot.AddDiceToSlot(_currentDice);
                    _currentDice = null;
                    return;
                }
            }
            
            _diceManager.AddDiceToTraySlot(_currentDice);
            _currentDice = null;
        }

        //Tracks the given dice along the mouse position
        private void MouseTracking()
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

        private void SetCurrentDice()
        {
            var ray = diceCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (!hit.collider.CompareTag("Dice")) return;
                _currentDice = hit.collider.GetComponent<DiceController>();
                if (!_currentDice.IsSelectable)
                {
                    _currentDice = null;
                    return;
                }
                OnPickUp();
            }
            else
            {
                _currentDice = null;
            }
        }
    }
}
