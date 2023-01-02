using System.Collections;
using System.Collections.Generic;
using DiceGame.ScriptableObjects;
using UnityEngine;

namespace DiceGame
{
    public class PartyManager : MonoBehaviour
    {
        [SerializeField]private List<HeroSO> partyMembers;
        [SerializeField] private int lifePool;

        public List<HeroSO> PartyMembers { get => partyMembers; set => partyMembers = value; }
        public int LifePool { get => lifePool; set => lifePool = value; }

        // Start is called before the first frame update
        void Start()
        {
            foreach (HeroSO Hero in partyMembers)
            {
                lifePool += Hero.LifeMod;
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
