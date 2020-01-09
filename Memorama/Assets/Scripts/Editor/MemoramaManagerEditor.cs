using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Memorama.CustomEditors
{
    [CustomEditor(typeof(MemoramaManager))]
    public class MemoramaManagerEditor : Editor
    {
        #region EDITOR METHODS
        private void OnEnable()
        {
            SetMainObjectVariables();
            suit_foldout = new bool[(int)CardsSuit.counter];
            for (int i = 0; i < (int)CardsSuit.counter; ++i)
                suit_foldout[i] = false;

            settings_foldout = new bool[(int)GameModes.THREE_SUITS];
            for (int i = 0; i < (int)GameModes.THREE_SUITS; ++i)
                settings_foldout[i] = false;

            ((MemoramaManager)target).CreateGameVariables();
            ((MemoramaManager)target).CreateDeck();
        }

        public override void OnInspectorGUI()
        {
            //serializedObject.UpdateIfRequiredOrScript();
            serializedObject.UpdateIfDirtyOrScript();
            EditorGUIUtility.labelWidth = 120;
            GUILayout.Space(6);

            if (board == null)
            {
                SetMainObjectVariables();
                return;
            }
            if (!guiStyles_created)
            {
                CreateGUIStyles();
                return;
            }
            if (DrawToolbar())
            {
                serializedObject.ApplyModifiedProperties();
                return;
            }
            GUILayout.Space(8);

            switch ((MemoramaToolbar)toolbar_index.intValue)
            {
                case MemoramaToolbar.CARD_MATCH:
                    DrawMainVariables();
                    break;
                case MemoramaToolbar.DECK:
                    DrawDeckVariables();
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);
                    GUILayout.Space(5);
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Height(GetDeckScrollviewSize()));
                    DrawDeck();
                    GUILayout.EndScrollView();
                    break;
                case MemoramaToolbar.SETTINGS:
                    DrawBoardSettings();
                    break;
                default: break;
            }
            GUILayout.Space(10);

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region VARIABLES
        private Vector2 scrollPosition = Vector2.zero;
        private bool[] suit_foldout;
        private bool[] settings_foldout;
        private int max_width = 350;
        //styles
        private bool guiStyles_created = false;
        private GUIStyle boldFont;
        private GUIStyle foldoutFont;

        private SerializedProperty board;
        private SerializedProperty game_variables;
        private SerializedProperty toolbar_index;
        private readonly string[] toolbar_windows = new string[(int)MemoramaToolbar.counter] { "Game Settings", "Card Match", "Deck" };

        #region game_variables (GameVariables)
        private SerializedProperty addBonusTime;
        private SerializedProperty bonusTime;

        private SerializedProperty minimum_match_time;
        private SerializedProperty maximum_match_time;
        private SerializedProperty match_times;

        private SerializedProperty pair_card_value;

        private SerializedProperty applyScoreMultiplier;
        private SerializedProperty ScoreMultipliers;

        private SerializedProperty multiplier;
        #endregion

        #region board (BoardTable)
        private SerializedProperty board_settings;
        private SerializedProperty all_cards;
        private SerializedProperty CanvasCenter;
        #endregion

        #region board_settings (GameModesBoardSettings)
        private SerializedProperty Offset;
        private SerializedProperty UpperLeftCorner;
        private SerializedProperty columns;
        private SerializedProperty CardScale;
        #endregion

        #region all_cards (Deck)
        private SerializedProperty BackImage;
        private SerializedProperty CardPrefab;
        private SerializedProperty deck;
        private SerializedProperty card_atlas;
        private SerializedProperty atlas_path;
        #endregion

        #region deck (Card)
        private SerializedProperty Front;
        #endregion

        #region game time
        SerializedProperty Seconds;
        SerializedProperty Minutes;
        #endregion
        #endregion

        #region PRIVATE METHODS
        private int GetDeckScrollviewSize()
        {
            foreach (bool foldout in suit_foldout)
                if (foldout)
                    return 200;
            return 120;
        }

        #region draw methods
        private bool DrawToolbar()
        {
            EditorGUI.BeginChangeCheck();
            toolbar_index.intValue = GUILayout.Toolbar(toolbar_index.intValue, toolbar_windows);
            if (EditorGUI.EndChangeCheck())
                return true;
            return false;
        }

        private void DrawDeckVariables()
        {
            EditorGUILayout.LabelField("Card", boldFont);
            EditorGUILayout.PropertyField(CardPrefab, new GUIContent("Prefab", CardPrefab.tooltip), GUILayout.MaxWidth(max_width));
            EditorGUILayout.PropertyField(BackImage, new GUIContent("Back", BackImage.tooltip), GUILayout.MaxWidth(max_width));
            GUILayout.Space(6);
            EditorGUILayout.PropertyField(atlas_path, new GUIContent("Atlas Path", atlas_path.tooltip), GUILayout.MaxWidth(max_width));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(card_atlas, new GUIContent("Atlas", card_atlas.tooltip), GUILayout.MaxWidth(max_width-80));
            EditorGUI.BeginDisabledGroup(card_atlas.objectReferenceValue == null);
            if (GUILayout.Button("Autofill", GUILayout.MaxWidth(80))) {
                ((MemoramaManager)target).AutoFillDeck();
                serializedObject.Update();
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawDeck()
        {
            int array_offset = 0;
            for (int suit = 0; suit < (int)CardsSuit.counter; ++suit)
            {
                GUILayout.Space(5);
                bool draw_cards = suit_foldout[suit];
                suit_foldout[suit] = EditorGUILayout.Foldout(suit_foldout[suit], new GUIContent(((CardsSuit)suit).ToString()), true);
                if (draw_cards)
                {
                    array_offset = suit * (int)CardNumbers.counter;

                    ++EditorGUI.indentLevel;
                    for (int numb = 0; numb < (int)CardNumbers.counter; ++numb)
                    {
                        int arr_sz = deck.arraySize;
                        SetCardVariables(deck.GetArrayElementAtIndex(array_offset + numb));
                        EditorGUILayout.LabelField(new GUIContent(((CardNumbers)numb).ToString()), boldFont, GUILayout.MaxWidth(90));
                        DrawCard();
                    }
                    --EditorGUI.indentLevel;
                }
                GUILayout.Space(5);
            }
        }

        private void DrawCard()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            ++EditorGUI.indentLevel;
            EditorGUILayout.PropertyField(Front, new GUIContent("Front Image: ", "The image to be displayed when the card is facing up."), GUILayout.MaxWidth(max_width));
            --EditorGUI.indentLevel;
            EditorGUILayout.EndVertical();

            if (Front.objectReferenceValue != null)
            {
                Texture2D tex = AssetPreview.GetAssetPreview(Front.objectReferenceValue);
                GUILayout.Label(tex, GUILayout.MaxHeight(40));
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(6);
        }

        private void DrawMainVariables()
        {
            EditorGUILayout.PropertyField(pair_card_value, new GUIContent("Pair Score", pair_card_value.tooltip), GUILayout.MaxWidth(max_width));

            EditorGUILayout.PropertyField(addBonusTime, new GUIContent("Bonus Time", addBonusTime.tooltip), GUILayout.MaxWidth(max_width));
            if (addBonusTime.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(bonusTime, new GUIContent("Seconds", bonusTime.tooltip), GUILayout.MaxWidth(max_width));
                EditorGUI.indentLevel--;
            }

            GUILayout.Space(8);
            EditorGUILayout.PropertyField(applyScoreMultiplier, new GUIContent("Score Multiplier ", applyScoreMultiplier.tooltip), GUILayout.MaxWidth(max_width));
            if (applyScoreMultiplier.boolValue) {
                EditorGUI.indentLevel++;
                for (int i = 0; i < ScoreMultipliers.arraySize; ++i)
                {
                    multiplier = ScoreMultipliers.GetArrayElementAtIndex(i);
                    string match_text = (i + 1).ToString();
                    multiplier.intValue = EditorGUILayout.IntSlider(
                        new GUIContent("Matches " + match_text, "The multiplier applied for " + match_text + " consecutive matches found."),
                        multiplier.intValue, 1, 10,
                        GUILayout.MaxWidth(max_width));
                }
                EditorGUI.indentLevel--;
            }
            GUILayout.Space(10);
        }

        private void DrawBoardSettings()
        {
            EditorGUILayout.PropertyField(CanvasCenter, new GUIContent("Canvas Center", CanvasCenter.tooltip), GUILayout.MaxWidth(max_width));
            GUILayout.Space(8);
            for (int setting = 0; setting < board_settings.arraySize; ++setting)
            {
                settings_foldout[setting] = EditorGUILayout.Foldout(settings_foldout[setting], new GUIContent(((GameModes)setting + 1).ToString()), true, foldoutFont);
                if (settings_foldout[setting])
                {
                    SetBoardSettingsVariables(board_settings.GetArrayElementAtIndex(setting));
                    if (DrawSetting() || DrawGameTime(setting))
                        return;
                }
                GUILayout.Space(6);
            }
        }

        private bool DrawSetting()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(columns, new GUIContent("Columns", columns.tooltip), GUILayout.MaxWidth(max_width));
            EditorGUILayout.PropertyField(UpperLeftCorner, new GUIContent("Corner Pos", UpperLeftCorner.tooltip), GUILayout.MaxWidth(max_width));
            EditorGUILayout.PropertyField(Offset, new GUIContent("Card Offset", Offset.tooltip),GUILayout.MaxWidth(max_width));
            EditorGUILayout.PropertyField(CardScale, new GUIContent("Card Scale", CardScale.tooltip), GUILayout.MaxWidth(max_width));
            EditorGUI.indentLevel--;
            if (EditorGUI.EndChangeCheck())
                return true;
            return false;
        }

        private bool DrawGameTime(int mode)
        {
            SetGameTimeVariables(match_times.GetArrayElementAtIndex(mode));
            EditorGUI.indentLevel++;
            string text = "Match Time\t" + Minutes.intValue.ToString() + " : " + Seconds.intValue.ToString();
            int ori_size = boldFont.fontSize;
            boldFont.fontSize = 10;
            EditorGUILayout.LabelField(new GUIContent(text, "The match duration in seconds."), boldFont);
            boldFont.fontSize = ori_size;
            EditorGUI.indentLevel--;

            int mins = (int)(((MemoramaManager)target).GetTime((GameModes)mode) / 60);
            if (DrawTimeVariable(ref mins, "Minutes", minimum_match_time.intValue, maximum_match_time.intValue))
            {
                Minutes.intValue = mins;
                return true;
            }
            int secs = (((MemoramaManager)target).GetTime((GameModes)mode) - (mins * 60));
            if (DrawTimeVariable(ref secs, "Seconds", 0, 59))
            {
                Seconds.intValue = secs;
                return true;
            }
            return false;
        }

        private bool DrawTimeVariable(ref int t_var, string label_text, int min_value, int max_value, int left_width = 30, int right_width = 35)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent(label_text), GUILayout.MaxWidth(70));
            EditorGUILayout.LabelField(min_value.ToString(), GUILayout.MaxWidth(left_width));
            t_var = (int)GUILayout.HorizontalSlider(t_var, min_value, max_value, GUILayout.MaxWidth(150));
            EditorGUILayout.LabelField(max_value.ToString(), GUILayout.MaxWidth(right_width));
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
            if (EditorGUI.EndChangeCheck())
                return true;
            return false;
        }
        #endregion

        #region variable setters
        private void SetMainObjectVariables()
        {
            ((MemoramaManager)target).CreateDeck();
            ((MemoramaManager)target).CreateGameVariables();
            serializedObject.ApplyModifiedProperties();

            board = serializedObject.FindProperty("board");
            game_variables = serializedObject.FindProperty("game_variables");
            toolbar_index = serializedObject.FindProperty("toolbar_index");

            SetBoardVariables();
            SetGameVariables();
        }

        private void SetGameVariables()
        {
            addBonusTime = game_variables.FindPropertyRelative("addBonusTime");
            bonusTime = game_variables.FindPropertyRelative("bonusTime");

            minimum_match_time = game_variables.FindPropertyRelative("minimum_match_time");
            maximum_match_time = game_variables.FindPropertyRelative("maximum_match_time");
            match_times = game_variables.FindPropertyRelative("match_times");

            pair_card_value = game_variables.FindPropertyRelative("pair_card_value");

            applyScoreMultiplier = game_variables.FindPropertyRelative("applyScoreMultiplier");
            ScoreMultipliers = game_variables.FindPropertyRelative("ScoreMultipliers");
    }

        private void SetBoardVariables()
        {
            board_settings = board.FindPropertyRelative("board_settings");
            all_cards = board.FindPropertyRelative("all_cards");
            CanvasCenter = board.FindPropertyRelative("CanvasCenter");

            SetDeckVariables();
        }

        private void SetDeckVariables()
        {
            BackImage = all_cards.FindPropertyRelative("BackImage");
            CardPrefab = all_cards.FindPropertyRelative("CardPrefab");
            deck = all_cards.FindPropertyRelative("deck");
            card_atlas = all_cards.FindPropertyRelative("card_atlas");
            atlas_path = all_cards.FindPropertyRelative("atlas_path");
        }

        private void SetBoardSettingsVariables(SerializedProperty setting)
        {
            Offset = setting.FindPropertyRelative("Offset");
            UpperLeftCorner = setting.FindPropertyRelative("UpperLeftCorner");
            columns = setting.FindPropertyRelative("columns");
            CardScale = setting.FindPropertyRelative("CardScale");
        }

        private void SetCardVariables(SerializedProperty card)
        {
            Front = card.FindPropertyRelative("Front");
        }

        private void SetGameTimeVariables(SerializedProperty time)
        {
            Seconds = time.FindPropertyRelative("Seconds");
            Minutes = time.FindPropertyRelative("Minutes");
        }

        private void CreateGUIStyles()
        {
            try
            {
                Color textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;
                boldFont = new GUIStyle(GUI.skin.label);
                boldFont.fontStyle = FontStyle.Bold;
                SetTextColor(boldFont, textColor);

                foldoutFont = new GUIStyle(EditorStyles.foldout);
                foldoutFont.fontStyle = FontStyle.Bold;
                SetTextColor(foldoutFont, textColor);
                guiStyles_created = true;
            }
            catch (System.Exception) { guiStyles_created = false; }
        }

        private void SetTextColor(GUIStyle style, Color color)
        {
            style.normal.textColor = color;
            style.active.textColor = color;
            style.focused.textColor = color;
            style.hover.textColor = color;
        }
        #endregion
        #endregion
    }
}