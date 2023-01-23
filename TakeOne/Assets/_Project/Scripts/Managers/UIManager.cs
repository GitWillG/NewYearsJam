using UnityEngine;

namespace DiceGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject confirmDice;
        [SerializeField] private GameObject rollDice;
        [SerializeField] private GameObject confirmAll;
        [SerializeField] private GameObject restartGame;

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
    }
}
