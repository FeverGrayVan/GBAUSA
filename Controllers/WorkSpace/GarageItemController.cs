using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Application.Models;
using UnityEditor;
using System;

public class GarageItemController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Text width, height, innerWidth, innerHeight;
    public bool resizeCol, resizeNew, isActive, xDrag;
    public GameObject[] allObjects;
    public Vector2 distance;
    public ComandManager cmd;
    public InputField inputWidth, inputHeight;


    public float curSizeX, curSizeY;
    public RectTransform area, garageRect;
    public GameObject settings, garage, target;
    public WorkSpaceController workSpace;
    
    void Start()
    {   
        workSpace = GameObject.Find("Area").GetComponent<WorkSpaceController>();
        garageRect = GetComponent<RectTransform>();
        area = GameObject.Find("Area").GetComponent<RectTransform>();
        cmd = GameObject.Find("Main Camera").GetComponent<ComandManager>();
        target = GameObject.Find("ColliderHelper");
        ShowBorderSizes();
        isActive = false;
    }

    void Update()
    {
        if (settings != null)
        {
            settings.transform.localScale = new Vector3(1.4f/area.localScale.x,1.4f/area.localScale.y,1.4f);
        }
        ShowBorderSizes();
        DragResize();        
    }

    public void MaxSizes()
    {
        if (curSizeX < 144){ curSizeX = 144;};
        if (curSizeY < 240){ curSizeY = 240;}; 
        if (curSizeX > 1200 - (Mathf.Abs(transform.localPosition.x) * 2)) {  curSizeX =  1200 - (Mathf.Abs(transform.localPosition.x) * 2); };
        if (curSizeY > 1200 - (Mathf.Abs(transform.localPosition.y) * 2)) {  curSizeY =  1200 - (Mathf.Abs(transform.localPosition.y) * 2); };
    }

    public void DragResize()
    {
        distance = transform.InverseTransformPoint(target.transform.position);

        curSizeX = Mathf.Abs(distance.x * 2);
        curSizeY = Mathf.Abs(distance.y * 2);
        MaxSizes();

        if (resizeNew == true)
        {
            Vector2 prevSize = garageRect.sizeDelta;
            
            if (xDrag)
            {
               garageRect.sizeDelta = new Vector2(curSizeX,prevSize.y);
               WinDoorResizeHorizontal();
            } 
            else if (!xDrag)
            {
                garageRect.sizeDelta = new Vector2(prevSize.x,curSizeY);
                WinDoorResizeVertical();
            }

            if (settings != null)
            {
                inputHeight.text = garageRect.sizeDelta.y.ToString("F" + 1);
                inputWidth.text = garageRect.sizeDelta.x.ToString("F" + 1);
            }
            ShowBorderSizes();           
        }
    }

    public void ResizeToolTip()
    {
        if (inputWidth.text != "" && inputHeight.text != "")
        {
            garageRect.sizeDelta = new Vector2(Convert.ToSingle(inputWidth.text),Convert.ToSingle(inputHeight.text));
        }
    }

    public void WinDoorResizeVertical()
    {
        foreach (Transform child in this.gameObject.transform) if (child.CompareTag("Object")) 
        {
            if (Mathf.Abs(child.transform.localPosition.x) < (garageRect.sizeDelta.x/2) - 5)
            {
                if (child.transform.localPosition.y > 0)
                {
                    child.transform.localPosition = new Vector2(child.transform.localPosition.x, garageRect.sizeDelta.y/2);
                } else if (child.transform.localPosition.y < 0)
                {
                    child.transform.localPosition = new Vector2(child.transform.localPosition.x, -(garageRect.sizeDelta.y/2));
                }       
            } else {
                float k2 = child.GetComponent<ObjItemController>().k2;
                child.transform.localPosition = new Vector2(child.transform.localPosition.x, garageRect.sizeDelta.y/k2);
            }      
        } 
    }

    public void WinDoorResizeHorizontal()
    {
        foreach (Transform child in this.gameObject.transform) if (child.CompareTag("Object")) 
        {
            if(child.GetComponent<ObjItemController>().k3 == true)
            {
                if (Mathf.Abs(child.transform.localPosition.y) < (garageRect.sizeDelta.y/2) - 40)
                {
                    if (child.transform.localPosition.x > 0)
                    {
                        child.transform.localPosition = new Vector2(garageRect.sizeDelta.x/2 + 15, child.transform.localPosition.y);
                    } else if (child.transform.localPosition.x < 0)
                    {
                        child.transform.localPosition = new Vector2(-(garageRect.sizeDelta.x/2 - 15), child.transform.localPosition.y);
                    }             
                } else {
                    float k1 = child.GetComponent<ObjItemController>().k1;
                    child.transform.localPosition = new Vector2(garageRect.sizeDelta.x/k1, child.transform.localPosition.y);
                }
            } else {
                if (Mathf.Abs(child.transform.localPosition.y) < (garageRect.sizeDelta.y/2) - 5)
                {
                    if (child.transform.localPosition.x > 0)
                    {
                        child.transform.localPosition = new Vector2(garageRect.sizeDelta.x/2, child.transform.localPosition.y);
                    } else if (child.transform.localPosition.x < 0)
                    {
                        child.transform.localPosition = new Vector2(-(garageRect.sizeDelta.x/2), child.transform.localPosition.y);
                    }             
                } else {
                    float k1 = child.GetComponent<ObjItemController>().k1;
                    child.transform.localPosition = new Vector2(garageRect.sizeDelta.x/k1, child.transform.localPosition.y);
                }   
            }
        } 
    }
    
    public void ShowBorderSizes()
    {
        var metric = GameObject.Find("TopMenu").GetComponent<TopMenuController>().metricUnits;

        if (metric)
        {
            width.text = garageRect.sizeDelta.y.ToString("F" + 1);
            height.text = garageRect.sizeDelta.x.ToString("F" + 1);
            innerWidth.text = ((garageRect.sizeDelta.y)-5f).ToString("F" + 1); 
            innerHeight.text = ((garageRect.sizeDelta.x)-5f).ToString("F" + 1); 
        } else {
            width.text = (garageRect.sizeDelta.y / 12).ToString("F" + 1);
            height.text = (garageRect.sizeDelta.x / 12).ToString("F" + 1);
            innerWidth.text = ((garageRect.sizeDelta.y / 12)-0.5f).ToString("F" + 1); 
            innerHeight.text = ((garageRect.sizeDelta.x / 12)-0.5f).ToString("F" + 1);  
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //garage.transform.SetAsLastSibling();
        if (resizeCol == true)
        {
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
            {
                xDrag = true;
            } 
            else 
            {
                xDrag = false;
            }
            resizeNew = true;
            cmd.AddCmd(garage, Operation.Resize, garage.transform.position, new Vector2(garageRect.sizeDelta.x, garageRect.sizeDelta.y));
        } 

        if (settings != null)
        {
            if (isActive)
            {
                settings.SetActive(false);
                isActive = false;
            }
            else
            {
                isActive = true;
                settings.SetActive(true);
            }  
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        resizeNew = false;
        if (garageRect.localPosition.x > 600 || garageRect.localPosition.x < -600 || garageRect.localPosition.y > 600 || garageRect.localPosition.y < -600)
        {
            DeleteTheGarage();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //garage.transform.SetAsLastSibling();
        if (resizeCol == false && this.gameObject.name != "MainGarage" && workSpace.activeTool == "MouseTool" && resizeNew == false)
        {
            gameObject.transform.position = eventData.position;
        }
    }

    public void DeleteTheGarage()
    {
        Destroy(gameObject);
    }

    public PlanDetails ToPlanDetails()
    {
        Vector2 localPos = area.InverseTransformPoint(transform.position);
        return new PlanDetails(
            localPos.x,
            localPos.y,
            0,
            garageRect.sizeDelta.x,
            garageRect.sizeDelta.y,
            gameObject.name,
            "GarageItemToPlan");
    }
}
