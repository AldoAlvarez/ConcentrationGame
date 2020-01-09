using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memorama.General
{
    [System.Serializable]
    public struct Vector2_Int 
    {
        #region Constructors
        public Vector2_Int(int x, int y) { this.x = x;  this.y = y; }
        #endregion

        #region Variables
        public int x;
        public int y;

        #endregion

        public static Vector2_Int zero { get { return new Vector2_Int(0, 0); } }
        public static Vector2_Int one { get { return new Vector2_Int(1, 1); } }

        #region Operators
        public static Vector2_Int operator +(Vector2_Int a, Vector2_Int b) {
            a.x += b.x;
            a.y += b.y;
            return a;
        }
        public static Vector2_Int operator -(Vector2_Int a, Vector2_Int b) {
            a.x += b.x;
            a.y += b.y;
            return a;
        }
        public static Vector2_Int operator *(int d, Vector2_Int a) {
            a.x *= d;
            a.y *= d;
            return a;
        }
        public static Vector2_Int operator *(Vector2_Int a, int d) {
            a.x *= d;
            a.y *= d;
            return a;
        }
        public static Vector2_Int operator /(Vector2_Int a, int d) {
            a.x /= d;
            a.y /= d;
            return a;
        }
        public static bool operator ==(Vector2_Int lhs, Vector2_Int rhs) {
            return (lhs.x == rhs.x) && (lhs.y == rhs.y);
        }
        public static bool operator !=(Vector2_Int lhs, Vector2_Int rhs) {
            return (lhs.x != rhs.x) || (lhs.y != rhs.y);
        }

        public static implicit operator Vector3(Vector2_Int v) {
            Vector3 result = Vector3.zero;
            result.x = v.x;
            result.y = v.y;
            result.z = 1;
            return result;
        }
        #endregion
    }

    [System.Serializable]
    public struct GameTime
    {
        #region Constructors
        public GameTime(int minutes = 0, int seconds = 0, int milliseconds = 0) {
            Minutes = minutes;
            Seconds = seconds;
            Milliseconds = milliseconds;
        }
        #endregion

        #region Variables
        public int Milliseconds;
        public int Seconds;
        public int Minutes;
        #endregion

        public static GameTime zero { get { return new GameTime(0, 0, 0); } }

        public static GameTime ConvertToGT(float seconds) {
            GameTime GT = zero;
            GT.Minutes = ((int)seconds) / 60;
            GT.Seconds = (int)seconds - (60 * GT.Minutes);
            GT.Milliseconds = (int)((seconds - ((int)seconds)) * 1000);
            return GT;
        }
        public static float ConvertToSeconds(GameTime GT) {
            float resulting_seconds = 0f;
            resulting_seconds += GT.Milliseconds / 1000;
            resulting_seconds += GT.Seconds;
            resulting_seconds += GT.Minutes * 60;
            return resulting_seconds;
        }
        public override string ToString()
        {
            string str = Minutes.ToString() + ": " + Seconds.ToString();
            return str;
        }

        #region Operators
        public static GameTime operator +(GameTime a, GameTime b)
        {
            a.Milliseconds += b.Milliseconds;
            a.Seconds += b.Seconds;
            a.Minutes += b.Minutes;

            a = VerifyTime(a);
            return a;
        }
        public static GameTime operator -(GameTime a, GameTime b)
        {
            a.Milliseconds -= b.Milliseconds;
            a.Seconds -= b.Seconds;
            a.Minutes -= b.Minutes;

            a = VerifyTime(a);
            return a;
        }
        public static GameTime operator *(int d, GameTime a)
        {
            a.Milliseconds *= d;
            a.Seconds *= d;
            a.Minutes *= d;

            a = VerifyTime(a);
            return a;
        }
        public static GameTime operator *(GameTime a, int d)
        {
            a.Milliseconds *= d;
            a.Seconds *= d;
            a.Minutes *= d;

            a = VerifyTime(a);
            return a;
        }
        public static GameTime operator /(GameTime a, int d)
        {
            if (d == 0) return GameTime.zero;
            a.Milliseconds /= d;
            a.Seconds /= d;
            a.Minutes /= d;

            a = VerifyTime(a);
            return a;
        }
        #endregion

        private struct TimeOperations
        {
            public TimeOperations(int resulting_time, int time_difference)
            {
                ResultingTime = resulting_time;
                TimeDifference = time_difference;
            }
            public int ResultingTime;
            public int TimeDifference;
        }

        #region Private operator methods
        private static GameTime VerifyTime(GameTime GT)
        {
            TimeOperations to_milliseconds = VerifyTimeVariable(GT.Milliseconds, 1000);
            GT.Milliseconds = to_milliseconds.ResultingTime;
            GT.Seconds += to_milliseconds.TimeDifference;

            TimeOperations to_seconds = VerifyTimeVariable(GT.Seconds, 60);
            GT.Seconds = to_seconds.ResultingTime;
            GT.Minutes += to_seconds.TimeDifference;

            return GT;
        }

        private static TimeOperations VerifyTimeVariable(int time_variable, int time_comparison)
        {
            TimeOperations Result = new TimeOperations(time_variable, 0);

            if (time_comparison == 0) return Result;
            if (time_comparison < 0) time_comparison *= -1;

            if (time_variable >= time_comparison)
            {
                Result.TimeDifference = time_variable / time_comparison;
                Result.ResultingTime = time_variable - (time_comparison * Result.TimeDifference);
            }
            else if (time_variable < 0)
            {
                Result.TimeDifference = (time_variable / time_comparison) - 1;
                Result.ResultingTime = time_variable + (time_comparison * -Result.TimeDifference);
            }

            return Result;
        }
        #endregion
    }

    [System.Serializable]
    public struct GameModesBoardSettings
    {
        #region Constructors
        public GameModesBoardSettings(int columns) {
            Offset = Vector2.zero;
            UpperLeftCorner = null;
            this.columns = columns;
            CardScale = Vector2.one;
            this.GameTime = new GameTime(1, 30, 0);
        }
        #endregion

        [Tooltip("The distance (canvas pixels) at which the cards will be placed apart.")]
        public Vector2 Offset;

        [Tooltip("The position at which the first card will be instantiated.")]
        public Transform UpperLeftCorner;

        [Tooltip("The number of columns on the board.")]
        [Range(0,10)]
        public int columns;

        [Tooltip("The scale (x,y) of the card prefab for this setting.")]
        public Vector2 CardScale;

        [Tooltip("The ")]
        public GameTime GameTime;
    }

    [System.Serializable]
    public struct Card
    {
        public Card(CardsSuit suit, CardNumbers number, Sprite front = null)
        {
            Suit = suit;
            Number = number;
            Front = front;
        }

        public CardsSuit Suit;
        public CardNumbers Number;
        public Sprite Front;
    }
}