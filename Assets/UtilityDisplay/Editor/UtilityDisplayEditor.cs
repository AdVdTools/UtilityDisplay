using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(UtilityDisplay))]
public class UtilityDisplayEditor : Editor {

	UnityEditorInternal.ReorderableList reorderableList;

	void OnEnable() {
		reorderableList = new UnityEditorInternal.ReorderableList(serializedObject, 
			serializedObject.FindProperty("sections"), true, true, true, true);
		reorderableList.drawHeaderCallback = ListHeader;
	}

	void ListHeader(Rect rect) {
		GUI.Label(rect, "Sections", EditorStyles.boldLabel);
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update();

		EditorGUILayout.Space();
		reorderableList.DoLayoutList();
		int index = reorderableList.index;
		SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
//		element.isExpanded = true;
//		EditorGUILayout.PropertyField(element, true);
		foreach(SerializedProperty field in element) {
			EditorGUILayout.PropertyField(field);
		}

		serializedObject.ApplyModifiedProperties();
	}
}
