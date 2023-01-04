using DiceGame.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
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

        //public int SelectedVal { get; private set; } <- uniused
        public DiceFace SelectedDie { get; set; }

        public HeroSO CharacterSoStats { get; set; }

        public DiceFace HoveredDie { get; set; }
        
        public Transform[] DiceTray => diceTray;

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
        

        private void Update()
        {
            DiceSelection();
        }
        
        [ContextMenu("Test Roll Dice")]
        public void RollDice()
        {
            for (int i = 0; i < CharacterSoStats.NumOfDice; i++)
            {
                _diceRoller.RollDie(CharacterSoStats.DiePrefab);
            }
        }

        public void CollectDice()
        {
            GameObject dice = SelectedDie.gameObject;
            DiceFace diceFace = dice.GetComponent<DiceFace>();
            SelectedDie.RemoveHighlight();
            SelectedDie.isInTray = true;
            SelectedDie = null;
            RolledDice.Remove(diceFace);
            SelectedDice.Add(diceFace);
            dice.GetComponent<Rigidbody>().isKinematic = true;

            var lerpEndPoint = DiceTray[SelectedDice.Count - 1].transform.position;
            var rotation = dice.transform.rotation;
            var lerpEndRotation = Quaternion.Euler(new Vector3(rotation.eulerAngles.x, 0, rotation.eulerAngles.z));
            
            StartCoroutine(LerpTowards(dice, lerpEndPoint, lerpEndRotation, tweenDuration));

            DestroyAllDiceAndCleanList(ref _rolledDice);
        }
        public void ConfirmAllDice()
        {
            //TODO: Use Dice
            StopAllCoroutines();
            DestroyAllDiceAndCleanList(ref _selectedDice);
        }

        public void DestroyAllDiceAndCleanList(ref List<DiceFace> diceFaces)
        {
            foreach (DiceFace dice in diceFaces)
            {
                dice.DestroyDice();
            }
            diceFaces.Clear();
        }
        
        private void Start()
        {
            _uiManager = GameObject.FindObjectOfType<UIManager>();
            _diceRoller = GameObject.FindObjectOfType<DiceRoller>();
        }
        public IEnumerator LerpTowards(GameObject obToLerp, Vector3 endPoint, Quaternion endRotation, float duration)
        {
            float startTime = Time.time;
            float t=0;
            float tempDuration = duration * 60f;
            
            while (Time.time < startTime + tempDuration)
            {
                t = (Time.time - startTime) / tempDuration;
                obToLerp.transform.position = Vector3.Lerp(obToLerp.transform.position, endPoint, t);
                obToLerp.transform.rotation = Quaternion.Slerp(obToLerp.transform.rotation, endRotation, t);
                yield return null;
            }
            obToLerp.transform.position = endPoint;
        }
        
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
