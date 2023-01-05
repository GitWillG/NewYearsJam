using DiceGame.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiceGame.Dice
{
    public class DiceManager : MonoBehaviour
    {
        private DiceRoller _diceRoller;
        private UIManager _uiManager;
       
        [SerializeField] private float tweenDuration = 100f;
        [SerializeField] private Transform[] diceTray = new Transform[5];
        [SerializeField] private Camera diceCam;

        private List<DiceFace> _rolledDice = new List<DiceFace>();
        private List<DiceFace> _selectedDice = new List<DiceFace>();
        private Dictionary<Transform, DiceFace> _diceTraySlotToFaceDictionary = new Dictionary<Transform, DiceFace>();


        private bool _shouldRaycast;

        //public int SelectedVal { get; private set; } <- uniused
        public DiceFace SelectedDie { get; set; }

        public HeroSO CharacterSoStats { get; set; }

        public DiceFace HoveredDie { get; set; }
        
        public Transform[] DiceTray => diceTray;
        public Transform FirstAvailableDiceTrayTransform => _diceTraySlotToFaceDictionary.First(x => x.Value == null).Key;

        public List<DiceFace> SelectedDice 
        { 
            get => _selectedDice; 
            set => _selectedDice = value; 
        }
        public List<DiceFace> RolledDice 
        { 
            get => _rolledDice; 
            set => _rolledDice = value; 
        }

        public bool ShouldRaycast
        {
            get => _shouldRaycast;
            set => _shouldRaycast = value;
        }

        public Dictionary<Transform, DiceFace> DiceSlotToFaceDictionary
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
            SelectedDie.isInTray = true;
            RolledDice.Remove(SelectedDie);
            SelectedDice.Add(SelectedDie);
            
            SelectedDie.GetComponent<Rigidbody>().isKinematic = true;

            AddDiceToTraySlot(SelectedDie);
            
            DestroyAllDiceAndCleanList(ref _rolledDice);
        }
        
        public void ConfirmAllDice()
        {
            StopAllCoroutines();
            DestroyAllDiceAndCleanList(ref _selectedDice);
            foreach (var key in _diceTraySlotToFaceDictionary.Keys.ToList())
            {
                _diceTraySlotToFaceDictionary[key] = null;
            }
        }

        public void RemoveFromDiceTray(DiceFace diceFace)
        {
            bool exists = _diceTraySlotToFaceDictionary.Any(x => x.Value == diceFace);

            if (!exists) return;
            
            var diceSlot =  _diceTraySlotToFaceDictionary.First(x => x.Value == diceFace).Key;
            
            if(diceSlot == null) return; 
            
            _diceTraySlotToFaceDictionary[diceSlot] = null;
        }

        public Transform AddDiceToTraySlot(DiceFace diceFace)
        {
            Debug.Log("AddDiceToTraySlot");
            var emptyDiceTransform = FirstAvailableDiceTrayTransform;
            _diceTraySlotToFaceDictionary[emptyDiceTransform] = diceFace;
            diceFace.SetAnchor(emptyDiceTransform);

            return emptyDiceTransform;
        }

        public void DestroyAllDiceAndCleanList(ref List<DiceFace> diceFaces)
        {
            foreach (DiceFace dice in diceFaces)
            {
                dice.DestroyDice();
            }
            diceFaces.Clear();
        }
        
        private void DiceSelection()
        {
            Debug.Log("Raycasting for dice in Dice Manager");
            var ray = diceCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (HoveredDie != null)
                {
                    HoveredDie.HoverOnDice(false);
                }

                if (!hit.collider.gameObject.CompareTag("Dice")) return;

                HoveredDie = hit.collider.gameObject.GetComponent<DiceFace>();
                if (!HoveredDie.isInTray)
                {
                    HoveredDie.HoverOnDice(true);
                }
                //TODO: hover selector
                if (!Input.GetMouseButtonUp(0) || HoveredDie.isInTray || !HoveredDie.IsResultFound) return;
                
                if (SelectedDie != null)
                {
                    SelectedDie.RemoveHighlight();
                }
                if (HoveredDie != SelectedDie)
                {
                    SelectedDie = hit.collider.gameObject.GetComponent<DiceFace>();
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
