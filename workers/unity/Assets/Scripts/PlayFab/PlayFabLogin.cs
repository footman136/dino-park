using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;


public class PlayFabLogin : MonoBehaviour
{
    public void Start()
    {
        //Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already.
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)){
            // 这个操蛋的玩意儿很重要，每个项目都不用，这是由PlayFab网站给你分配的，必须设置。
            // 
            // 地址在：https://developer.playfab.com/en-US/my-games
            PlayFabSettings.TitleId = "C5F82"; // Please change this value to your own titleId from PlayFab Game Manager
        }
    }

    public void Register(string account, string password)
    {
        var request = new RegisterPlayFabUserRequest { Username = account, Password = password, RequireBothUsernameAndEmail = false};
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
        Debug.Log("PlayFab : Register... <" + account + "," + password + ">");
    }
    public void Login(string account, string password)
    {
        //var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true};
        var request = new LoginWithPlayFabRequest { Username = account, Password = password};
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
        Debug.Log("PlayFab : Login... <" + account + "," + password + ">");
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        string id = result.EntityToken.Entity.Id;
        UIManager.Instance.SystemTips("PlayFab 注册成功！ " + id, PanelSystemTips.MessageType.Success);
        ClientManager.Instance.StateMachine.TriggerTransition(ConnectionFSMStateEnum.StateEnum.START); 
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        UIManager.Instance.SystemTips("PlayFab 注册失败！ "+error.GenerateErrorReport(), PanelSystemTips.MessageType.Error);
        Debug.LogError("PlayFab Register failed! " + error.GenerateErrorReport());
        ClientManager.Instance.StateMachine.TriggerTransition(ConnectionFSMStateEnum.StateEnum.START); 
    }

    private void OnLoginSuccess(LoginResult result)
    {
        string idStr = result.EntityToken.Entity.Id;
        long id = long.Parse(idStr,NumberStyles.HexNumber);
        UIManager.Instance.SystemTips("PlayFab 登录成功！ <" + idStr +"> <" + id + ">", PanelSystemTips.MessageType.Success);
        ClientManager.Instance.StateMachine.TriggerTransition(ConnectionFSMStateEnum.StateEnum.CONNECTING); 
    }

    private void OnLoginFailure(PlayFabError error)
    {
        UIManager.Instance.SystemTips("PlayFab 登录失败！ "+error.GenerateErrorReport(), PanelSystemTips.MessageType.Error);
        Debug.LogError("PlayFab : login failed:" + error.GenerateErrorReport());
        ClientManager.Instance.StateMachine.TriggerTransition(ConnectionFSMStateEnum.StateEnum.START); 
    }
}