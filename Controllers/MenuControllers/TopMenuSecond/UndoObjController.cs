using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UndoObjController : MonoBehaviour
{
    public ComandManager com;
    bool first_click = false;
    public void Start()
    {
        com = GameObject.Find("Main Camera").GetComponent<ComandManager>();
    }
    public void UndoBtn()
    {
        if (!first_click)
        {
            first_click = true;
            GameObject toolTip = Resources.Load<GameObject>("Prefabs/ToolTipPref");
            //toolTip.GetComponent<ToolTipController>().setMessage("Undo position of object");
            //GameObject.Instantiate(toolTip);
        }
        if (com.cmd.Count > 0)
        {
            UndoObject tmp = com.cmd.Pop();
            if (tmp.Item.gameObject == null){
                Debug.Log("ObjectNull");
                return;}
            if (tmp.Opt == Operation.Delete)
            {
                print(tmp.Item.name);

                Instantiate(tmp.Item);
            }
            if (tmp.Opt == Operation.Rotate)
            {
                print(tmp.Rot);
                tmp.Item.transform.rotation =  Quaternion.Euler(0,0,tmp.Rot);
                
            }
            if (tmp.Opt == Operation.Create)
            {
                Debug.Log("Object Created");
                Destroy(tmp.Item);
            }
            if (tmp.Opt == Operation.Resize)
            {
                print(tmp.Item.name);

                Debug.Log("Resize get?");
                tmp.Item.transform.GetComponent<RectTransform>().sizeDelta = tmp.Rect;
                Debug.Log(tmp.Rect.x);
            }
            if (tmp.Opt == Operation.Drag)
            {
                var tr = GameObject.Find(tmp.Item.name).transform;
                //print("Exist: "+tr.position);
                print("InList: "+tmp.Item.transform.position);
                tmp.Item.transform.SetAsLastSibling();
                tmp.Item.transform.position = tmp.Trans;
            }
        }
    }
}
