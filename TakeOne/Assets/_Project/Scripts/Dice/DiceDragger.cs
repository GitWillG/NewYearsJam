using System.Collections.Generic;
using System.Linq;
using DiceGame.ScriptableObjects;
using UnityEngine;

namespace DiceGame.Dice
{ 
    public class DiceDragger : MonoBehaviour
    {
        [SerializeField] private float releaseThreshold;
        [SerializeField] private Camera diceCam;
        [SerializeField] private DiceSlotContainerSO diceSlotCollectionSO;
        
        private List<DiceSlot> DiceSlots => diceSlotCollectionSO.DiceSlots.ToList();

        private DiceManager _diceManager;
        private DiceFace _currentDice;
        
        private void Awake()
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
