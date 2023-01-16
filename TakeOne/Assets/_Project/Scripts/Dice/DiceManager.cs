using DiceGame.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiceGame.Dice
{
    public class DiceManager : MonoBehaviour
    {
        private DiceRoller _diceRoller;
        private UIManager _uiManager;
       
        [SerializeField] private Transform[] diceTray = new Transform[5];
        [SerializeField] private Camera diceCam;

        private List<DiceController> _rolledDice = new List<DiceController>();
        private List<DiceController> _selectedDice = new List<DiceController>();
        private Dictionary<Transform, DiceController> _diceTraySlotToFaceDictionary = new Dictionary<Transform, DiceController>();


        private bool _shouldRaycast;

        //public int SelectedVal { get; private set; } <- uniused
        public DiceController SelectedDie { get; set; }

        public HeroSO CharacterSoStats { get; set; }

        public DiceController HoveredDie { get; set; }
        
        public Transform[] DiceTray => diceTray;
        public Transform FirstAvailableDiceTrayTransform => _diceTraySlotToFaceDictionary.First(x => x.Value == null).Key;
        public bool HasAvailableTrySlot => _diceTraySlotToFaceDictionary.Any(x => x.Value == null);

        public List<DiceController> SelectedDice 
        { 
            get => _selectedDice; 
            set => _selectedDice = value; 
        }
        public List<DiceController> RolledDice 
        { 
            get => _rolledDice; 
            set => _rolledDice = value; 
        }

        public bool ShouldRaycast
        {
            get => _shouldRaycast;
            set => _shouldRaycast = value;
        }

        public Dictionary<Transform, DiceController> DiceSlotToFaceDictionary
        {
            get => _diceTraySlotToFaceDictionary;
            set => _diceTraySlotToFaceDictionary = value;
        }

        private void Start()
        {
            _uiManager = FindObjectOfType<UIManager>();
            _diceRoller = FindObjectOfType<DiceRoller>();

            foreach (var diceTrayTransform in diceTray)
            {
                if (!_diceTraySlotToFaceDictionary.ContainsKey(diceTrayTransform))
                {
                    _diceTraySlotToFaceDictionary.Add(diceTrayTransform, null);
                }
            }
        }
        
        private void Update()
        {
            if(!_shouldRaycast) return;
            
            DiceSelection();
        }
        
        public void RollDice()
        {
            for (int i = 0; i < CharacterSoStats.NumOfDice; i++)
            {
                _diceRoller.RollDie(CharacterSoStats.DiePrefab);
            }
        }

        public void AddCurrentDiceToTray()
        {
            if(SelectedDie == null) return;
            
            SelectedDie.RemoveHighlight();
            SelectedDie.IsInTray = true;
            RolledDice.Remove(SelectedDie);
            SelectedDice.Add(SelectedDie);
            
            SelectedDie.GetComponent<Rigidbody>().isKinematic = true;

            AddDiceToTraySlot(SelectedDie);
            
            DestroyAllDiceAndCleanList(ref _rolledDice);
        }
        
        public void ConfirmAllDice()
        {
            StopAllCoroutines();
            DestroyAllDiceAndCleanList(ref _selectedDice, true); // Added this extra bool cause selected dice are currently being destroyed by the dice slot 
            foreach (var key in _diceTraySlotToFaceDictionary.Keys.ToList())
            {
                _diceTraySlotToFaceDictionary[key] = null;
            }
        }

        public void RemoveFromDiceTray(DiceController diceFace)
        {
            bool exists = _diceTraySlotToFaceDictionary.Any(x => x.Value == diceFace);

            if (!exists) return;
            
            var diceSlot =  _diceTraySlotToFaceDictionary.First(x => x.Value == diceFace).Key;
            
            if(diceSlot == null) return; 
            
            _diceTraySlotToFaceDictionary[diceSlot] = null;
        }

        public Transform AddDiceToTraySlot(DiceController diceFace)
        {
            Debug.Log("AddDiceToTraySlot");
            if (!HasAvailableTrySlot) return null;
            
            var emptyDiceTransform = FirstAvailableDiceTrayTransform;
            _diceTraySlotToFaceDictionary[emptyDiceTransform] = diceFace;
            diceFace.SetAnchor(emptyDiceTransform);

            return emptyDiceTransform;
        }

        private void DestroyAllDiceAndCleanList(ref List<DiceController> diceControllers, bool destroyIfInTray = false)
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
                foreach (DiceController dice in diceControllers)
                {
                    if (dice.IsInTray)
                    {
                        dice.DestroyDice();
                    }
                }
            }

            diceControllers.Clear();
        }
        
        //TODO: Refactor this, very dense function
        private void DiceSelection()
        {
            var ray = diceCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (HoveredDie != null)
                {
                    HoveredDie.HoverOnDice(false);
                }

                if (!hit.collider.gameObject.CompareTag("Dice")) return;

                HoveredDie = hit.collider.gameObject.GetComponent<DiceController>();
                if (!HoveredDie.IsInTray)
                {
                    HoveredDie.HoverOnDice(true);
                }
                if (!Input.GetMouseButtonUp(0) || HoveredDie.IsInTray || !HoveredDie.IsResultFound) return;
                
                if (SelectedDie != null)
                {
                    SelectedDie.RemoveHighlight();
                }
                if (HoveredDie != SelectedDie)
                {
                    SelectedDie = hit.collider.gameObject.GetComponent<DiceController>();
                    SelectedDie.HighlightDice();
                    _uiManager.ConfirmDice.SetActive(true);
                }
                else
                {
                    SelectedDie.RemoveHighlight();
                    SelectedDie = null;
                    _uiManager.ConfirmDice.SetActive(false);
                }
            }
        }
    }
}
