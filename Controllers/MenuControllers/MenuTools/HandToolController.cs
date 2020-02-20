using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HandToolController : MonoBehaviour
{
    public WorkSpaceController workSpace;

    public void Update()
    {
        if (workSpace.activeTool == "HandTool" || workSpace.activeTool == "MouseTool")
        {
            GameObject.Find("Scroll View").GetComponent<ScrollRect>().enabled = true;
        } else {
            GameObject.Find("Scroll View").GetComponent<ScrollRect>().enabled = false;
        }
    }
}
