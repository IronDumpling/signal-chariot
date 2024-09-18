using InGame.BattleFields.Common;
using UnityEngine;
using UnityEngine.UI;

using Utils.Common;

namespace InGame.UI
{
    public class SlideBarUI : IHidable
    {
        private GameObject m_slideBar;
        private Image m_max;
        private Image m_value;

        public SlideBarUI(GameObject obj, IPropertyRegisterable register,           
                        LimitedPropertyType type)
        {
            GameObject canvas = obj.transform.Find("Canvas").gameObject;
            m_slideBar = canvas.transform.Find("SlideBar").gameObject;

            m_max = m_slideBar.transform.Find("max").GetComponent<Image>();
            m_value = m_slideBar.transform.Find("value").GetComponent<Image>();

            register.RegisterPropertyEvent(type, SetBarUI);
        }
        
        public SlideBarUI(GameObject obj)
        {
            GameObject canvas = obj.transform.Find("Canvas").gameObject;
            m_slideBar = canvas.transform.Find("SlideBar").gameObject;

            m_max = m_slideBar.transform.Find("max").GetComponent<Image>();
            m_value = m_slideBar.transform.Find("value").GetComponent<Image>();
            
        }

        public void SetBarUI(float value, float max)
        {
            m_value.fillAmount = value / max;
        }

        public void Hide()
        {
            m_slideBar.SetActive(false);
        }

        public void Show()
        {
            m_slideBar.SetActive(true);
        }
    }
}
