using System.Collections.Generic;
using DiceGame.ScriptableObjects.Dice;
using DiceGame.Utility;
using UnityEngine;

namespace DiceGame.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CreateCharacter", menuName = "Assets/Create/CreateCharacter", order = 1)]
    public class HeroSO : ScriptableObject, IDiceOwner
    {
        [SerializeField] private string characterName;
        [SerializeField] private int healthContribution;
        [SerializeField] private List<DiceSO> characterDice;
        [SerializeField] private GameObject attackEffectPrefab;
        [SerializeField] private Sprite visual;
        
        public int HealthContribution => healthContribution;
        public string CharacterName => characterName;
        public GameObject AttackEffectPrefab => attackEffectPrefab;
        
        public Sprite Visual => visual;
        public List<DiceSO> CharacterDice => characterDice;
    }
}
