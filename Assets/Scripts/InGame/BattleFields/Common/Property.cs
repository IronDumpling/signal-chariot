using UnityEngine;
using UnityEngine.Events;

namespace InGame.BattleFields.Common
{
    public enum LimitedPropertyType
    {
        Health,
        Mod,
        Crystal,
        Default,
        Armor,
    }

    public enum UnlimitedPropertyType
    {
        Defense,
        Speed,
        BulletCount,
        Damage,
        Multiplier,
        Range,
        Interval,
        Default,
    }

    public class UnlimitedProperty
    {
        public UnityEvent<float> onValueChanged = new();
        public UnlimitedPropertyType type { get; private set; }

        private float m_value;
        public float value
        {
            get => m_value;
            set
            {
                if(value <= 0) m_value = 0;
                else m_value = value;
                onValueChanged.Invoke(m_value);
            }
        }

        public UnlimitedProperty(UnlimitedPropertyType type)
        {
            this.value = 0;
            this.type = type;
        }

        public UnlimitedProperty(float initial)
        {
            this.value = initial;
            this.type = UnlimitedPropertyType.Default;
        }

        public UnlimitedProperty(float initial, UnlimitedPropertyType type)
        {
            this.value = initial;
            this.type = type;
        }
    }

    public class LimitedProperty
    {
        public UnityEvent<float, float> onValueChanged = new();
        public LimitedPropertyType type { get; private set; }

        private float m_current;
        public float current
        {
            get => m_current;
            set
            {
                m_current = Mathf.Clamp(value, 0, m_max);
                onValueChanged.Invoke(m_current, m_max);
            }
        }

        private float m_max;
        public float max
        {
            get => m_max;
            set
            {
                m_max = Mathf.Max(0, value);
                onValueChanged.Invoke(m_current, m_max);
            }
        }

        public LimitedProperty(float max)
        {
            this.m_max = max;
            this.m_current = max;
            this.type = LimitedPropertyType.Default;
        }

        public LimitedProperty(float max, LimitedPropertyType type)
        {
            this.m_max = max;
            this.m_current = max;
            this.type = type;
        }

        public LimitedProperty(float max, float initial)
        {
            this.m_max = max;
            this.m_current = initial;
            this.type = LimitedPropertyType.Default;
        }

        public LimitedProperty(float max, float initial, LimitedPropertyType type)
        {
            this.m_max = max;
            this.m_current = initial;
            this.type = type;
        }
    }
}