using UnityEngine;
using System.Collections.Generic;

public class UtilityDisplay : MonoBehaviour {

	enum Step { ShortTap, LongTap }
	[SerializeField]
	Step[] switchSequence;
	int currentStep = 0;
	float lastEventTime;
	bool stepRunning;

	[SerializeField]
	float maxStepDelay = 0.5f;
	[SerializeField]
	float longTapLength = 0.5f;



	void Update() {
		if (switchSequence != null && switchSequence.Length > 0) {
			Step current = switchSequence[currentStep];
			float currentTime = Time.unscaledTime;
			bool mouse = Input.GetMouseButton(0);
			switch (current) {
			case Step.ShortTap:
				if (!stepRunning) {
					if (mouse) {
						lastEventTime = currentTime;
						stepRunning = true;
					}
				}
				else {
					if (!mouse){
						if (currentTime > lastEventTime + longTapLength) {//Failed
							currentStep = 0;
						}
						else {
							currentStep++;
						}
						lastEventTime = currentTime;
						stepRunning = false;
					}
				}
				break;
			case Step.LongTap:
				if (!stepRunning) {
					if (mouse) {
						lastEventTime = currentTime;
						stepRunning = true;
					}
				}
				else {
					if (!mouse){
						if (currentTime < lastEventTime + longTapLength) {//Failed
							currentStep = 0;
						}
						else {
							currentStep++;
						}
						lastEventTime = currentTime;
						stepRunning = false;
					}
				}
				break;
			}
			if (currentStep >= switchSequence.Length) {
				currentStep = 0;
				sectionIndex = (sectionIndex+1)%sections.Count;
			}
			if (!stepRunning && currentTime > lastEventTime+maxStepDelay) {//Sequence stopped
				currentStep = 0;
			}
		}
	}

	[System.Serializable]
	public class DisplayInfo {
		public string id;
		[Multiline] public string text;
		public TextAsset textFile;
		public TextAnchor anchor = TextAnchor.LowerLeft;
		public int textSize = 12;
		public Color color = Color.black;

		public override string ToString ()
		{
			return text + (textFile != null ? textFile.text : "");
		}
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

		GUI.Label(rect, info.ToString(), style);
	}

	public DisplayInfo GetSectionById(string id) {
		DisplayInfo info = sections.Find((section) => section.id == id);
		if (info == null) {
			sections.Add(info = new DisplayInfo());
			info.id = id;
		}
		return info;
	}

//	#if UNITY_EDITOR
//	public bool timeScaleToggle;
//	[Range(0f,2f)]
//	public float timeScale;
//
//	void OnValidate() {
//		if (timeScaleToggle) {
//			Time.timeScale = timeScale;
//		}
//		else {
//			Time.timeScale = 1f;
//		}
//	}
//	#endif
}
