using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Proyecto26;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DBController : MonoBehaviour
{
    public delegate void PostUserCallback();
    public delegate void GetUserCallback(User user);

    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private GameObject _playButton;

    private const string DbPath = "https://urvos-2020.firebaseio.com/";
    private const string AuthKey = "AIzaSyByIvr_ewxC6xxxDeaTWcIC3GBoWBNqY-c";

    private static string _name;
    private static string _idToken;
    private static string _localId;

#if UNITY_EDITOR
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
    }

    private void InitializeSDK()
    {
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile("C:\\Users\\FiercePC\\Downloads\\urvos-2020-firebase-adminsdk-9vnr8-1df8cfd048.json")
        });
    }

    private async void MakeUserAdmin()
    {
        var claims = new Dictionary<string, object>()
        {
            {"admin", true}
        };

        await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(_localId, claims);
    }

    private async void CheckClaims()
    {
        UserRecord user = await FirebaseAuth.DefaultInstance.GetUserAsync(_localId);
        Debug.Log(user.CustomClaims["admin"]);
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
#endif

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

    private static void PutUser(User user, string userId, string idToken, PostUserCallback callback)
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
