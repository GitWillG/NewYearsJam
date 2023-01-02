using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class EnemyManager : MonoBehaviour
    {

        [SerializeField] private string encounterName;
        [SerializeField] private TurnManager turnOrder;
        [SerializeField] private string monsterIntent;
        [SerializeField] private int attackDamage;
        [SerializeField] private int lifePool;
        private bool isAlive;

        [SerializeField] private List<NPMonster> monsterPool = new List<NPMonster>();


        public string EncounterName => encounterName;
        public int AttackDamage
        {
            get => attackDamage;
            set => attackDamage = value;
        }

        public int LifePool => lifePool;

        public TurnManager TurnOrder => turnOrder;

        // Start is called before the first frame update
        void Start()
        {
            foreach (NPMonster monster in monsterPool)
            {
                lifePool += monster.LifeMod;
            }
            
        
        }

        // Update is called once per frame
        void Update()
        {
            if (!isAlive)
            {
                endEncounter();
            }
        
        }
        public void endEncounter()
        {

        }
    }
}
