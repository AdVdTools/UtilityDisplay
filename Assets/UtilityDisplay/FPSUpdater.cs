using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(UtilityDisplay))]
public class FPSUpdater : MonoBehaviour {
	UtilityDisplay.DisplayInfo fpsInfo;

	void OnEnable () {
		UtilityDisplay display = GetComponent<UtilityDisplay>();
		fpsInfo = display.GetSectionById("fps");
		fpsInfo.anchor = TextAnchor.UpperCenter;
	}
	

	float deltaTime = 0.0f;
	void Update() {
		#if UNITY_EDITOR
		if (!UnityEditor.EditorApplication.isPlaying) return;
		#endif

		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

		float msec = deltaTime * 1000f;
		float fps = 1f / deltaTime;
		fpsInfo.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		fpsInfo.textSize = (int)(Screen.height / 25f);
		fpsInfo.color = fps < 30f ? Color.red : Color.green;
	}

	void OnDisable() {
		UtilityDisplay display = GetComponent<UtilityDisplay>();
		display.RemoveSectionById("fps");
		fpsInfo = null;
	}
}
