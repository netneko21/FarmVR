using TMPro;
using VRKeys;

namespace CurioAssets
{
    #region Usings
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using UnityEngine;
    using UnityEngine.UI;
    using Facebook.Unity;
    using PlayFab;
    using PlayFab.ClientModels;
    #endregion
    public class EasyPlayFabIntegration : MonoBehaviour
    {
        #region public Variable declartions
       // public InputField[] signUpFields;
       // public InputField loginEmail, loginPassword, resetPasswordEmail;
       public Button[] // loginItems,
           loginItemsFB;
            //, logoutItems;
        //GameObjects for loading leaderboard, parent of leaderboard, reset password panel, panel that holds login items, and panel that hold leaderboard
        public GameObject loadingLeaderboard, leaderboardPanel;
       // public Text[] invalidInfoText;
        // Error and correct Message Info text Colors
       // public Color[] valTextClr;
        // Avatar to be used for user's whose profile picture is protected or who are not your friends on FB
        public Sprite defaultAvatar;

        public TextMeshProUGUI welcomeText;
        // Array to hold all the UILeadboard Item
        UILeaderboardItem[] lstLeader;
        public static List<PlayerLeaderboardEntry> leaderboardItems = new List<PlayerLeaderboardEntry>();
        #endregion

        #region Init
        void Start()
        {
            //AfterLoginSteps(false);
            PlayFabSettings.TitleId = PLAYFAB_GAMEID;
            lstLeader = leaderboardPanel.GetComponentsInChildren<UILeaderboardItem>();
            if (LoginMode == 0)
            {
             
               /// loginEmail.text = "newPlayer";
                LoginWithEmail();
            }
            else
            {
                InitNLogin();
            }
        }
        #endregion

        #region PlayFab Login & Registration
        // Loggin in with Email account
        public void LoginWithEmail(bool firstTime = false)
        {
            LoginMode = 0;
            string email = string.Empty, password = string.Empty;
            if (firstTime && PlayerPrefs.HasKey(EMAIL_ACCOUNT))
            {
                email = PlayerPrefs.GetString(EMAIL_ACCOUNT);
                //keyboard.text = email;
                password = "password";//PlayerPrefs.GetString(PASSWORD);
                welcomeText.text = "welcome, " + email;
            }
            else
            {
                email = "newPlayer";//loginEmail.text;
                //nameInput.text = email;
                welcomeText.text = "welcome, " + email;
                password = "password";//loginPassword.text;
            }
            
            LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                Email = email+"@gmail.com",
                Password = password
            };
            
            PlayFabClientAPI.LoginWithEmailAddress(request, (result) =>
            {
                Debug.Log("Got PlayFabID: " + result.PlayFabId);
                PlayerPrefs.SetString(EMAIL_ACCOUNT, email);
                PlayerPrefs.SetString(PASSWORD, password);
                PlayerPrefs.Save();
                //loginPanel.SetActive(false);
                //signUpPanel.SetActive(false);
                if(result.NewlyCreated)
                {
                    Debug.Log("new account"); // Here we can do the first time stuff !
                }
                else
                    Debug.Log("existing account");
            //    AfterLoginSteps(true);
            }, (error) =>
            {
                //invalidInfoText[0].gameObject.SetActive(true);
                //invalidInfoText[0].text = "Email or Password isn't correct " + error.ErrorMessage;
                
            });
        }

        // Method to display UI for Login/Register Panel
        public void ShowLogin()
        {
          //  loginPanel.SetActive(true);
      //      HideErrorMessageInStart();
        }
        
        public void ShowSignUp()
        {
           // signUpPanel.SetActive(true);
            Logout_FB_PlayFab();
        //    HideErrorMessageInStart();
        }

        public void ShowSignUp2()
        {
          //  signUpPanel.SetActive(true);
            Logout_FB_PlayFab();
       //     HideErrorMessageInStart();
        }
        
