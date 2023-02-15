using DiceGame.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using DiceGame.Managers;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

namespace DiceGame.Dice
{
    /// <summary>
    /// Tracks and manages the lifecycle of all dice
    /// </summary>
    public class DiceManager : MonoBehaviour
    {
        public UnityEvent<List<DiceController>> onDiceRolled;
        
        [SerializeField] private Transform[] diceTray = new Transform[5];
        [SerializeField] private MMF_Player onConfirmHoverFeedback;

        private DiceRoller _diceRoller;
        private DiceSelector _diceSelector;
        private RelicManager _relicManager;
        private PartyManager _partyManager;
        
        private List<DiceController> _rolledDice = new List<DiceController>();
        private List<DiceController> _selectedDice = new List<DiceController>();
        
        private Dictionary<Transform, DiceController> _TrayToDiceController = new Dictionary<Transform, DiceController>();
        
        private bool _shouldRaycast;

        public DiceController SelectedDie => _diceSelector.SelectedDie;
        public HeroSO CharacterSoStats { get; set; }
        public Transform[] DiceTray => diceTray;
        public Transform FirstAvailableDiceTrayTransform => _TrayToDiceController.First(x => x.Value == null).Key;
        public bool HasAvailableTrySlot => _TrayToDiceController.Any(x => x.Value == null);

        public bool HasUnUsedDice => _TrayToDiceController.Any(x => x.Value != null);

        public List<DiceController> SelectedDice => _selectedDice;
        public List<DiceController> RolledDice => _rolledDice;

        private void Start()
        {
            AssignReferences();

            InitializeDictionary();
        }

        private void AssignReferences()
        {
            _diceRoller = FindObjectOfType<DiceRoller>();
            _diceSelector = GetComponent<DiceSelector>();
            _relicManager = FindObjectOfType<RelicManager>();
            _partyManager = FindObjectOfType<PartyManager>();
        }
        
        private void InitializeDictionary()
        {
            foreach (var diceTrayTransform in diceTray)
            {
                if (!_TrayToDiceController.ContainsKey(diceTrayTransform))
                {
                    _TrayToDiceController.Add(diceTrayTransform, null);
                }
            }
        }
        
        //Rolls x number of dice based on data
        public void RollDice()
        {
            foreach (var diceSo in CharacterSoStats.CharacterDice)
            {
                _rolledDice.Add(_diceRoller.RollDie(CharacterSoStats, diceSo));
            }
            _relicManager.OnDiceRolled(_partyManager, _rolledDice);
            onDiceRolled?.Invoke(_rolledDice);
        }

        //Logic for selecting a dice for later use
        public void AddCurrentDiceToTray()
        {
            if(SelectedDie == null) return;
            
            SelectedDie.RemoveHighlight();
            SelectedDie.IsInTray = true;
            _rolledDice.Remove(SelectedDie);
            SelectedDice.Add(SelectedDie);
            
            AddDiceToTraySlot(SelectedDie);
            
            DestroyAllDiceAndCleanList(ref _rolledDice);
        }

        public void OnHoverConfirm()
        {
            if (HasUnUsedDice)
            {
                onConfirmHoverFeedback?.PlayFeedbacks();
            }
        }
        
        //Cleans up any extra dice that are not used at the end of player turn
        public void ConfirmAllDice()
        {
            StopAllCoroutines();
            DestroyAllDiceAndCleanList(ref _selectedDice, true); // Added this extra bool cause selected dice are currently being destroyed by the dice slot 
            foreach (var key in _TrayToDiceController.Keys.ToList())
            {
                _TrayToDiceController[key] = null;
            }
        }

        public void RemoveFromDiceTray(DiceController diceFace)
        {
            bool exists = _TrayToDiceController.Any(x => x.Value == diceFace);

            if (!exists) return;
            
            var diceSlot =  _TrayToDiceController.First(x => x.Value == diceFace).Key;
            
            if(diceSlot == null) return; 
            
            _TrayToDiceController[diceSlot] = null;
        }

        public Transform AddDiceToTraySlot(DiceController diceFace)
        {
            if (!HasAvailableTrySlot) return null;
            
            var emptyDiceTransform = FirstAvailableDiceTrayTransform;
            _TrayToDiceController[emptyDiceTransform] = diceFace;
            diceFace.SetAnchor(emptyDiceTransform);

            return emptyDiceTransform;
        }

        //Cleanup logic
        private static void DestroyAllDiceAndCleanList(ref List<DiceController> diceControllers, bool destroyIfInTray = false)
        {
            if (!destroyIfInTray)
            {
                foreach (DiceController dice in diceControllers)
                {
                    dice.DestroyDice();
                }
            }
            else
            {
                foreach (var dice in diceControllers.Where(dice => dice.IsInTray))
                {
                    dice.DestroyDice();
                }
            }

            diceControllers.Clear();
        }
    }
}
