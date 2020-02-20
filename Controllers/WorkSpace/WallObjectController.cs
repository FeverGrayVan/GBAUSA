using Application.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace Application
{
    public class WallObjectController : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public GameObject wall, settings;
        public RectTransform image;
        public Text width, height;
        public Button delete;
        public bool isActive = false;
        public ComandManager cmd;
        public InputField inputWidth, inputHeight;
        private WorkSpaceController workSpace;

        public bool halfSize;

        public void Start()
        {
            cmd = GameObject.Find("Main Camera").GetComponent<ComandManager>();
            workSpace = GameObject.Find("Area").GetComponent<WorkSpaceController>();
        }

        public void ShowSize()
        {
            var metric = GameObject.Find("TopMenu").GetComponent<TopMenuController>().metricUnits;
            if (!metric)
            {
                width.text = ((image.sizeDelta.x / 12)*(wall.GetComponent<RectTransform>().localScale.x)).ToString("F" + 1);
                height.text = ((image.sizeDelta.y / 12)*(wall.GetComponent<RectTransform>().localScale.y)).ToString("F" + 1);
            }
            else if(metric)
            {
                width.text = ((image.sizeDelta.x)*(wall.GetComponent<RectTransform>().localScale.x)).ToString("F" + 1);
                height.text = ((image.sizeDelta.y)*(wall.GetComponent<RectTransform>().localScale.x)).ToString("F" + 1);
            }
        }
        public void Deleting()
        {
            //cmd.AddCmd(this.gameObject,Operation.Delete,this.gameObject.transform.position);
            GameObject.Destroy(wall);
        }

        public void ActiveBtn()
        {
            
            width.gameObject.SetActive(true);
            height.gameObject.SetActive(true);
            settings.SetActive(true);
            inputWidth.text = Convert.ToString(image.sizeDelta.x);
            inputHeight.text = Convert.ToString(image.sizeDelta.y);

        }
         public void ResizeToolTip()
         {
            if (inputWidth.text != "" && inputHeight.text != "")
            {
            image.sizeDelta = new Vector2(Convert.ToSingle(inputWidth.text),Convert.ToSingle(inputHeight.text));
            }
    }
        public void DiactiveBtn()
        {
            
            width.gameObject.SetActive(false);
            height.gameObject.SetActive(false);
            settings.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            GameObject[] allSettings = GameObject.FindGameObjectsWithTag("Settings");
            ActiveBtn();
            foreach (GameObject item in allSettings)
            {
                //item.SetActive(false);
            }
            if (isActive)
            {
                DiactiveBtn();
                isActive = false;
            }
            else
            {
                isActive = true;
                ShowSize();
                ActiveBtn();
            }
        }
        public void OnMouseUp()
        {
            isActive = false;
            DiactiveBtn();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isActive = true;
            //ShowSize();
            //ActiveBtn();
            cmd.AddCmd(this.gameObject,Operation.Drag,this.gameObject.transform.position);
            
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (workSpace.activeTool == "MouseTool" && this.transform.parent.name != "garageItem"){
            
            isActive = true;
            DiactiveBtn();
            wall.transform.position = eventData.position;
            if (wall.transform.localPosition.x > 600 || wall.transform.localPosition.x < -600 || wall.transform.localPosition.y > 600 || wall.transform.localPosition.y < -600) 
            {
                Debug.LogError("Boom");
                Destroy(wall);
            }
            }
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            isActive = false;
            DiactiveBtn();
        }
        public PlanDetails ToPlanDetails()
        {
            return new PlanDetails(
            gameObject.transform.position.x,
            gameObject.transform.position.y,
            gameObject.transform.rotation.z,
            gameObject.GetComponent<RectTransform>().sizeDelta.x,
            gameObject.GetComponent<RectTransform>().sizeDelta.y,
            gameObject.name,
            "WallItemToPlan");
        }
    }
}


