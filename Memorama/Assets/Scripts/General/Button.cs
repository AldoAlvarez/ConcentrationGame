using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGAC.General
{
    public class Button : MonoBehaviour
    {
        #region UNITY METHODS
        private void Start()
        {
            Transparent();
        }
        #endregion

        #region VARIABLES

        [SerializeField]
        private GameObject menu_to_show, menu_to_hide;
        #endregion

        #region PUBLIC METHODS

        public void DisplayNext(bool hide_me = true)
        {
            ShowMenu();
            if (hide_me)
                HideMenu();
        }

        public void HideMenu()
        {
            if (menu_to_hide == null) return;
            menu_to_hide.SetActive(false);
        }

        public void ShowMenu()
        {
            if (menu_to_show == null) return;
            menu_to_show.SetActive(true);
        }

        #endregion

        #region PRIVATE METHODS
        private void Transparent()
        {
            if (GetComponent<UnityEngine.UI.Image>() != null)
                GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 0);
        }
        #endregion
    }
}
