using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WallResizeAbuse : MonoBehaviour
{
    public  PointerEventData eventData;
    public GameObject gObj;
    public bool objects;
    public SliderController slider;
    public float ZoomValue;
    public GameObject listView;
    
    public void Update()
    {
        //Debug.Log(objects);
        gObj.transform.position = Input.mousePosition;
        //Debug.Log("Count is = "+ objects);
        if (objects == false)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f && listView.activeSelf == false)
            {
                ZoomValue += Input.GetAxis("Mouse ScrollWheel");

                if (ZoomValue > 0)
                {
                    slider.ZoomIn();
                    //RotateValueZ += Mathf.Abs(RotateValue) * 20;
                } else if (ZoomValue < 0)
                {
                    slider.ZoomOut();
                    //RotateValueZ -= Mathf.Abs(RotateValue) * 20;
                }
                ZoomValue = 0;
            }
        }

    }
    
     
}
