using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

namespace AdVd {

	[CustomEditor(typeof(InputSequence))]
	public class StepDrawer : Editor {
		ReorderableList rList;
		SerializedProperty maxStepDelayProp;
		InputSequence sequence;

		public void OnEnable() {
			rList = new ReorderableList(serializedObject, serializedObject.FindProperty("steps"), true, true, true, true);
			sequence = target as InputSequence;

			rList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, new GUIContent("Steps"));
			rList.drawElementCallback = (rect, index, active, focused) => StepGUI(rect, rList.serializedProperty.GetArrayElementAtIndex(index));
			rList.elementHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			maxStepDelayProp = serializedObject.FindProperty("maxStepDelay");
		}


		public override void OnInspectorGUI ()
		{
			serializedObject.Update();
			rList.DoLayoutList();
			EditorGUILayout.PropertyField(maxStepDelayProp);
			serializedObject.ApplyModifiedProperties();
		}

		public void StepGUI (Rect position, SerializedProperty property)
		{
			float yMin = position.y + EditorGUIUtility.standardVerticalSpacing * 0.5f, yMax = position.height - EditorGUIUtility.standardVerticalSpacing;
			Rect typeRect = new Rect(position.x, yMin, position.width * 0.3f, yMax);
			Rect labelRect = new Rect(typeRect.xMax, yMin, position.width * 0.2f, yMax);
			Rect vRect = new Rect(labelRect.xMax, yMin, position.width * 0.25f, yMax);

			SerializedProperty stepType = property.FindPropertyRelative("type");
			SerializedProperty v0 = property.FindPropertyRelative("v0");
			SerializedProperty v1 = property.FindPropertyRelative("v1");

			EditorGUI.PropertyField(typeRect, stepType, new GUIContent(""));
			float currentLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 30f;
			switch (stepType.GetEnum<StepType>()) {
			case StepType.Tap:
				EditorGUI.LabelField(labelRect, new GUIContent("Length"));
				EditorGUI.PropertyField(vRect, v0, new GUIContent("Min"));
				vRect.x+=vRect.width;
				EditorGUI.PropertyField(vRect, v1, new GUIContent("Max"));
				break;
			case StepType.Delay:
				EditorGUI.LabelField(labelRect, new GUIContent("Length"));
				EditorGUI.PropertyField(vRect, v0, new GUIContent("Min"));
				vRect.x+=vRect.width;
				EditorGUI.PropertyField(vRect, v1, new GUIContent("Max"));
				break;
			case StepType.Swipe:
				EditorGUI.LabelField(labelRect, new GUIContent("Main direction"));
				EditorGUI.PropertyField(vRect, v0, new GUIContent("X"));
				vRect.x+=vRect.width;
				EditorGUI.PropertyField(vRect, v1, new GUIContent("Y"));
				break;
			case StepType.RotateSwipe:
				EditorGUI.LabelField(labelRect, new GUIContent("Rotation"));
				EditorGUI.PropertyField(vRect, v0, new GUIContent(" "));
				break;
			}
			EditorGUIUtility.labelWidth = currentLabelWidth;
		}
	}

	public static class SerializedPropertyEnumUtility {
		public static E GetEnum<E>(this SerializedProperty property) where E : struct, System.IConvertible {
			if (!typeof(E).IsEnum) throw new System.ArgumentException("E must be an enumerated type");

			if (property.enumValueIndex >= 0 && property.enumValueIndex < property.enumNames.Length) {
				string enumName = property.enumNames [property.enumValueIndex];
				foreach (E item in System.Enum.GetValues(typeof(E))) {
					if (item.ToString ().ToLower ().Equals (enumName.Trim ().ToLower ()))
						return item;
				}
			}
			return default(E);
		}

		public static void SetEnum<E>(this SerializedProperty property, E value) where E : struct, System.IConvertible {
			if (!typeof(E).IsEnum) throw new System.ArgumentException("E must be an enumerated type");

			property.enumValueIndex = ArrayUtility.FindIndex<string> (property.enumNames, (name) => name.ToLower().Equals (value.ToString ().ToLower()));
		}
	}
}
