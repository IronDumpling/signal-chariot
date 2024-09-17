using UnityEngine;

using InGame.BattleEffects;
using System.Collections.Generic;



#if UNITY_EDITOR

using UnityEditor;

namespace Editors.BattleEffects
{
    public class EffectEdit : MonoBehaviour
    {
        [Header("Common")]
        public EffectType effectType = EffectType.None;
        public TriggerType triggerType = TriggerType.Collision;
        public int count;
        
        [Header("Damage Effects")]
        public int damage;
        public float duration;
        public float interval;
        public float radius;

        [Header("Count Effects")]
        public int splitSize;
       
        [Header("Spawn & Destroy Effects")]
        public List<GameObject> objectsToSpawn;

        public Effect CreateEffect()
        {
            return effectType switch
            {
                EffectType.SingleOnceDamageEffect => new SingleOnceDamageEffect(damage),
                EffectType.SingleContinuousDamageEffect => new SingleContinuousDamageEffect(damage, duration, interval),
                EffectType.RangeOnceDamageEffect => new RangeOnceDamageEffect(radius, damage),
                EffectType.RangeContinuousDamageEffect => new RangeContinuousDamageEffect(radius, damage, duration, interval),
                EffectType.SpawnEffect => new SpawnEffect(objectsToSpawn.ToArray()),
                EffectType.BouncingEffect => new BouncingEffect(count),
                EffectType.PenetrationEffect => new PenetrationEffect(count),
                EffectType.SplittingEffect => new SplittingEffect(splitSize, count),
                _ => null,
            };
        }
    }

    [CustomEditor(typeof(EffectEdit))]
    public class EffectEditTemplate : Editor
    {
        private SerializedObject effectEdit;

        // common
        public SerializedProperty count;
        public SerializedProperty effectType;
        public SerializedProperty triggerType;

        // damage
        public SerializedProperty damage;
        public SerializedProperty duration;
        public SerializedProperty interval;
        public SerializedProperty radius;

        // count
        public SerializedProperty splitSize;

        // spawn
        public SerializedProperty objectsToSpawn;

        private void OnEnable()
        {
            effectEdit = new SerializedObject(target);
            count = effectEdit.FindProperty("count");
            effectType = effectEdit.FindProperty("effectType");
            triggerType = effectEdit.FindProperty("triggerType");

            damage = effectEdit.FindProperty("damage");
            duration = effectEdit.FindProperty("duration");
            interval = effectEdit.FindProperty("interval");
            radius = effectEdit.FindProperty("radius");

            splitSize = effectEdit.FindProperty("splitSize");

            objectsToSpawn = effectEdit.FindProperty("objectsToSpawn");
        }

        public override void OnInspectorGUI()
        {
            // Update the serialized object before making changes
            effectEdit.Update();

            // Display and edit the EffectType and TriggerType enums
            EditorGUILayout.PropertyField(effectType);
            EditorGUILayout.PropertyField(triggerType);

            // Switch based on the selected effect type to display appropriate fields
            switch ((EffectType)effectType.enumValueIndex)
            {
                case EffectType.SingleOnceDamageEffect:
                    EditorGUILayout.PropertyField(damage);
                    break;

                case EffectType.SingleContinuousDamageEffect:
                    EditorGUILayout.PropertyField(damage);
                    EditorGUILayout.PropertyField(duration);
                    EditorGUILayout.PropertyField(interval);
                    break;

                case EffectType.RangeOnceDamageEffect:
                    EditorGUILayout.PropertyField(damage);
                    EditorGUILayout.PropertyField(radius);
                    break;

                case EffectType.RangeContinuousDamageEffect:
                    EditorGUILayout.PropertyField(damage);
                    EditorGUILayout.PropertyField(radius);
                    EditorGUILayout.PropertyField(duration);
                    EditorGUILayout.PropertyField(interval);
                    break;

                case EffectType.SpawnEffect:
                    EditorGUILayout.PropertyField(objectsToSpawn, true);
                    break;

                case EffectType.BouncingEffect:
                case EffectType.PenetrationEffect:
                    EditorGUILayout.PropertyField(count);
                    break;
                
                case EffectType.SplittingEffect:
                    EditorGUILayout.PropertyField(splitSize);
                    EditorGUILayout.PropertyField(count);
                    break;
            }

            // Apply the modified properties to the serialized object
            effectEdit.ApplyModifiedProperties();
        }
    }
}

#endif