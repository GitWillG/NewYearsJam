using System.Collections;
using System.Collections.Generic;
using DiceGame.Dice;
using DiceGame.Managers;
using DiceGame.ScriptableObjects;
using DiceGame.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DiceGame.Enemy
{
    public class Monster : MonoBehaviour
    {
        public UnityEvent onAttack;
        
        [SerializeField] private float attackDuration;
        [SerializeField] private float paddingBetweenAttacks;
        [SerializeField] private Transform visualsHolder;
        [SerializeField] private TextMeshProUGUI intentText;
        [SerializeField] private TextMeshProUGUI damageConditionText;
        [SerializeField] private DamageHandler damageHandler;

        public bool HasAttacked { get; set; }
        
        private MonsterSO _monsterSo;
        private List<int> _dieResults;
        private int _currentHealth;
        private MonsterManager _monsterManager;

        private DiceSlotHolder _diceSlotHolder;

        public MonsterSO MonsterSo => _monsterSo;
        private int CurrentHealth => _currentHealth;
        public int DamageAmount => _monsterSo.Damage;
        public IDamageable Damageable => damageHandler;
        

        public void InitializeMonster(MonsterSO so, Transform spawnLocation, Transform diceSlotLocation, MonsterManager monsterManager)
        {
            _monsterSo = so;
            transform.parent = spawnLocation;
            //Spawn Visuals
            
            Instantiate(MonsterSo.MonsterVisualPrefab, visualsHolder);

            transform.localPosition = Vector3.zero;

            //Spawn Die slots
            _diceSlotHolder = Instantiate(MonsterSo.DiceSlotSo.SlotPrefab, transform).GetComponent<DiceSlotHolder>();
            _diceSlotHolder.transform.position = diceSlotLocation.position;
            _currentHealth = MonsterSo.MAXHealth;
            _monsterManager = monsterManager;
            
            intentText.GetComponent<UISnapWithOffset>().SetTarget(_diceSlotHolder.transform);
            damageConditionText.GetComponent<UISnapWithOffset>().SetTarget(_diceSlotHolder.transform);
            
            UpdateIntentText("Does Damage up to ( " + _monsterSo.DamageMinMax.y + " )");
            UpdateDamageConditionText(_monsterSo.DamageCondition.ConditionDescription);
            
            Damageable.Init(_currentHealth, _monsterSo.MAXHealth, _monsterSo.DamageCondition, _diceSlotHolder);
        }

        private void UpdateIntentText(string newIntentText)
        {
            intentText.text = newIntentText;
        }

        private void UpdateDamageConditionText(string newConditionText)
        {
            damageConditionText.text = newConditionText;
        }

        public void SpawnDamageParticles(IDamageDealer damageDealer)
        {
            // var particlePrefab = Instantiate(attackingHero.AttackEffectPrefab, transform.position, quaternion.identity);
            // var transformPosition = particlePrefab.transform.position;
            // transformPosition.z -= 1f;
            //
            // particlePrefab.transform.position = transformPosition;
            // Destroy(particlePrefab, 2f);
        }

        public IEnumerator Attack(PartyManager partyManager)
        {
            onAttack?.Invoke();
            
            yield return new WaitForSeconds(attackDuration);
            
            DealDamage(partyManager.Damageable);
            
            yield return new WaitForSeconds(paddingBetweenAttacks);
            
            HasAttacked = true;
        }
        
        public void KillSelf()
        {
            _monsterManager.RemoveDead(this);
            Destroy(gameObject);   
        }

        public void DealDamage(IDamageable damageable)
        {
            damageable.TryTakeDamage(_monsterSo, out int damageTaken);
            Debug.Log(name + "Tries to deal : " + DamageAmount + " Damage Taken was : " + damageTaken);
        }
    }
}