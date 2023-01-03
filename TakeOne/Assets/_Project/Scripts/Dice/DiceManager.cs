using DiceGame.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DiceGame.Dice
{
    public class DiceManager : MonoBehaviour
    {
        [FormerlySerializedAs("characterStats")] [SerializeField] private HeroSO characterSoStats;
        [SerializeField] private DiceRoller diceRoller;
        public List<GameObject> rolledDice;
        //public List<GameObject> RolledDice { get => rolledDice; set => rolledDice = value; }
        //public GameObject selectedDie;

        public DiceFace hoveredDie;
        private DiceFace selectedDie;

        public List<GameObject> SelectedDice;
        public Transform[] DiceHolder = new Transform[5];
        public int SelectedVal { get; private set; }
        public DiceFace SelectedDie { get => selectedDie; set => selectedDie = value; }

        public UIManager _uiMan;

        private Camera _mainCam;
        private void Awake()
        {
            _mainCam = Camera.main;
        }
        private void Update()
        {
            DiceSelection();
        }


        [ContextMenu("Test Roll Dice")]
        public void RollDice()
        {
            for (int i = 0; i < characterSoStats.NumOfDice; i++)
            {
                diceRoller.RollDie(characterSoStats.DiePrefab);
            }
        }

        public void CollectDice()
        {
            GameObject Dice = SelectedDie.gameObject;
            SelectedDie.RemoveHighlight();
            SelectedDie.isInTray = true;
            SelectedDie = null;
            rolledDice.Remove(Dice);
            SelectedDice.Add(Dice);
            Dice.GetComponent<Rigidbody>().isKinematic = true;

            StartCoroutine(LerpTowards(Dice, DiceHolder[SelectedDice.Count - 1].transform.position, Quaternion.Euler(new Vector3(Dice.transform.rotation.eulerAngles.x, 0, Dice.transform.rotation.eulerAngles.z)), 100f));
            //Dice.transform.rotation = Quaternion.Euler(new Vector3(Dice.transform.rotation.eulerAngles.x, 0, Dice.transform.rotation.eulerAngles.z));
           
            //Dice.transform.position = DiceHolder[SelectedDice.Count - 1].transform.position;
            foreach(GameObject dice in rolledDice)
            {
                Destroy(dice);
            }

        }
        public IEnumerator LerpTowards(GameObject obToLerp, Vector3 endPoint, Quaternion endRotation, float duration)
        {
            float startTime = Time.time;
            float t=0;

            while (Time.time < startTime + duration)
            {
                t = (Time.time - startTime) / duration;
                obToLerp.transform.position = Vector3.Lerp(obToLerp.transform.position, endPoint, t);
                obToLerp.transform.rotation = Quaternion.Slerp(obToLerp.transform.rotation, endRotation, t);
                yield return new WaitForEndOfFrame();
            }

            obToLerp.transform.position = endPoint;

        }
        private void DiceSelection()
        {
            var ray = _mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (!hit.collider.gameObject.CompareTag("Dice")) return;

                hoveredDie = hit.collider.gameObject.GetComponent<DiceFace>();
                //TODO: hover selector
                if (Input.GetMouseButtonUp(0) && !hoveredDie.isInTray && hoveredDie.IsResultFound)
                {
                    if (SelectedDie != null)
                    {
                        SelectedDie.RemoveHighlight();
                    }
                    if (hoveredDie != SelectedDie)
                    {
                        SelectedDie = hit.collider.gameObject.GetComponent<DiceFace>();
                        SelectedDie.HighlightDice();
                        _uiMan.confirmDice.SetActive(true);
                    }
                    else
                    {
                        SelectedDie.RemoveHighlight();
                        SelectedDie = null;
                        _uiMan.confirmDice.SetActive(false);
                    }
                }
            }
        }
    }
}
