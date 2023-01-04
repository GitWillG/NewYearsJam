using System.Collections.Generic;
using DiceGame.ScriptableObjects;
using UnityEngine;

namespace DiceGame
{
    //Spawn monsters, invoke effects on monster turns, manage monster pool, handle end encounter.
    public class MonsterManager : MonoBehaviour
    {
        [SerializeField] private List<MonsterSO> encounterMembers;
        private TurnManager turnOrder;
        [SerializeField] private string encounterName;
        [SerializeField] private string monsterIntent;
        [SerializeField] private int attackDamage;

        private int currentTurn = 0;
        public List<MonsterSO> EncounterMembers 
        { 
            get => encounterMembers; 
            set => encounterMembers = value; 
        }

        private void Start()
        {
            turnOrder = GameObject.FindObjectOfType<TurnManager>();
        }
        private void Update()
        {
            TakeMonsterAction();
        }

        //[SerializeField] private List<MonsterSO> monsterPool = new List<MonsterSO>();

        public string EncounterName => encounterName;

        public TurnManager TurnOrder => turnOrder;
        
        public void InitializeMonsters()
        {
            //foreach (var monsterSO in monsterPool)
            //{
            //    //Spawn a monster prefab
            //    //Call some function in the script and pass along the MonsterSO.
            //    //In the script, make this monster class look like the MonsterSO
            //}
        }
        //Figure out how to deal damage to monster??
        //When monster dies, check if all monsters are dead
        //If so call EndEncounter.
     
        //maybe belongs in encounter manager?
        public void EndEncounter()
        {
            
        }
        public void TakeMonsterAction()
        {
            if (!turnOrder.IsPlayerTurn)
            {
                //TODO: do attack
                Debug.Log("deal" + attackDamage + "damage");
                if (currentTurn < EncounterMembers.Count)
                {
                    currentTurn++;
                }
                else
                {
                    currentTurn = 0;
                    turnOrder.EndTurn();
                }
                currentTurn++;
            }

        }

    }
}
