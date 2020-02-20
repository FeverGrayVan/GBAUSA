using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
 
public class TabInput : MonoBehaviour
{
    public EventSystem system;
    public InputField next;
    public InputField WidthText;
 
    void Start()
    {
        system = EventSystem.current;
    }
    
    void Update()
    {
        if(this.gameObject.name == "SharePopup")
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {   
                switch (system.currentSelectedGameObject.name)
                {
                    case "emailShare:InputField":
                    next = GameObject.Find("textMsg:InputField").GetComponent<InputField>();
                    break;
                    case "textMsg:InputField":
                    next = GameObject.Find("emailShare:InputField").GetComponent<InputField>();
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
        else if(this.gameObject.name == "ToolsWrapper")
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {   
                switch (system.currentSelectedGameObject.name)
                {
                    case "NameInputField":
                    Debug.Log("Detected NameInputField");
                    next = WidthText;
                    Debug.Log("current obj is NameInputField" + next.gameObject.name);
                    break;
                    case "WidthText":
                    next = GameObject.Find("HeightText").GetComponent<InputField>();
                    break;
                    case "HeightText":
                    next = GameObject.Find("RotateText").GetComponent<InputField>();
                    break;
                    case "RotateText":
                    next = GameObject.Find("NameInputField").GetComponent<InputField>();
                    break;
                    default :
                    Debug.Log("NotFound the next");
                    break;
                }

                InputField inputfield = next;
                if (inputfield != null)
                {
                    inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret
                    system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
                }   
            }
        }
    }
}