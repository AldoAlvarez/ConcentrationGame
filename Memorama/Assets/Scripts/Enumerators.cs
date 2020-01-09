using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memorama
{
    public enum GamePhases : int
    {
        INITIALIZE = 0,
        INSTRUCTIONS,
        IN_GAME,
        PAUSE,
        GAME_OVER,
        POST_GAME,
        counter
    }

    public enum CardNumbers : int
    {
        None = -1, JACK, QUEEN, KING, ACE, counter
    }

    public enum CardsSuit : int
    {
        None = -1, HEARTS, DIAMONDS, CLUBS, SPADES, counter
    }

    public enum GameModes : int
    {
        NONE = 0,
        ONE_SUIT = 1,
        TWO_SUITS = 2,
        THREE_SUITS = 3,
    }

    public enum CardStates : int
    {
        CREATE = 0,
        FACE_DOWN,
        FACE_UP,
        DESTOYED
    }

    public enum UIPlayerInfo { NONE=-1, GAME_TIME, GAME_SCORE, FINAL_SCORE, TOTAL_TIME, counter }

#if UNITY_EDITOR
    public enum MemoramaToolbar : int
    {
        NONE = -1,
        SETTINGS,
        CARD_MATCH,
        DECK,
        counter
    }
#endif
}

