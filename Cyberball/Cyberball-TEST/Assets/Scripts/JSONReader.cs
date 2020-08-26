using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/*
You simply need to call `data = new JSONData(path)`
Then query the specific elements using `data.GameMode`
*/

public class JSONData {

	public string GameMode = "Inclusive";
	public string Age = "Child";
	public string Gender = "Only Boys";
	public int Rounds = 20;

	string path;

	public JSONData (string path)
	{
		//Converts relative path to global path
		//string filePath = Path.Combine(Application.streamingAssetsPath, path);
        //Debug.LogError(filePath);
		//Read in file if exists, and parse to JSONData object
		if(File.Exists(path))
		{
			string dataAsJson = File.ReadAllText(path);
			Debug.Log(dataAsJson);
			loadSelf(JsonUtility.FromJson<GameData>(dataAsJson));
		}
		else {
			Debug.LogError("Cannot load game data!");
		}
	}

	//Loads data from JSON object into parent
	private void loadSelf(GameData gd) {
		this.GameMode = gd.GameMode;
		this.Age = gd.Age;
		this.Gender = gd.Gender;
		this.Rounds = gd.Rounds;
	}

}

class GameData {
	public string GameMode = "Inclusive";
	public string Age = "Child";
	public string Gender = "Only Boys";
	public int Rounds = 20;
}