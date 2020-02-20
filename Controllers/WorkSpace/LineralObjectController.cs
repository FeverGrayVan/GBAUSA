using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Application.Models;

public class LineralObjectController : MonoBehaviour, IDragHandler//, IPointerDownHandler
{
    public GameObject gObj, settings;

    public RectTransform tt;
    public RectTransform width;
    public ComandManager cmd;
    public WorkSpaceController workSpace;

    public void Start()
    {
        workSpace = GameObject.Find("Area").GetComponent<WorkSpaceController>();
        Measurements();
    }

    public void DeleteBtn()
    {
        Destroy(gObj);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (workSpace.activeTool == "MouseTool")
        {
            if (settings.activeSelf)
                settings.SetActive(false);

            gameObject.transform.position = eventData.position;
            gObj.transform.SetAsLastSibling();
            if (gameObject.transform.localPosition.x > 600 || gameObject.transform.localPosition.x < -600 || gameObject.transform.localPosition.y > 600 || gameObject.transform.localPosition.y < -600) 
            {
                Debug.LogError("Boom");
                Destroy(gameObject);
            }
        }
    }

    public void OnResizeDrag(BaseEventData data)
    {
        PointerEventData eventData = data as PointerEventData;
        
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            RectTransform area = GameObject.Find("Area").GetComponent<RectTransform>();
            Vector3 startTemp = tt.localPosition;
            Vector3 endTemp = GameObject.Find("Area").transform.InverseTransformPoint(eventData.position);

            Vector3 differenceVector = (endTemp) - startTemp;
            tt.sizeDelta = new Vector2(differenceVector.magnitude, 2);
            
            tt.pivot = new Vector2(0, 0.5f);
            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
            tt.rotation = Quaternion.Euler(0, 0, angle + area.eulerAngles.z);

            RectTransform settingsRect = settings.GetComponent<RectTransform>();
            if ((angle > 90 && angle < 180) || (angle < -90 && angle > -180))
            {
                width.localPosition = new Vector2(width.localPosition.x , -10);
                width.localRotation = Quaternion.Euler(0,0,180);
                settingsRect.localRotation = Quaternion.Euler(0,0,-angle + 180);
            } 
            else 
            {
                width.localPosition = new Vector2(width.localPosition.x , 10);
                width.localRotation = Quaternion.Euler(0,0,0);
                settingsRect.localRotation = Quaternion.Euler(0,0,-angle);
            }
            Measurements();
        }
    }

    public void Measurements()
    {
        var metric = GameObject.Find("TopMenu").GetComponent<TopMenuController>().metricUnits;
        if (!metric)
        {
            gObj.GetComponentInChildren<Text>().text = (((tt.sizeDelta.x )/12)*(tt.localScale.x)).ToString("F" + 1);  
        }   
        else if (metric)
        {
            gObj.GetComponentInChildren<Text>().text = ((tt.sizeDelta.x ) * (tt.localScale.x)).ToString("F" + 1);
        }
    }

    public PlanDetails ToPlanDetails()
    {
        return new PlanDetails(
            tt.localPosition.x,
            tt.localPosition.y,
            transform.eulerAngles.z,
            tt.sizeDelta.x,
            tt.sizeDelta.y,
            gameObject.name,
            "LineralToPlan");
    }
}
