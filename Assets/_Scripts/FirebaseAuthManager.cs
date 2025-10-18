using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using System.Collections;
using TMPro;
using System.Diagnostics.Eventing.Reader;
using UnityEditor.Search;
using UnityEngine.SceneManagement;

public class FirebaseAuthManager : MonoBehaviour
{
    [Header("Firebase")]
    private DependencyStatus _dependencyStatus;
    private FirebaseAuth _auth;
    private FirebaseUser _user;

    [Space]
    [Header("Login")]
    [SerializeField] private TMP_InputField emailLoginField;
    [SerializeField] private TMP_InputField passwordLoginField;

    [Space]
    [Header("Registration")]
    [SerializeField] private TMP_InputField nameRegisterField;
    [SerializeField] private TMP_InputField emailRegisterField;
    [SerializeField] private TMP_InputField passwordRegisterField;
    [SerializeField] private TMP_InputField confirmPasswordRegisterField;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            _dependencyStatus = task.Result;

            if (_dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all firebase dependencies: " + _dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        //Set the default instance object
        _auth = FirebaseAuth.DefaultInstance;

        FirebaseApp app = FirebaseApp.DefaultInstance;

        if (app != null && string.IsNullOrEmpty(app.Options.DatabaseUrl.ToString()))
        {
            app.Options.DatabaseUrl = new System.Uri("https://arfitness-1e168-default-rtdb.firebaseio.com/");
        }

        _auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    public void ChangeSceneOnMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void CheckUserEmailProfile()
    {
        /*if(User exists in Sqlite)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }
        else
        {
            ShowFieldsForRegistration
        }*/
    }

    // Track state changes of the auth object.
    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (_auth.CurrentUser != _user)
        {
            bool signedIn = _user != _auth.CurrentUser && _auth.CurrentUser != null;

            if (!signedIn && _user != null)
            {
                Debug.Log("Signed out " + _user.UserId);
            }

            _user = _auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in " + _user.UserId);
            }
        }
    }

    public void Login()
    {
        StartCoroutine(LoginWithEmailAsync(emailLoginField.text, passwordLoginField.text));
    }

    private IEnumerator LoginWithEmailAsync(string email, string password)
    {
        var loginTask = _auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "Login Failed! Because ";

            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing";
                    break;
                default:
                    failedMessage = "Login Failed";
                    break;
            }

            Debug.Log(failedMessage);
        }
        else
        {
            _user = loginTask.Result.User;

            Debug.LogFormat("{0} You Are Successfully Logged In", _user.DisplayName);

            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }
    }

    public void Register()
    {
        StartCoroutine(RegisterAsync(nameRegisterField.text, emailRegisterField.text, passwordRegisterField.text, confirmPasswordRegisterField.text));
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if (name == "")
        {
            Debug.LogError("User Name is empty");
        }
        else if (email == "")
        {
            Debug.LogError("email field is empty");
        }
        else if (passwordRegisterField.text != confirmPasswordRegisterField.text)
        {
            Debug.LogError("Password does not match");
        }
        else
        {
            var registerTask = _auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogError(registerTask.Exception);

                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Registration Failed! Becuase ";
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "Wrong Password";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "Email is missing";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "Password is missing";
                        break;
                    default:
                        failedMessage = "Registration Failed";
                        break;
                }

                Debug.Log(failedMessage);
            }
            else
            {
                _user = registerTask.Result.User;

                UserProfile userProfile = new UserProfile();
                userProfile.DisplayName = name;

                var updateProfileTask = _user.UpdateUserProfileAsync(userProfile);

                yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                if (updateProfileTask.Exception != null)
                {
                    // Delete the user if user update failed
                    _user.DeleteAsync();

                    Debug.LogError(updateProfileTask.Exception);

                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;

                    string failedMessage = "Profile update Failed! Becuase ";
                    switch (authError)
                    {
                        case AuthError.InvalidEmail:
                            failedMessage += "Email is invalid";
                            break;
                        case AuthError.WrongPassword:
                            failedMessage += "Wrong Password";
                            break;
                        case AuthError.MissingEmail:
                            failedMessage += "Email is missing";
                            break;
                        case AuthError.MissingPassword:
                            failedMessage += "Password is missing";
                            break;
                        default:
                            failedMessage = "Profile update Failed";
                            break;
                    }

                    Debug.Log(failedMessage);
                }
                else
                {
                    Debug.Log("Registration Sucessful Welcome " + _user.DisplayName);
                    SceneManager.LoadScene("Main");
                }
            }
        }
    }
}
