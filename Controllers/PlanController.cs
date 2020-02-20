using UnityEngine;
using System.Collections;
using Application.Loader;
using System.Collections.Generic;
using Helpers.HTTP;
using UnityEngine.UI;
using System;
using Newtonsoft.Json;
using AOT;
using Application.Models;
using UnityEngine.EventSystems;
using System.Linq;
using Application.Config;
using UnityEngine.Networking;
using Application;
using System.Text.RegularExpressions;

public class PlanController : MonoBehaviour
{
    private List<UnityEngine.Object> texturesList;
    //private Dictionary<string, string> categoryKeys;
    public GameObject toolTip, plansListView;
    public GameObject SavePopUp;

    public void Start()
    {
        plansListView = GameObject.Find("Canvas").transform.Find("PlansListView").gameObject;
        //SavePopUp = GameObject.Find("SavePopup");
        toolTip = Resources.Load<GameObject>("Prefabs/ToolTipPref");
    }

    public void OpenMyPlans()
    {
        if (UserManager.instance.isLoggedIn)
        {
            GetPlansList();
        }
        else
        {
            ModalMagaer.instance.OpenAuth();
        }
    }

    public void GetPlansList()
    {
        StartCoroutine(HttpManager.instance.plans.GetList(HttpManager.instance.token,
            (string json) =>
            {
                Debug.Log("LATEST UPDATE ");
                plansListView.SetActive(true);
                FillPlans(json);
                //GameObject popup = Resources.Load("Prefabs/PlansListView") as GameObject;
                // GameObject popupCloned =
                // Instantiate(popup, new Vector3(100, 100, 2), Quaternion.identity.normalized);
                //popupCloned.transform.SetParent(GameObject.Find("Canvas").transform);
                //popupCloned.transform.localPosition = new Vector3(0, 0, 2);
                //print(json);
                //Debug.Log("JSON LIST =" + json);
            },
            (string error) => 
            {
                toolTip.GetComponent<ToolTipController>().setMessage(error);
                GameObject.Instantiate(toolTip);
            }
        ));
    }

