using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Application.Loader;
using Application.Models;
using UnityEngine.SceneManagement;
using Helpers.HTTP;
using System.Linq;
using Application;

public class SaveController : MonoBehaviour 
{
    public bool exitModal;
    private NewController newController;

    public void Start()
    {
        //exitModal=false;
    }
    
    public void GetAllObjectOnArea()
    {
        var childs = GameObject.Find("Area").GetComponentsInChildren<Transform>();
        Debug.Log("AREA LOCALSCALE = " + GameObject.Find("Area").GetComponent<RectTransform>().localScale);
       PlanManager.instance.currentPlan.SetScale(GameObject.Find("Area").GetComponent<RectTransform>().localScale);

        foreach (Transform item in childs)
        {
            if (item.gameObject.name.Contains("garageItem") || item.gameObject.name.Contains("MainGarage"))
            {
                PlanManager.instance.currentPlan.AppendDetails(item.gameObject.GetComponent<GarageItemController>().ToPlanDetails());
            }
            if (item.gameObject.name.Contains("ObjItem"))
            {
                PlanManager.instance.currentPlan.AppendDetails(item.gameObject.GetComponent<ObjItemController>().ToPlanDetails());
            }
            if (item.gameObject.name.Contains("Photo"))
            {
                PlanManager.instance.currentPlan.AppendDetails(item.gameObject.GetComponent<PhotoController>().ToPlanDetails());

            }
            if (item.gameObject.name.Contains("Wall"))
            {
                PlanManager.instance.currentPlan.AppendDetails(item.gameObject.GetComponent<WallObjectController>().ToPlanDetails());
            }
            if (item.gameObject.name.Contains("TextElem"))
            {
                PlanManager.instance.currentPlan.AppendDetails(item.gameObject.GetComponent<TextObjectController>().ToPlanDetails());

            }
            if (item.gameObject.name.Contains("Line"))
            {
                PlanManager.instance.currentPlan.AppendDetails(item.gameObject.GetComponent<LineralObjectController>().ToPlanDetails());

            }
        }
    }

    public void EnableExit()
    {
        exitModal = true;
    }

    public void DisableExit()
    {
        exitModal = false;
    }

    public void SaveOnClick() {
        Debug.Log(gameObject.name);
        InputField plan_name_go = GameObject.Find("NameField").GetComponent<InputField>();
        String plan_name = plan_name_go.text;
        SaveModal(plan_name);
    }

    private void SaveModal(string plan_name)
    {
        GetAllObjectOnArea();
        Plan planToSave = PlanManager.instance.currentPlan;
        planToSave.user_id = UserManager.instance.user.id;
        planToSave.plan_name = plan_name;

        //GameObject[] tmp = GameObject.Find("Area").GetComponentsInParent<GameObject>();
        

        PlanRequest plan = new PlanRequest
        {
            plan_details = new ItemData[planToSave.details.Count],
            name = plan_name
            //testing = ("yes im saving") 
        };
        Debug.Log("Plan name? = " + plan.name + " plan_details.count = " + plan.plan_details.Count());
        int i = 0;
        foreach (Application.Models.PlanDetails det in planToSave.details) 
        {
            plan.plan_details[i] = det.ToItemData();
            Debug.Log("Processing the " +plan.plan_details[i].ItemMisc);
            i++;
        }
        //planToSave.testing = Application.Models.PlanDetails.ToItemData();

        StartCoroutine(HttpManager.instance.plans.Save(HttpManager.instance.token,
                (string json) => {
                    UnityEngine.Debug.Log(json);
                    CancelOnClick();
                },
                (string error) => {
                    Debug.Log("Save Error = " + error);
                    CancelOnClick();
                }, plan));
    }

    public void CancelOnClick() {
        Debug.Log("Cancel!");
        GameObject.Find("SavePopup").SetActive(false);
        if(exitModal)
        {
            SceneManager.LoadScene(0);
        }
    }

}
