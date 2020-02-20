using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderController : MonoBehaviour {

    private float zoomSpeed = 0.1f;
    private Vector3 initialScale = Vector3.one;
    public Slider mainSlider;
    public Transform areaTransform;
    

    public GameObject grgItem;

    public float zoomAmount;

    public float width,height,k,num;

    public void Start()
    {
        grgItem = GameObject.Find("MainGarage");
        StartCoroutine(StartFit());
    }

    public void Update()
    {
        GetK();
        mainSlider.value = ((areaTransform.localScale - initialScale)/zoomSpeed).x;      
    }

    public IEnumerator StartFit()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Start Fit");
        GetK();
        SetNormal();
    }

    public void ZoomIn()
    {
        HideSettings();
        zoomAmount = areaTransform.localScale.x + 0.5f;
        if (zoomAmount > 6){ zoomAmount = 6f;}
        areaTransform.localScale = new Vector3(
            zoomAmount,
            zoomAmount,
            zoomAmount 
        );
    }
    
    public void ZoomOut()
    {
        HideSettings();
        zoomAmount = areaTransform.localScale.x - 0.5f;
        if (zoomAmount < 0.4f){ zoomAmount = 0.4f;}
        areaTransform.localScale = new Vector3(
            zoomAmount,
            zoomAmount,
            zoomAmount 
        );
    }

    public void OnSliderValueChanged()
    {
        var delta = Vector3.one * (mainSlider.value * zoomSpeed);
        var desiredScale = initialScale + delta;
        areaTransform.localScale = desiredScale;
        
    } 

    public void SetNormal()
    {
        num = 500;
        GetK();
        Debug.Log("Area local Pos =" + areaTransform.localPosition );
        if (Screen.fullScreen)
        {
            areaTransform.localPosition = new Vector3(539.5f,-339.2f,0);
        } else {
            areaTransform.localPosition = new Vector3(368.9f,-339.2f,0);
        }
       
        areaTransform.localScale = new Vector3(num/k,num/k,1);    
    }

    public void HideSettings()
    {
        GameObject[] allSettings = GameObject.FindGameObjectsWithTag("Settings");

        foreach (GameObject item in allSettings)
        {
            item.SetActive(false);
        }
    }

    public void GetK()
    {
        float bonus = ObjectDistance();
        //Debug.Log("Bonus is " + bonus);
        width = grgItem.GetComponent<RectTransform>().sizeDelta.x + bonus;
        height = grgItem.GetComponent<RectTransform>().sizeDelta.y + bonus;
        
        if (width > height)
        {
            if (Screen.fullScreen)
            {num = 1000;}
            k = width;   
        } else if (height > width)
        {
            k = height;
        } else if (height == width)
        {
            k = height;
        }
    }

    public float ObjectDistance()
    {
        float maxDistance = 0;
        foreach (Transform child in areaTransform) if (child.CompareTag("Object") || child.CompareTag("Wall")) 
        {
            float distance = 0;
            if (Mathf.Abs(child.localPosition.x) > Mathf.Abs(child.localPosition.y))
            {
                distance = Mathf.Abs(child.localPosition.x);
            } else if (Mathf.Abs(child.localPosition.y) > Mathf.Abs(child.localPosition.x))
            {
                distance = Mathf.Abs(child.localPosition.y);
            } else if (Mathf.Abs(child.localPosition.y) == Mathf.Abs(child.localPosition.x))
            {
                distance = Mathf.Abs(child.localPosition.y);
            }
            //Debug.Log("Distance to the "+ child.name + " is " + distance);
            if (distance > maxDistance)
            {
                maxDistance = distance;
            }
        }
        //Debug.Log("Max distance is "+ maxDistance);
        return maxDistance;
    }
}
