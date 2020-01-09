using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memorama
{
    public class Player : MonoBehaviour
    {
        private void Awake()
        {
            CreateInstance();
        }

        #region VARIABLES
        private static Player _instance;
        public static Player Instance
        {
            get
            {
                CreateInstance();
                return _instance;
            }
        }
        public void DebuggerFunction() { Debug.Log("algo mundo"); }
        private List<GameCard> cards_in_hand = new List<GameCard>();
        private bool can_flip_cards = false;
        #endregion

        #region PUBLIC METHODS
        public void EnableCardinteraction()
        {
            if (can_flip_cards) return;
            can_flip_cards = true;
        }

        public void DisableCardinteraction()
        {
            if (!can_flip_cards) return;
            can_flip_cards = false;
        }

        public void SelectCard(GameCard card) {
            if (!can_flip_cards) return;
            if (card == null) return;
            if (cards_in_hand.Contains(card)) return;

            cards_in_hand.Add(card);
            card.FlipCard();

            if (cards_in_hand.Count >= GameVariables.RequiredCardsToFlip)
            {
                MemoramaManager.Instance.CheckSelectedCards(cards_in_hand.ToArray());
                Restart();
            }
        }

        public void Restart()
        {
            ClearHand();
            can_flip_cards = false;
        }
        #endregion

        #region PRIVATE METHODS
        private void ClearHand()
        {
            cards_in_hand.Clear();
        }

        private static void CreateInstance()
        {
            if (_instance != null) return;
            _instance = General.GeneralMethods.GetIntance<Player>("Player");
        }
        #endregion
    }
}
