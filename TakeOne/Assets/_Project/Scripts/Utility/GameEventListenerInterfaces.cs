using System.Collections.Generic;
using DiceGame.Dice;
using DiceGame.Managers;

namespace DiceGame.Utility
{
    public interface IRelic
    {
        public bool CanTrigger { get; set; }

        public void TriggerPrimaryEffect();
        public void FlashWhenAvailable();
    }
    
    /// <summary>
    /// Implements all game related event Listeners.
    /// </summary>
    public interface IAllGameEventListener : IEncounterEventListener, ICombatEventListener, IDiceEventListener, IPartyEventListener { }
    
    /// <summary>
    /// Implement this when you want to be notified of the following:
    /// Then simply call the function as you would "Start" or "Update"
    ///<para><see cref="OnEncounterStart"/></para>
    /// <see cref="OnEncounterEnd"/>
    /// </summary>
    public interface IEncounterEventListener
    {
        public void OnEncounterStart() { }
        public void OnEncounterEnd() { }
    }

    /// <summary>
    /// Implement this when you want to be notified of the following:
    /// Then simply call the function as you would "Start" or "Update"
    /// <para> <see cref="OnPartyTurnStart"/>
    /// <see cref="OnPartyTurnEnd"/>
    /// <see cref="OnEnemyTurnStart"/>
    /// <see cref="OnEnemyTurnEnd"/>
    /// <see cref="OnDealDamage"/>
    /// <see cref="OnBlock"/></para>
    /// </summary>
    public interface ICombatEventListener
    {
        //TODO: Refactoring required here too. Everything should work fine for now, but in the future we should use interfaces instead of concrete class
        //This will resolve over time too, this can be a good place to check for what still needs to be refactored!
        public void OnPartyTurnStart(PartyManager partyManager){}
        public void OnPartyTurnEnd(PartyManager partyManager){}
        public void OnEnemyTurnStart(MonsterManager monsterManager){}
        public void OnEnemyTurnEnd(MonsterManager monsterManager){}
        public void OnDealDamage(IDamageable target, IDamageDealer owner){}
        public void OnBlock(IDamageable target, IDamageDealer owner){}
    }
    
    /// <summary>
    /// Implement this when you want to be notified of the following:
    /// Then simply call the function as you would "Start" or "Update"
    /// <para> <see cref="OnDiceRolled"/>
    /// <see cref="OnDiceSelected"/>
    /// <see cref="OnDiceAttachToSlot"/>
    /// <see cref="OnConfirmAllDie"/>
    /// <see cref="OnRelicDieResultFound"/></para>
    /// </summary>
    public interface IDiceEventListener
    {
        public void OnDiceRolled(IDiceOwner diceOwner, List<DiceController> diceControllers){}
        public void OnDiceSelected(DiceController diceController){}
        public void OnDiceAttachToSlot(DiceController diceController, DiceSlotHolder diceSlotHolder){}
        public void OnConfirmAllDie(List<DiceController> diceControllers){}
        void OnRelicDieResultFound(int value){}
    }

    /// <summary>
    /// Implement this when you want to be notified of the following:
    /// Then simply call the function as you would "Start" or "Update"
    /// <para> <see cref="OnPartyCreated"/>
    /// <see cref="OnPartyDeath"/></para>
    /// </summary>
    public interface IPartyEventListener
    {
        public void OnPartyCreated(PartyManager partyManager){}
        public void OnPartyDeath(PartyManager partyManager){}
    }
    
}