using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sharer : MonoBehaviour {

    public int throws, counter;
    public GameObject popUp;
    public GameObject BL;
    public GameObject BR;
    public GameObject GL;
    public GameObject GR;

    public JSONData set;

    public string VER, AER, DER;

    public void Start()
    {
        string dataPath = string.Format("{0}/saveFile.json", Application.persistentDataPath); 
        set = new JSONData(dataPath);

        if (set.Gender == "Only Girls")
        {
            GR.SetActive(true);
            GL.SetActive(true);
            BL.SetActive(false);
            BR.SetActive(false);
        }
        else if (set.Gender == "Only Boys")
        {
            BR.SetActive(true);
            BL.SetActive(true);
            GL.SetActive(false);
            GR.SetActive(false);
        }
        else
        {
            BR.SetActive(true);
            BL.SetActive(false);
            GL.SetActive(true);
            GR.SetActive(false);
        }
    }
}
