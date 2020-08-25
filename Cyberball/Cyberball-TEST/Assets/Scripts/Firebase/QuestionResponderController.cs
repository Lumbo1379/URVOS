using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionResponderController : MonoBehaviour
{
    public static List<QuestionDialogue> Questions { get; set; }
    public static int QuestionsLeftToAsk { get; set; }

    [SerializeField] private GameObject _detailedInputField;
    [SerializeField] private GameObject _yesOrNoDropdown;
    [SerializeField] private GameObject _middleScaleText;
    [SerializeField] private TMP_Text _title;
    [SerializeField] [Range(0, 100)] private int _distanceBetweenScalePickers;
    [SerializeField] private Color _scaleSelectColour;
    [SerializeField] private GameObject[] _vSam;
    [SerializeField] private GameObject[] _aSam;
    [SerializeField] private GameObject[] _dSam;
    [SerializeField] private Vector3 _samOffset;
    [SerializeField] private GameObject _submitButton;

    private GameObject _previousSelectedScale;
    private List<GameObject> _scaleButtons;
    private int _lowerScale;
    private int _questionIndex;

    public void AskDetailedQuestion(int index)
    {
        SetQuestionIndex(index);
        UpdateTitle(index);
        _detailedInputField.SetActive(true);
    }

    public void AskYesOrNoQuestion(int index)
    {
        SetQuestionIndex(index);
        UpdateTitle(index);
        _yesOrNoDropdown.SetActive(true);
    }

    public void AskScaleQuestion(int index)
    {
        int upperScale = Questions[index].UpperScale;
        int lowerScale = Questions[index].LowerScale;

        AskScaleQuestion(index, upperScale, lowerScale, null);
    }

    public void AskSamQuestion(int type, int index)
    {
        switch (type)
        {
            case QuestionConstants.DROPDOWN_VALENCE_RESPONSE:
                AskScaleQuestion(index, 1, 9, _vSam);
                break;
            case QuestionConstants.DROPDOWN_AROUSAL_RESPONSE:
                AskScaleQuestion(index, 1, 9, _aSam);
                break;
            default:
                AskScaleQuestion(index, 1, 9, _dSam);
                break;
        }
    }

    private void AskScaleQuestion(int index, int lowerScale, int upperScale, GameObject[] sams)
    {
        SetQuestionIndex(index);
        _submitButton.SetActive(false);
        _scaleButtons = new List<GameObject>();

        UpdateTitle(index);

        int middle = (upperScale + lowerScale) / 2;
        _lowerScale = lowerScale;

        _middleScaleText.transform.GetChild(0).GetComponent<TMP_Text>().text = middle.ToString();
        _middleScaleText.transform.GetComponent<Button>().onClick.AddListener(delegate { OnScaleClick(middle); });
        _middleScaleText.SetActive(true);

        int samIndex = 0;

        for (int i = lowerScale; i < middle; i++)
        {
            var nextScale = Instantiate(_middleScaleText);
            nextScale.GetComponent<RectTransform>().position -= new Vector3((middle - i) * _distanceBetweenScalePickers, 0, 0);
            nextScale.transform.GetChild(0).GetComponent<TMP_Text>().text = i.ToString();
            nextScale.transform.SetParent(transform, false);
            var i1 = i;
            nextScale.transform.GetComponent<Button>().onClick.AddListener(delegate { OnScaleClick(i1); });
            nextScale.SetActive(true);

            if (sams != null && i % 2 == 1)
            {
                AddSam(sams, nextScale.transform, samIndex);
                
                samIndex++;
            }

            _scaleButtons.Add(nextScale);
        }

        _scaleButtons.Add(_middleScaleText);
        samIndex++;

        for (int i = middle + 1; i <= upperScale; i++)
        {
            var nextScale = Instantiate(_middleScaleText);
            nextScale.GetComponent<RectTransform>().position += new Vector3((i - middle) * _distanceBetweenScalePickers, 0, 0);
            nextScale.transform.GetChild(0).GetComponent<TMP_Text>().text = i.ToString();
            nextScale.transform.SetParent(transform, false);
            var i1 = i;
            nextScale.transform.GetComponent<Button>().onClick.AddListener(delegate { OnScaleClick(i1); });
            nextScale.SetActive(true);

            if (sams != null && i % 2 == 1)
            {
                AddSam(sams, nextScale.transform, samIndex);

                samIndex++;
            }

            _scaleButtons.Add(nextScale);
        }

        if (sams != null)
            AddSam(sams, _middleScaleText.transform, 3);
    }

    private void AddSam(GameObject[] sams, Transform parent, int index)
    {
        var samPic = Instantiate(sams[index]);
        samPic.GetComponent<RectTransform>().position += _samOffset;
        samPic.transform.SetParent(parent, false);
    }

    public void OnSubmit()
    {
        QuestionsLeftToAsk--;

        if (QuestionsLeftToAsk == 0)
            Time.timeScale = 1;

        int questionType = Questions[_questionIndex].QuestionType;
        string response;

        if (questionType == QuestionConstants.DROPDOWN_DETAILED_RESPONSE)
            response = _detailedInputField.GetComponent<TMP_InputField>().text;
        else if (questionType == QuestionConstants.DROPDOWN_SCALE_RESPONSE)
            response = _previousSelectedScale.transform.GetChild(0).GetComponent<TMP_Text>().text;
        else
            response = _yesOrNoDropdown.GetComponent<TMP_Dropdown>().value == 0 ? "Yes" : "No";

        DBController.SubmitQuestionResponse(Questions[_questionIndex].Question, response);

        Destroy(gameObject);
    }

    private void UpdateTitle(int index)
    {
        _title.text = Questions[index].Question;
    }

    public void OnScaleClick(int value)
    {
        if (_previousSelectedScale != null)
            _previousSelectedScale.GetComponent<Image>().color = Color.white;
        else
            _submitButton.SetActive(true);

        _previousSelectedScale = _scaleButtons[value - _lowerScale];
        _previousSelectedScale.GetComponent<Image>().color = _scaleSelectColour;
    }

    private void SetQuestionIndex(int index)
    {
        _questionIndex = index;
    }
}
