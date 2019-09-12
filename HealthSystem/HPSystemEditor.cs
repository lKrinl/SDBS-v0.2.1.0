using UnityEditor;
using UnityEngine;

namespace UniCraft.HPMechanism.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(HealthSystem))]
    public class HPSystemEditor : UnityEditor.Editor
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
		private SerializedProperty _hp;
		private SerializedProperty _maxHP;
		private SerializedProperty _onTakeDamageEvents;
	    private SerializedProperty _onRecoverHPEvents;
	    private SerializedProperty _onDeathEvents;
	    private SerializedProperty _onResurrectionEvents;

		////////// Method //////////
    
		////////// MonoBehaviour callback //////////
		
		private void OnEnable()
		{
			_hp = serializedObject.FindProperty("HP");
			_maxHP = serializedObject.FindProperty("MaxHP");
			_onTakeDamageEvents = serializedObject.FindProperty("OnTakeDamageEvents");
			_onRecoverHPEvents = serializedObject.FindProperty("OnRecoverHPEvents");
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
		    if ( _maxHP.intValue < _hp.intValue )
		    {
			    _hp.intValue = _maxHP.intValue;
		    }
		    GUILayout.Space(SpaceBetweenField);
		    EditorGUILayout.LabelField(InformationTitle, EditorStyles.boldLabel);
		    EditorGUILayout.IntSlider(_maxHP, HealthSystem.DeathHP, HPSystem.MaxHPLimit);
		    EditorGUILayout.IntSlider(_hp, HPSystem.DeathHP, HPSystem.MaxHPLimit);
	    }

	    private void DrawDamageEvent()
	    {
		    GUILayout.Space(SpaceBetweenField);
		    EditorGUILayout.LabelField(DamageEventTitle, EditorStyles.boldLabel);
		    EditorGUILayout.PropertyField(_onTakeDamageEvents, true);
		    EditorGUILayout.PropertyField(_onRecoverHPEvents, true);
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
