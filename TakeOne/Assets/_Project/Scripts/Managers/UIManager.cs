using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject confirmDice;
        [SerializeField] private GameObject rollDice;
        [SerializeField] private GameObject confirmAll;
        [SerializeField] private GameObject restartGame;
        List<GameObject> UIElements = new List<GameObject>();

        public GameObject ConfirmDice => confirmDice;

        public GameObject RollDice => rollDice;

        public GameObject ConfirmAll => confirmAll;
        public GameObject RestartGame => restartGame;

        public void EnableUIElement(GameObject element)
        {
            element.SetActive(true);
        }
        public void DisableUIElement(GameObject element)
        {
            element.SetActive(false);
        }
        public void checkBeforeDisabling(GameObject element)
        {
            if (element.activeSelf == true)
            {
                UIElements.Add(element);
                element.SetActive(false);
            }
        }
        public void checkThenEnable()
        {
            foreach (GameObject uiElement in UIElements)
            {
                uiElement.SetActive(true);
            }
            UIElements.Clear();
        }

    }
}
