using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoginHandler : MonoBehaviour {

  protected Firebase.Auth.FirebaseAuth auth;
  private Firebase.Auth.FirebaseAuth otherAuth;
  protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
        new Dictionary<string, Firebase.Auth.FirebaseUser>();
  private string logText = "";
  public Text UsernameText;
  public InputField PasswordText;
  public Text GameCodeText;
  protected string email = "";
  protected string password = "";
  protected string gameCode = "";
  protected string displayName = "";
  private bool fetchingToken = false;
  const int kMaxLogSize = 16382;
  public DatabaseHandler database;

  Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

  public GamePlay gamePlay;
  public ToggleChecker toggleChecker;
  public UpdateScreens updateScreen;

  public void Start() {
       
      updateScreen.switchCameras(true);
      updateScreen.switchCanvas(true);
        
      Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
      dependencyStatus = task.Result;
      if (dependencyStatus == Firebase.DependencyStatus.Available) {
        InitializeFirebase();
      } else {
        Debug.LogError(
          "Could not resolve all Firebase dependencies: " + dependencyStatus);
      }
    });
  }

  // Handle initialization of the necessary firebase modules:
  void InitializeFirebase() {
    DebugLog("Setting up Firebase Auth");
    auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    auth.StateChanged += AuthStateChanged;
    auth.IdTokenChanged += IdTokenChanged;
    AuthStateChanged(this, null);
  }

  // Exit if escape (or back, on mobile) is pressed.
  public void Update() {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      Application.Quit();
    }
        email = UsernameText.text;
        password = PasswordText.text;
        gameCode = GameCodeText.text;

        if (toggleChecker.checkWhosPlaying())
        {
            updateScreen.switchLoginType(true);
        }
        else
        {
            updateScreen.switchLoginType(false);
        }
  }

  void OnDestroy() {
    auth.StateChanged -= AuthStateChanged;
    auth.IdTokenChanged -= IdTokenChanged;
    auth = null;
  }

  // Output text to the debug log text field, as well as the console.
  public void DebugLog(string s) {
    Debug.Log(s);
    logText += s + "\n";

    while (logText.Length > kMaxLogSize) {
      int index = logText.IndexOf("\n");
      logText = logText.Substring(index + 1);
    }
  }

  // Display user information.
  void DisplayUserInfo(Firebase.Auth.IUserInfo userInfo, int indentLevel) {
    string indent = new String(' ', indentLevel * 2);
    var userProperties = new Dictionary<string, string> {
      {"Display Name", userInfo.DisplayName},
      {"Email", userInfo.Email},
      {"Photo URL", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null},
      {"Provider ID", userInfo.ProviderId},
      {"User ID", userInfo.UserId}
    };
    foreach (var property in userProperties) {
      if (!String.IsNullOrEmpty(property.Value)) {
        DebugLog(String.Format("{0}{1}: {2}", indent, property.Key, property.Value));
      }
    }
  }

  // Display a more detailed view of a FirebaseUser.
  void DisplayDetailedUserInfo(Firebase.Auth.FirebaseUser user, int indentLevel) {
    DisplayUserInfo(user, indentLevel);
    DebugLog("  Anonymous: " + user.IsAnonymous);
    DebugLog("  Email Verified: " + user.IsEmailVerified);
    var providerDataList = new List<Firebase.Auth.IUserInfo>(user.ProviderData);
    if (providerDataList.Count > 0) {
      DebugLog("  Provider Data:");
      foreach (var providerData in user.ProviderData) {
        DisplayUserInfo(providerData, indentLevel + 1);
      }
    }
  }

  // Track state changes of the auth object.
  void AuthStateChanged(object sender, System.EventArgs eventArgs) {
    Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
    Firebase.Auth.FirebaseUser user = null;
    if (senderAuth != null) userByAuth.TryGetValue(senderAuth.App.Name, out user);
    if (senderAuth == auth && senderAuth.CurrentUser != user) {
      bool signedIn = user != senderAuth.CurrentUser && senderAuth.CurrentUser != null;
      if (!signedIn && user != null) {
        DebugLog("Signed out " + user.UserId);
                //user is logged out, load login screen 
                //SceneManager.LoadSceneAsync("scene_01");
      }
      user = senderAuth.CurrentUser;
      userByAuth[senderAuth.App.Name] = user;
      if (signedIn) {
        DebugLog("Signed in " + user.UserId);
        displayName = user.DisplayName ?? "";
        DisplayDetailedUserInfo(user, 1);
      }
    }
  }

  // Track ID token changes.
  void IdTokenChanged(object sender, System.EventArgs eventArgs) {
    Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
    if (senderAuth == auth && senderAuth.CurrentUser != null && !fetchingToken) {
      senderAuth.CurrentUser.TokenAsync(false).ContinueWith(
        task => DebugLog(String.Format("Token[0:8] = {0}", task.Result.Substring(0, 8))));
    }
  }

  public bool LogTaskCompletion(Task task, string operation) {
    bool complete = false;
    if (task.IsCanceled) {
      DebugLog(operation + " canceled.");
    }
    else if (task.IsFaulted)
    {
        DebugLog(operation + " encounted an error.");
        foreach (Exception exception in task.Exception.Flatten().InnerExceptions) {

                string authErrorCode = "";
                Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;

            if (firebaseEx != null) {

                authErrorCode = String.Format("AuthError.{0}: ",
                ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
            }
        DebugLog(authErrorCode + exception.ToString());
        }
    }
    else if (task.IsCompleted) {
      DebugLog(operation + " completed");
      complete = true;
    }
    return complete;
  }

  public void CreateUserAsync() {
    DebugLog(String.Format("Attempting to create user {0}...", email));
    string newDisplayName = displayName;
    auth.CreateUserWithEmailAndPasswordAsync(email, password)
      .ContinueWith((task) => {
        return HandleCreateUserAsync(task, newDisplayName: newDisplayName);
      }).Unwrap();
  }

  Task HandleCreateUserAsync(Task<Firebase.Auth.FirebaseUser> authTask,
                             string newDisplayName = null) {
    if (LogTaskCompletion(authTask, "User Creation")) {
      if (auth.CurrentUser != null) {
        DebugLog(String.Format("User Info: {0}  {1}", auth.CurrentUser.Email,
                               auth.CurrentUser.ProviderId));
        return UpdateUserProfileAsync(newDisplayName: newDisplayName);
      }
    }
    return Task.FromResult(0);
  }

  public Task UpdateUserProfileAsync(string newDisplayName = null) {
    if (auth.CurrentUser == null) {
      DebugLog("Not signed in, unable to update user profile");
      return Task.FromResult(0);
    }
    displayName = newDisplayName ?? displayName;
    DebugLog("Updating user profile");
    return auth.CurrentUser.UpdateUserProfileAsync(new Firebase.Auth.UserProfile {
        DisplayName = displayName,
        PhotoUrl = auth.CurrentUser.PhotoUrl,
      }).ContinueWith(HandleUpdateUserProfile);
  }

  void HandleUpdateUserProfile(Task authTask) {
    if (LogTaskCompletion(authTask, "User profile")) {
      DisplayDetailedUserInfo(auth.CurrentUser, 1);
    }
  }
  
  public void SigninAsync() {
    DebugLog(String.Format("Attempting to sign in as {0}...", email));
        Debug.Log(password);
        auth.SignInWithEmailAndPasswordAsync(email, password)   
      .ContinueWith(HandleSigninResult);
  }

  void HandleSigninResult(Task<Firebase.Auth.FirebaseUser> authTask) {      
        LogTaskCompletion(authTask, "Sign-in");

        if (toggleChecker.checkWhosPlaying())
        {
            gamePlay.setUsersEmail(email);
            gamePlay.setGameCode(gameCode);
            database.getPlayerNumber(email, "Human", gameCode);
            updateScreen.switchCameras(false);
            updateScreen.switchCanvas(false);
        }
        else
        {
            SceneManager.LoadSceneAsync("ProctorScene");
        } 
    }

  public void SignOut() {
    DebugLog("Signing out.");
    auth.SignOut();
  }

}
