using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBall : MonoBehaviour
{

    public GameObject Ball;
    public GameObject PlayerBall;

    JSONData set;

    static Animator anim;

    // Use this for initialization
    void Start()
    {

        string path = (Application.streamingAssetsPath + "/data.json");
        set = new JSONData(path);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerRel()
    {
        Ball.transform.parent = null;
        PlayerBall.SetActive(false);
        if (set.Age == "Child")
        {
            anim.SetTrigger("PlayerThrowR");
        }
        else
        {
            anim.SetTrigger("TPTR");
        }

    }

    public void PlayerRelLeft()
    {
        Ball.transform.parent = null;
        PlayerBall.SetActive(false);
        if (set.Age == "Child")
        {
            anim.SetTrigger("PlayerThrowL");
        }
        else
        {
            anim.SetTrigger("TPTL");
        }
    }
}