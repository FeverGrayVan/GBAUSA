using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using Application.Models;
using System;
using System.Text.RegularExpressions;

public class ObjItemController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RawImage image;
    public Text width, height;

    public Text widthToolTip, heightToolTip;
    public InputField widthTool, heightTool, RotateTool;
    public Button resize, rotate;
    public GameObject gObj, settings, img, target, rotateBtn;
    private float rotateSpeed = 100.0f;
    private float resizeSpeed = 10f;
    public bool active = false, rotateHover;
    public ComandManager cmd;
    public RectTransform rectObj,area, imageRect;
    public RaycastHit[] hit;
    public Vector2[] directions;
    public bool snapping, k3;
    public Vector2 colPoint, cursorDistanceToObj;
    
    public GameObject nearestGobj;
    public float rotationSpeed = 5f, k1, k2, reCor;
    bool metric = false;
    public float RotateValue, RotateValueZ;
    public WallResizeAbuse wallScript;
    public WorkSpaceController workSpace;

    public bool triggerOverlap;

    float rotSpeed = 20;

    public void Start()
    {   
        workSpace = GameObject.Find("Area").GetComponent<WorkSpaceController>();
        target = GameObject.Find("ColliderHelper");
        wallScript = GameObject.Find("ColliderHelper").GetComponent<WallResizeAbuse>();
        area = GameObject.Find("Area").GetComponent<RectTransform>();
        cmd = GameObject.Find("Main Camera").GetComponent<ComandManager>();
        widthTool.text = Convert.ToString(image.rectTransform.sizeDelta.x);
        heightTool.text = Convert.ToString(image.rectTransform.sizeDelta.y);
        settings.transform.localPosition = new Vector2(0, image.rectTransform.sizeDelta.y/2 + 120);
        wallScript = GameObject.Find("ColliderHelper").GetComponent<WallResizeAbuse>();   
        imageRect = transform.Find("Image").GetComponent<RectTransform>();
    }

    public void ToolTipResize()
    {
        if (widthToolTip.text != "" && heightToolTip.text != "")
        {
            cmd.AddCmd(this.img, Operation.Resize, this.img.transform.position, new Vector2(imageRect.sizeDelta.x, imageRect.sizeDelta.y));
            imageRect.sizeDelta = new Vector2(Convert.ToInt32(widthToolTip.text),Convert.ToInt32(heightToolTip.text));
        }
    }

    public void SetDefaultName()
    {
        string objTextureString = Regex.Match(this.gameObject.name, @"\((.*?)\)").Groups[1].Value;
        this.gameObject.transform.Find("ToolsWrapper").transform.Find("Label").transform.Find("NameInputField").GetComponent<InputField>().text = objTextureString;
    }

    public void MouseHover()
    {
        rotateHover = true;
        wallScript.objects = true;
    }

    public void MouseHoverOver()
    {
        rotateHover = false;
        wallScript.objects = false;
    }

    public void GetNearestGameObject()
    {
        RectTransform testObjRect = gObj.transform.GetComponent<RectTransform>();
        LayerMask mask = LayerMask.GetMask("SnapController");
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, mask);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, mask);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity, mask);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, mask);
        RaycastHit2D[] hits = new RaycastHit2D[4]{hitUp, hitDown, hitLeft, hitRight};

        float minDistance = 9000;
        float snapDistance = 50;

        foreach (RaycastHit2D i in hits)
        {
            float distance = 10000;

            if (i.collider != null)
            {
                distance = Vector2.Distance(transform.position, i.point);
            } 
            
            if (distance < minDistance)
            {
                minDistance = distance;  
            }

            if (distance <= snapDistance)
            {
                colPoint = i.point;
                snapping = true;
                
            } else { snapping = false;}
            
            if (distance == minDistance)
            {
                
                nearestGobj = i.collider.gameObject;
                
            }
        }
    }

    public void  OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "SnapController")
        {
            Debug.Log("COLLISION WITH FRAME");
            triggerOverlap = true;
        }
    }
    
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "SnapController")
        {
            Debug.Log("HELPER COLLIDER EXITED");
            triggerOverlap = false;
        }
    }

    public void FixedUpdate()
    {
        if (image.texture.name == "Roll Up Door"  || image.texture.name == "Door 1"  || image.texture.name =="Window 1")
        {
            GetNearestGameObject();
        }

        settings.transform.localScale = new Vector3(1.4f/area.localScale.x,1.4f/area.localScale.y,1.4f);
        float scalePosX = 100/area.localScale.x;
        float scalePosY = 100/area.localScale.y;
        //Debug.Log("Inverse Transform point = " + transform.InverseTransformPoint(area.transform.position));
        if (transform.InverseTransformPoint(area.transform.position).x < -400 && transform.InverseTransformPoint(area.transform.position).y < -400)
        {
            settings.transform.localPosition = new Vector2(-scalePosX,-scalePosY);
        } 
        else if (transform.InverseTransformPoint(area.transform.position).y < -400)
        {
            settings.transform.localPosition = new Vector2(scalePosX,-scalePosY);
        } 
        else if (transform.InverseTransformPoint(area.transform.position).x < -400)
        {
            settings.transform.localPosition = new Vector2(-scalePosX,scalePosY);
        } else {
            settings.transform.localPosition = new Vector2(scalePosX,scalePosY);
         }

         //Debug.Log("localPos " + settings.transform.localPosition);
        //settings.transform.localPosition = new Vector2(50/area.localScale.x,50/area.localScale.x);

        //Debug.Log("Snapping is " + snapping);
    }

    public void Update()
    {
        Measurements();
        HoverRotate();
        
        if (settings.activeSelf)
        {
            this.gameObject.transform.SetAsLastSibling();
        }
    }
    
    public void HoverRotate()
    {
        if (rotateHover == true)
        {
            RotateValueZ = imageRect.eulerAngles.z;

            if (Input.GetAxis("Mouse ScrollWheel") != 0f )
            {
                RotateValue += Input.GetAxis("Mouse ScrollWheel");

                if (RotateValue > 0)
                {
                    RotateValueZ += Mathf.Abs(RotateValue) * 20;
                } else if (RotateValue < 0)
                {
                    RotateValueZ -= Mathf.Abs(RotateValue) * 20;
                }
                RotateValue = 0;
            }
            imageRect.rotation = Quaternion.Euler(0,0,RotateValueZ);
        }
    }

    public void ToolTipRotate()
    {
        if (RotateTool.text != "")
        {
            cmd.AddCmd(this.gameObject, Operation.Rotate, imageRect.eulerAngles.z);
            imageRect.rotation =  Quaternion.Euler(0,0,Convert.ToInt32(RotateTool.text));
        }
    }

    public void Measurements()
    {
        metric = GameObject.Find("TopMenu").GetComponent<TopMenuController>().metricUnits;

        width.text = (image.rectTransform.sizeDelta.y*image.transform.localScale.y/12).ToString("F" + 1);
        height.text = (image.rectTransform.sizeDelta.x*image.transform.localScale.x/12).ToString("F" + 1);

        if(metric)
        {
            width.text = (image.rectTransform.sizeDelta.y*image.transform.localScale.y).ToString("F" + 1);
            height.text = (image.rectTransform.sizeDelta.x*image.transform.localScale.x).ToString("F" + 1);
        }
    }

    public void ShowBtn()
    {
        width.gameObject.SetActive(true);
        height.gameObject.SetActive(true);
        active = true;
        settings.SetActive(true);
        rotateBtn.SetActive(true);
        RotateTool.text = imageRect.eulerAngles.z.ToString("F" + 1);
    }

    public void HideBtn()
    {
        width.gameObject.SetActive(false);
        height.gameObject.SetActive(false);
        active = false;
        settings.SetActive(false);
        rotateBtn.SetActive(false);
    }

    public void DeleteBtn()
    {
        wallScript.objects = false;
        Destroy(gameObject);
    }

    public void TestRotate()
    {
        Vector3 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        imageRect.rotation = Quaternion.AngleAxis(angle , Vector3.forward);
        RotateTool.text = Math.Truncate(imageRect.eulerAngles.z).ToString();
    }
    
    public void OnMouseDown()
    {
        cursorDistanceToObj = transform.InverseTransformPoint(target.transform.position);
        Debug.Log("Local position of mouse on pointer down is " + cursorDistanceToObj);

        GameObject[] allSettings = GameObject.FindGameObjectsWithTag("Settings");
        this.gameObject.transform.SetAsLastSibling();

        foreach (GameObject item in allSettings)
        {
            item.SetActive(false);
        }

        if (workSpace.activeTool == "MouseTool")
        {
            ShowBtn();
            // if (active)
            // {
            //     HideBtn();
            //     active = false;
            // }
            // else
            // {
            //     ShowBtn();
            //     active = true;
            // }
        }
    }

    public void OnMouseOver()
    {
        Debug.Log("Mouse is over " + gameObject);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        float cursorDistance = Vector2.Distance(eventData.position, colPoint);
        
        if(Screen.fullScreen)
        {
            reCor = 25 * (area.localScale.x - 1);
            
        } else {
            reCor = (18*area.localScale.x) - 25;
        }

        if (eventData.button == PointerEventData.InputButton.Left && workSpace.activeTool == "MouseTool")
        {
            if (settings.activeSelf)
            {
                settings.SetActive(false);
            }
   
            if (image.texture.name == "Roll Up Door"  || image.texture.name == "Door 1"  || image.texture.name =="Window 1")
            {   
                if(snapping)
                {
                    //Debug.Log("TriggerOverlapTrue");
                
                    gObj.transform.SetParent(nearestGobj.transform.parent.transform);
                } else {
                    //Debug.Log("TriggerOverlapFalse");

                    gObj.transform.SetParent(GameObject.Find("Area").transform);
                }

                float relativeDistance = 0;
                if (image.texture.name == "Roll Up Door"  ||  image.texture.name =="Window 1")
                {
                    relativeDistance = 15;
                } else if (image.texture.name == "Door 1"){
                    relativeDistance = 30;
                }

                Vector2 relative = nearestGobj.transform.InverseTransformPoint(eventData.position);

                if(cursorDistance > relativeDistance)
                {
                    gObj.transform.position = eventData.position;
                    gObj.transform.SetAsLastSibling();
                } 
                else 
                {
                    if (Mathf.Abs(relative.x) > Mathf.Abs(relative.y))
                    {
                        if (imageRect.eulerAngles.z == 90 || imageRect.eulerAngles.z == 270)
                        {
                            //Debug.Log("Rotated");
                        } 
                        else 
                        {
                            if (imageRect.eulerAngles.z <= 180 && imageRect.eulerAngles.z >= 0)
                            {
                                imageRect.rotation = Quaternion.Euler(0,0,90);
                            } else if(imageRect.eulerAngles.z >= 180 && imageRect.eulerAngles.z <= 360)
                            {
                                imageRect.rotation = Quaternion.Euler(0,0,270);
                            } else {
                                imageRect.rotation = Quaternion.Euler(0,0,0);
                            }
                            
                            //Debug.Log("Not rotated");
                        }

                        if (image.texture.name == "Door 1" )
                        {
                            if (imageRect.eulerAngles.z == 90)
                            {
                                gObj.transform.position = new Vector2(colPoint.x + (imageRect.sizeDelta.x/2)+ reCor, eventData.position.y);
                            } 
                            else if (imageRect.eulerAngles.z == 270)
                            {
                                gObj.transform.position = new Vector2(colPoint.x - (imageRect.sizeDelta.x/2)- reCor, eventData.position.y);
                            }
                        } 
                        else 
                        {
                            gObj.transform.position = new Vector2(colPoint.x, eventData.position.y);
                        }
                    }
                    else if (Mathf.Abs(relative.y) > Mathf.Abs(relative.x))
                    {
                        if (imageRect.eulerAngles.z == 0 || imageRect.eulerAngles.z == 180)
                        {
                            //Debug.Log("Rotated");
                        } 
                        else 
                        {
                            if (imageRect.eulerAngles.z <= 270 && imageRect.eulerAngles.z >= 90)
                            {
                                imageRect.rotation = Quaternion.Euler(0,0,180);
                            } else if(imageRect.eulerAngles.z <= 90 && imageRect.eulerAngles.z >= 0 || imageRect.eulerAngles.z <= 360 && imageRect.eulerAngles.z >= 270)
                            {
                                imageRect.rotation = Quaternion.Euler(0,0,0);
                            } 
                        }

                        if (image.texture.name == "Door 1" )
                        {
                            if (imageRect.eulerAngles.z == 0)
                            {
                                gObj.transform.position = new Vector2(eventData.position.x ,colPoint.y - (imageRect.sizeDelta.x/2) - reCor);
                            } 
                            else if (imageRect.eulerAngles.z == 180)
                            {
                                gObj.transform.position = new Vector2(eventData.position.x ,colPoint.y + (imageRect.sizeDelta.x/2) + reCor);
                            }
                        } 
                        else 
                        {
                            gObj.transform.position = new Vector2(eventData.position.x ,colPoint.y);
                        }                
                    }
                }
            }  
            else 
            {
                /*if (cursorDistanceToObj.y > 0)
                {
                    gObj.transform.position = new Vector2(eventData.position.x, eventData.position.y - cursorDistanceToObj.y);
                } else if (cursorDistanceToObj.y < 0)
                {
                    gObj.transform.position = new Vector2(eventData.position.x, eventData.position.y - cursorDistanceToObj.y);
                } else {
                    gObj.transform.position = new Vector2(eventData.position.x, eventData.position.y - cursorDistanceToObj.y);
                }*/
                
                Vector2 startPos = cursorDistanceToObj; 
                gObj.transform.position = eventData.position - (startPos*area.localScale.x);
                Vector2 nowPos = gObj.transform.InverseTransformPoint(eventData.position);
                Debug.Log("Current pos " + gObj.transform.position + " = " + eventData.position + " - startPos (" + startPos +"). Also now local pos for cursor is " + gObj.transform.InverseTransformPoint(eventData.position));
                Debug.Log(" So result of startPos/nowPos = " + startPos.x/nowPos.x + " of area scale " + area.localScale.x + " a/f = "+ area.localScale.x / 0.76923076923077f);
                gObj.transform.SetAsLastSibling();
            }

            if (gObj.transform.localPosition.x > 600 || gObj.transform.localPosition.x < -600 || gObj.transform.localPosition.y > 600 || gObj.transform.localPosition.y < -600) 
            {
                Debug.LogError("Boom");
                 Destroy(gObj);
            }
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {  
        if (eventData.button == PointerEventData.InputButton.Left && workSpace.activeTool == "MouseTool")
        {
            cmd.AddCmd(this.gameObject, Operation.Drag, this.gameObject.transform.position);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (image.texture.name == "Roll Up Door" ||  image.texture.name =="Window 1")
        {
            //Debug.Log("Neares Object is = " + nearestGobj.transform.parent.name);
            k1 = nearestGobj.transform.parent.GetComponent<RectTransform>().sizeDelta.x/transform.localPosition.x;
            k2 = nearestGobj.transform.parent.GetComponent<RectTransform>().sizeDelta.y/transform.localPosition.y;
            k3 = false;      
        } 
        else if(image.texture.name == "Door 1")
        {
            k1 = nearestGobj.transform.parent.GetComponent<RectTransform>().sizeDelta.x/transform.localPosition.x;
            k2 = nearestGobj.transform.parent.GetComponent<RectTransform>().sizeDelta.y/transform.localPosition.y;
            k3 = true;
        }
    }

    public PlanDetails ToPlanDetails()
    {
        Vector2 localPos = area.InverseTransformPoint(gameObject.transform.position);
        return new PlanDetails(
            localPos.x,
            localPos.y,
            imageRect.eulerAngles.z,
            imageRect.sizeDelta.x,
            imageRect.sizeDelta.y,
            gameObject.name,
            "ObjItemToPlan");
    }  
}
