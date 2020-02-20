using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Application.Loader;

public class TopMenuController : MonoBehaviour
{
    public bool metricUnits = false;
    public int rotation;
    private float rotateSpeed = 100.0f;
    private bool Rotate = false;
    public Transform area;
    public GameObject target;
    public Button rotateBtn;
    public InputField numG;

    public Vector3 startPos;
    public RectTransform startRect;
    public Image imgTest;
    public Text snapTxt;
    public float areaRot;

    public Texture2D mainTex;
    
    public string url_help;

    public void OnPointerUpRotate(BaseEventData arg0) => this.Rotate = false;
    public void OnPointerDownRotate(BaseEventData arg0) => this.Rotate = true;

    public InputField url_text;

    public Rect zoneRect;

    public RectTransform[] allObjects;
    public static Vector2[] allObjectsPos;
    public static float[] allObjectsRot;

    public void Start()
    {
        area = GameObject.Find("Area").GetComponent<Transform>();
        startPos = area.transform.localPosition;
        //url_text = GameObject.Find("TextUrlField").GetComponent<InputField>();
        target = GameObject.Find("ColliderHelper");
    }

    public void Update()
    {
        if (Rotate == true)
        {
            float zRotate = Input.GetAxis("Mouse Y") + Input.GetAxis("Mouse X");
            area.Rotate(new Vector3(0, 0, zRotate) * Time.deltaTime * rotateSpeed);
            rotateBtn.transform.Rotate(new Vector3(0, 0, zRotate) * Time.deltaTime * rotateSpeed);
            numG.text = area.rotation.eulerAngles.z.ToString();
        }
    }

    public void RotateTextEnter()
    {
        area.localRotation = Quaternion.Euler(0,0,Convert.ToInt32(numG.text));
    }

    public void MirrorHorizontal()
    {
        SaveTransform();

        foreach (RectTransform item in allObjects) 
        {
            item.localPosition = new Vector2(item.localPosition.x,item.localPosition.y * -1);
            float rot = item.eulerAngles.z;
            if(item.gameObject.name.Contains("Line"))
            {
                item.localRotation = Quaternion.Euler(0,0,rot * -1);
            }
        }
    }

    public void MirrorVertical()
    {
        SaveTransform();

        foreach (RectTransform item in allObjects) 
        {
            item.localPosition = new Vector2(item.localPosition.x * -1,item.localPosition.y);
            
            float rot = item.eulerAngles.z;
            if(item.gameObject.name.Contains("Line"))
            {
                item.localRotation = Quaternion.Euler(0,0,(rot * -1)+180);
            }
        }
    }

    public void SetNormal()
    {
        SaveTransform();    

        for (int i = 0; i < allObjectsPos.Length; i++) 
        {
            allObjects[i].rotation = Quaternion.Euler(0,0,0);
            allObjects[i].localPosition = allObjectsPos[i];
        }
        area.rotation = Quaternion.Euler(0, 0, areaRot);
    }

    public void SafeStorage()
    {
        SaveTransform();
        areaRot = area.eulerAngles.z;
        allObjectsPos = new Vector2[allObjects.Length];
        allObjectsRot = new float[allObjects.Length];
        Debug.Log(allObjectsPos.Length);
        for (int i = 0; i < allObjects.Length; i++) 
        {
            allObjectsPos[i] = allObjects[i].localPosition;
            allObjectsRot[i] = allObjects[i].eulerAngles.z;
            Debug.Log("Position #"+i+" = " + allObjectsPos[i]);
        }
    }

    public void SaveTransform()
    {
        allObjects = new RectTransform[GameObject.Find("Area").transform.childCount];
        int i = 0;

        foreach (Transform sc_object in GameObject.Find("Area").transform)
        {
            allObjects[i] = sc_object.GetComponent<RectTransform>();
            i++;
        }
    }

    public void ToMetric()
    {
        metricUnits = true;
        GameObject.Find("Feet").GetComponent<Toggle>().isOn = false;
    }

    public void ToFeet()
    {
        metricUnits = false;
        GameObject.Find("Metric").GetComponent<Toggle>().isOn = false;
    }

    public void ClearArea()
    {
        GameObject area = GameObject.Find("Area");
        foreach (Transform item in area.transform)
        {
            if (item.gameObject.name.Equals("MainGarage") || item.gameObject.name.Equals("ColliderHelper")){}           
            else Destroy(item.gameObject);
        }
    }

    public void RotateMiror()
    {
        GameObject tmp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Rotate_MirorPlan"));
        tmp.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    public void HelpClick()
    {        
    #if !UNITY_EDITOR
        JavaScriptManager.openWindowURL("https://gba-usa.project-release.info/contact-us/");
    #endif
    }

    public void PlanRotate()
    {
        Vector3 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        area.rotation = Quaternion.AngleAxis(angle , Vector3.forward);
    }

    public void PlanSetNormal()
    {
        area.rotation = Quaternion.Euler(0,0,0);
    }

    //Screenshot maintenance

    public void PrintView()
    {
        JavaScriptManager.printView();
    }

    public void OpenView()
    {
		JavaScriptManager.openView();
    }

    public void LoadToCookies()
    {
        StartCoroutine("TakeScreenAndLoad");
    }

    public IEnumerator TakeScreenAndLoad()
    {
        yield return new WaitForEndOfFrame();
        mainTex = ScreenCapture.CaptureScreenshotAsTexture();
        byte[] textureBytes = mainTex.EncodeToJPG();
        string encodedText = System.Convert.ToBase64String(textureBytes);
        JavaScriptManager.cookieSet(encodedText);
    }

    
}