using System.Collections;
using System.Collections.Generic;
using DiceGame.Dice;
using DiceGame.Managers;
using DiceGame.ScriptableObjects;
using DiceGame.ScriptableObjects.Conditions;
using DiceGame.Utility;
using MoreMountains.Tools;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace DiceGame.Enemy
{
    public class Monster : MonoBehaviour, IDamageDealer
    {
        public UnityEvent onAttack;
        public UnityEvent<int> onTakeDamage;
        
        [SerializeField] private float attackDuration;
        [SerializeField] private float paddingBetweenAttacks;
        [SerializeField] private Transform visualsHolder;
        [SerializeField] private TextMeshProUGUI intentText;
        [SerializeField] private TextMeshProUGUI damageConditionText;

        public bool HasAttacked { get; set; }
        
        private MonsterSO _monsterSo;
        private List<int> _dieResults;
        private int _currentHealth;
        private MonsterManager _monsterManager;
        private MMHealthBar _healthBar; 
        private TextExposer _textExposer; 
        
        private DiceSlotHolder _diceSlotHolder;

        public MonsterSO MonsterSo => _monsterSo;
        private int CurrentHealth => _currentHealth;
        public int DamageAmount => _monsterSo.Damage;
        public Condition DamageCondition => _monsterSo.DamageCondition;


        private void Awake()
        {
            _healthBar = GetComponent<MMHealthBar>();
        }

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
            
            //Scale Health Bar based on monster stats
            _textExposer = _healthBar.ProgressBar.GetComponent<TextExposer>();

            UpdateIntentText("Does Damage up to ( " + _monsterSo.DamageMinMax.y + " )");
            UpdateDamageConditionText(_monsterSo.DamageCondition.ConditionDescription);
            UpdateHealthBar();
        }

        private void UpdateIntentText(string newIntentText)
        {
            intentText.text = newIntentText;
        }

        private void UpdateDamageConditionText(string newConditionText)
        {
            damageConditionText.text = newConditionText;
        }
        
        public bool TryDealDamage(HeroSO attackingHero)
        {
            var dieRolls = _diceSlotHolder.GetDiceResults();
            
            if (dieRolls == null || dieRolls.Count < 1) return false;
            
            var damage = MonsterSo.DamageFromCondition(dieRolls);
            
            onTakeDamage?.Invoke(damage);
            var particlePrefab = Instantiate(attackingHero.AttackEffectPrefab, transform.position, quaternion.identity);
            var transformPosition = particlePrefab.transform.position;
            transformPosition.z -= 1f;

            particlePrefab.transform.position = transformPosition;
            Destroy(particlePrefab, 2f);
            
            if (_currentHealth - damage <= 0)
            {
                _currentHealth = 0;
                Invoke(nameof(KillSelf), 0.2f);
                UpdateHealthBar();
            }
            else
            {
                _currentHealth -= damage;
                UpdateHealthBar();
            }

            return true;
        }
        
        public IEnumerator Attack(PartyManager partyManager)
        {
            onAttack?.Invoke();
            
            yield return new WaitForSeconds(attackDuration);
            
            DealDamage(partyManager.Damageable);
            
            yield return new WaitForSeconds(paddingBetweenAttacks);
            
            HasAttacked = true;
        }

        private void UpdateHealthBar()
        {
            _healthBar.UpdateBar(_currentHealth, 0, _monsterSo.MAXHealth, true);
            _textExposer.UpdateText(_currentHealth + " / " + _monsterSo.MAXHealth);
        }

        public void KillSelf()
        {
            _monsterManager.RemoveDead(this);
            Destroy(gameObject);   
        }

        public void DealDamage(IDamageable damageable)
        {
            damageable.TryTakeDamage(this, out int damageTaken);
            Debug.Log(name + "Tries to deal : " + DamageAmount + " Damage Taken was : " + damageTaken);
        }
    }
}