    private void ClearPlans()
    {
        GameObject plansContent = plansListView.transform.Find("Viewport").transform.Find("Content").gameObject;
        foreach (Transform child in plansContent.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void FillPlans(string json)
    {
        try
        {
            PlanResponse[] plans =  JsonConvert.DeserializeObject<PlanResponse[]>(json);
            //Debug.Log("Plans count: " + plans.Length);
            ClearPlans();
            GameObject plansContent = plansListView.transform.Find("Viewport").transform.Find("Content").gameObject;
            GameObject planObject = Resources.Load("Prefabs/Plan") as GameObject;
            
            for (int i = 0; i < plans.Length; i++)
            {
                GameObject plan_clone = Instantiate(planObject, new Vector3(100, 100, 2), Quaternion.identity.normalized);
                plan_clone.transform.SetParent(plansContent.transform);
                plan_clone.transform.Find("PlanNumber").gameObject.GetComponent<Text>().text = (i + 1).ToString();
                plan_clone.transform.Find("PlanTitle").gameObject.GetComponent<Text>().text = plans[i].name;

                EventTrigger planTrigger = plan_clone.AddComponent<EventTrigger>();
                EventTrigger.Entry planEntry = new EventTrigger.Entry();
                planEntry.eventID = EventTriggerType.PointerClick;
                planEntry.callback.AddListener(Plan_onClick);
                planTrigger.triggers.Add(planEntry);

                Plan plan = new Plan();
                PlanComponent planComponent = plan_clone.AddComponent<PlanComponent>() as PlanComponent;
                plan.plan_id = plans[i].id;
                planComponent.plan = plan;
            }
        }
        catch (JsonException ex)
        {
            Debug.Log(ex);
            toolTip.GetComponent<ToolTipController>().setMessage(ex.Message);
            GameObject.Instantiate(toolTip);
        }
        
        if(texturesList == null)
        {
            StartCoroutine(LoadTextures());
        } 
    }

    private IEnumerator LoadTextures()
    {
        List<UnityEngine.Object> downloadedTextures = new List<UnityEngine.Object>();

        Dictionary<string, string> categoryKeys = new Dictionary<string, string>()
        {
            {"Vehicles", "vehicles"},
            {"Shop Equipment", "shop_equipment"},
            {"Equine and livestock Items", "equine_and_livestock_items"},
            {"Farm Equipment", "farm_equipment"},
            {"Windows & Doors", "windows_doors"}
        };

        foreach (var key in categoryKeys)
        {
            string folderName = key.Value;
            string uri = Settings.ASSET_URL + "Assets/AssetsBundles/objects/" + folderName;
            using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(uri))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                    UnityEngine.Object[] assetObjects = bundle.LoadAllAssets(typeof(UnityEngine.Object));
                    foreach (var asset in assetObjects)
                    {
                        downloadedTextures.Add(asset);
                    }
                    texturesList = downloadedTextures;
                    bundle.Unload(false);
                }
            }
        }
        Debug.Log("Textures Loaded");
    }

    public void Plan_onClick(BaseEventData data)
    {
        GameObject planObject = data.selectedObject;

        // TODO
        Plan plan = planObject.GetComponent<PlanComponent>().plan;
        PlanManager.instance.currentPlan = plan;
        // TODO

        plansListView.SetActive(false);
        StartCoroutine(HttpManager.instance.plans.Get(HttpManager.instance.token, plan.plan_id,
            (string json) =>
            {
                DetailsToPlan(json);
            },
            (string error) => 
            {
                Debug.Log(error);
                toolTip.GetComponent<ToolTipController>().setMessage(error);
                GameObject.Instantiate(toolTip);
            }
        ));
    }

    public void DetailsToPlan(string json)
    {
        PlanResponse response =  JsonConvert.DeserializeObject<PlanResponse>(json);
        ItemData[] details = response.plan_details;
        List<ItemData> itemdata = new List<ItemData>();
        foreach(ItemData item in response.plan_details)
        {
            itemdata.Add(item);
        }
        
        InitializeGameObjects(itemdata);
    }

    public void InitializeGameObjects(List<ItemData> itemData)
    {
        GameObject area = GameObject.Find("Area");
        foreach (Transform item in area.transform)
        {
            if (item.gameObject.name.Equals("MainGarage") || item.gameObject.name.Equals("ColliderHelper")){}           
            else Destroy(item.gameObject);
        }

        foreach(ItemData item in itemData)
        {
            if(item.ItemName.Contains("MainGarage"))
            {
                GameObject ObjItem = GameObject.Find("MainGarage");
                RectTransform ObjRect = ObjItem.GetComponent<RectTransform>();

                ObjItem.transform.SetParent(area.transform);
                ObjItem.transform.localScale = new Vector3(1,1,1);
                ObjItem.transform.localPosition = new Vector3(item.position.x, item.position.y,1);
                ObjRect.sizeDelta = new Vector2(item.sDelta.x, item.sDelta.y);
            }
            else if(item.ItemName.Contains("ObjItem"))
            {
                string objTextureString = Regex.Match(item.ItemName, @"\((.*?)\)").Groups[1].Value;
                GameObject ObjItem = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/ObjItem"));
                RectTransform ImageRect = ObjItem.transform.Find("Image").GetComponent<RectTransform>();

                ObjItem.transform.SetParent(area.transform);
                ObjItem.transform.localScale = new Vector3(1,1,1);
                ObjItem.transform.localPosition = new Vector2(item.position.x, item.position.y);

                ImageRect.sizeDelta = new Vector3(item.sDelta.x, item.sDelta.y,1);
                ImageRect.localScale = new Vector3(1,1,1);
                ImageRect.localRotation = Quaternion.Euler(0,0,item.position.z);

                RawImage gObjTexture = ObjItem.transform.Find("Image").GetComponent<RawImage>();
                var item_search = texturesList.Where(im => im.name == objTextureString);
                Texture2D texSearchResult = item_search.First() as Texture2D;
                gObjTexture.texture = texSearchResult;
                ObjItem.name = "ObjItem("+objTextureString+")";
            } 
            else if(item.ItemName.Contains("garageItem"))
            {
                GameObject ObjItem = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/garageItem"));
                RectTransform ObjRect = ObjItem.GetComponent<RectTransform>();

                ObjItem.transform.SetParent(area.transform);
                ObjItem.transform.localScale = new Vector3(1,1,1);
                ObjItem.transform.localPosition = new Vector3(item.position.x, item.position.y,1);
                ObjRect.sizeDelta = new Vector2(item.sDelta.x, item.sDelta.y);
            }
            else if(item.ItemName.Contains("TextElem"))
            {
                GameObject ObjItem = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/TextElem"));
                InputField ObjInputField = ObjItem.transform.Find("InputField/Text/Settings/OriginInputField").GetComponent<InputField>();
                Dropdown ObjDrop = ObjItem.transform.Find("InputField/Text/Settings/Dropdown").GetComponent<Dropdown>();

                ObjItem.transform.SetParent(area.transform);
                ObjItem.transform.localScale = new Vector3(1,1,1);
                ObjItem.transform.localPosition = new Vector3(item.position.x, item.position.y,1);

                ObjInputField.text = item.ItemMisc;
                ObjDrop.value = (int)Math.Floor(item.sDelta.x);

                ObjItem.transform.Find("InputField/Text/Settings").gameObject.SetActive(false);
            }
            else if(item.ItemName.Contains("Line"))
            {
                GameObject ObjItem = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Line"));
                RectTransform ObjRect = ObjItem.GetComponent<RectTransform>();

                ObjItem.transform.SetParent(area.transform);
                ObjItem.transform.localScale = new Vector3(1,1,1);
                ObjItem.transform.localPosition = new Vector3(item.position.x, item.position.y,1);

                ObjRect.sizeDelta = new Vector3(item.sDelta.x,item.sDelta.y,1);
                ObjRect.localRotation = Quaternion.Euler(0,0,item.position.z);

                ObjItem.transform.Find("Width/Settings").gameObject.SetActive(false);
            }
            else if(item.ItemName.Contains("Photo"))
            {
                GameObject ObjItem = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Photo"));
                PhotoController photoController = ObjItem.GetComponent<PhotoController>();

                ObjItem.transform.SetParent(area.transform);
                ObjItem.transform.localScale = new Vector3(1,1,1);
                ObjItem.transform.localPosition = new Vector3(item.position.x, item.position.y,1);

                photoController.urlPhoto.text = item.ItemMisc;
                photoController.GetImage();
                ObjItem.transform.Find("Settings").gameObject.SetActive(false);
            }
        }
    }

    public void Save(bool exitModal=false)
    {
        if (UserManager.instance.isLoggedIn)
        {
            if (PlanManager.instance.currentPlan != null)
            {
                if(exitModal)
                {
                    SavePopUp.transform.Find("Save").GetComponent<SaveController>().exitModal = true;
                    Debug.Log("SetEvrythinToExit");
                } else {
                    SavePopUp.transform.Find("Save").GetComponent<SaveController>().exitModal = false;
                    Debug.Log("NotExit");
                }
                //ModalMagaer.instance.OpenSave();
                SavePopUp.SetActive(true);
            }
            else
            {
                Debug.Log(PlanManager.instance.currentPlan);
            }
        }
        else
        {
            ModalMagaer.instance.OpenAuth();
        }
    }

    public int GetPlanID()
    {
        PlanComponent plan = gameObject.GetComponent<PlanComponent>();
        if(plan != null)
        {
            return plan.plan.plan_id;
        } else {
            return 0;
        }
    }

    public void DeletePlan()
    {
        StartCoroutine(HttpManager.instance.plans.Delete(HttpManager.instance.token, GetPlanID(),
                (string good) =>
                {
                    Debug.Log("Succes");
                    //StartCoroutine(DeletePlanUpdate());
                    //FillPlans(json);
                    //GameObject popup = Resources.Load("Prefabs/PlansListView") as GameObject;
                    // GameObject popupCloned =
                    // Instantiate(popup, new Vector3(100, 100, 2), Quaternion.identity.normalized);
                    //popupCloned.transform.SetParent(GameObject.Find("Canvas").transform);
                    //popupCloned.transform.localPosition = new Vector3(0, 0, 2);
                    //print(json);
                    //Debug.Log("JSON LIST =" + json);
                },
                (string error) => 
                {
                    toolTip.GetComponent<ToolTipController>().setMessage(error);
                    GameObject.Instantiate(toolTip);
                }
            ));
    }
}

public class PlanComponent : MonoBehaviour
{
    public Plan plan;
}