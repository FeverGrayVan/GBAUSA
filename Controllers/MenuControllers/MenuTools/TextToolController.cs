using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextToolController : MonoBehaviour, IPointerClickHandler
{
    public ComandManager cmd;
    public WorkSpaceController workSpace;

    public void Start()
    {
        workSpace = GameObject.Find("Area").GetComponent<WorkSpaceController>();
        cmd = GameObject.Find("Main Camera").GetComponent<ComandManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (workSpace.activeTool == "TextTool")
        {
            CreateTextElem(eventData.position);
        }
    }

    public void CreateTextElem(Vector2 pos)
    {
        var tmp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/TextElem"));
        tmp.transform.position = pos;
        
        tmp.transform.SetParent(GameObject.Find("Area").transform);
        tmp.transform.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        tmp.transform.SetAsLastSibling();
        cmd.AddCmd(tmp, Operation.Create);
        tmp = null;
        workSpace.ToolSwitch("MouseTool");
    }
}