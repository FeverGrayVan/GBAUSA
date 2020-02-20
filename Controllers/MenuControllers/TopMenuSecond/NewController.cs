using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Application.Loader;

public class NewController : MonoBehaviour
{

    public void OpenNew()
    {
        SceneManager.LoadScene(0);
        //var tmp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/NewPlan"));
        //GameObject dropDown = GameObject.Find("OpenPlanDropDown");
        //int indexValue = dropDown.GetComponent<Dropdown>().value;
        //string valueDD = dropDown.GetComponent<Dropdown>().options[indexValue].text;
        //PlanManager.instance.SetCurrentGarage(valueDD);
        //SceneManager.LoadScene(1);
    }
}
