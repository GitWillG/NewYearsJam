namespace DiceGame.Utility
{
    public interface IEncounterEventListener
    {
        public void OnEncounterStart();
        public void OnEncounterEnd();
    }

    public interface ICombatEventListener
    {
        public void OnPartyTurnStart();
        public void OnPartyTurnEnd();
        public void OnEnemyTurnStart();
        public void OnEnemyTurnEnd();
        public void OnDealDamage(IDamageable target, IDamageDealer owner);
        public void OnBlock();
    }
}