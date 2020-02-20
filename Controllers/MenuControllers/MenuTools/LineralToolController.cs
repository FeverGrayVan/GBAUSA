using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LineralToolController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{    
    Vector3 start = Vector3.zero;
    Vector3 end = Vector3.zero;

    public RectTransform tt;
    public GameObject tmp;

    public ComandManager cmd;
    public WorkSpaceController workSpace;


    public void Start()
    {
        workSpace = GameObject.Find("Area").GetComponent<WorkSpaceController>();
        cmd = GameObject.Find("Main Camera").GetComponent<ComandManager>();
    }  

    public void OnPointerDown(PointerEventData eventData)
    {
        if (workSpace.activeTool == "LineralTool")
        {
            start = GameObject.Find("Area").transform.InverseTransformPoint(eventData.position);
            tmp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Line"));
            tt = tmp.GetComponent<RectTransform>();
            tmp.transform.SetParent(GameObject.Find("Area").transform);
            tt.localScale = new Vector3(1,1,1);
            tmp.transform.SetAsLastSibling();
            tmp.transform.position = transform.TransformPoint(start);
        }   
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (workSpace.activeTool == "LineralTool")
        {
            end = GameObject.Find("Area").transform.InverseTransformPoint(eventData.position);
            RectTransform width = tmp.transform.Find("Width").GetComponent<RectTransform>();
            Vector3 differenceVector = end - start;
            tt.sizeDelta = new Vector2(differenceVector.magnitude, 2);
            
            tt.pivot = new Vector2(0, 0.5f);
            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
            tt.rotation = Quaternion.Euler(0, 0, angle);

            if ((angle > 90 && angle < 180) || (angle < -90 && angle > -180))
            {
                width.localPosition = new Vector2(width.localPosition.x , -10);
                width.localRotation = Quaternion.Euler(0,0,180);
            } 
            else 
            {
                width.localPosition = new Vector2(width.localPosition.x , 10);
                width.localRotation = Quaternion.Euler(0,0,0);
            }

            RectTransform settings = tmp.transform.Find("Width").transform.Find("Settings").GetComponent<RectTransform>();

            if (angle > -270 && angle <-90)
            {
                settings.localRotation = Quaternion.Euler(0,0,-angle + 180);
            } else if(angle > 90 && angle < 180){
                settings.localRotation = Quaternion.Euler(0,0,-angle + 180);
            } else {
                settings.localRotation = Quaternion.Euler(0,0,-angle);
            }
            var metric = GameObject.Find("TopMenu").GetComponent<TopMenuController>().metricUnits;
            if (!metric)
            {
                tmp.GetComponentInChildren<Text>().text = (((tt.sizeDelta.x )/12)*(tmp.GetComponent<RectTransform>().localScale.x)).ToString("F" + 1);  
            }   
            else if (metric)
            {
                tmp.GetComponentInChildren<Text>().text = ((tt.sizeDelta.x ) * (tmp.GetComponent<RectTransform>().localScale.x)).ToString("F" + 1);
            }
        }   
    }

    public void OnPointerUp(PointerEventData eventData)
    {   
        if(tmp != null)
        {
            cmd.AddCmd(tmp, Operation.Create);
            if (tt.sizeDelta.x < 12)
            {
                Destroy(tmp);
            }
        }
        //tmp = null;
        //tt = null;
    }    
}
