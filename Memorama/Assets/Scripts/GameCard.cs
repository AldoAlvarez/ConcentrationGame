using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memorama
{
    public class GameCard : MonoBehaviour
    {
        #region UNITY METHODS
        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }

        #endregion
        /// <summary>
        /// voltear carta al momento de destruir, o al crearla
        /// </summary>
        #region VARIABLES
        private CardStates state;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private UnityEngine.UI.Image img;

        public CardNumbers Number { protected set; get; }
        public CardsSuit Suit { protected set; get; }
        private Sprite Front;
        #endregion

        #region PUBLIC METHODS
        public void AddToPlayerHand()
        {
            Player.Instance.SelectCard(this);
        }

        public void RedrawCardFace()
        {
            if (state == CardStates.FACE_UP)
                img.sprite = Front;
            else
                img.sprite = MemoramaManager.Instance.GetCardBack();
        }

        public void SetCard(CardsSuit suit, CardNumbers number, Sprite front)
        {
            Suit = suit;
            Number = number;
            Front = front;
            Restart();
            img.sprite = front;
        }

        public void FlipCard()
        {
            if (!isActive()) return;
            if (_animator == null) return;
            if (state == CardStates.FACE_UP)
                state = CardStates.FACE_DOWN;
            else
                state = CardStates.FACE_UP;
            UpdateAnimation();
        }
        ///
        private void Restart()
        {
            state = CardStates.CREATE;
            UpdateAnimation();
        }

        public void DestroyCard()
        {
            state = CardStates.DESTOYED;
            UpdateAnimation();
        }

        public bool isActive() { return state != CardStates.DESTOYED; }
        #endregion

        private void UpdateAnimation()
        {
            if (_animator == null) return;
            _animator.SetInteger("State", (int)state);
        }
    }
}