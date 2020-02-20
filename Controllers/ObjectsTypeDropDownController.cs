using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using System.Collections;
using Application.Config;

public class ObjectsTypeDropDownController : MonoBehaviour
{
    //public ScrollView scrollView;
    private static List<GameObject> scrollContent;
    private Dictionary<string, string> categoryKeys;
    public GameObject scrollObjects, leanButton;
  

    // Use this for initialization
    void Start()
    {
        //scrollObjects = GameObject.Find("ContentScrollObjects");
        scrollContent = new List<GameObject>();
        SetCategoryKeys();
        LoadCategory("Vehicles");
        //clearCategory();
    }

    private void SetCategoryKeys()
    {
        categoryKeys = new Dictionary<string, string>()
        {
            {"Vehicles", "vehicles"},
            {"Shop Equipment", "shop_equipment"},
            {"Equine and livestock Items", "equine_and_livestock_items"},
            {"Farm Equipment", "farm_equipment"},
            {"Windows & Doors", "windows_doors"}
        };
    }

    public void DropDownSelected(Dropdown change)
    {
        String valueText = change.options[change.value].text;
        LoadCategory(valueText);
    }

    private void LoadCategory(String categoryName)
    {
        if (categoryName != "Lean-tos")
        {
        String rb_name = String.Empty;
        rb_name = categoryKeys?[categoryName];
        leanButton.SetActive(false);
        clearCategory();
        StartCoroutine(GetFilesInCategory(rb_name));
        
        } else 
        {
            clearCategory();
            leanButton.SetActive(true);
        }
    }

    private IEnumerator GetFilesInCategory(string folderName)
    {
        List<string> files = new List<string>();
        string uri = Settings.ASSET_URL + "Assets/AssetsBundles/objects/" + folderName;
        Debug.Log(uri);
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
                Debug.Log(bundle.ToString());
                UnityEngine.Object[] assetObjects = bundle.LoadAllAssets(typeof(UnityEngine.Object));
                FillList(assetObjects);
                bundle.Unload(false);
            }
        }
    }

    private void FillList(UnityEngine.Object[] data)
    {

        for (int i = 0; i < data.Length; i++)
        {
            Texture2D item = data[i] as Texture2D;
            if (item != null)
            {
                
                GameObject obj = Instantiate(Resources.Load("Prefabs/ObjectItemMenu") as GameObject);
                obj.name = item.name;
                Vector2 pivot = new Vector2(0.5f, 0.5f);
                Sprite sp = Sprite.Create(item, new Rect(0, 0, item.width, item.height), pivot);
                obj.GetComponentInChildren<Image>().sprite = sp;
                var objTransform = obj.GetComponentInChildren<Image>().gameObject.GetComponent<RectTransform>();
                float width = item.width, height = item.height, target = 0;
                if (width > height)
                {
                    target = (100 / width);
                }
                else
                {
                    target = (100 / height);
                }
                width = Mathf.Round(width * target);
                height = Mathf.Round(height * target);

                objTransform.sizeDelta = new Vector2(width, height);

                obj.gameObject.transform.SetParent(scrollObjects.gameObject.transform);

                //GameObject go_item = new GameObject();
                //Image go_image = go_item.AddComponent<Image>();
                //BoxCollider2D go_collider = go_item.AddComponent<BoxCollider2D>();
                //RectTransform go_transform = go_item.GetComponent<RectTransform>();
                //CanvasGroup go_canvasGroup = go_item.AddComponent<CanvasGroup>();
                //ObjectModel go_objectModel = go_item.AddComponent<ObjectModel>();

                //Vector2 pivot = new Vector2(0.5f, 0.5f);
                //Rect rect = new Rect(0, 0, item.width, item.height);
                //Sprite sprite = Sprite.Create(item, rect, pivot, 100);
                //go_image.sprite = sprite;


                //Vector2 newSize = new Vector2();

                //float originalWidth = go_image.sprite.rect.width;
                //float originalHeight = go_image.sprite.rect.height;

                //if (originalWidth > maxWidth)
                //{
                //    float ratio = originalWidth / originalHeight;
                //    float newHeight = maxWidth / ratio;
                //    newSize.x = maxWidth;
                //    newSize.y = newHeight;
                //    go_item.GetComponent<ObjectModel>().size = newSize;
                //}

                //if (originalHeight > maxHeight)
                //{
                //    float ratio = originalHeight / originalWidth;
                //    float newWidth = maxHeight / ratio;
                //    newSize.x = newWidth;
                //    newSize.y = maxHeight;
                //    go_item.GetComponent<ObjectModel>().size = newSize;
                //}


                //go_transform.sizeDelta = newSize; //newSize;
                //go_collider.size = newSize;//newSize;

                //go_objectModel.texture = item;
                //go_transform.SetParent(scrollObjects.transform);
                //go_collider.isTrigger = true;
                //go_item.name = item.name + "_dragg";
                //go_canvasGroup.alpha = 1;
                //go_canvasGroup.interactable = true;
                //go_canvasGroup.blocksRaycasts = true;
                //go_item.AddComponent<ObjectController>();
            }
        }
    }

    private void clearCategory()
    {
        GameObject[] allObjects = new GameObject[scrollObjects.transform.childCount];
        int i = 0;

        foreach (Transform sc_object in scrollObjects.transform)
        {
            allObjects[i] = sc_object.gameObject;
            i++;
        }

        foreach (GameObject child in allObjects)
        {
            DestroyImmediate(child);
        }
    }
}