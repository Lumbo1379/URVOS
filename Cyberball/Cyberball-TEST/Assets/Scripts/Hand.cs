using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour {

    public GameObject parent;
    public GameObject RightHand;
    public GameObject PlayerBall;
    public GameObject Ball;
    public JSONData set;
    public Button Left, Right;
    static Animator anim;

	// Use this for initialization
	void Start () {
        string dataPath = string.Format("{0}/saveFile.json", Application.persistentDataPath);
        set = new JSONData(dataPath);

        anim = GetComponent<Animator>();
        PlayerBall.SetActive(false);
        PlayerBall.transform.parent = parent.transform;
        anim.SetBool("Ball", false);
    }
	
	// Update is called once per frame
	void Update () {
        if (anim.GetBool("Ball"))
        {
            Left.interactable = true;
            Right.interactable = true;
        }
        else
        {
            Left.interactable = false;
            Right.interactable = false;
        }

    }
    public void Updater()
    {
        Sharer share = Ball.GetComponent<Sharer>();
        share.throws++;
    }

    public void CatchBall()
    {
        anim.SetTrigger("Catching");
        Updater();
    }

    public void CatchBallLeft()
    {
        anim.SetTrigger("CatchLeft");

    }

    public void BallCatch() {
        Ball.SetActive(false);
        PlayerBall.SetActive(true);
        anim.SetBool("Ball", true);
        Sharer share = Ball.GetComponent<Sharer>();
        share.counter++;
        if (share.throws == set.Rounds)
        {
            share.popUp.SetActive(true);
            Left.interactable = false;
            Right.interactable = false;
            parent.SetActive(false);
            RightHand.SetActive(false);
        }
    }

    public void BallThrow()
    {
        if (anim.GetBool("Ball"))
        {
            anim.SetTrigger("Throwing");
            anim.SetBool("Ball", false);
        }
    }

    public void BallThrowLeft()
    {
        if (anim.GetBool("Ball"))
        {
            anim.SetTrigger("ThrowLeft");
            anim.SetBool("Ball", false);
        }
    }

    public void ReleaseBall()
    {
        PlayerBall.SetActive(false);
        Ball.SetActive(true);
        HandBall handBall = (HandBall)Ball.GetComponent("HandBall");
        handBall.PlayerRel();
    }

    public void ReleaseBallLeft()
    {
        PlayerBall.SetActive(false);
        Ball.SetActive(true);
        HandBall handBall = (HandBall)Ball.GetComponent("HandBall");
        handBall.PlayerRelLeft();
    }
}