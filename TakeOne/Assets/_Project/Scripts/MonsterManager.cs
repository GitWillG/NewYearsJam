using System.Collections;
using System.Collections.Generic;
using DiceGame.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace DiceGame
{
    //Spawn monsters, invoke effects on monster turns, manage monster pool, handle end encounter.
    public class MonsterManager : MonoBehaviour
    {
        private List<MonsterSO> encounterMembers = new List<MonsterSO>();
        private Object[] allMonsters;
        [SerializeField] private List<Transform> spawnLocations = new List<Transform>();
        [SerializeField] private List<Transform> diceHolderSpawn = new List<Transform>();
        private TurnManager turnOrder;
        [SerializeField] private string encounterName;
        [SerializeField] private string monsterIntent;
        [SerializeField] private int attackDamage;
        private PartyManager _partyManager;
        [SerializeField] private Monster monster;
        [SerializeField] private List<Monster> spawnedMonsters = new List<Monster>();


        private int currentTurn = 0;
        public List<MonsterSO> EncounterMembers 
        { 
            get => encounterMembers; 
            set => encounterMembers = value; 
        }
        public string EncounterName => encounterName;

        public TurnManager TurnOrder => turnOrder;

        public List<Transform> SpawnLocations { get => spawnLocations; set => spawnLocations = value; }
        public List<Monster> SpawnedMonsters { get => spawnedMonsters; set => spawnedMonsters = value; }

        private void Awake()
        {
            //allMonsters.Add((MonsterSO)AssetDatabase.LoadAssetAtPath("Assets/_Project/Scriptable Objects Assets/Monsters", typeof(MonsterSO)));
            allMonsters = Resources.LoadAll("Monsters", typeof(MonsterSO));
        }

        private void Start()
        {
            //monster = GameObject.FindObjectOfType<Monster>();
            _partyManager = GameObject.FindObjectOfType<PartyManager>();
            turnOrder = GameObject.FindObjectOfType<TurnManager>();
            spawnLocations = new List<Transform>();
            foreach (GameObject spawns in GameObject.FindGameObjectsWithTag("MonsterSpawn"))
            {
                spawnLocations.Add(spawns.transform);
            }
            foreach (GameObject holder in GameObject.FindGameObjectsWithTag("HolderSpawn"))
            {
                diceHolderSpawn.Add(holder.transform);
            }

            CreateEncounter();

        }

       

        public void InitializeMonsters()
        {
            for (int i=0; i<EncounterMembers.Count; i++)
            {
                var newMonster = Instantiate(monster).GetComponent<Monster>();
                newMonster.InitializeMonster(encounterMembers[i], spawnLocations[i], diceHolderSpawn[i], this);
                SpawnedMonsters.Add(newMonster);
                //Spawn a monster prefab
                //Call some function in the script and pass along the MonsterSO.
                //In the script, make this monster class look like the MonsterSO
            }
        }
        //Figure out how to deal damage to monster??
        //When monster dies, check if all monsters are dead
        //If so call EndEncounter.
     
        //maybe belongs in encounter manager?
        public void EndEncounter()
        {
            StopAllCoroutines();
            ProgressTurn();
            CreateEncounter();
            
        }

        public void ProgressTurn()
        {                
            if (currentTurn < EncounterMembers.Count-1)
            {
                currentTurn++;
                StartCoroutine(PlayAnimations(1));
            }
            else
            {
                currentTurn = 0;
                turnOrder.EndTurn();
            }
            //currentTurn++;
        }

        public IEnumerator PlayAnimations(float duration)
        {
            yield return new WaitForSeconds(duration);
            int DamageToDeal = encounterMembers[currentTurn].Damage;
            Debug.Log(encounterMembers[currentTurn] + "dealt " + DamageToDeal);
            _partyManager.LifePool -= DamageToDeal;// make a forloop
            if (_partyManager.LifePool <= 0) /// <- refactor rahul
            {
                _partyManager.TPK();
            }
            ProgressTurn();
        }

        [ContextMenu("create encounter")]
        public void CreateEncounter()
        {
            if (encounterMembers != null){
                encounterMembers.Clear();
            }
            int PartySize = Random.Range(1, 4);
            for (int i = 0; i< PartySize; i++)
            {
                int PickMonster = Random.Range(0, allMonsters.Length);
                encounterMembers.Add((MonsterSO)allMonsters[PickMonster]);
            }
            InitializeMonsters();


        }
        public void MonsterTakeDamage()
        {
            for (int i = 0; i < SpawnedMonsters.Count; i++)
            {
                    Monster aliveMonster = SpawnedMonsters[i];
                    aliveMonster.TryDealDamage();
            }

        }
        public void removeDead(Monster monster)
        {
            SpawnedMonsters.Remove(monster);
            encounterMembers.Remove(monster.MonsterSo);
            if (SpawnedMonsters.Count == 0)
            {
                EndEncounter();
            }

        }


    }
}
