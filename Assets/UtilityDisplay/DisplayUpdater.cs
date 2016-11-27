using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(UtilityDisplay))]
public class DisplayUpdater : MonoBehaviour {
	UtilityDisplay.DisplayInfo unityInfo;

	// Use this for initialization
	void Start () {
		GetSections();
	}

	void GetSections() {
		UtilityDisplay display = GetComponent<UtilityDisplay>();
		unityInfo = display.GetSectionById("unity");
		unityInfo.anchor = TextAnchor.LowerLeft;
	}

	#if UNITY_EDITOR
	void OnValidate() {
		GetSections();
		unityInfo.text = Application.unityVersion+"\n"
			+ UnityEditor.EditorUserBuildSettings.activeBuildTarget +"\n"
//			+ GetConsoleOutput()+"\n"
			+ GetFileContent("test.txt")+"\n"
			+ string.Join(" ", System.Environment.GetCommandLineArgs())+"\n"
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
