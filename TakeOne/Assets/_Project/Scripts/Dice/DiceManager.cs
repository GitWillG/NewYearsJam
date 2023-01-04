using DiceGame.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DiceGame.Dice
{
    public class DiceManager : MonoBehaviour
    {
        private HeroSO characterSoStats;
        private List<GameObject> rolledDice = new List<GameObject>();
        private List<GameObject> selectedDice = new List<GameObject>();
        [SerializeField] private DiceRoller diceRoller;
        [SerializeField] private float tweenDuration = 100f;
        [SerializeField] private Transform[] diceHolder = new Transform[5];
        [SerializeField] private UIManager uiMan;
        [SerializeField] private Camera diceCam;

        private DiceFace hoveredDie;
        private DiceFace selectedDie;
        //public int SelectedVal { get; private set; } <- uniused
        public DiceFace SelectedDie 
        { 
            get => selectedDie; 
            set => selectedDie = value; 
        }
        public HeroSO CharacterSoStats 
        { 
            get => characterSoStats; 
            set => characterSoStats = value; 
        }
        public DiceFace HoveredDie 
        { 
            get => hoveredDie; 
            set => hoveredDie = value; 
        }
        public UIManager UiMan 
        { 
            get => uiMan; 
            set => uiMan = value;
        }
        public Camera DiceCam 
        { 
            get => diceCam; 
            set => diceCam = value; 
        }
        public Transform[] DiceHolder 
        { 
            get => diceHolder; 
            set => diceHolder = value; 
        }
        public List<GameObject> SelectedDice 
        { 
            get => selectedDice; 
            set => selectedDice = value; 
        }
        public List<GameObject> RolledDice 
        { 
            get => rolledDice; 
            set => rolledDice = value; 
        }

        private void Awake()
        {
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
                diceRoller.RollDie(CharacterSoStats.DiePrefab);
            }
        }

        public void CollectDice()
        {
            GameObject Dice = SelectedDie.gameObject;
            SelectedDie.RemoveHighlight();
            SelectedDie.isInTray = true;
            SelectedDie = null;
            RolledDice.Remove(Dice);
            SelectedDice.Add(Dice);
            Dice.GetComponent<Rigidbody>().isKinematic = true;

            StartCoroutine(LerpTowards(Dice, DiceHolder[SelectedDice.Count - 1].transform.position, Quaternion.Euler(new Vector3(Dice.transform.rotation.eulerAngles.x, 0, Dice.transform.rotation.eulerAngles.z)), tweenDuration));
            //Dice.transform.rotation = Quaternion.Euler(new Vector3(Dice.transform.rotation.eulerAngles.x, 0, Dice.transform.rotation.eulerAngles.z));
           
            //Dice.transform.position = DiceHolder[SelectedDice.Count - 1].transform.position;
            foreach(GameObject dice in RolledDice)
            {
                Destroy(dice);
            }


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
            var ray = DiceCam.ScreenPointToRay(Input.mousePosition);
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
                    UiMan.ConfirmDice.SetActive(true);
                }
                else
                {
                    SelectedDie.RemoveHighlight();
                    SelectedDie = null;
                    UiMan.ConfirmDice.SetActive(false);
                }
            }
        }
    }
}
