using System.Linq;
using DiceGame.Dice;
using DiceGame.Managers;
using DiceGame.ScriptableObjects.Conditions;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace DiceGame.Utility
{
    public class DamageHandler : MonoBehaviour, IDamageable
    {
        [SerializeField] private DiceSlotHolder blockDiceSlot;
        [SerializeField] private DiceSlotHolder damageDiceSlot;
        
        [SerializeField] private MMF_Player damageFeedbackPlayer;
        [SerializeField] private MMF_Player damageNegationFeedbackPlayer;
        [SerializeField] private Transform damageNumberTransform;
        [SerializeField] private Transform damageNegationNumberTransform;
        
        private int _damageNegation;
        private int _health;
        private int _maxHealth;
        private Condition _damageCondition;
        private MMHealthBar _healthBar; 
        private TextExposer _textExposer;
        private RelicManager _relicManager;
        
        public UnityEvent<IDamageable> onTryTakeDamage;
        public UnityEvent<int> onTakeDamage;
        public UnityEvent<int> onDamageNegated;
        public UnityEvent onDeath;
        public Condition DamageCondition => _damageCondition;

        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                UpdateHealthBar();
            }
        }

        private void Awake()
        {
            _healthBar = GetComponent<MMHealthBar>();
            _relicManager = FindObjectOfType<RelicManager>();
        }
        
        public void Init(int health, int maxHealth, Condition damageCondition = null, DiceSlotHolder damageSlot = null)
        {
            _health = health;
            _maxHealth = maxHealth;
            _damageCondition = damageCondition;
            damageDiceSlot = damageSlot;
            _textExposer = _healthBar.ProgressBar.GetComponent<TextExposer>();
            UpdateHealthBar();
        }
        
        public int CalculateDamageNegation()
        {
            if (blockDiceSlot == null) return 0;
            
            var dieRolls = blockDiceSlot.PeekDiceResults();

            return dieRolls is { Count: > 0 } ? dieRolls.Sum(x => x) : 0;
        }

        public void UseDamageNegationDice()
        {
            blockDiceSlot.GetDiceResults();
        }
        
        public bool TryTakeDamage(IDamageDealer damageDealer, out int damageTaken)
        {
            _relicManager.OnDealDamage(this, damageDealer);
            
            onTryTakeDamage?.Invoke(this);
            _damageNegation = CalculateDamageNegation();

            damageTaken = 0;

            var dieRolls = damageDiceSlot? damageDiceSlot.GetDiceResults() : null;
            
            if ((dieRolls == null || dieRolls.Count < 1) && damageDealer.DamageAmount < 1) return false;

            var damage = DamageCondition ? ((IDamageable)this).DamageFromCondition(dieRolls) : damageDealer.DamageAmount;
            
            var totalDamage = damage - _damageNegation;
            
            if (_damageNegation > 0)
            {
                _relicManager.OnBlock(this, damageDealer);
                damageNegationFeedbackPlayer?.PlayFeedbacks(damageNegationNumberTransform.position, _damageNegation);
            }
            
            if(totalDamage < 1) return false; // Don't take the damage
            
            damageFeedbackPlayer?.PlayFeedbacks(damageNumberTransform.position, totalDamage);

            onTakeDamage?.Invoke(totalDamage);
            onDamageNegated?.Invoke(_damageNegation);
            
            if (_health - totalDamage <= 0)
            {
                _health = 0;
                onDeath?.Invoke();
            }
            else
            {
                _health -= totalDamage;
            }

            UpdateHealthBar();
            damageTaken = totalDamage;
            return true;
        }
        
        public void UpdateHealthBar()
        {
            _healthBar.UpdateBar(_health, 0, _maxHealth, true);
            _textExposer.UpdateText(_health + " / " + _maxHealth);
        }
    }
}