        // After Checks done proceed with Account registeration 
        void RegisterOrGo()
        {
            ValidateNewAccountCreation();
            
            //loginEmail.text = "newPlayer";
        }
                
        void LoginToDefault()
        {
          
         //   AfterLoginSteps(false);
            if (LoginMode == 0)
            {
                //keyboard.text = "newPlayer";
                LoginWithEmail();
            }
        }
        

        void ContinueSignUp()
        {
            string email = keyboard.text;
            
            
            string password = "password";//"signUpFields[4].text;
 //           string LastName = "LastName";//signUpFields[2].text;
            string theName = "theName";//signUpFields[1].text;
            
//            string contactNum = "79998887766";//signUpFields[3].text;
            welcomeText.text = "Welcome, " +email;
            RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                Email = email+"@gmail.com",
                Password = password,
                RequireBothUsernameAndEmail = false
            };
            string[] msgs2 = { "Title Display name updated successfully", "Title Display name updated error" };

            PlayFabClientAPI.RegisterPlayFabUser(request, result =>
            {
                Debug.LogFormat("Got PlayFabID: {0}", result.PlayFabId);
                PlayerPrefs.SetString(EMAIL_ACCOUNT, email);
                PlayerPrefs.SetString(PASSWORD, password);
                PlayerPrefs.Save();
                UpdateUserTitleDisplayNameRequest displayNameRequest = new UpdateUserTitleDisplayNameRequest()
                {
                    DisplayName = theName
                };
                
                PlayFabClientAPI.UpdateUserTitleDisplayName(displayNameRequest, (response) =>
                {
                    Debug.Log(msgs2[0]);
                }, (error) =>
                {
                    Debug.Log(msgs2[1] + error.Error);

                }, null);
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("TheName", theName); // Here you can store user Data! Like their name, sir name, telephone or  stick, coins etc as well.
                //data.Add("TheSirName", sirName);
                //data.Add("TheTelephone", contactNum);
                UpdateUserDataRequest userDataRequest = new UpdateUserDataRequest()
                {
                    Data = data,
                    Permission = UserDataPermission.Public
                };
                PlayFabClientAPI.UpdateUserData(userDataRequest, result1 =>
                {
                    Debug.Log("Data updated successfull ");
                }, error1 =>
                {
                    Debug.Log("Data updated error " + error1.ErrorMessage);
                }, null);
                LoginWithEmail(true);
            }, error =>
            {
             /*   invalidInfoText[1].gameObject.SetActive(true);
                if (error.Error == PlayFabErrorCode.EmailAddressNotAvailable)
                    invalidInfoText[1].text = "Email address isn't available!";
                else if (error.Error == PlayFabErrorCode.InvalidEmailAddress)
                    invalidInfoText[1].text = "Email address is invalid!";
                else
                    invalidInfoText[1].text = error.ErrorMessage;
                    */
            });
        }
        internal string[] errorMsgs = new string[5] { "required Fields can't be left empty!", "Email isn't in correct format",
            "Password should be atleast 6 characters!", "Password doesn't match", "Proceeding Now !" };
        // Validation (checks) to ensure all requires fields are filled and no invalid data is entered.
        //public TextMeshProUGUI nameInput;
        public Keyboard keyboard;
        public void ValidateNewAccountCreation()
        {
            string email = keyboard.text;//signUpFields[0].text;
            Debug.Log("validate email "+email);
            string password = "password";//"signUpFields[4].text;
            string LastName = "LastName";//signUpFields[2].text;
            string theName = "theName";//signUpFields[1].text;
            string tele = "79998887766";//signUpFields[3].text;
            string errorMessage = "";

            if (string.IsNullOrEmpty(email))
            {
                errorMessage = errorMsgs[0];
            }
            //else if (!Regex.IsMatch(email, EMAIL_REGEX))
            //{
            //    errorMessage = errorMsgs[1];
            //}
           // else if (password.Length < 6)
          //  {
           //     errorMessage = errorMsgs[2];
            //}
            //else if (!string.Equals(password, signUpFields[5].text))
           //{
            //    errorMessage = errorMsgs[3];
            //}
            else
            {
                errorMessage = errorMsgs[4];
                ContinueSignUp();
            }
            
          // invalidInfoText[1].gameObject.SetActive(true);
            //invalidInfoText[1].text = errorMessage;
        }
        /*
        // Hiding error message for a new start
        void HideErrorMessageInStart()
        {
            foreach (var item in invalidInfoText)
            {
                item.gameObject.SetActive(false);
            }
            for (int i = 0; i < signUpFields.Length; i++)
            {
                signUpFields[i].text = string.Empty;
            }
            resetPasswordPanel.SetActive(false);
        }*/
        
        void SetUserMailFromFB_ForPlayFabAccount()
        {
            // Only needed if you wish to require User email and set on playfab
            FB.API("me?fields=email", HttpMethod.GET, result =>
            {
                string email = "";
                Dictionary<string, object> FBUserDetails = (Dictionary<string, object>)result.ResultDictionary;
                try
                {
                    email = FBUserDetails["email"].ToString();
                }
                catch (Exception)
                {
                    //email = "";
                }
                AddOrUpdateContactEmailRequest request = new AddOrUpdateContactEmailRequest
                {
                    EmailAddress = email
                };
                PlayFabClientAPI.AddOrUpdateContactEmail(request, result1 =>
                {
                    Debug.Log("Successfully Stored Email");
                }, error =>
                {
                    Debug.Log("Failed To Store Value " + error.ErrorMessage);
                }, null);
            }, new Dictionary<string, string>() { });
        }
        public const string PLAYFAB_GAMEID = "AF48", EMAIL_ACCOUNT = "email_account", PASSWORD = "password",
            LEADERBOARD_STRING = "Leaderboard", LOGIN_MODE = "LOGIN_TYPE",
        EMAIL_REGEX = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
             @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        #endregion
