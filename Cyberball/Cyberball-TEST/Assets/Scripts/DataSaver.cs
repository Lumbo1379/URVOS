using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


/*
You simply need to call `new DataSaver(Gamemode, Age, Gender, Rounds, P_Catch_Count, Emotional_Rating)`
*/


public class DataSaver {

		string filepath = "results.csv";

		public DataSaver(string Gamemode, string Age, string Gender, string Rounds, string P_Catch_Count, string VALENCE, string AROUSAL, string DOMINANCE)
		{
			//Converts relative path to global path
			string path = Path.Combine(Application.streamingAssetsPath, filepath);

			//Format Datetime
			string DateTime = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

			//Format data to save to file
			string data = DateTime + "," + Gamemode + "," + Age + "," + Gender + "," + Rounds + "," + P_Catch_Count + "," + VALENCE + "," + AROUSAL + "," + DOMINANCE + Environment.NewLine;

			//If there is no file, create one and populate with headings
			if (!File.Exists(path))
			{
				string headings = "Date_Time" + "," + "Gamemode" + "," + "Age" + "," + "Gender" + "," + "Rounds" + "," + "P_Catch_Count" + "," + "VALENCE" + "," + "AROUSAL" + "," + "DOMINANCE" + Environment.NewLine;
				File.WriteAllText(path, headings);
			}

			//Add data to file
			File.AppendAllText(path, data);

			DBController.OnSubmit(DateTime, Gamemode, Age, Gender, int.Parse(Rounds), int.Parse(P_Catch_Count), int.Parse(VALENCE), int.Parse(AROUSAL), int.Parse(DOMINANCE));
		}


}
