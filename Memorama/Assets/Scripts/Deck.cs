using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Memorama.General;

namespace Memorama
{
    [System.Serializable]
    public class Deck
    {
        #region VARIABLES
        [Tooltip("The back side of the card.")]
        public Sprite BackImage;
        [Tooltip("The prefab of the card.")]
        public GameObject CardPrefab;

        [SerializeField]
        [Tooltip("The texture containing all of the card sprites for the deck.")]
        private Texture2D card_atlas;
        [SerializeField]
        [Tooltip("The path in which the Card Atlas will be found.")]
        private string atlas_path = "Sprites/";

        [SerializeField]
        private Card[] deck;
        #endregion

        #region PUBLIC METHODS

        #region Get multiple cards
        public static Card[] GetCards(Card[] cards, CardsSuit suit)
        {
            List<Card> _cards = new List<Card>();

            foreach (Card card in cards)
                if (card.Suit == suit)
                    _cards.Add(card);
            return _cards.ToArray();
        }

        public Card[] GetCards(CardsSuit[] suits)
        {
            List<Card> cards = new List<Card>();

            foreach (Card card in deck)
                foreach (CardsSuit suit in suits)
                    if (card.Suit == suit)
                        cards.Add(card);
            return cards.ToArray();
        }

        public Card[] GetCards(CardsSuit suit)
        {
            List<Card> cards = new List<Card>();

            foreach (Card card in deck)
                    if (card.Suit == suit)
                        cards.Add(card);
            return cards.ToArray();
        }
        #endregion

        #region Get single card
        public static Card GetCard(Card[] cards, CardNumbers number)
        {
            foreach (Card card in cards)
                if (card.Number == number)
                    return card;
            return new Card(CardsSuit.None, CardNumbers.None);
        }

        public Card GetCard(CardsSuit suit, CardNumbers number)
        {
            foreach (Card card in GetCards(suit))
                if (card.Suit == suit && card.Number == number)
                    return card;
            return new Card(CardsSuit.None, CardNumbers.None);
        }
        #endregion

        #endregion

        #region editor
#if UNITY_EDITOR
        public void CreateDeck()
        {
            int total_cards = (int)CardsSuit.counter * (int)CardNumbers.counter;
            if (deck == null || deck.Length <= 0 || deck.Length < total_cards || deck.Length > total_cards)
                deck = new Card[total_cards];

            int offset = 0;
            for (int suit = 0; suit < (int)CardsSuit.counter; ++suit)
            {
                offset = suit * (int)CardNumbers.counter;
                for (int number = 0; number < (int)CardNumbers.counter; ++number)
                    deck[offset + number] = new Card((CardsSuit)suit, (CardNumbers)number);
            }
        }

        public void AutoFill()
        {
            if (card_atlas == null) return;
            Sprite[] sprites = Resources.LoadAll<Sprite>(atlas_path + card_atlas.name);
            int required_cards = (int)CardsSuit.counter * (int)CardNumbers.counter;
            if (sprites.Length < required_cards)
            {
                Debug.LogWarning("The number of sprites in the Atlas is insufficient to fill the deck.");
            }
            else
            {
                for (int card = 0; card < required_cards; ++card)
                    deck[card].Front = sprites[card];
            }
        }
#endif
        #endregion
    }
}