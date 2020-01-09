using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGAC.General
{
    public class Toggle : MonoBehaviour
    {
        [System.Serializable]
        public struct ToggleEvent
        {
            public UnityEngine.Events.UnityEvent Action;
            public Color image_color;
        }

        private void Start()
        {
            if (initial_state)
                SemiEnable();
            else SemiDisable();
        }

        [SerializeField]
        private bool initial_state = false;
        [SerializeField]
        private UnityEngine.UI.Image img;
        public bool State { private set; get; }
        [SerializeField]
        private ToggleEvent OnEnable, OnDisable;

        public void CallAction() {
            if (State)
                Disable();
            else
                Enable();
        }

        public void Enable()
        {
            if (State) return;
            State = true;
            img.color = OnEnable.image_color;
            OnEnable.Action.Invoke();
        }

        public void Disable()
        {
            if (!State) return;
            State = false;
            img.color = OnDisable.image_color;
            OnDisable.Action.Invoke();
        }

        public void SemiEnable()
        {
            if (State) return;
            State = true;
            img.color = OnEnable.image_color;
        }

        public void SemiDisable()
        {
            if (!State) return;
            State = false;
            img.color = OnDisable.image_color;
        }

        public void SetInteraction(bool active)
        {
            gameObject.SetActive(active);
        }

    }
}