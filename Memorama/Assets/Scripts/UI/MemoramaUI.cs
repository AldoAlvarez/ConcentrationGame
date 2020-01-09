using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Memorama.General;

namespace Memorama
{
    public class MemoramaUI : MonoBehaviour
    {
        #region UNITY METHODS
        private void Awake()
        {
            if (_instance == null)
                CreateInstance();
        }
        private void Start()
        {
            foreach (GameObject panel in main_panels)
                if (panel != null)
                    panel.SetActive(false);
            main_panels[(int)MemoramaManager.Instance.GameVariables.GamePhase].SetActive(true);
        }
        #endregion

        #region VARIABLES
        private static MemoramaUI _instance;
        public static MemoramaUI Instance
        {
            get
            {
                if (_instance == null)
                    CreateInstance();
                return _instance;
            }
        }

        [SerializeField]
        private GameObject[] main_panels;
        [SerializeField]
        private UI.InGameMenu In_Game = new UI.InGameMenu();
        #endregion

        #region PUBLIC METHODS
        #region show
        public void DisplayPanel()
        {
            DisplayPanel((int)MemoramaManager.Instance.GameVariables.GamePhase);
        }

        public void DisplayPanel(int panel)
        {
            if (panel < 0 || panel >= main_panels.Length) return;
            if (main_panels[panel] == null) return;
            main_panels[panel].SetActive(true);
        }

        public void DisplayPanel(GamePhases panel)
        {
            DisplayPanel((int)panel);
        }
        #endregion

        #region hide
        public void HidePanel()
        {
            HidePanel((int)MemoramaManager.Instance.GameVariables.GamePhase);
        }

        public void HidePanel(int panel)
        {
            if (panel < 0 || panel >= main_panels.Length) return;
            if (main_panels[panel] == null) return;
            main_panels[panel].SetActive(false);
        }

        public void HidePanel(GamePhases panel)
        {
            HidePanel((int)panel);
        }
        #endregion

        public void UpdatePlayerUI(UIPlayerInfo info, string text)
        {
            CreateInGameRef();
            In_Game.UpdatePlayerUI(info, text);
        }
        #endregion

        #region PRIVATE METHODS
        private static void CreateInstance()
        {
            _instance = GeneralMethods.GetIntance<MemoramaUI>("Memorama UI");
        }
        private void CreateInGameRef() {
            if (In_Game == null)
                In_Game = new UI.InGameMenu();
        }
        #endregion

        #region editor
#if UNITY_EDITOR
        [SerializeField]
        private string[] panel_names = new string[(int)GamePhases.counter] {
            "Game Setup",
            "Instructions",
            "In-Game",
            "Pause",
            "Game Over",
            "Credits/Restart"
        };

        public void CreatePanels()
        {
            if (main_panels != null && main_panels.Length == (int)GamePhases.counter) return;
            main_panels = new GameObject[(int)GamePhases.counter];
        }

        public void CreatePlayerUI() {
            CreateInGameRef();
            In_Game.VerifyPlayerUI();
        }
#endif
        #endregion
    }
}