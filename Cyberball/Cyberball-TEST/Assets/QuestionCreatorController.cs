using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionCreatorController : MonoBehaviour
{
    [SerializeField] private TMP_InputField _titleInputField;
    [SerializeField] private TMP_InputField _questionInputField;
    [SerializeField] private Toggle _toggle;

    private void OnEnable()
    {
        _titleInputField.text = "";
        _questionInputField.text = "";
        _toggle.isOn = false;
    }

    public void OnSubmit()
    {
        var question = new QuestionDialogue
        {
            Question = _questionInputField.text,
            IsYesOrNo = _toggle.isOn
        };

        DBController.AddQuestion(question, _titleInputField.text);

        gameObject.SetActive(false);
    }

    public void OnCancel()
    {
        gameObject.SetActive(false);
    }
}
