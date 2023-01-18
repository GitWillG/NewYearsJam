using UnityEngine;

namespace DiceGame.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CreateCharacter", menuName = "Assets/Create/CreateCharacter", order = 1)]
    public class HeroSO : ScriptableObject
    {
        [SerializeField] private string characterName;
        [SerializeField] private int numOfDice;
        [SerializeField] private int healthContribution;
        [SerializeField] private GameObject diePrefab;
        [SerializeField] private GameObject attackEffectPrefab;
        [SerializeField] private Sprite visual;
        public int HealthContribution => healthContribution;
        public string CharacterName => characterName;
        public GameObject DiePrefab => diePrefab;
        //TODO: Refactor this to work with multiple types of dice and new dice
        
        public GameObject AttackEffectPrefab => attackEffectPrefab;


        public Sprite Visual 
        { 
            get => visual; 
            set => visual = value; 
        }
        public int NumOfDice
        {
            get => numOfDice;
            set => numOfDice = value;
        }

    }
}
