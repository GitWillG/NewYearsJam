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
        [SerializeField] private Monster.Monster monster;
        [SerializeField] private List<Monster.Monster> spawnedMonsters = new List<Monster.Monster>();

        private TurnManager _turnOrder;
        private PartyManager _partyManager;
        private List<MonsterSO> _encounterMembers = new List<MonsterSO>();
        private MonsterSO[] _allMonsters;

        private List<DataToMonoBehaviour<MonsterSO, Monster.Monster>> _dataToMonoBehaviours =
            new List<DataToMonoBehaviour<MonsterSO, Monster.Monster>>();

        private int _currentTurn = 0;
        
        public List<MonsterSO> EncounterMembers => _encounterMembers;
        public string EncounterName => encounterName;
        
        public List<Monster.Monster> SpawnedMonsters => spawnedMonsters;

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
            _dataToMonoBehaviours = new List<DataToMonoBehaviour<MonsterSO, Monster.Monster>>();
            
            for (int i=0; i<EncounterMembers.Count; i++)
            {
                var newMonster = Instantiate(monster).GetComponent<Monster.Monster>();
                
                newMonster.InitializeMonster(_encounterMembers[i], spawnLocations[i], diceHolderSpawn[i], this);
                
                SpawnedMonsters.Add(newMonster);
                _dataToMonoBehaviours.Add(new DataToMonoBehaviour<MonsterSO, Monster.Monster>(_encounterMembers[i], newMonster));
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
                StartCoroutine(PlayAnimations());
                return;
            }
            
            StopAllCoroutines();
            
            _currentTurn = 0;
            _turnOrder.EndTurn();
        }

        private IEnumerator PlayAnimations()
        {
            var currentMonster = _dataToMonoBehaviours[_currentTurn].monoBehaviour;
            
            yield return StartCoroutine(currentMonster.Attack(_partyManager));
            
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
        
        public IEnumerator MonsterTakeDamage()
        {
            for (int i = 0; i < SpawnedMonsters.Count; i++)
            {
                Monster.Monster aliveMonster = SpawnedMonsters[i];
                if (aliveMonster.TryDealDamage())
                {
                    yield return new WaitForSeconds(1f);
                }

                yield return new WaitForEndOfFrame();
            }
        }
        
        public void RemoveDead(Monster.Monster currentMonster)
        {
            SpawnedMonsters.Remove(currentMonster);
            _encounterMembers.Remove(currentMonster.MonsterSo);
            
            if (SpawnedMonsters.Count == 0)
            {
                EndEncounter();
            }
        }
    }

    public struct DataToMonoBehaviour<T, U>
    {
        public T data;
        public U monoBehaviour;

        public DataToMonoBehaviour(T data, U monoBehaviour)
        {
            this.data = data;
            this.monoBehaviour = monoBehaviour;
        }
    }
}
