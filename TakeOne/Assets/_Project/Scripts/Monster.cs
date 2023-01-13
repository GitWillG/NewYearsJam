using System.Collections.Generic;
using DiceGame.ScriptableObjects;
using MoreMountains.Tools;
using UnityEngine;

namespace DiceGame
{
    public class Monster : MonoBehaviour
    {
        private MonsterSO _monsterSo;
        private List<int> _dieResults;
        private int _currentHealth;
        private MonsterManager _monsterManager;
        private MMHealthBar _healthBar; 
        private TextExposer _textExposer; 
        
        private DiceSlot _diceSlot;

        public MonsterSO MonsterSo => _monsterSo;
        private int CurrentHealth => _currentHealth;

        private void Awake()
        {
            _healthBar = GetComponent<MMHealthBar>();
        }

        public void InitializeMonster(MonsterSO so, Transform spawnLocation, Transform diceSlotLocation, MonsterManager MManager)
        {
            _monsterSo = so;
            transform.parent = spawnLocation;
            //Spawn Visuals
            var visuals = Instantiate(MonsterSo.MonsterVisualPrefab, transform);

            transform.localPosition = Vector3.zero;

            Debug.Log(MonsterSo.DiceSlotSo.SlotPrefab);
            //Spawn Die slots
            _diceSlot = Instantiate(MonsterSo.DiceSlotSo.SlotPrefab, transform).GetComponent<DiceSlot>();
            _diceSlot.transform.position = diceSlotLocation.localPosition;
            _currentHealth = MonsterSo.MAXHealth;
            _monsterManager = MManager;
            
            //Scale Health Bar based on monster stats
            _healthBar.UpdateBar(_currentHealth, 0, _monsterSo.MAXHealth, true);
            _textExposer = _healthBar.ProgressBar.GetComponent<TextExposer>();
            UpdateHealthBar();
        }
        
        public void TryDealDamage()
        {
            var dieRolls = _diceSlot.GetDiceResults();
            
            if (dieRolls == null || dieRolls.Count < 1) return;
            
            var damage = MonsterSo.DamageFromCondition(dieRolls);
            
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
        }

        private void UpdateHealthBar()
        {
            _healthBar.UpdateBar(_currentHealth, 0, _monsterSo.MAXHealth, true);
            _textExposer.UpdateText(_currentHealth + " / " + _monsterSo.MAXHealth);
        }

        public void KillSelf()
        {
            _monsterManager.RemoveDead(this);
            GameObject.Destroy(this.gameObject);   
        }
    }
}