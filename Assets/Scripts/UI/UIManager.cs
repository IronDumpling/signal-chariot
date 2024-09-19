using System.Collections.Generic;
using UnityEngine;

using Utils;
using Utils.Common;

namespace UI
{
    public enum UIElements
    {
        MainMenu,
        Tutorial,
        BattleConsole,
        BattleResult,
        BoardConsole,
        Default,
    }

    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField] private List<string> m_uiPrefabPaths = new();
        private List<(UIElements, IHidable)> m_uiElements = new();
        private Dictionary<UIElements, int> m_uiDisplayFlags = new();

        private void Awake()
        {
            SpawnUIList();
            GenerateUIFlags();
        }

        private void SpawnUIList()
        {
            foreach(string uiPath in m_uiPrefabPaths)
            {
                GameObject prefab = Resources.Load<GameObject>(Constants.GO_UI_COMMON_PATH + uiPath);
                if(prefab == null)
                {
                    Debug.LogWarning($"Prefab at path {uiPath} could not be loaded.");
                    continue;
                }
                GameObject uiElement = Instantiate(prefab, transform);
                if(uiElement.TryGetComponent<IHidable>(out var uiComponent))
                {
                    string[] directory = uiPath.Split("/");
                    UIElements type = GetUIType(directory[^1]);
                    m_uiElements.Add((type, uiComponent));
                }
                else Debug.LogWarning($"Prefab at path {uiPath} does not have UI component");
            }
        }

        private void GenerateUIFlags()
        {
            m_uiDisplayFlags.Clear();
            int index = 0;
            foreach(var pair in m_uiElements)
            {
                UIElements type = pair.Item1;
                m_uiDisplayFlags[type] = 1 << index;
                index++;
            }
        }

        public void SetDisplayUI(int bitmask)
        {
            for(int i = 0; i < m_uiElements.Count; i++)
            {
                if((bitmask & (1 << i)) != 0)
                    m_uiElements[i].Item2.Show();
                else
                    m_uiElements[i].Item2.Hide();
            }
        }

        public void AddDisplayUI(int bitmask)
        {
            int i = 0;
            while(((bitmask >> i) & 1) != 1) i++;
            m_uiElements[i].Item2.Show();
        }

        public void RemoveDisplayUI(int bitmask)
        {
            int i = 0;
            while(((bitmask >> i) & 1) != 1) i++;
            m_uiElements[i].Item2.Hide();
        }

        public int GetDisplayBit(params UIElements[] elements)
        {
            int bitmask = 0;
            foreach(UIElements element in elements)
            {
                if(m_uiDisplayFlags.ContainsKey(element)) 
                    bitmask |= m_uiDisplayFlags[element];
            }
            return bitmask;
        }

        public IHidable GetUI(UIElements type)
        {
            foreach(var pair in m_uiElements)
            {
                if(pair.Item1 == type)
                    return pair.Item2;
            }
            Debug.LogWarning($"Can't get {type} type UI");
            return null;
        }

        public static UIElements GetUIType(string uiName)
        {
            switch(uiName)
            {
                case "BattleConsole":
                    return UIElements.BattleConsole;
                case "BattleResult":
                    return UIElements.BattleResult;
                case "BoardConsole":
                    return UIElements.BoardConsole;
                case "MainMenu":
                    return UIElements.MainMenu;
                case "Tutorial":
                    return UIElements.Tutorial;
                default:
                    return UIElements.Default;
            }
        }
    }
}
