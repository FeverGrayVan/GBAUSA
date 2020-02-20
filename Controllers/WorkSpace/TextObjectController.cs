using Application.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextObjectController : MonoBehaviour, IBeginDragHandler, IDragHandler//, IPointerDownHandler
{
    public GameObject gObj, settings;
    public RectTransform area;
    public InputField input, field;
    public Dropdown drop;
    public ComandManager cmd;
    private WorkSpaceController workSpace;
    public GameObject placeholder;
    public Text outputText;
    public RectTransform textOutputRect;

    public void Start()
    {
        workSpace = GameObject.Find("Area").GetComponent<WorkSpaceController>();
        cmd = GameObject.Find("Main Camera").GetComponent<ComandManager>();
        area = GameObject.Find("Area").GetComponent<RectTransform>();
        DropDownOnValueChange();
    }

    public void Update()
    {
        settings.transform.localScale = new Vector3(1.4f/area.localScale.x,1.4f/area.localScale.y,1.4f);
        settings.transform.localPosition = new Vector2(150/area.localScale.x,80/area.localScale.x);

        ToolTipView();

        if (input.text != ""){
            placeholder.SetActive(false);
        } else {
            placeholder.SetActive(true);
        }

        outputText.text = input.text;

        int size = outputText.fontSize;
        int lines = outputText.cachedTextGenerator.lineCount;
        textOutputRect.sizeDelta = new Vector2(textOutputRect.sizeDelta.x, size*lines);
    }

    public void ToolTipView()
    {
        float scalePosX = 100/area.localScale.x;
        float scalePosY = 100/area.localScale.y;

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
    }

    public void DropDownOnValueChange()
    {
        var size = drop.options[drop.value].text;
        string[] s = size.Split(new char[] { 'p' });
        outputText.fontSize = Convert.ToInt32(s[0]);
    }

    public void OnPointerDown(BaseEventData data)
    {
        PointerEventData eventData = data as PointerEventData;
        workSpace.HideSettings();
        settings.SetActive(true);
        gObj.transform.SetAsLastSibling();
    }

    public void CloneBtn()
    {
        var tmp = Resources.Load<GameObject>("Prefabs/TextElem");      
        GameObject tmp2 = Instantiate(tmp, GameObject.Find("Area").transform);

        tmp2.transform.position = new Vector2(gObj.transform.position.x + 20, gObj.transform.position.y - 20);
        string tempText = this.gameObject.transform.Find("InputField").transform.Find("Text").transform.Find("Settings").transform.Find("OriginInputField").GetComponent<InputField>().text;
        tmp2.transform.Find("InputField").transform.Find("Text").transform.Find("Settings").transform.Find("OriginInputField").GetComponent<InputField>().text = tempText;
        tmp2.transform.Find("InputField").transform.Find("Text").GetComponent<Text>().text = tempText;
        tmp2.transform.Find("InputField").transform.Find("Text").GetComponent<Text>().fontSize = outputText.fontSize;
        tmp2.transform.Find("InputField").transform.Find("Text").transform.Find("Settings").transform.Find("Dropdown").GetComponent<Dropdown>().value = drop.value;
        tmp2.transform.SetAsLastSibling();
    }

    public void DeleteBtn()
    {
        Destroy(gObj);
        cmd.AddCmd(this.gameObject, Operation.Delete, this.gameObject.transform.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        gObj.transform.SetAsLastSibling();
        if (workSpace.activeTool == "MouseTool")
        {
            gObj.transform.position = eventData.position;
        }
    }

    public void OnBeginDrag(PointerEventData eventData )
    {
        gObj.transform.SetAsLastSibling();
        if (workSpace.activeTool == "MouseTool")
        {
            cmd.AddCmd(this.gameObject, Operation.Drag, this.gameObject.transform.position);
        }
    }

    public void OnTextBeginDrag(BaseEventData data)
    {
        PointerEventData eventData = data as PointerEventData;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            cmd.AddCmd(this.gameObject, Operation.Drag, this.gameObject.transform.position);
        }
    }

    public void onTextEndDrag(BaseEventData data)
    {
        PointerEventData eventData = data as PointerEventData;
        if (transform.localPosition.x > 600 || transform.localPosition.x < -600 || transform.localPosition.y > 600 || transform.localPosition.y < -600)
        {
            Destroy(gObj);
        }
    }
    
    public void TextDrag(BaseEventData data)
    {
        PointerEventData eventData = data as PointerEventData;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //this.transform.parent.position = new Vector2(eventData.position.x, eventData.position.y);
            transform.position = new Vector2(eventData.position.x, eventData.position.y);
        }

        if (this.transform.parent.localPosition.x > 600 || this.transform.parent.localPosition.x < -600 || this.transform.parent.localPosition.y > 600 || this.transform.parent.localPosition.y < -600) 
        {
            //Destroy(this.transform.parent.gameObject);
        }
        Debug.Log(outputText.text);
        Debug.Log(outputText.fontSize);
    }
       
    public PlanDetails ToPlanDetails()
    {
        Vector2 localPos = area.InverseTransformPoint(gameObject.transform.position);
        return new PlanDetails(
            localPos.x,
            localPos.y,
            0,
            drop.value,
            0,
            gameObject.name,
            outputText.text);
    }
}
