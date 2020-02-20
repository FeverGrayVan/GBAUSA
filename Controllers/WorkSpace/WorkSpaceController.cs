using UnityEngine;
using System.Collections;
using Application.Loader;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Application.Models;
using UnityEditor;
using System;
using System.Linq;

public class WorkSpaceController : MonoBehaviour , IPointerDownHandler
{
    public GameObject garageobj;
    public Text infoText;
    public ToggleGroup tglGrp;
    public string activeTool = "MouseTool";

    public void Start() 
    {
        LegacyStart();
        ToolSwitch("MouseTool");
        activeTool = "MouseTool";
    }

    public void LegacyStart()
    {
        if (UserManager.instance.isLoggedIn)
        {
            Button loginBtn = GameObject.Find("Login").GetComponent<Button>();
            loginBtn.GetComponentInChildren<Text>().text = "Logout";

            Text userName = GameObject.Find("UserName").GetComponent<Text>();
            userName.text = UserManager.instance.user.firstname + " " + UserManager.instance.user.lastname;
        }

        if (PlanManager.instance.currentPlan == null)
        {
            if (UserManager.instance.isLoggedIn)
            {
                PlanManager.instance.currentPlan = new Plan("tmp_name", 0, UserManager.instance.user.id);
            } else
            {
                PlanManager.instance.currentPlan = new Plan("tmp_name", 0, 0);
            }
        }
       
        Debug.Log(PlanManager.instance.currentGarage.height);
        garageobj.AddComponent<BoxCollider2D>();
        garageobj.GetComponent<BoxCollider2D>().size = new Vector2(PlanManager.instance.currentGarage.height * 10, PlanManager.instance.currentGarage.width * 10);
        garageobj.GetComponent<GarageItemController>().width.text = PlanManager.instance.currentGarage.width.ToString();
        garageobj.GetComponent<GarageItemController>().height.text = PlanManager.instance.currentGarage.height.ToString();

        infoText.text = (PlanManager.instance.currentGarage.key + " " + (PlanManager.instance.currentGarage.height/1.2f).ToString() + "x" + (PlanManager.instance.currentGarage.width/1.2f).ToString());

        garageobj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(PlanManager.instance.currentGarage.height * 10, PlanManager.instance.currentGarage.width * 10);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        HideSettings();
    }

    public void HideSettings()
    {
        GameObject[] allSettings = GameObject.FindGameObjectsWithTag("Settings");
        foreach (GameObject item in allSettings)
        {
            item.SetActive(false);
        }
    }

    public void Update()
    {
        //ToolRaycastSwitch();
        //Debug.Log("Current active toggle is " + activeTool);
    }

    public void AreaItemsRaycast(bool itemState, bool areaState)
    {
        Image[] objects = GameObject.Find("Area").GetComponentsInChildren<Image>();
        RawImage[] objects2 = GameObject.Find("Area").GetComponentsInChildren<RawImage>();

        foreach (Image item in objects)
        {
            item.raycastTarget = itemState;
        }

        foreach (RawImage item in objects2)
        {  
            item.raycastTarget = itemState;
        }

        GameObject.Find("Area").GetComponent<Image>().raycastTarget = areaState;
    }

    public void ToolSwitch(string curTool)
    {
        switch(curTool)
        {
            case "MouseTool":
                AreaItemsRaycast(true, false); 
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;
            case "HandTool":
                AreaItemsRaycast(false, false);
                var cur = Resources.Load("Cur/hand_cur");
                Cursor.SetCursor(cur as Texture2D, Vector2.zero, CursorMode.Auto);
                break;
            case "LineralTool":
                AreaItemsRaycast(false, true);
                var _cur = Resources.Load("Cur/liner_cur");
                Cursor.SetCursor(_cur as Texture2D, Vector2.zero, CursorMode.Auto);
                break;
            case "WallTool":
            case "TextTool":
            case "CameraTool":
                AreaItemsRaycast(false, true);
                break;
            default:
                AreaItemsRaycast(true, true);
                break;
        }
        activeTool = curTool;
        Debug.Log("Current tool is " + curTool);
    }
}
