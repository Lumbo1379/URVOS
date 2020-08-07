using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Left : MonoBehaviour {

    public GameObject Ball;
    public GameObject BallHandR;
    public GameObject parentB;
    public GameObject parentG;
    public GameObject BL;
    public GameObject GL;

    public JSONData set;
    static Animator anim;
   
    int rand;

    // Use this for initialization
    void Start () {
        string path = (Application.streamingAssetsPath + "/data.json");
        set = new JSONData(path);

        if (set.Gender == "Only Boys")
        {
            anim = BL.GetComponent<Animator>();
        }
        else
        {
            anim = GL.GetComponent<Animator>();
        }

        anim.SetBool("isHoldingBall", false);
        BallHandR.SetActive(false);

        if (set.Gender == "Only Boys")
        {
            //Debug.Log(set.Gender);
            BallHandR.transform.parent = parentB.transform;
            if (set.Age == "Teen")
            {
                BallHandR.transform.localScale = new Vector3(0.24f, 0.24f, 0.24f);
                BallHandR.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
        }
        else
        {
            BallHandR.transform.parent = parentG.transform;
        }
    }
	
	// Update is called once per frame
	void Update () {
        Ball.transform.parent = null;
    }

    public void Updater()
    {
        Sharer share = Ball.GetComponent<Sharer>();
        share.throws++;
    }

    public void randGen()
    {
        rand = Random.Range(1, 3);
    }

    IEnumerator WaitSeconds()
    {
        Sharer share = Ball.GetComponent<Sharer>();
        var randomWait = Random.Range(1, 4); //between 1 to 3 seconds
        
        if (share.throws < set.Rounds)
        {
            yield return new WaitForSeconds(randomWait); //call random fucntion
            randGen();
           //Debug.Log("LRandom " + rand);

            if (set.GameMode == "Inclusive" || set.GameMode == "Random Inclusive")
            {
                if (anim.GetBool("isHoldingBall")) //if holding a ball
                {
                    if (rand == 1) //if random number is 1 throw to AI
                    {
                        anim.SetTrigger("isThrowing");
                        ThrowBall();
                      //  Debug.Log("I1");
                    }
                    else //if random number is 2 the AI throws to the player
                    {
                        anim.SetTrigger("L2P");
                        ThrowBall();
                       // Debug.Log("I2");
                    }
                }
            }
            else
            {
                if (anim.GetBool("isHoldingBall")) //if holding a ball
                {
                   // Debug.Log("Share" + share.throws);
                    if (share.throws <= (set.Rounds / 2))
                    {
                        if (rand == 1) //if random number is 1 throw to AI
                        {
                            anim.SetTrigger("isThrowing");
                            ThrowBall();
                           // Debug.Log("E1");
                        }
                        else //if random number is 2 the AI throws to the player
                        {
                            anim.SetTrigger("L2P");
                            ThrowBall();
                          //  Debug.Log("E2");
                        }
                    }
                    else
                    {
                        anim.SetTrigger("isThrowing");
                        ThrowBall();
                       // Debug.Log("Ex1");
                    }
                }
            }
        }
        else
        {
           // Debug.Log("pop");
            share.popUp.SetActive(true);
            //SceneManager.LoadScene("Thank You");
            GameObject.Find("PlayerLeftHand").SetActive(false);
            GameObject.Find("PlayerRightHand").SetActive(false);
        }
    }

    void ThrowBall()
    {
        anim.SetBool("isHoldingBall", false);
    }

    void ReleaseBall()
    {
        Sharer share = Ball.GetComponent<Sharer>();
        if (set.GameMode == "Inclusive" || set.GameMode == "Random Inclusive")
        {
            if (rand == 1)
            {
                BallScript ballscript = (BallScript)BallHandR.GetComponent("BallScript");
                ballscript.RelL();
                Updater();
            }
            else
            {
                BallScript ballscript = (BallScript)BallHandR.GetComponent("BallScript");
                ballscript.RelPlayerL();
                Updater();
            }
        }
        else
        {
            if (share.throws <= (set.Rounds / 2))
            {
                if (rand == 1)
                {
                    BallScript ballscript = (BallScript)BallHandR.GetComponent("BallScript");
                    ballscript.RelL();
                    Updater();
                }
                else
                {
                    //Debug.Log("HoldBall");
                    BallScript ballscript = (BallScript)BallHandR.GetComponent("BallScript");
                    ballscript.RelPlayerL();
                    Updater();
                }
            }
            else
            {
                BallScript ballscript = (BallScript)BallHandR.GetComponent("BallScript");
                ballscript.RelL();
                Updater();
            }
        }
//        Debug.Log(share.throws);
    }

    public void LeftPlayerCatch()
    {
        anim.SetTrigger("P2L");
    }

    public void LeftCatch()
    {
        anim.SetTrigger("catch");
    }

    public void catchMe(){
        Ball.SetActive(false);
        Ball.transform.parent = null;
        if (set.Gender == "Only Boys")
        {
            //Debug.Log(set.Gender);
            BallHandR.transform.parent = parentB.transform;
        }
        else
        {
            BallHandR.transform.parent = parentG.transform;
        }
        BallHandR.SetActive(true);
        anim.SetBool("isHoldingBall", true);
    }
}