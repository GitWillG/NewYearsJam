using UnityEngine;

namespace DiceGame.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CreateMonster", menuName = "Assets/Create/createMonster", order = 1)]
    public class MonsterSO : ScriptableObject
    {
        [SerializeField] private string monsterName;
        [SerializeField] private string monsterSkills;
        [SerializeField] private int attackDamage;
        [SerializeField] private int lifeMod;
        
        //TBD
        //[SerializeField] private Sprite visual;
        //[SerializeField] private GameObject visual;

        public string MonsterName => monsterName;
        public string MonsterSkills => monsterSkills;
        
        public int AttackDamage
        {
            get => attackDamage;
            set => attackDamage = value;
        }

        public int LifeMod => lifeMod;
        
        public bool IsAlive { get; set; }
    }
}
