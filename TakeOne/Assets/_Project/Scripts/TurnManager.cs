using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] private bool playerTurn;

        public bool PlayerTurn { get => playerTurn; set => playerTurn = value; }

        public void endTurn()
        {
            playerTurn = !playerTurn;
        }
    }
}
