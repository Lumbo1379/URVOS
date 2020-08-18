using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Proyecto26;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DBController : MonoBehaviour
{
    public delegate void PutUserCallback();
    public delegate void GetUserCallback(User user);
    public delegate void PutQuestionCallback();

    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private GameObject _playButton;
    [SerializeField] private GameObject _questionCreator;

    private const string DbPath = "https://urvos-2020.firebaseio.com/";
    private const string AuthKey = "AIzaSyByIvr_ewxC6xxxDeaTWcIC3GBoWBNqY-c";

    private static string _name;
    private static string _idToken;
    private static string _localId;

    private void Start()
    {
        InitializeSDK();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
            MakeUserAdmin();

        if (Input.GetKeyDown(KeyCode.F3))
            CheckIfAdmin();

        if (Input.GetKeyDown(KeyCode.F4))
            AddQuestion();
    }

    private void InitializeSDK()
    {
        try
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential =
                    GoogleCredential.FromFile(
                        "C:\\Users\\FiercePC\\Downloads\\urvos-2020-firebase-adminsdk-9vnr8-1df8cfd048.json")
            });
        }
        catch
        {
            Debug.Log("That file does not exist!");
        }
    }

    private async void MakeUserAdmin()
    {
        var claims = new Dictionary<string, object>()
        {
            {"admin", true}
        };

        await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(_localId, claims);
    }

    private async Task<bool> CheckClaim(string claim)
    {
        UserRecord user = await FirebaseAuth.DefaultInstance.GetUserAsync(_localId);
        return user.CustomClaims.ContainsKey(claim);
    }

    private async void CheckIfAdmin()
    {
        FirebaseToken decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(_idToken);
        object isAdmin;

        if (decoded.Claims.TryGetValue("admin", out isAdmin))
        {
            if ((bool)isAdmin)
                Debug.Log($"You ARE an admin:\nid: {_localId}\ntoken: {_idToken}");
            else
                Debug.Log($"You are NOT an admin:\nid: {_localId}\ntoken: {_idToken}");
        }
        else
            Debug.Log("You don't have that claim assigned!");
    }

    private static void PutQuestion(QuestionDialogue question, string title, string idToken, PutQuestionCallback callback)
    {
        RestClient.Put<QuestionDialogue>($"{DbPath}questions/{title}.json?auth={idToken}", question).Then(response => { callback(); }).Catch(Debug.Log);
    }

    public static void AddQuestion(QuestionDialogue question, string title)
    {
        PutQuestion(question, title, _idToken, () =>
        {
            Debug.Log($"Question: {title} added successfully");
        });
    }

    private async void AddQuestion()
    {
        bool isAdmin = await CheckClaim("admin");

        if (isAdmin)
            _questionCreator.SetActive(true);
        else
            Debug.Log("You are not authorized to add questions!");
    }

    public static void OnSubmit(string date, string gameMode, string age, string gender, int noOfRounds, int playerCatchCount, int valence, int arousal, int dominance)
    {
        var user = new User
        {
            Date = date,
            GameMode = gameMode,
            Age = age,
            Gender = gender,
            NoOfRounds = noOfRounds,
            PlayerCatchCount = playerCatchCount,
            Valence = valence,
            Arousal = arousal,
            Dominance = dominance
        };

        PutUser(user, _localId, _idToken, () =>
        {
            Debug.Log($"{_name} added successfully!");
        });
    }

    public static void OnGetUserScore()
    {
        GetUser(_name, _idToken, user =>
        {

        });
    }

    public void OnSignIn()
    {
        SignInUser(_emailInputField.text, _passwordInputField.text);

        _emailInputField.text = "";
        _passwordInputField.text = "";
    }

    private static void PutUser(User user, string userId, string idToken, PutQuestionCallback callback)
    {
        RestClient.Put<User>($"{DbPath}users/{userId}.json?auth={idToken}", user).Then(response => { callback(); }).Catch(Debug.Log);
    }

    private static void GetUser(string userId, string idToken, GetUserCallback callback)
    {
        RestClient.Get<User>($"{DbPath}users/{userId}.json?auth={idToken}").Then(user => { callback(user); }).Catch(Debug.Log);
    }

    private void SignInUser(string email, string password)
    {
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + AuthKey, userData).Then(
            response =>
            {
                _idToken = response.idToken;
                _localId = response.localId;
                _playButton.SetActive(true);
                Debug.Log("Signed in successfully!");
            }).Catch(Debug.Log);
    }
}

[Serializable]
public class User
{
    public string Date;
    public string GameMode;
    public string Age;
    public string Gender;
    public int NoOfRounds;
    public int PlayerCatchCount;
    public int Valence;
    public int Arousal;
    public int Dominance;
}

[Serializable]
public class SignResponse
{
    public string localId;
    public string idToken;
}

[Serializable]
public class QuestionDialogue
{
    public string Question;
    public int QuestionType;
    public int LowerScale;
    public int UpperScale;
    public int AskFrequency;
}
