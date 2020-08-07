using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandRight : MonoBehaviour {

    public GameObject PlayerBall;
    public GameObject movingBall;
    public Button Left, Right;

    static Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        PlayerBall.SetActive(false);
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

    public void CatchBall()
    {
        anim.SetTrigger("Catching");
    }

    public void CatchBallLeft()
    {
        anim.SetTrigger("CatchLeft");
    }

    public void BallCatch()
    {
        movingBall.SetActive(false);
        anim.SetBool("Ball", true);
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
}