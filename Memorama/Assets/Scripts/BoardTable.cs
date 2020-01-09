using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Memorama.General;

namespace Memorama
{
    [System.Serializable]
    public class BoardTable
    {
        #region VARIABLES
        [SerializeField]
        private GameModesBoardSettings[] board_settings;

        [SerializeField]
        private Deck all_cards = new Deck();

        public Deck deck {
            set { }
            get { return all_cards; }
        }

        [SerializeField]
        [Tooltip("The center of the Canvas or the parent reference for all intantiated cards.")]
        private Transform CanvasCenter;

        private GameCard[,] cards_on_board = null;
        private List<GameCard> usable_cards = new List<GameCard>();

        private int columns = 0;
        private int rows = 0;
        #endregion

        #region PUBLIC METHODS
        public void ShuffleCards()
        {
            if (deck == null || deck.CardPrefab == null) return;
            if (cards_on_board == null)
                CreateBoard();

            GameVariables GV = MemoramaManager.Instance.GameVariables;

            int totalPairs = GameVariables.RequiredCardsToFlip;
            List<Vector2_Int> occupiedPosition = new List<Vector2_Int>();
            Vector2_Int position = Vector2_Int.zero;

            Card[] cards_to_play = deck.GetCards(GV.GameSuits.ToArray());
            Card[] cards_to_check;
            short half_pairs = (short)(totalPairs / 2);

            for (int pairs = 0; pairs < totalPairs; ++pairs)
            {
                for (int suit = 0; suit < (int)GV.GameMode; ++suit)
                {
                    cards_to_check = Deck.GetCards(cards_to_play, GV.GameSuits[suit]);

                    for (int number = 0; number < (int)CardNumbers.counter; ++number)
                    {

                        if (pairs < half_pairs)
                            position = GetCardPosition(occupiedPosition, true);
                        else
                            position = GetCardPosition(occupiedPosition, false);

                        occupiedPosition.Add(position);
                        CardsSuit c_suit = GV.GameSuits[suit];
                        cards_on_board[position.x, position.y].SetCard(c_suit, (CardNumbers)number, Deck.GetCard(cards_to_check, (CardNumbers)number).Front);
                    }
                }
            }
        }

        public void CreateBoard()
        {
            //Creates all card spaces and objects, without assigning the identity or value of each card
            if (deck == null || deck.CardPrefab == null) return;
            GameVariables GV = MemoramaManager.Instance.GameVariables;

            int total_cards = (int)GV.GameMode * (int)CardNumbers.counter * 2;
            columns = GetSetting(GV.GameMode).columns;
            rows = total_cards / columns;

            cards_on_board = new GameCard[columns, rows];
            Vector3 cardPosition = GetSetting(GV.GameMode).UpperLeftCorner.localPosition;

            for (int column = 0; column < columns; ++column)
            {
                for (int row = 0; row < rows; ++row)
                {
                    GameCard card = usable_cards.Count > 0 ? SetRecycledCard(cardPosition) : InstantiateCard(cardPosition);
                    cards_on_board[column, row] = card;
                    cardPosition.y += GetSetting(GV.GameMode).Offset.y;
                }
                cardPosition.x += GetSetting(GV.GameMode).Offset.x;
                cardPosition.y = GetSetting(GV.GameMode).UpperLeftCorner.localPosition.y;
            }
        }

        public void RecycleGameCard(GameCard card)
        {
            usable_cards.Add(card);
        }

        public void FlipAllCards()
        {
            foreach (GameCard card in cards_on_board)
                card.FlipCard();
        }

        public void DestroyAllCards()
        {
            foreach (GameCard card in cards_on_board)
            {
                if (!usable_cards.Contains(card))
                    usable_cards.Add(card);
                card.DestroyCard();
            }
        }

        public bool HasActiveCards() {
            foreach (GameCard card in cards_on_board)
                if (card.isActive()) return true;
            return false;
        }

        public static void HideCards(GameCard[] cards)
        {
            foreach (GameCard card in cards)
                card.FlipCard();
        }
        public static void DestroyCards(GameCard[] cards)
        {
            foreach (GameCard card in cards)
                card.DestroyCard();
        }
        #endregion
 
        #region PRIVATE METHODS
        private Vector2_Int GetCardPosition(List<Vector2_Int> occupied, bool random = true)
        {
            Vector2_Int position = Vector2_Int.zero;
            if (random)
            {
                for (int i = 0; i < 100; ++i) 
                {
                    position.x = Random.Range(0, columns);
                    position.y = Random.Range(0, rows);
                    if (!occupied.Contains(position))
                        return position;
                }
            }
            return GetManualPosition(occupied);
        }

        private Vector2_Int GetManualPosition(List<Vector2_Int> occupied)
        {
            Vector2_Int position = Vector2_Int.one * -1;
            for (int column = 0; column < columns; ++column)
            {
                for (int row = 0; row < rows; ++row)
                {
                    position.x = column;
                    position.y = row;
                    if (!occupied.Contains(position))
                        return position;
                }
            }
            return position;
        }

        private GameCard InstantiateCard(Vector3 position)
        {
            GameVariables GV = MemoramaManager.Instance.GameVariables;

            GameObject newCard = Object.Instantiate(deck.CardPrefab, CanvasCenter);
            newCard.transform.localPosition = position;
            newCard.transform.localScale = GetSetting(GV.GameMode).CardScale;

            if (newCard.GetComponent<GameCard>() == null)
                newCard.AddComponent<GameCard>();
            return newCard.GetComponent<GameCard>();
        }

        private GameCard SetRecycledCard(Vector3 position) {
            if (usable_cards == null || usable_cards.Count <= 0) return null;

            GameVariables GV = MemoramaManager.Instance.GameVariables;
            GameCard card = usable_cards[0];
            usable_cards.RemoveAt(0);
            card.transform.localPosition = position;
            card.transform.localScale = GetSetting(GV.GameMode).CardScale;

            return card;
        }

        private GameModesBoardSettings GetSetting(GameModes mode)
        {
            return board_settings[(int)mode-1];
        }
        #endregion

        #region editor
#if UNITY_EDITOR
        public void CreateBoardSettings()
        {
            if (board_settings == null || board_settings.Length < (int)GameModes.THREE_SUITS)
            {
                board_settings = new GameModesBoardSettings[(int)GameModes.THREE_SUITS];
                for (int setting = 0; setting < (int)GameModes.THREE_SUITS; ++setting)
                    board_settings[setting] = new GameModesBoardSettings(2);
            }
        }

        public void CreateDeck()
        {
            if (all_cards == null)
                all_cards = new Deck();
            all_cards.CreateDeck();
        }

        public void AutoFillDeck()
        {
            all_cards.AutoFill();
        }
#endif
        #endregion
    }
}