/*
        #region Reset Password PlayFab
        public void ShowResetPassword()
        {
            resetPasswordEmail.text = "";
            invalidInfoText[2].gameObject.SetActive(false);
            resetPasswordPanel.SetActive(true);
        }
        public void ResetPassword()
        {
            invalidInfoText[2].gameObject.SetActive(false);

            string emailResetPassword = resetPasswordEmail.text;
            if (string.IsNullOrEmpty(emailResetPassword) || !Regex.IsMatch(emailResetPassword, EMAIL_REGEX))
            {
                invalidInfoText[2].gameObject.SetActive(true);
                invalidInfoText[2].color = valTextClr[0];
                invalidInfoText[2].text = "Email isn't in correct Format";

            }
            else
            {
                SendAccountRecoveryEmailRequest request = new SendAccountRecoveryEmailRequest()
                {
                    TitleId = PlayFabSettings.TitleId,
                    Email = resetPasswordEmail.text
                };

                PlayFabClientAPI.SendAccountRecoveryEmail(request, (result) =>
                {
                    invalidInfoText[2].gameObject.SetActive(true);
                    invalidInfoText[2].color = valTextClr[1];
                    invalidInfoText[2].text = "Email sent for account recovery!";
                }, (error) =>
                {
                    invalidInfoText[2].gameObject.SetActive(true);
                    invalidInfoText[2].color = valTextClr[0];
                    invalidInfoText[2].text = "The provided email doesn't exist!";

                });
            }
        }
        #endregion
*/
        #region Show Leaderboard and Post Score To Leaderboard
        // Method that helps posting score for test purposes. Takes data from the Input field. 
        public void PostScoreTest(string t)
        {
            PostValueOnLeaderboard(int.Parse(t), true);
        }
        const string CONTESTANT = "Contestant {0}";
        // This method shows and refreshes leaderboard 
        public void ShowLeaderboard()
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                leaderboardPanel.SetActive(true);
                loadingLeaderboard.SetActive(true);
                PlayerProfileViewConstraints pc = new PlayerProfileViewConstraints();
                pc.ShowAvatarUrl = true;
                pc.ShowDisplayName = true;
                var request = new GetLeaderboardRequest
                {
                    MaxResultsCount = 10,
                    StatisticName = LEADERBOARD_STRING,
                    StartPosition = 0,
                    ProfileConstraints = pc
                };
                PlayFabClientAPI.GetLeaderboard(request, result =>
                {
                    leaderboardItems = result.Leaderboard;
                    int count = 0;
                    print(" Leaderboard count " + leaderboardItems.Count);
                    foreach (var item in leaderboardItems)
                    {
                        lstLeader[count].AssignValues(item.Position + 1,
                        string.IsNullOrEmpty(item.DisplayName) ? string.Format(CONTESTANT, item.Position + 1) : item.DisplayName, string.Format("{0} Score", item.StatValue));
                        if (!string.IsNullOrEmpty(item.Profile.AvatarUrl))
                            StartCoroutine(GetPicRoutine(item.Profile.AvatarUrl, count));
                        else
                            lstLeader[item.Position].picImg.sprite = defaultAvatar;
                        count++;
                    }
                    if (count < lstLeader.Length)
                    {
                        for (int z = count; z < lstLeader.Length; z++)
                        {
                            lstLeader[z].AssignValues(z + 1, string.Format(CONTESTANT, z + 1), "00000 Score");
                            lstLeader[z].picImg.sprite = defaultAvatar;
                        }
                    }
                    loadingLeaderboard.SetActive(false);
                }, error => { print(error.ErrorMessage); });
            }
        }

        public void PostValueOnLeaderboard(int score, bool refresh = false)
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
                PlayFabClientAPI.UpdatePlayerStatistics(
                    new UpdatePlayerStatisticsRequest()
                    {
                        Statistics = new List<StatisticUpdate>() { new StatisticUpdate() {
                        StatisticName = LEADERBOARD_STRING,
                        Value = score}}
                    },
                    result =>
                    {
                        Debug.Log("Score Posted");
                        if (refresh)
                            ShowLeaderboard();
                    },
                    error => Debug.Log(error.GenerateErrorReport())
                );
        }
        #endregion

        #region Facebook Login, Invite & Share
        List<string> readPermission = new List<string>() { "public_profile", "email" };
        const string message = "This is a Demo!", title = "Easy PlayFab Integration",
          SHARE_URL_STRING = "https://assetstore.unity.com/publishers/22181", SHARE_PIC_STRING = "https://i.imgur.com/KP1flWv.png",
           getUserPicName = "me?fields=id,name,picture.height(128)";
        public void InitNLogin()
        {
            if (FB.IsInitialized)
            {
                OnFacebookInitialized();
            }
            else
            {
                print("Initializing Facebook...");
                FB.Init(OnFacebookInitialized);
            }
        }
        private void OnFacebookInitialized()
        {
            print("Logging with Facebook...");
            //We invoke basic login procedure and pass in the callback to process the result
            StartCoroutine(WaitForASecBeforeLogin());
        }
        IEnumerator WaitForASecBeforeLogin()
        {
            yield return new WaitForSeconds(1);
            if (FB.IsLoggedIn)
            {
                PostLoginStepsFB();
            }
            else
            {
                FB.LogInWithReadPermissions(readPermission, result =>
                {
                    if (result == null || string.IsNullOrEmpty(result.Error))//If result has no errors, it means we have authenticated in Facebook successfully
                    {
                        print("Facebook Auth Complete! Access Token: " + AccessToken.CurrentAccessToken.TokenString + "\nLogging into PlayFab...");
                        PostLoginStepsFB();
                    }
                    else
                    {
                        //If Facebook authentication failed, we stop the cycle with the message
                        Debug.LogError("Facebook Auth Failed: " + result.Error + "\n" + result.RawResult);
                    }
                });
            }
        }

        void GetRequiredDataFBAndSetOnPlayFab()
        {
            FB.API(getUserPicName, HttpMethod.GET,
              result =>
                {
                    if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
                    {
                        string currPName = result.ResultDictionary["name"] as string;
                        IDictionary picData = result.ResultDictionary["picture"] as IDictionary;
                        IDictionary data = picData["data"] as IDictionary;
                        string currAvatarURL = data["url"] as string;
                        var request = new UpdateAvatarUrlRequest
                        {
                            ImageUrl = currAvatarURL
                        };
                        PlayFabClientAPI.UpdateAvatarUrl(request, emptyResult => { print("PICTURE UPDATED"); }, error => { print("Failed to Update Picture"); });
                        var request1 = new UpdateUserTitleDisplayNameRequest
                        {
                            DisplayName = currPName
                        };
                        PlayFabClientAPI.UpdateUserTitleDisplayName(request1, updateUserTitleDisplayNameResult => { print("NAME UPDATED"); },
                            error => { Debug.Log(error.ErrorMessage); });
                    }
                    Debug.Log(result.RawResult);
                });
        }
        // Coroutine that helps in getting user picture from the url stored on playfab or default avatar if no picture is found
        private IEnumerator GetPicRoutine(string url, int index)
        {
            WWW www = new WWW(url);
            yield return www;
            if (www.error != null || (www.texture.width == 8 && www.texture.height == 8))
            {
                // We got an Error or an Error Texture
                lstLeader[index].picImg.sprite = defaultAvatar;
            }
            else // Everything is good. Can assign Picture
            {
                Texture2D tex = www.texture;
                lstLeader[index].picImg.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            }

        }
        Uri fbShareURI = new Uri(SHARE_URL_STRING), fbSharePicURI = new Uri(SHARE_PIC_STRING);
        public void ShareOnFacebook()
        {
            if (FB.IsLoggedIn)
            {
                FB.ShareLink(
                    contentURL: fbShareURI,
                    contentTitle: title,
                    contentDescription: message,
                    photoURL: fbSharePicURI,
                    callback: result => { Debug.Log(result.RawResult); }
                    );
            }
        }
        public void InviteFriends()
        {
            if (FB.IsLoggedIn)
            {
                FB.AppRequest(
                message, null, null, null, null, null, title,
                r =>
                {
                    Debug.Log(r.RequestID);
                });
            }
            else
                InitNLogin();
        }
        #endregion

        #region Misc Common Stuff
        void PostLoginStepsFB()
        {
            PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest
            { CreateAccount = true, AccessToken = AccessToken.CurrentAccessToken.TokenString },
                 (response) =>
                 {
                     LoginMode = 1;
                     if (response.NewlyCreated) { }// First time stuff!!!
                    // AfterLoginSteps(true);
                     GetRequiredDataFBAndSetOnPlayFab();
                 }, (error) =>
                 {
                     Debug.Log("Failed to Login With Facebook");
                 });
        }
        public static int LoginMode
        {
            get { return PlayerPrefs.GetInt(LOGIN_MODE, 0); }
            set
            {
                PlayerPrefs.SetInt(LOGIN_MODE, value);
                PlayerPrefs.Save();
            }
        }
/*
        void AfterLoginSteps(bool login = true)
        {
            foreach (var item in loginItems)
            {
            //    item.interactable = login;
            }
            
            foreach (var item in logoutItems)
            {
            //    item.interactable = !login;
            }
            
            foreach (var item in loginItemsFB)
            {
         //       item.interactable = LoginMode == 1 && login;
            }
        }
*/
        public void Logout_FB_PlayFab()
        {
            PlayerPrefs.DeleteKey(LOGIN_MODE);
            PlayerPrefs.DeleteKey(EMAIL_ACCOUNT);
            PlayerPrefs.DeleteKey(PASSWORD);
            if (FB.IsInitialized && FB.IsLoggedIn)
                FB.LogOut();
            //AfterLoginSteps(false);
        }

        #endregion
    }
}