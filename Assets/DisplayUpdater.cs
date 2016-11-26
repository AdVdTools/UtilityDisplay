using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(UtilityDisplay))]
public class DisplayUpdater : MonoBehaviour {
	UtilityDisplay.DisplayInfo fpsInfo;
	UtilityDisplay.DisplayInfo unityInfo;

	// Use this for initialization
	void Start () {
		GetSections();
	}

	void GetSections() {
		UtilityDisplay display = GetComponent<UtilityDisplay>();
		fpsInfo = display.GetSectionById("fps");
		fpsInfo.anchor = TextAnchor.UpperCenter;

		unityInfo = display.GetSectionById("unity");
		unityInfo.anchor = TextAnchor.LowerLeft;
	}

	float deltaTime = 0.0f;
	void Update() {
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

		float msec = deltaTime * 1000f;
		float fps = 1f / deltaTime;
		fpsInfo.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		fpsInfo.textSize = (int)(Screen.height / 25f);
		fpsInfo.color = fps < 30f ? Color.red : Color.green;
	}


	#if UNITY_EDITOR
	void OnValidate() {
		GetSections();
		unityInfo.text = Application.unityVersion+"\n"
			+ UnityEditor.EditorUserBuildSettings.activeBuildTarget +"\n"
//			+ GetConsoleOutput()+"\n"
			+ GetFileContent("test.txt")+"\n"
			+ System.Environment.GetEnvironmentVariable("PATH");
		Debug.Log("OnValidate "+UnityEditor.EditorUserBuildSettings.activeBuildTarget+" "+UnityEditor.BuildPipeline.isBuildingPlayer);
//		RunCMD();
    }

	void RunCMD() {
		if (!UnityEditor.BuildPipeline.isBuildingPlayer) return;

		System.Diagnostics.Process p = new System.Diagnostics.Process();
		p.StartInfo.FileName = "CMD.exe";
		p.StartInfo.Arguments = "pwd > Assets/testFile.txt";
		p.Start();
		p.WaitForExit();
	}

	string GetFileContent(string path) {
		if (System.IO.File.Exists(path)) {
			return System.IO.File.ReadAllText(path);
		}
		else {
			return path+" not found";
		}
	}

	string GetConsoleOutput() {
		if (!UnityEditor.BuildPipeline.isBuildingPlayer) return "";

		System.Diagnostics.Process p = new System.Diagnostics.Process();
		p.StartInfo.UseShellExecute = false;
		p.StartInfo.RedirectStandardOutput = true;
		p.StartInfo.FileName = "CMD.exe";
		p.StartInfo.Arguments = "pwd";
		p.Start();

		string output = p.StandardOutput.ReadToEnd();
		p.WaitForExit();
		return output;
	}
	#endif
}
