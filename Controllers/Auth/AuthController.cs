using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using System;
using UnityEngine.UI;
using Helpers.HTTP;
using Application.Loader;
using UnityEngine.EventSystems;


public class AuthController : MonoBehaviour
{
    public InputField Email, Password, SecondPassword, FirstName, LastName, State, Zip, Phone, LogEmail, LogPassword;
    public GameObject toolTip;
    public EventSystem system;
    public Selectable next;

    public string pwd;

    public void Start()
    {
        system = EventSystem.current;
        toolTip = Resources.Load<GameObject>("Prefabs/ToolTipPref");
        //if (PlayerPrefs.HasKey("Login"))
        //{
            //string login = PlayerPrefs.GetString("Login");
            //Button loginBtn = GameObject.Find("Login").GetComponent<Button>();
            //loginBtn.GetComponentInChildren<Text>().text = login;
        //}
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            switch (system.currentSelectedGameObject.name)
            {
                case "InputField : Email":
                next = GameObject.Find("InputField : Password").GetComponent<Selectable>();
                break;
                case "InputField : Password":
                next = GameObject.Find("InputField : PasswordConfirm").GetComponent<Selectable>();
                break;
                case "InputField : PasswordConfirm":
                next = GameObject.Find("InputField : State").GetComponent<Selectable>();
                break;
                case "InputField : State":
                next = GameObject.Find("InputField : Zip").GetComponent<Selectable>();
                break;
                case "InputField : Zip":
                next = GameObject.Find("InputField : FirstName").GetComponent<Selectable>();
                break;
                case "InputField : FirstName":
                next = GameObject.Find("InputField : LastName").GetComponent<Selectable>();
                break;
                case "InputField : LastName":
                next = GameObject.Find("InputField : Phone").GetComponent<Selectable>();
                break;
                case "InputField : Phone":
                next = GameObject.Find("InputField : LogEmail").GetComponent<Selectable>();
                break;
                case "InputField : LogEmail":
                next = GameObject.Find("InputField : LogPassword").GetComponent<Selectable>();
                break;
                case "InputField : LogPassword":
                next = GameObject.Find("InputField : Email").GetComponent<Selectable>();
                break;
            }

            InputField inputfield = next.GetComponent<InputField>();
            if (inputfield != null)
            {
                inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret
                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }   
        }
    }

    /*public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            Debug.Log("system.currentSelectedGameObject = " + system.currentSelectedGameObject);
            if (system.currentSelectedGameObject.name == "InputField : Zip" )
            {   
                next = GameObject.Find("InputField : FirstName").GetComponent<Selectable>();
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                    inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret
             
                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));

            } 
            else if (system.currentSelectedGameObject.name == "RegisterBtn" )
            {
                next = GameObject.Find("InputField : LogEmail").GetComponent<Selectable>();
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                    inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret
             
                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
            else if (system.currentSelectedGameObject.name == "LoginBtn" )
            {
                next = GameObject.Find("InputField : Email").GetComponent<Selectable>();
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                    inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret
             
                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
            else if (next != null)
            {
             
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                    inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret
             
                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            } else if (next == null)
            {
                next = GameObject.Find("InputField : Email").GetComponent<Selectable>();
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                    inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret
             
                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
            //else Debug.Log("next nagivation element not found");
         
        }
    }*/

    public void Register()
    {
        try
        {
            String email = Email.text;
            String password = Password.text;
            String secpasword = SecondPassword.text;
            String firstname = FirstName.text;
            String lastname = LastName.text;
            String state = State.text;
            String zip = Zip.text;
            String phone = Phone.text;

            int int_zip = Int32.Parse(zip);

            //todo:validation for this fields
            var go = Validation(email, password, secpasword, firstname, lastname, state, int_zip, phone);
            Debug.Log("validation is = " + go);
            
            if(go)
                StartCoroutine(HttpManager.instance.auth.Register(email, password, firstname, lastname, state, int_zip, phone,
                    (() =>
                    {
                        Debug.Log("Registration success");
                        updateView(true);
                        Close();
                    }),
                    ((string error) =>
                    {
                        Debug.Log(error);
                        toolTip.GetComponent<ToolTipController>().setMessage(error);
                        GameObject.Instantiate(toolTip);
                    })));
            
        }
        catch (Exception ex)
        {
            Debug.Log("Found exception : " + ex);
            if (ex.Message.Contains("correct format"))
            {
                Debug.Log("correct format exception");
                toolTip.GetComponent<ToolTipController>().setMessage(ex.ToString());
                GameObject.Instantiate(toolTip);
                Debug.Log(ex.Message);
            }
            else
            {
                toolTip.GetComponent<ToolTipController>().setMessage(ex.Message);
                GameObject.Instantiate(toolTip);
                Debug.Log("additional exception : " + ex.Message);
            }
        }
    }
    private bool Validation(String email, String pwd, String secpwd, String firstname, String lastname, String state, int zip, String phone)
    {
        if (!email.Contains("@"))
        {
            toolTip.GetComponent<ToolTipController>().setMessage("Email not valid >> missing '@'");
            GameObject.Instantiate(toolTip);
            Email.Select();
        }
        else if (!email.Contains(".com"))
        {
            toolTip.GetComponent<ToolTipController>().setMessage("Email not valid >> missing '.com'");
            GameObject.Instantiate(toolTip);
            Email.Select();
        }
        else if (pwd.Length < 8)
        {
            toolTip.GetComponent<ToolTipController>().setMessage("Password is too short. Minimum of 8 characters is required.");
            GameObject.Instantiate(toolTip);
            Password.Select();
        } else if (pwd != secpwd)
            {
                toolTip.GetComponent<ToolTipController>().setMessage("Password not valid >> Passwords do not match");
                GameObject.Instantiate(toolTip);
            }
        else if (firstname.Length <= 2)
        {
            toolTip.GetComponent<ToolTipController>().setMessage("First Name not valid >> First Name to Short");
            GameObject.Instantiate(toolTip);
        }
        else if (lastname.Length <= 2)
        {
            toolTip.GetComponent<ToolTipController>().setMessage("Last Name >> Last Name to Short");
            GameObject.Instantiate(toolTip);
        }
        else
            return true;

        return false;
    }
    public void Auth() {
        Button loginBtn = GameObject.Find("Login").GetComponent<Button>();
        String text = loginBtn.GetComponentInChildren<Text>().text;
        switch (text) {

            case "Login":
                //PlayerPrefs.SetString("Login", text);
                //PlayerPrefs.Save();
                ModalMagaer.instance.OpenAuth();
                break;

            case "Logout":
                Logout();
                break;

            default:
                break;

        }
    }

    private void Logout() {
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

    public void Login() {
        Dictionary<String, String[]> rules = new Dictionary<string, string[]>();
        rules.Add("email", new string[] { "5", "any" });
        rules.Add("password", new string[] { "8", "any" });
        Validation validation = new Validation(rules);

        String email = LogEmail.text;
        String password = LogPassword.text;
        pwd = LogPassword.text;
        Dictionary<String, String> data = new Dictionary<string, string>();
        data.Add("email", email);
        data.Add("password", password);

        Error validResponse = validation.Validate(data);

        if (validResponse.Success) {

            //todo: begin loader

            StartCoroutine(HttpManager.instance.auth.SignIn(email, password,
            (() => {
                StartCoroutine(HttpManager.instance.auth.Me(HttpManager.instance.token, (() => {
                    //Debug.Log(UserManager.instance.user.phone);
                    //todo: end loader
                    updateView(true);
                    Close();
                }), ((String error) => 
                {
                    Debug.Log(error);
                    toolTip.GetComponent<ToolTipController>().setMessage(error);
                    GameObject.Instantiate(toolTip);
                })));
            }),
            ((string error) => {
                Debug.Log(error);
                toolTip.GetComponent<ToolTipController>().setMessage(error);
                GameObject.Instantiate(toolTip);
            })));
        } else {
            Debug.Log(validResponse.Message);
            toolTip.GetComponent<ToolTipController>().setMessage(validResponse.Message);
            GameObject.Instantiate(toolTip);
        }
    }

    private void updateView(bool login = true) {
        if (login)
        {
            Button loginBtn = GameObject.Find("Login").GetComponent<Button>();
            loginBtn.GetComponentInChildren<Text>().text = "Logout";

            Text userName = GameObject.Find("UserName").GetComponent<Text>();
            userName.text = UserManager.instance.user.firstname + " " + UserManager.instance.user.lastname;
        } else {
            Button loginBtn = GameObject.Find("Login").GetComponent<Button>();
            loginBtn.GetComponentInChildren<Text>().text = "Login";

            Text userName = GameObject.Find("UserName").GetComponent<Text>();
            userName.text = String.Empty;
        }

    }

    public void Close() {
        GameObject popup = GameObject.Find("AuthModal(Clone)");
        Destroy(popup);
    }
}
