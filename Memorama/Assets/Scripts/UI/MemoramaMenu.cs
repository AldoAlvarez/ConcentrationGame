using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGAC.General;

namespace Memorama.UI
{
    public class MemoramaMenu : MonoBehaviour
    {
        #region UNITY METHODS
        private void Start()
        {
            GV = MemoramaManager.Instance.GameVariables;
            OnStart();
        }
        #endregion

        #region VARIABLES
        [SerializeField]
        protected Toggle[] toggle_array;
        [SerializeField]
        protected Toggle continue_button;

        protected GameVariables GV;
        #endregion

        public virtual void CreateToggleArray() { }
        protected virtual void OnStart() { }
    }
}