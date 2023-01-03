using UnityEngine;

namespace DiceGame.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CreateCharacter", menuName = "Assets/Create/CreateCharacter", order = 1)]
    public class HeroSO : ScriptableObject
    {
        [SerializeField] private string characterName;
        [SerializeField] private int numOfDice;
        [SerializeField] private int lifeMod;
        [SerializeField] private GameObject diePrefab;
        private Sprite visual;

        public string CharacterName => characterName;

        public int NumOfDice
        {
            get => numOfDice;
            set => numOfDice = value;
        }

        public int LifeMod => lifeMod;

        //TODO: Refactor this to work with multiple types of dice and new dice
        public GameObject DiePrefab => diePrefab;

        public Sprite Visual { get => visual; set => visual = value; }
    }
}
