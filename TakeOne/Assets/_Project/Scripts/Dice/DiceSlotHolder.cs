using System.Collections.Generic;
using System.Linq;
using DiceGame.ScriptableObjects;
using DiceGame.ScriptableObjects.Dice;
using DiceGame.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace DiceGame.Dice
{
    public class DiceSlotHolder : MonoBehaviour, ICollectionElement<DiceSlotHolder>
    {
        [SerializeField] private List<Transform> diceSlotTransforms = new List<Transform>();
        [SerializeField] private  DiceSlotHolderCollection diceSlotHolderCollection;

        public UnityEvent OnAwake, OnAttachToSlot, OnDetachFromSlot;
        
        private List<DiceController> _diceControllers = new List<DiceController>();

        private Dictionary<Transform, DiceController> _diceSlotToFaceDictionary = new Dictionary<Transform, DiceController>();

        public CollectionExposerSO<DiceSlotHolder> CollectionReference
        {
            get => diceSlotHolderCollection;
            set => diceSlotHolderCollection = (DiceSlotHolderCollection)value;
        }

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
            
            ((ICollectionElement<DiceSlotHolder>)this).Register();
            OnAwake?.Invoke();
        }
        
        public void AddDiceToSlot(DiceController diceController)
        {
            var hasFreeSlot = _diceSlotToFaceDictionary.Any(x => x.Value == null);

            if (!hasFreeSlot)
            {
                //No Free Slot Fuck off.
                return;
            }
            
            if (_diceControllers.Contains(diceController)) return; // Already have this dice? Wth?

            var emptyDiceSlot = _diceSlotToFaceDictionary.First(x => x.Value == null).Key;
            if (emptyDiceSlot == null) return;

            _diceSlotToFaceDictionary[emptyDiceSlot] = diceController;
            _diceControllers.Add(diceController);
            diceController.CurrentSlotHolder = this;
            diceController.SetAnchor(emptyDiceSlot, false, true);
            OnAttachToSlot?.Invoke();
        }
        
        public void RemoveFromDiceSlot(DiceController diceController)
        {
            if (!_diceControllers.Contains(diceController)) return;
            
            _diceControllers.Remove(diceController);

            var slotForDice = _diceSlotToFaceDictionary.First(x => x.Value == diceController).Key;
            _diceSlotToFaceDictionary[slotForDice] = null;
            OnDetachFromSlot?.Invoke();
            
            var diceSlot = slotForDice.GetComponent<DiceSlot>();
            diceSlot.onDetachFromSlot?.Invoke();
        }
        
        public List<int> GetDiceResults()
        {
            var returnList = new List<int>();
            foreach (var diceController in _diceControllers)
            {
                returnList.Add(diceController.FaceValue);
            }
            
            for (var i = 0; i < _diceControllers.Count; i++)
            {
                var diceFace = _diceControllers[i];
                diceFace.UseDice();
            }

            _diceControllers = new List<DiceController>();
            foreach (var key in _diceSlotToFaceDictionary.Keys.ToList())
            {
                _diceSlotToFaceDictionary[key] = null;
            }
            return returnList;
        }

        private void OnDestroy()
        {
            ((ICollectionElement<DiceSlotHolder>)this).UnRegister();
        }
    }
}