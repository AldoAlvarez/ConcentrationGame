using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Memorama.General;

namespace Memorama
{
    public class MemoramaManager : MonoBehaviour
    {
        #region UNITY METHODS
        private void Awake()
        {
            CreateInstance();
            ResetMatchSettings();
            GameVariables.GamePhase = GamePhases.INITIALIZE;
        }
       
        private void FixedUpdate()
        {
            switch (GameVariables.GamePhase)
            {
                case GamePhases.IN_GAME:
                    GameVariables.ModifyGameTimeBy(-Time.fixedDeltaTime);
                    MemoramaUI.Instance.UpdatePlayerUI(UIPlayerInfo.GAME_TIME, GameVariables.GameTime.ToString());
                    if (GameTime.ConvertToSeconds(GameVariables.GameTime) <= 0f)
                        EndGame();
                    return;
                default: return;
            }
        }
        #endregion

        #region VARIABLES
        private static MemoramaManager _instance;
        public static MemoramaManager Instance {
            get {
                    CreateInstance();
                return _instance;
            }
        }

        [SerializeField]
        private GameVariables game_variables = new GameVariables();

        public GameVariables GameVariables {
            get {
                if(game_variables==null)
                    game_variables = new GameVariables();
                return game_variables;
            }
        }

        [SerializeField] private BoardTable board;
        #endregion

        #region PUBLIC METHODS
        public void SetGameMode(int mode)
        {
            GameVariables.SetGameMode((GameModes)mode);
        }

        public void AddMatchSuit(int suit)
        {
            GameVariables.AddMatchSuit((CardsSuit)suit);
        }

        public void Restart()
        {
            GameVariables.Restart();
            board.CreateBoard();
            board.ShuffleCards();
            ChangePhaseNPanel(GamePhases.IN_GAME);
            MemoramaUI.Instance.UpdatePlayerUI(UIPlayerInfo.GAME_SCORE, GameVariables.Score.ToString());
            MemoramaUI.Instance.UpdatePlayerUI(UIPlayerInfo.GAME_TIME, GameVariables.GameTime.ToString());
            Player.Instance.Restart();
            StartCoroutine(LateRestart());
        }

        public void ResetMatchSettings()
        {
            GameVariables.ResetMatchSettings();
        }

        public void DestroyBoard()
        {
            board.DestroyAllCards();
        }

        public Sprite GetCardBack() { return board.deck.BackImage; }

        public void CheckSelectedCards(GameCard[] cards)
        {
            StartCoroutine(CheckCardsMatch(cards));          
        }        

        public void ChangePhaseNPanel(GamePhases new_phase)
        {
            MemoramaUI.Instance.HidePanel();
            GameVariables.GamePhase = new_phase;
            MemoramaUI.Instance.DisplayPanel();
        }

        public void Pause ()
        {
            ChangePhaseNPanel(GamePhases.PAUSE);
        }
        public void Resume()
        {
            ChangePhaseNPanel(GamePhases.IN_GAME);
        }

        public void ExitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }
        #endregion

        #region PRIVATE METHODS
        private static void CreateInstance()
        {
            if (_instance != null) return;
            _instance = GeneralMethods.GetIntance<MemoramaManager>("Memorama Manager");
        }

        private void EndGame()
        {
            GameVariables.AddScore(((int)GameTime.ConvertToSeconds(GameVariables.GameTime)) * 5);
            MemoramaUI.Instance.UpdatePlayerUI(UIPlayerInfo.FINAL_SCORE, GameVariables.Score.ToString());
            MemoramaUI.Instance.UpdatePlayerUI(UIPlayerInfo.TOTAL_TIME, GameVariables.GetTotalMatchTime().ToString());
            DestroyBoard();
            ChangePhaseNPanel(GamePhases.GAME_OVER);
        }

        private bool CompareCards(GameCard[]cards)
        {
            if (cards == null || cards.Length <= 0) return false;
            GameCard reference_card = cards[0];
            for (int i = 1; i < cards.Length; ++i)
                if (cards[i].Suit != reference_card.Suit || cards[i].Number != reference_card.Number)            
                    return false;
            return true;
        }

        private IEnumerator LateRestart() {
            GameVariables.GamePhase = GamePhases.PAUSE;
            yield return new WaitForSeconds(0.5f);
            board.FlipAllCards();
            yield return new WaitForSeconds(1f);
            board.FlipAllCards();
            GameVariables.GamePhase = GamePhases.IN_GAME;
            Player.Instance.EnableCardinteraction();
        }

        private IEnumerator CheckCardsMatch(GameCard[]cards) {
            if (CompareCards(cards))
            {
                foreach (GameCard card in cards)
                    board.RecycleGameCard(card);

                yield return new WaitForSeconds(1f);
                GameVariables.AddScore(cards.Length * GameVariables.PairCardValue);
                GameVariables.AddBonusTime();
                GameVariables.ApplyScoreMultiplier();
                GameVariables.CountMatchedPair();

                MemoramaUI.Instance.UpdatePlayerUI(UIPlayerInfo.GAME_SCORE, GameVariables.Score.ToString());
                BoardTable.DestroyCards(cards);
            }
            else
            {
                yield return new WaitForSeconds(1f);
                GameVariables.ResetScoreMultiplier();
                BoardTable.HideCards(cards);
            }
            Player.Instance.EnableCardinteraction();

            if (!board.HasActiveCards()) {
                yield return new WaitForSeconds(1);
                while (GameVariables.GamePhase != GamePhases.IN_GAME)
                    yield return new WaitForFixedUpdate();
                EndGame();
            }
        }
        #endregion

        #region editor
#if UNITY_EDITOR
        [SerializeField]
        private int toolbar_index = 0;

        public void CreateDeck() {
            if (board != null) return;
            board = new BoardTable();
            board.CreateDeck();
            board.CreateBoardSettings();
        }

        public void CreateGameVariables()
        {
            if (game_variables != null) return;
            game_variables = new GameVariables();
            game_variables.CreateGameTimes();
            game_variables.InitializeCardPairValue();
            GameVariables.CreateScoreMultipliers();
        }

        public int GetTime(GameModes mode)
        {
            return game_variables.GetTime(mode);
        }

        public void AutoFillDeck()
        {
            board.AutoFillDeck();
        }
#endif
        #endregion
    }
}