using UnityEditor;
using UnityEngine;

namespace UniCraft.HPMechanism.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(HealthSystem))]
    public class HealthSystemEditor : UnityEditor.Editor
    {
		////////// Attribute //////////

		////////// Default setting //////////

		////////// Label //////////
		private const string InformationTitle = "Information";
		private const string DamageEventTitle = "Damage Event";
		private const string LifeEventTitle = "Life Event";
		
		////////// Value //////////
		private const float SpaceBetweenField = 6f;
		
		////////// Serialized property //////////
		private SerializedProperty _health;
		private SerializedProperty _maxHealth;
		private SerializedProperty _onTakeDamageEvents;
	    private SerializedProperty _onRecoverHealthEvents;
	    private SerializedProperty _onDeathEvents;
	    private SerializedProperty _onResurrectionEvents;

		////////// Method //////////
    
		////////// MonoBehaviour callback //////////
		
		private void OnEnable()
		{
			_health = serializedObject.FindProperty("Health");
			_maxHealth = serializedObject.FindProperty("MaxHealth");
			_onTakeDamageEvents = serializedObject.FindProperty("OnTakeDamageEvents");
			_onRecoverHealthEvents = serializedObject.FindProperty("OnRecoverHealthEvents");
			_onDeathEvents = serializedObject.FindProperty("OnDeathEvents");
			_onResurrectionEvents = serializedObject.FindProperty("OnResurrectionEvents");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			DrawInformation();
			DrawDamageEvent();
			DrawLifeEvent();
			serializedObject.ApplyModifiedProperties();
		}

		////////// Drawing //////////

	    private void DrawInformation()
	    {
		    if ( _maxHealth.intValue < _health.intValue )
		    {
			    _health.intValue = _maxHealth.intValue;
		    }
		    GUILayout.Space(SpaceBetweenField);
		    EditorGUILayout.LabelField(InformationTitle, EditorStyles.boldLabel);
		    EditorGUILayout.IntSlider(_maxHealth, HealthSystem.DeathHealth, HealthSystem.MaxHealthLimit);
		    EditorGUILayout.IntSlider(_health, HealthSystem.DeathHealth, HealthSystem.MaxHealthLimit);
	    }

	    private void DrawDamageEvent()
	    {
		    GUILayout.Space(SpaceBetweenField);
		    EditorGUILayout.LabelField(DamageEventTitle, EditorStyles.boldLabel);
		    EditorGUILayout.PropertyField(_onTakeDamageEvents, true);
		    EditorGUILayout.PropertyField(_onRecoverHealthEvents, true);
	    }

	    private void DrawLifeEvent()
	    {
		    GUILayout.Space(SpaceBetweenField);
		    EditorGUILayout.LabelField(LifeEventTitle, EditorStyles.boldLabel);
		    EditorGUILayout.PropertyField(_onDeathEvents, true);
		    EditorGUILayout.PropertyField(_onResurrectionEvents, true);
	    }
    }
}
