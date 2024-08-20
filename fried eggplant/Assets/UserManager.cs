using Assets.Scripts.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserManager : MonoBehaviour
{
    public GameObject userCreationPanel;
    public TMP_InputField usernameTxt;
    void Start()
    {
        if (GameManager.GetInstance().hasId == false)
        {
            userCreationPanel.SetActive(true);
        }
        else userCreationPanel.SetActive(false);
    }

    async public void createUser()
    {
        if (usernameTxt.text.Length > 0)
        {
            bool status = await DBManager.instance.insertPlayer(usernameTxt.text);
            if (status)
            {
                userCreationPanel.SetActive(false);
            }
        }
    }

    void Update()
    {
        
    }
}
