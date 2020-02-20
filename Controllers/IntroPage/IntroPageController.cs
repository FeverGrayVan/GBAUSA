using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Application.Loader;

public class IntroPageController : MonoBehaviour {

    public void AuthClick() {
        Button loginBtn = GameObject.Find("Login").GetComponent<Button>();
        String text = loginBtn.GetComponentInChildren<Text>().text;
        switch (text) {

            case "Login":
                login("Prefabs/AuthModal");
                break;

            case "Logout":
                logout();
                break;

            default:
                break;

        }
    }

    public void Start()
    {
        //StartCoroutine(viewFix()); //updateView();
    }

    private void login(String prefabPath)
    {
        GameObject popup = Resources.Load(prefabPath) as GameObject;
        if (popup == null) {
            Debug.Log("Error loading prefab");
            //Application.Quit;
        } else {
            GameObject popupCloned = Instantiate(popup, new Vector3(100, 100, 2), Quaternion.identity.normalized);
            popupCloned.transform.SetParent(GameObject.Find("Canvas").transform);
            popupCloned.transform.localPosition = new Vector3(0, 0, 2);
        }
    }

    private void logout() {
        StartCoroutine(HttpManager.instance.auth.Logout(
            HttpManager.instance.token,
            (() => {
                HttpManager.instance.token = String.Empty;
                UserManager.instance.Logout();
                updateView(false);
            }),
            ((string error) => {
                Debug.Log(error);
            })
            ));
    }

    // public IEnumerator viewFix()
    // {
    //     yield return new WaitForEndOfFrame();
    //     try 
    //     {
    //         if (UserManager.instance.user != null)// && UserManager.instance.user.lastname != null)
    //         {
    //             Debug.Log("updateView cast");
    //             updateView(true);
    //         }
    //     }
    //     catch (Exception ex) 
    //     {
    //         Debug.Log("Found Exception " + ex);
    //     }
    //     //Debug.Log("Username is "+ UserManager.instance.user.firstname + UserManager.instance.user.lastname);       
    // }

    private void updateView(bool login = true) {
        if (login)
        {
            Button loginBtn = GameObject.Find("Login").GetComponent<Button>();
            loginBtn.GetComponentInChildren<Text>().text = "Logout";

            Text userName = GameObject.Find("UserName").GetComponent<Text>();
            userName.text = UserManager.instance.user.firstname + " " + UserManager.instance.user.lastname;
        }
        else
        {
            Button loginBtn = GameObject.Find("Login").GetComponent<Button>();
            loginBtn.GetComponentInChildren<Text>().text = "Login";

            Text userName = GameObject.Find("UserName").GetComponent<Text>();
            userName.text = String.Empty;
        }
    }

}
