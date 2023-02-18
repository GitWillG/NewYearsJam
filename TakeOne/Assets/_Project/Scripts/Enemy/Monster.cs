using System.Collections;
using System.Collections.Generic;
using DiceGame.Dice;
using DiceGame.Managers;
using DiceGame.ScriptableObjects;
using DiceGame.ScriptableObjects.Conditions;
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
            
            Instantiate(_monsterSo.MonsterVisualPrefab, visualsHolder);

            transform.localPosition = Vector3.zero;

            //Spawn Die slots
            _diceSlotHolder = Instantiate(_monsterSo.DiceSlotSo.SlotPrefab, transform).GetComponent<DiceSlotHolder>();
            _diceSlotHolder.transform.position = diceSlotLocation.position;
            _currentHealth = _monsterSo.MAXHealth;
            _monsterManager = monsterManager;


            intentText.GetComponent<UISnapWithOffset>().SetTarget(_diceSlotHolder.transform);
            damageConditionText.GetComponent<UISnapWithOffset>().SetTarget(_diceSlotHolder.transform);
            
            UpdateIntentText("Does Damage up to ( " + _monsterSo.DamageMinMax.y + " )");

            GenerateFromTemplate(_monsterSo.DamageCondition);
            UpdateConditionalUI(_monsterSo.DamageCondition);

            Damageable.Init(_currentHealth, _monsterSo.MAXHealth, _monsterSo.DamageCondition, _diceSlotHolder);
        }
        private void GenerateFromTemplate(Condition ConToCheck)
        {
            if (ConToCheck.ConditionType == ConditionType.Odd || ConToCheck.ConditionType == ConditionType.Even)
            {
                return;
            }

            //TODO: Move to ConditionSO
            if (ConToCheck.Amount < 0)
            {
                var _newCondition = Instantiate(ConToCheck);
                int newVal = Random.Range(2, 4);
                _newCondition.Amount = newVal;
                _newCondition.ConditionDescription += newVal.ToString();
                _monsterSo.DamageCondition = _newCondition;
            }
        }

        private void UpdateIntentText(string newIntentText)
        {
            intentText.text = newIntentText;
        }

        private void UpdateConditionalUI(Condition newCondition)
        {
            damageConditionText.text = newCondition.ConditionDescription;
            //TODO: change dice slots, not background
            //Move to dice slots
            _diceSlotHolder.UIBackground.Color = newCondition.ConditionColor;

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