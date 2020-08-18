using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionCreatorController : MonoBehaviour
{
    [SerializeField] private TMP_InputField _titleInputField;
    [SerializeField] private TMP_InputField _questionInputField;
    [SerializeField] private TMP_Dropdown _QuestionTypeDropdown;
    [SerializeField] private GameObject _scaleInputFields;
    [SerializeField] private TMP_InputField _lowerScaleInputField;
    [SerializeField] private TMP_InputField _upperScaleInputField;
    [SerializeField] private TMP_Dropdown _frequencyDropdown;

    private void OnEnable()
    {
        _titleInputField.text = "";
        _questionInputField.text = "";
        _scaleInputFields.SetActive(false);
        _lowerScaleInputField.text = "0";
        _upperScaleInputField.text = "10";
        _QuestionTypeDropdown.value = 0;
        _frequencyDropdown.value = 0;
    }

    public void OnQuestionTypeDropdownValueChanged()
    {
        _scaleInputFields.SetActive(_QuestionTypeDropdown.value == QuestionConstants.DROPDOWN_SCALE_RESPONSE);
    }

    public void OnSubmit()
    {
        var question = new QuestionDialogue
        {
            Question = _questionInputField.text,
            QuestionType = _QuestionTypeDropdown.value,
            LowerScale = int.Parse(_lowerScaleInputField.text),
            UpperScale = int.Parse(_upperScaleInputField.text),
            AskFrequency = _frequencyDropdown.value
        };

        DBController.AddQuestion(question, _titleInputField.text);

        gameObject.SetActive(false);
    }

    public void OnCancel()
    {
        gameObject.SetActive(false);
    }
}
