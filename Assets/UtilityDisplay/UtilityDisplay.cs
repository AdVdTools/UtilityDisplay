using UnityEngine;
using System.Collections.Generic;

public class UtilityDisplay : MonoBehaviour {
	
	[System.Serializable]
	public class DisplayInfo {
		public string id;
		[Multiline] public string text;
		public TextAnchor anchor = TextAnchor.LowerLeft;
		public int textSize = 12;
		public Color color = Color.black;
	}

	GUIStyle style = new GUIStyle();
	[HideInInspector]
	public int sectionIndex;
	public List<DisplayInfo> sections = new List<DisplayInfo>();

	void OnGUI() {
		if (sectionIndex >= sections.Count || sectionIndex < 0) return;

		int w = Screen.width, h = Screen.height;
		Rect rect = new Rect(0f, 0f, w, h);
		DisplayInfo info = sections[sectionIndex];

		style.alignment = info.anchor;
		style.fontSize = info.textSize;
		style.normal.textColor = info.color;

		GUI.Label(rect, info.text, style);
	}

	public void ShowNext() {
		sectionIndex = (sectionIndex+1)%sections.Count;
	}

	public void Show(string id) {
		int index = sections.FindIndex((section) => section.id.Equals(id));
		if (index >= 0) {
			sectionIndex = index;
		}
	}

	public DisplayInfo GetSectionById(string id) {
		DisplayInfo info = sections.Find((section) => section.id.Equals(id));
		if (info == null) {
			sections.Add(info = new DisplayInfo());
			info.id = id;
		}
		return info;
	}

	public void RemoveSectionById(string id) {
		sections.RemoveAll((section) => section.id.Equals(id));
	}
}
