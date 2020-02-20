using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraToolController : MonoBehaviour, IPointerClickHandler
{
    public ComandManager cmd;
    public GameObject tmp;
    public WorkSpaceController workSpace;

    public void Start()
    {
        workSpace = GameObject.Find("Area").GetComponent<WorkSpaceController>();
        cmd = GameObject.Find("Main Camera").GetComponent<ComandManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (workSpace.activeTool == "CameraTool")
        {
            CreatePhoto(eventData.position);
            cmd.AddCmd(tmp, Operation.Create);
        }
    }

    public void CreatePhoto(Vector2 pos)
    {
        tmp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Photo"));
        tmp.transform.position = pos;
        tmp.transform.SetParent(GameObject.Find("Area").transform);
        tmp.transform.localScale = new Vector3(1,1,1);
        tmp.transform.SetAsLastSibling();
        workSpace.ToolSwitch("MouseTool");
        tmp = null;
    }

}
