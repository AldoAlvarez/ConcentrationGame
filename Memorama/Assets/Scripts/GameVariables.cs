using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Memorama.General;

namespace Memorama
{
    [System.Serializable]
    public class GameVariables
    {
        #region VARIABLES
        #region pre-game info
        public GameModes GameMode { private set; get; }
        public List<CardsSuit> GameSuits { private set; get; }
        #endregion

        #region in_game_info
        public GamePhases GamePhase = GamePhases.INITIALIZE;

        private float score = 0f;
        public float Score { get { return score; } }

        private GameTime game_timer = GameTime.zero;
        public GameTime GameTime { get { return game_timer; } }

        [SerializeField]
        private int minimum_match_time = 0;
        [SerializeField]
        private int maximum_match_time = 10;
        [SerializeField]
        private GameTime[] match_times = new GameTime[(int)GameModes.THREE_SUITS];

        [SerializeField]
        [Tooltip("This will add extra time when a pair is matched.")]
        private bool addBonusTime = true;
        [SerializeField]
        [Range(0, 30)]
        [Tooltip("The time to be added when a pair is matched.")]
        private int bonusTime = 10;

        [SerializeField]
        [Range(1, 100)]
        [Tooltip("The value of the score to be added when a pair is matched.")]
        private int pair_card_value = 50;
        public int PairCardValue { get { return pair_card_value; } }

        [SerializeField]
        [Tooltip("This will add a multiplier accordingly to the consecutive pairs that are matched.")]
        private bool applyScoreMultiplier = true;
        [SerializeField]
        private int[] ScoreMultipliers = new int[5] { 1, 2, 3, 5, 10 };
        private int currentScoreMultiplier = 0;

        public const int RequiredCardsToFlip = 2;
        private int matched_pairs = 0;

        private int lastAddedScore = 0;
        #endregion
        #endregion

        #region PUBLIC METHODS
        public void SetGameMode(GameModes mode)
        {
            GameMode = mode;
        }

        public void AddMatchSuit(CardsSuit suit)
        {
            CreateMatchSuitList();
            if (GameSuits.Contains(suit))
                GameSuits.Remove(suit);
            else if (GameSuits.Count < (int)GameMode)
                GameSuits.Add(suit);
        }

        public void CountMatchedPair() { matched_pairs++; }

        public GameTime GetTotalMatchTime()
        {
            GameTime total_time = match_times[(int)GameMode-1] - GameTime;
            if (addBonusTime)
                total_time += GameTime.ConvertToGT(matched_pairs * bonusTime);
            return total_time;
        }

        public void Restart()
        {
            game_timer = match_times[(int)GameMode - 1];
            score = 0f;
            matched_pairs = 0;
            ResetScoreMultiplier();
        }

        public void ResetMatchSettings()
        {
            CreateMatchSuitList();
            GameMode = GameModes.NONE;
            GameSuits.Clear();
        }

        #region variables modifiers
        public void ModifyGameTimeBy(float seconds_value)
        {
            game_timer += GameTime.ConvertToGT(seconds_value);
        }

        public void AddBonusTime()
        {
            if (!addBonusTime) return;
            game_timer += GameTime.ConvertToGT(bonusTime);
        }

        public void ResetScoreMultiplier()
        {
            currentScoreMultiplier = 0;
        }

        public void ApplyScoreMultiplier()
        {
            if (!applyScoreMultiplier || ScoreMultipliers == null || ScoreMultipliers.Length <= currentScoreMultiplier) return;
            score += (lastAddedScore * ScoreMultipliers[currentScoreMultiplier]);
            score -= lastAddedScore;
            lastAddedScore = 0;
            if (currentScoreMultiplier < (ScoreMultipliers.Length - 1))
                currentScoreMultiplier++;
        }

        public void AddScore(int value)
        {
            score += value;
            lastAddedScore = value;
        }
        #endregion
        #endregion

        #region PRIVATE METHODS
        private void CreateMatchSuitList()
        {
            if (GameSuits == null)
                GameSuits = new List<CardsSuit>();
        }
        #endregion

        #region editor
#if UNITY_EDITOR
        public void CreateScoreMultipliers()
        {
            if (ScoreMultipliers != null && ScoreMultipliers.Length == 5) return;
            ScoreMultipliers = new int[5] { 1, 2, 3, 5, 10 };
    }

    public void CreateGameTimes()
        {
            if (match_times != null) return;
            match_times = new GameTime[(int)GameModes.THREE_SUITS];
            for (int i = 0; i < (int)GameModes.THREE_SUITS; ++i)
                match_times[i] = new GameTime(2);
        }
        public void InitializeCardPairValue()
        {
            pair_card_value = 50;
        }

        public int GetTime(GameModes mode)
        {
            return (int)GameTime.ConvertToSeconds(match_times[(int)mode]);
        }
#endif
        #endregion
    }
}