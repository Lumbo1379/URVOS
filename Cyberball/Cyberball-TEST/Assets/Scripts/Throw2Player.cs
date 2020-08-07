using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw2Player : MonoBehaviour {

    public GameObject BallHandL;
    public GameObject parentbone;
    public GameObject MovingBall;

    static Animator anim;

    // Use this for initialization
    void Start () {
        transform.parent = parentbone.transform;
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void ThrowBall()
    {
        transform.parent = null;
        BallHandL.SetActive(false);
        anim.SetTrigger("throw");
    }

}
