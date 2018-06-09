using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameData;
using System.IO;

public class BuildPlatforms : MonoBehaviour
{
    //place inside awake() on new script and put script on things that only apply to the Dev version
    //gameObject.SetActive(Debug.isDebugBuild);

	//Get these from your own URL
	const string SPREADSHEET_ID = "1GvHw-AHf6WG9b4BUynYco02D9wds4FdgGOpmSlLnSEE"; //after the d/
	const string CONSTANTS_TAB_ID = "0";
	const string LOCALIZATION_TAB_ID = "620389613"; //gid
	const string URL_FORMAT = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&id={0}&gid={1}";

	const string RESOURCE_FOLDER_CONSTANTS = "/Resources/Constants.csv";
	const string RESOURCE_FOLDER_LOCALIZATION = "/Resources/Localization.csv";
	//Get url by using File->Download As->CSV
	//Then open download history in chrome/firefox and copy url
	//Make sure accessiblity is set to Anyone with link can view

	private static int _devBuildNum = PlayerPrefs.GetInt("dbn");
	private static int _relBuildNum = PlayerPrefs.GetInt("rbn");

    #region MenuItems
    [MenuItem("Build/iOS/Dev")]
    public static void BuildiOSDev()
    {
		Build(true, BuildTarget.iOS);
    }

    [MenuItem("Build/iOS/Rel")]
    public static void BuildiOSRel()
    {
		Build(false, BuildTarget.iOS);
    }

	[MenuItem("Build/iOS/Both")]
	public static void BuildiOSBoth()
	{
		BuildiOSDev();
		BuildiOSRel();
	}

	[MenuItem("Build/Android/Dev")]
	public static void BuildAndroidDev()
	{
		Build(true, BuildTarget.Android);
	}

	[MenuItem("Build/Android/Rel")]
	public static void BuildAndroidRel()
	{
		Build(false, BuildTarget.Android);
	}

	[MenuItem("Build/Android/Both")]
	public static void BuildAndroidBoth()
	{
		BuildAndroidDev();
		BuildAndroidRel();
	}

	[MenuItem("Build/All")]
	public static void BuildAllTypes()
	{
		BuildiOSDev();
		BuildiOSRel();
		BuildAndroidDev();
		BuildAndroidRel();
	}
    #endregion

	public static void Build(bool pIsDev, BuildTarget pTarget)
    {
		Init();

		if(!pIsDev)	
		{
			AssetDatabase.MoveAsset("Assets/DevTools", "Assets/Editor/DevTools");
			AssetDatabase.Refresh();
		}

		BuildPlayerOptions options = new BuildPlayerOptions();
		string extension = string.Empty;

		switch(pTarget) 
		{
		case BuildTarget.iOS:
			extension = ".ipa";

			break;
		case BuildTarget.StandaloneWindows:
			extension = ".exe";

			break;
		case BuildTarget.Android:
			extension = ".apk";

			break;
		}
			
        string pathToAssets = Application.dataPath;
        string pathToProject = pathToAssets.Replace("/Assets", "");
		string buildPath = string.Format("{0}/Builds/{1}/{2}/Runner_{3}{4}",
            pathToProject,
            pTarget.ToString(),
			pIsDev ? BuildOptions.Development : BuildOptions.None,
			pIsDev ? _devBuildNum : _relBuildNum,
			extension);

        Debug.Log(buildPath);

        options.locationPathName = buildPath;
        options.options = pIsDev ? BuildOptions.Development : BuildOptions.None;
		options.target = pTarget;
        options.scenes = new string[] { "Assets/Platform/Scenes/SinglePlatrform.unity" };

        Debug.Log("Building...");
        BuildPipeline.BuildPlayer(options);
        Debug.Log("Build complete.");

		if(pIsDev)	
		{
			_devBuildNum++;
			PlayerPrefs.SetInt("dbn", /*0*/_devBuildNum);
		}
		else 		
		{
			_relBuildNum++;
			PlayerPrefs.SetInt("rbn", /*0*/_relBuildNum);
			AssetDatabase.MoveAsset("Assets/Editor/DevTools", "Assets/DevTools");
			AssetDatabase.Refresh();
		}
    }

	public static void Init() 
	{
		string locFilePath = string.Format("{0}{1}", Application.dataPath, RESOURCE_FOLDER_LOCALIZATION);
		string conFilePath = string.Format("{0}{1}", Application.dataPath, RESOURCE_FOLDER_CONSTANTS);

		DownloadCSV(LOCALIZATION_TAB_ID, locFilePath);
		DownloadCSV(CONSTANTS_TAB_ID, conFilePath);
	}

	private static void DownloadCSV(string tabID, string filePath) {

		Debug.Log(filePath);

		//Get the formatted URL
		string downloadUrl = string.Format(URL_FORMAT, SPREADSHEET_ID, tabID);

		Debug.LogFormat("Downloading {0}", tabID);

		//Download the data
		WWW website = new WWW(downloadUrl);

		//Wait for data to download
		while (!website.isDone) { }

		if (string.IsNullOrEmpty(website.text)) 
		{
			Debug.LogError("NO DATA WAS RECEIVED");
			//Load the last cached values
		} 
		else 
		{
			Debug.Log(website.text);
			//Successfully got the data, process it
			//Save to disk
			File.WriteAllText(filePath, website.text);
		}
	}
}
