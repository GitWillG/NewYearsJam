using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "CreateMonster", menuName = "Assets/Create/createMonster", order = 1)]
    public class NPMonster : ScriptableObject
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

        
    }
}
