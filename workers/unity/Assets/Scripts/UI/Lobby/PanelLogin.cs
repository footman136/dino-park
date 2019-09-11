using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelLogin : MonoBehaviour
{
    [SerializeField] private Text _lbAccount;

    [SerializeField] private Text _lbPassword;

    // Start is called before the first frame update
    void Start()
    {
        _lbAccount.text = PlayerPrefs.GetString(GameSettings.KEY_ACCOUNT);
        _lbPassword.text = PlayerPrefs.GetString(GameSettings.KEY_PASSWORD);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickLogin()
    {
        //???寫不進去，不知道爲什麽
        PlayerPrefs.SetString(GameSettings.KEY_ACCOUNT, _lbAccount.text);
        PlayerPrefs.SetString(GameSettings.KEY_PASSWORD, _lbPassword.text);
        PlayerPrefs.SetInt("key_myint", 12345);
        PlayerPrefs.Save();
        long accountId = Crc32.GetCRC32(_lbAccount.text);
        GameManager.Instance.StateMachine.TriggerTransition(ConnectionFSMStateEnum.StateEnum.CONNECTING);
    }
}
