using UnityEngine;
using System.Collections;

public class FileDisplayUpdater : MonoBehaviour {
	#if UNITY_EDITOR
	[Delayed]
	[SerializeField]
	string id;
	[SerializeField]
	string path;

	UtilityDisplay.DisplayInfo fileInfo;

	void OnValidate() {
		if (string.IsNullOrEmpty(id)) {
			return;
		}

		UtilityDisplay display = GetComponent<UtilityDisplay>();
		fileInfo = display.GetSectionById(id);

		fileInfo.text = GetFileContent(path);
	}

	string GetFileContent(string path) {
		if (System.IO.File.Exists(path)) {
			return System.IO.File.ReadAllText(path);
		}
		else {
			return "";
		}
	}
	#endif
}
