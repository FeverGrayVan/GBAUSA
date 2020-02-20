using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WallToolController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector3 start = Vector3.zero;
    private Vector3 startAssist;
    private RectTransform objRect;
    private GameObject tmp;
    public ComandManager cmd;
    public GameObject helper;
    public WorkSpaceController workSpace;

    public void Start()
    {
        workSpace = GameObject.Find("Area").GetComponent<WorkSpaceController>();
        helper = GameObject.Find("ColliderHelper");
        cmd = GameObject.Find("Main Camera").GetComponent<ComandManager>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (workSpace.activeTool == "WallTool")
        {
            start = eventData.position;
            tmp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/garageItem"));
            objRect = tmp.GetComponent<RectTransform>();
            objRect.localScale = new Vector3(1,1,1);
            tmp.transform.SetParent(GameObject.Find("Area").transform);
            tmp.transform.SetAsLastSibling();
            tmp.transform.localScale = new Vector3(1,1,1);
            objRect.position = start; 
            startAssist = objRect.localPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform areaRect = GameObject.Find("Area").GetComponent<RectTransform>();
        if (workSpace.activeTool == "WallTool")
        {
            Vector3 end = eventData.position;
            float xScale = 0.76f / areaRect.localScale.x;
            float yScale = 0.76f / areaRect.localScale.y;
            float xdir = end.x - start.x;
            float ydir = end.y - start.y;
            float curSizeX=0, curSizeY=0;

            if (Mathf.Abs(helper.transform.localPosition.x) <= 600 && Mathf.Abs(helper.transform.localPosition.y) <= 600)
            {
                curSizeX = Mathf.Abs(xdir*xScale);
                curSizeY = Mathf.Abs(ydir*yScale);
            }

            objRect.sizeDelta = new Vector2(curSizeX, curSizeY);

            if (end.x < start.x  && end.y > start.y) {
                objRect.localPosition = new Vector2(startAssist.x - (objRect.sizeDelta.x /2),startAssist.y + (objRect.sizeDelta.y / 2));
            } else if (end.x < start.x && end.y < start.y){
                objRect.localPosition = new Vector2(startAssist.x - (objRect.sizeDelta.x /2),startAssist.y - (objRect.sizeDelta.y / 2));
            } else if (end.x > start.x && end.y < start.y){
                objRect.localPosition = new Vector2(startAssist.x + (objRect.sizeDelta.x /2),startAssist.y - (objRect.sizeDelta.y / 2));
            } else {
                objRect.localPosition = new Vector2(startAssist.x + (objRect.sizeDelta.x /2),startAssist.y + (objRect.sizeDelta.y / 2));
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (workSpace.activeTool == "WallTool")
        {
            GarageItemController garageScript = tmp.transform.GetComponent<GarageItemController>();
            garageScript.inputHeight.text = objRect.sizeDelta.y.ToString("F" + 1);
            garageScript.inputWidth.text = objRect.sizeDelta.x.ToString("F" + 1);
            workSpace.ToolSwitch("MouseTool");
            cmd.AddCmd(tmp, Operation.Create);
            tmp = null;
        }
    }
}
