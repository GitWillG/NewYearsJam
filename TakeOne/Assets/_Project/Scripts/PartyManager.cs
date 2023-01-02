using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PartyManager : MonoBehaviour
    {
        [SerializeField]private List<PlayableCharacter> partyMembers;
        [SerializeField] private int lifePool;

        public List<PlayableCharacter> PartyMembers { get => partyMembers; set => partyMembers = value; }
        public int LifePool { get => lifePool; set => lifePool = value; }

        // Start is called before the first frame update
        void Start()
        {
            foreach (PlayableCharacter Hero in partyMembers)
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
