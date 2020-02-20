using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Application.Models;
using Application.Loader;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

public class PhotoController : MonoBehaviour, IPointerDownHandler, IDragHandler,IBeginDragHandler
{
    public Image photo, icon;
    public Image image;
    public InputField width, height,namePhoto;
    public Toggle placeOnPlane, trace, inWindow;
    public GameObject gObj, settings;
    public RectTransform area;
    private Sprite areaSp;
    private Color clr;
    public Button viewIm;
    public ComandManager cmd;
    public InputField urlPhoto;
    TextEditor mTe = null;
    public static string copyPasteInfo = "";
    private WorkSpaceController workSpace;

    




    void Awake()
    {
        mTe = new TextEditor();
    }

    public void Start()
    {
        area = GameObject.Find("Area").GetComponent<RectTransform>();
        workSpace = GameObject.Find("Area").GetComponent<WorkSpaceController>();
        areaSp = GameObject.Find("Area").GetComponent<Image>().sprite;
        cmd = GameObject.Find("Main Camera").GetComponent<ComandManager>();
    }

    public void Update()
    {
        OnInputClick();

        if(trace.isOn)
        {
            TraceBtn();
        }
        if(placeOnPlane.isOn)
        {
            PlaceOnPlaneBtn();
        }

        if(inWindow.isOn)
        {
            InWindowBtn();
        }

        settings.transform.localScale = new Vector3(1f/area.localScale.x,1f/area.localScale.y,1f);
        //Debug.Log("plane = " + placeOnPlane.isOn + " | widnow = "+inWindow.isOn+ " | trace = " + trace.isOn);
    
    }

    public void OnValueChange()
    {
        //print(gameObject.name+" ");
    }

    public void GetImage()
    {
        //Debug.Log(urlPhoto.text);
        
        if (urlPhoto.text != "")
        {
            Debug.Log(urlPhoto.text);
            StartCoroutine(DownloadImage(urlPhoto.text));
        }
    }
    public IEnumerator DownloadImage(string url)
    {
        Debug.Log("Go2");
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);    
        www.SetRequestHeader("Access-Control-Allow-Origin", "*");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            if (myTexture != null)
            {
                Rect rec = new Rect(0, 0, myTexture.width, myTexture.height);
                Sprite hello = Sprite.Create(myTexture, rec, new Vector2(0, 0), 1);
                image.sprite = hello;
                image.rectTransform.sizeDelta = new Vector2(myTexture.width, myTexture.height);
                image.gameObject.SetActive(true);
                width.text = myTexture.width.ToString();
                height.text = myTexture.height.ToString();
            }
        }
        
    }
    
    public void Resize()
    {
        if (width.text != "" && height.text != "")
        {
            image.rectTransform.sizeDelta = new Vector2(Convert.ToInt32(width.text),Convert.ToInt32(height.text));
        }
    }
    public void CloneBtn()
    {
        var tmp = GameObject.Instantiate(gObj);
        tmp.transform.position = gObj.transform.position;
        tmp.transform.SetParent(GameObject.Find("Area").transform);
        tmp.transform.SetAsLastSibling();
    }
    public void CloseBtn()
    {
        settings.SetActive(false);
        Debug.Log("Closed");
    }
    public void DeleteBtn()
    {
        Destroy(gObj);
    }

    public void PlaceOnPlaneBtn()
    {
        viewIm.gameObject.SetActive(false);
        if (!image.gameObject.activeSelf)
            image.gameObject.SetActive(true);
        //gObj.transform.SetAsLastSibling();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        //Debug.Log(image.color);
    }
    public void TraceBtn()
    {
        viewIm.gameObject.SetActive(false);
        if (!image.gameObject.activeSelf)
            image.gameObject.SetActive(true);
        image.transform.SetAsFirstSibling();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.7f);
        //Debug.Log(image.color);
    }
    public void InWindowBtn()
    {
        image.gameObject.SetActive(false);
        viewIm.gameObject.SetActive(true);
        gObj.transform.SetAsLastSibling();
    }
    public void ViewInWindow()
    {
        var tmp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PhotoView"));
        tmp.GetComponent<Image>().sprite = image.sprite;
        tmp.GetComponent<RectTransform>().sizeDelta = image.rectTransform.sizeDelta;
        tmp.transform.position = GameObject.Find("Area").transform.position;
        tmp.transform.SetParent(GameObject.Find("Area").transform);
        tmp.transform.SetAsLastSibling();
    }
    public void CloseView()
    {
        Destroy(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        GameObject[] allSettings = GameObject.FindGameObjectsWithTag("Settings");
            foreach (GameObject item in allSettings)
            {
                item.SetActive(false);
            }
        if (Input.GetMouseButton(0))
        {
            //settings.transform.position = eventData.position;
            settings.SetActive(true);
            gObj.transform.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (workSpace.activeTool == "MouseTool")
        {
            if (settings.activeSelf)
                settings.SetActive(false);
            gObj.transform.position = eventData.position;
            gObj.transform.SetAsLastSibling();
        }
    }
    public void OnBeginDrag(PointerEventData eventData )
    {
        if (workSpace.activeTool == "MouseTool")
        {
            cmd.AddCmd(this.gameObject, Operation.Drag, this.gameObject.transform.position);
        }
    }

    public void OnInputClick()
    {
        //Debug.Log("Got input Click");
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.V))
        {
            Debug.Log("Got Ctrl+v");
            JavaScriptManager.CopyPastePlugin(this.gameObject.name, "PasteInto");
            //urlPhoto.text = GUIUtility.systemCopyBuffer; 
        }
    }

    public void PasteInto(string pasteValue)
    {
        Debug.Log("Trying to PasteInto");
        urlPhoto.text = pasteValue; 
    }

    public PlanDetails ToPlanDetails()
    {
        Vector2 localPos = GameObject.Find("Area").transform.InverseTransformPoint(gameObject.transform.position);
        return new PlanDetails(
            localPos.x,
            localPos.y,
            0,
            0,
            0,
            gameObject.name,
            urlPhoto.text);
    }
}
