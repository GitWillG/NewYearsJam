using System.Collections;
using System.Collections.Generic;
using DiceGame.Dice;
using DiceGame.Managers;
using DiceGame.ScriptableObjects;
using MoreMountains.Tools;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace DiceGame.Monster
{
    public class Monster : MonoBehaviour
    {
        public UnityEvent onAttack;
        public UnityEvent<int> onTakeDamage;
        
        [SerializeField] private float attackDuration;
        [SerializeField] private float paddingBetweenAttacks;
        [SerializeField] private Transform visualsHolder;

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

            Debug.Log(MonsterSo.DiceSlotSo.SlotPrefab);
            //Spawn Die slots
            _diceSlotHolder = Instantiate(MonsterSo.DiceSlotSo.SlotPrefab, transform).GetComponent<DiceSlotHolder>();
            _diceSlotHolder.transform.position = diceSlotLocation.localPosition;
            _currentHealth = MonsterSo.MAXHealth;
            _monsterManager = monsterManager;
            
            //Scale Health Bar based on monster stats
            _textExposer = _healthBar.ProgressBar.GetComponent<TextExposer>();

            UpdateHealthBar();
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

            var damageToDeal = _monsterSo.Damage;

            partyManager.TryDealDamage(damageToDeal);
            Debug.Log(name + "Tries to deal : " + damageToDeal);

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
    }
}