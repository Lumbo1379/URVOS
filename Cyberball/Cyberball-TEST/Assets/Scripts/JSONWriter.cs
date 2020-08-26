using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public class JSONWriter : MonoBehaviour {

   // public InputField PlayerName;
    public Dropdown GMD;
    public Dropdown AgeDrop;
    public Dropdown GenderDrop;
    public InputField RoundNum;

    public JSONData data;
    
    private int DropdownValA, DropdownValG, DropdownValGM, rand;

    [DllImport("__Internal")]
    private static extern void SyncFiles();

    [DllImport("__Internal")]
    private static extern void WindowAlert(string message);

    string filename = "data.json";
    string path;

    FileStream fileStream;
    string dataPath;

    // Use this for initialization
    void Start()
    {
        dataPath = string.Format("{0}/saveFile.json", Application.persistentDataPath);
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        path = Application.streamingAssetsPath + "/" + filename;
      //  Debug.Log(path);
        //C:\Users\Student\AppData\LocalLow\UROS\JSON_TEST

        if (dataPath.Contains(":/") || dataPath.Contains(":///"))
        {
            //JSONData data = new JSONData(path);
            //GMD.value = GMD.options.FindIndex(option = > options.text == data.GameMode);
            if (data.GameMode.StartsWith("Random"))
                data.GameMode = "Random";

            GMD.value = GMD.options.FindIndex((I) => { return I.text.Equals(data.GameMode); });
            AgeDrop.value = AgeDrop.options.FindIndex((I) => { return I.text.Equals(data.Age); });
            GenderDrop.value = GenderDrop.options.FindIndex((I) => { return I.text.Equals(data.Gender); });
            RoundNum.text = data.Rounds.ToString();
        }

    }

    public void SaveData()
    {
        SceneManager.LoadScene("Play");

        SaveObject saveObject = new SaveObject
        {
           // Player = PlayerName.text.ToString(),
            GameMode = randGen(GMD.options[DropdownValGM].text.ToString()),
            Age = AgeDrop.options[DropdownValA].text.ToString(),
            Gender = GenderDrop.options[DropdownValG].text.ToString(),
            Rounds = RoundNum.text.ToString(),
        };

        string content = JsonUtility.ToJson(saveObject, true);
        Debug.Log(content);
        Debug.Log(dataPath);
        File.WriteAllText(dataPath, content);
        fileStream = File.Open(dataPath, FileMode.Open);

        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            SyncFiles();
        }

        fileStream.Close();

        Debug.Log("Saved!");

        

    }
    private class SaveObject
    {
        public string GameMode, Age, Gender, Rounds;
    }

    public string randGen(string GameMode)
    {

        if (GameMode == "Random")
        {
            if (Random.Range(1, 3) == 1)
            {
                GameMode = "Random Inclusive";
            }
            else
            {
                GameMode = "Random Exclusive";
            }
        }
       // Debug.Log(GameMode);
        return GameMode;
    }

    void Update()
    {   
        DropdownValA = AgeDrop.value;
        DropdownValG = GenderDrop.value;
        DropdownValGM = GMD.value;

        if (Input.GetKeyDown(KeyCode.End))
        {
            Debug.Log("AgeDrop: " + AgeDrop.options[DropdownValA].text);
            Debug.Log("GenderDrop: " + GenderDrop.options[DropdownValG].text);
            Debug.Log("Gamemode: " + GMD.options[DropdownValGM].text);
        }
    }

    public void GetRounds(string rounds)
    {
        Debug.Log("RoundNum: " + rounds);
    }

    public void GetPlayer(string player)
    {
       Debug.Log("Name: " + player);
    }

    public void back()
    {
        SceneManager.LoadScene("Tester");
    }
}