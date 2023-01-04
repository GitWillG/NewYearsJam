using DiceGame.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DiceGame.Dice
{
    public class DiceManager : MonoBehaviour
    {
        private DiceRoller diceRoller;
        private UIManager uiManager;
        [SerializeField] private float tweenDuration = 100f;
        [SerializeField] private Transform[] diceTray = new Transform[5];
        [SerializeField] private Camera diceCam;

        private List<GameObject> _rolledDice = new List<GameObject>();
        private List<GameObject> _selectedDice = new List<GameObject>();

        //public int SelectedVal { get; private set; } <- uniused
        public DiceFace SelectedDie { get; set; }

        public HeroSO CharacterSoStats { get; set; }

        public DiceFace HoveredDie { get; set; }
        
        public Transform[] DiceTray => diceTray;

        public List<GameObject> SelectedDice 
        { 
            get => _selectedDice; 
            set => _selectedDice = value; 
        }
        public List<GameObject> RolledDice 
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

            var lerpEndPoint = DiceTray[SelectedDice.Count - 1].transform.position;
            var rotation = Dice.transform.rotation;
            var lerpEndRotation = Quaternion.Euler(new Vector3(rotation.eulerAngles.x, 0, rotation.eulerAngles.z));
            
            StartCoroutine(LerpTowards(Dice, lerpEndPoint, lerpEndRotation, tweenDuration));

            foreach(GameObject dice in RolledDice)
            {
                Destroy(dice);
            }
        }

        private void Start()
        {
            uiManager = GameObject.FindObjectOfType<UIManager>();
            diceRoller = GameObject.FindObjectOfType<DiceRoller>();
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
                    uiManager.ConfirmDice.SetActive(true);
                }
                else
                {
                    SelectedDie.RemoveHighlight();
                    SelectedDie = null;
                    uiManager.ConfirmDice.SetActive(false);
                }
            }
        }
    }
}
