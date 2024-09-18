using System;
using InGame.Boards.Modules;
using InGame.UI;
using UnityEngine;
using Utils.Common;

namespace InGame.Views
{
    public class ModuleView: MonoBehaviour
    {
        private Module m_module;
        private Animator m_animator;
        private GameObject m_range;
        private SlideBarUI m_slideBarUI;
        
        public void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_slideBarUI = new SlideBarUI(gameObject);
            m_range = transform.Find("Range")?.gameObject;
            HideRange();
        }
        

        public void PlayAnimation(string animationName)
        {
            m_animator.Play(animationName, -1, 0f);
        }

        public void SetWorldPos(Vector3 pos)
        {
            transform.localPosition = pos;
        }

        public void SetGlobalWorldPos(Vector3 pos)
        {
            transform.position = pos;
        }

        public void Rotate()
        {
            m_module.Rotate();
            var rotation = m_module.GetRotationDegree();
            transform.rotation = Quaternion.Euler(0, 0, rotation);
            
        }
        
        public static ModuleView CreateModuleView(ModuleView prefab, Transform parent, Module module, Quaternion rotation)
        {
            var moduleView = Instantiate(prefab, parent);
            moduleView.m_module = module;
            moduleView.transform.rotation = rotation;
            return moduleView;
        }

        public void SelfDestroy()
        {
            // Todo: Maybe Destroy it in ModuleLib
            Destroy(gameObject);
        }
        
        public void DisplayRange()
        {
            m_range?.SetActive(true);
        }

        public void HideRange()
        {
            m_range?.SetActive(false);
        }
        
        // To DO: If there are multiple bar, we might need to have a bar manager
        public void DisplayUI(float current, float max)
        {
            if (max == 0f || current == 0f)
            {
                m_slideBarUI.Hide();
            }
            else
            {
                m_slideBarUI.Show();
                m_slideBarUI.SetBarUI(current, max);
            }
        }
    }
}