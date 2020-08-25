using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

    public GameObject BallHandL;
    public GameObject BallHandR;
    public GameObject Ball;
    public GameObject HandL;
    public GameObject HandR;
    public GameObject BR;
    public GameObject BL;
    public GameObject GL;
    public GameObject GR;

    [SerializeField] private GameObject _questionResponder;
    [SerializeField] private Transform _canvas;
    [SerializeField] private bool _logThrowCount;

    JSONData set;

    static Animator anim;

    private Sharer _sharer;
    private int _throwNumber;

    // Use this for initialization
    void Awake ()
    {
        string path = (Application.streamingAssetsPath + "/data.json");
        set = new JSONData(path);
        anim = Ball.GetComponent<Animator>();
        _sharer = Ball.GetComponent<Sharer>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void Rel()
    {
        BallHandL.SetActive(false);
        Ball.SetActive(true);
        if (set.Age == "Child")
        {
            anim.SetTrigger("isThrowing");
        }
        else
        {
            anim.SetTrigger("TisT");
        }
    }

    public void RelPlayer()
    {
        BallHandL.SetActive(false);
        Ball.SetActive(true);
        if(set.Age == "Child")
        {
            anim.SetTrigger("T2P");
        }
        else
        {
            anim.SetTrigger("TT2P");
        }
    }

    public void RelL()
    {
        BallHandR.SetActive(false);
        Ball.SetActive(true);

        if (set.Age == "Child")
        {
            anim.SetTrigger("Throw");
        }
        else
        {
            anim.SetTrigger("TThrow");
        }
    }

    public void RelPlayerL()
    {
        BallHandR.SetActive(false);
        Ball.SetActive(true);
        
        if (set.Age == "Child")
        {
            anim.SetTrigger("L2P");
        }
        else
        {
            anim.SetTrigger("TL2P");
        }
    }

    public void playercatch()
    {
        Hand HandScript = (Hand)HandL.GetComponent("Hand");
        HandScript.CatchBall();
        HandRight HandRight = (HandRight)HandR.GetComponent("HandRight");
        HandRight.CatchBall();
    }

    public void playercatchL()
    {
        Hand HandScript = (Hand)HandL.GetComponent("Hand");
        HandScript.CatchBallLeft();
        HandRight HandRight = (HandRight)HandR.GetComponent("HandRight");
        HandRight.CatchBallLeft();
    }

    public void rightplayercatch()
    {
        Right RightScript = (Right)BR.GetComponent("Right");
        RightScript.RightPlayerCatch();
        Right RightScriptP = (Right)GR.GetComponent("Right");
        RightScriptP.RightPlayerCatch();
    }

    public void leftplayercatch()
    {
        Left LeftScript = (Left)BL.GetComponent("Left");
        LeftScript.LeftPlayerCatch();
        Left LeftScriptP = (Left)GL.GetComponent("Left");
        LeftScriptP.LeftPlayerCatch();
    }

    public void leftcatch()
    {
        Left LeftScript = (Left)BL.GetComponent("Left");
        LeftScript.LeftCatch();
        Left LeftScriptP = (Left)GL.GetComponent("Left");
        LeftScriptP.LeftCatch();
    }

    public void rightcatch()
    {
        Right RightScript = (Right)BR.GetComponent("Right");
        RightScript.RightCatch();
        Right RightScriptP = (Right)GR.GetComponent("Right");
        RightScriptP.RightCatch();
    }

    private void AskQuestions()
    {
        Time.timeScale = 0;

        for (int i = 0; i < QuestionResponderController.Questions.Count; i++)
        {
            var question = QuestionResponderController.Questions[i];

            if ((question.AskFrequency == QuestionConstants.FREQUENCY_FIRST_THROW_ONLY && _throwNumber == 1) ||
                (question.AskFrequency == QuestionConstants.FREQUENCY_EVERY_OTHER_THROW && _throwNumber % 2 == 1) ||
                (question.AskFrequency == QuestionConstants.FREQUENCY_EVERY_THROW))
            {
                var responder = Instantiate(_questionResponder, _canvas);
                var questionController = responder.GetComponent<QuestionResponderController>();

                if (question.QuestionType == QuestionConstants.DROPDOWN_DETAILED_RESPONSE)
                    questionController.AskDetailedQuestion(i);
                else if (question.QuestionType == QuestionConstants.DROPDOWN_YES_OR_NO)
                    questionController.AskYesOrNoQuestion(i);
                else
                    questionController.AskScaleQuestion(i);

                responder.SetActive(true);

                QuestionResponderController.QuestionsLeftToAsk++;
            }
            
        }
    }


    private void OnDisable()
    {
        if (_logThrowCount)
        {
            _throwNumber++;
            AskQuestions();
        }
    }
}