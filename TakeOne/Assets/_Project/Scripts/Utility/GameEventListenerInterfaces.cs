using System.Collections.Generic;
using DiceGame.Dice;
using DiceGame.Managers;
using DiceGame.Relics;

namespace DiceGame.Utility
{
    public interface IRelic
    {
        public void Initialize(RelicSO relicSo);
    }
    
    public interface IAllGameEventListener : IEncounterEventListener, ICombatEventListener, IDiceEventListener, IPartyEventListener { }
    
    public interface IEncounterEventListener
    {
        public void OnEncounterStart();
        public void OnEncounterEnd();
    }

    public interface ICombatEventListener
    {
        public void OnPartyTurnStart(PartyManager partyManager);
        public void OnPartyTurnEnd(PartyManager partyManager);
        public void OnEnemyTurnStart(MonsterManager monsterManager);
        public void OnEnemyTurnEnd(MonsterManager monsterManager);

        public void OnDealDamage(IDamageable target, IDamageDealer owner);
        public void OnBlock(IDamageable target, IDamageDealer owner);
    }

    public interface IDiceEventListener
    {
        public void OnDiceRolled(IDiceOwner diceOwner, List<DiceController> diceControllers);
        public void OnDiceSelected(DiceController diceController);
        public void OnDiceAttachToSlot(DiceController diceController, DiceSlotHolder diceSlotHolder);
        public void OnConfirmAllDie(List<DiceController> diceControllers);
    }

    public interface IPartyEventListener
    {
        public void OnPartyCreated(PartyManager partyManager);
        public void OnPartyDeath(PartyManager partyManager);
    }
}