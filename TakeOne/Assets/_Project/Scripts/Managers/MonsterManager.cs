using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceGame.ScriptableObjects;
using UnityEngine;

namespace DiceGame.Managers
{
    //Spawn monsters, invoke effects on monster turns, manage monster pool, handle end encounter.
    public class MonsterManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> spawnLocations = new List<Transform>();
        [SerializeField] private List<Transform> diceHolderSpawn = new List<Transform>();
        [SerializeField] private string encounterName;
        [SerializeField] private string monsterIntent;
        [SerializeField] private int attackDamage;
        [SerializeField] private Monster monster;
        [SerializeField] private List<Monster> spawnedMonsters = new List<Monster>();

        private TurnManager _turnOrder;
        private PartyManager _partyManager;
        private List<MonsterSO> _encounterMembers = new List<MonsterSO>();
        private MonsterSO[] _allMonsters;

        private int _currentTurn = 0;
        
        public List<MonsterSO> EncounterMembers => _encounterMembers;
        public string EncounterName => encounterName;
        
        public List<Monster> SpawnedMonsters => spawnedMonsters;

        private void Awake()
        {
            _allMonsters = Resources.LoadAll("Monsters", typeof(MonsterSO)).Cast<MonsterSO>().ToArray();
        }

        private void Start()
        {
            //monster = GameObject.FindObjectOfType<Monster>();
            _partyManager = FindObjectOfType<PartyManager>();
            _turnOrder = FindObjectOfType<TurnManager>();
            
            spawnLocations = new List<Transform>();
            
            //TODO: This should probably be just set in the editor so we don't do this call each time.
            //Maybe some sort of editor extension that does this functionality as is, so it only runs in editor once and not during runtime in the build.
            //Generally bad practice to do FindObject for anything tbh...
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

        private void InitializeMonsters()
        {
            for (int i=0; i<EncounterMembers.Count; i++)
            {
                var newMonster = Instantiate(monster).GetComponent<Monster>();
                
                newMonster.InitializeMonster(_encounterMembers[i], spawnLocations[i], diceHolderSpawn[i], this);
                
                SpawnedMonsters.Add(newMonster);
            }
        }

        //maybe belongs in encounter manager?
        private void EndEncounter()
        {
            StopAllCoroutines();
            
            _currentTurn = 0;
            _turnOrder.EndTurn();
            
            CreateEncounter();
        }

        public void ProgressTurn()
        {                
            if (_currentTurn < EncounterMembers.Count)
            {
                StartCoroutine(PlayAnimations(1));
                return;
            }
            
            StopAllCoroutines();
            
            _currentTurn = 0;
            _turnOrder.EndTurn();
        }

        public IEnumerator PlayAnimations(float duration)
        {
            yield return new WaitForSeconds(duration);
            
            int damageToDeal = _encounterMembers[_currentTurn].Damage;
            
            Debug.Log(_encounterMembers[_currentTurn] + "Tries to deal : " + damageToDeal);
            
            _partyManager.TryDealDamage(damageToDeal);
            
            _currentTurn++;
            ProgressTurn();
        }

        [ContextMenu("create encounter")]
        public void CreateEncounter()
        {
            if (_encounterMembers != null)
            {
                _encounterMembers.Clear();
            }

            _encounterMembers = new List<MonsterSO>();
            
            int partySize = Random.Range(1, 4);
            
            for (int i = 0; i< partySize; i++)
            {
                int pickMonster = Random.Range(0, _allMonsters.Length);
                _encounterMembers.Add((MonsterSO)_allMonsters[pickMonster]);
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
        
        public void RemoveDead(Monster currentMonster)
        {
            SpawnedMonsters.Remove(currentMonster);
            _encounterMembers.Remove(currentMonster.MonsterSo);
            
            if (SpawnedMonsters.Count == 0)
            {
                EndEncounter();
            }
        }
    }
}
