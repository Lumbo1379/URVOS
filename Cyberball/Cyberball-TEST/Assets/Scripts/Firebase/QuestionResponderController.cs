using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionResponderController : MonoBehaviour
{
    public static List<QuestionDialogue> Questions { get; set; }
    public static int QuestionsLeftToAsk { get; set; }

    [SerializeField] private GameObject _detailedInputField;
    [SerializeField] private GameObject _yesOrNoDropdown;
    [SerializeField] private GameObject _middleScaleText;
    [SerializeField] private TMP_Text _title;
    [SerializeField] [Range(0, 100)] private int _distanceBetweenScalePickers;

    public void AskDetailedQuestion(int index)
    {
        UpdateTitle(index);
        _detailedInputField.SetActive(true);
    }

    public void AskYesOrNoQuestion(int index)
    {
        UpdateTitle(index);
        _yesOrNoDropdown.SetActive(true);
    }

    public void AskScaleQuestion(int index)
    {
        UpdateTitle(index);

        int upperScale = Questions[index].UpperScale;
        int lowerScale = Questions[index].LowerScale;
        int middle = (upperScale + lowerScale) / 2;

        _middleScaleText.transform.GetChild(0).GetComponent<TMP_Text>().text = middle.ToString();
        _middleScaleText.SetActive(true);

        for (int i = lowerScale; i < middle; i++)
        {
            var nextScale = Instantiate(_middleScaleText, transform);
            nextScale.transform.position -= new Vector3((middle - i) * _distanceBetweenScalePickers, 0, 0);
            nextScale.transform.GetChild(0).GetComponent<TMP_Text>().text = i.ToString();
            nextScale.SetActive(true);
        }

        for (int i = upperScale; i > middle; i--)
        {
            var nextScale = Instantiate(_middleScaleText, transform);
            nextScale.transform.position += new Vector3((i - middle) * _distanceBetweenScalePickers, 0, 0);
            nextScale.transform.GetChild(0).GetComponent<TMP_Text>().text = i.ToString();
            nextScale.SetActive(true);
        }
    }

    public void OnSubmit()
    {
        QuestionsLeftToAsk--;

        if (QuestionsLeftToAsk == 0)
            Time.timeScale = 1;

        Destroy(gameObject);
    }

    private void UpdateTitle(int index)
    {
        _title.text = Questions[index].Question;
    }
}
