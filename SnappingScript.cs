using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SnappingScript : MonoBehaviour//, IPointerDownHandler
{
    public RectTransform rctT,prntT;
    public SpriteRenderer sprtR;
    public GarageItemController garageCtrl;
    public ComandManager cmd;

    

    
    // Start is called before the first frame update
    void Start()
    {
        rctT = gameObject.GetComponent<RectTransform>();
        sprtR = gameObject.GetComponent<SpriteRenderer>();
        cmd = GameObject.Find("Main Camera").GetComponent<ComandManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Resize();
        
        
    }
    public void  OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Helper")
        {
            //Debug.Log("COLLISION WITH FRAME");
            garageCtrl.resizeCol = true;
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Helper")
        {
            //Debug.Log("HELPER COLLIDER EXITED");
            garageCtrl.resizeCol = false;
        }
    }
    public void OnMouseDown()
    {
        
        if (Input.GetMouseButton(0))
        {
            //Debug.Log("mouse on border");
        }
        
    }

    void Resize()
    {
         rctT.localScale = new Vector3(
            prntT.sizeDelta.x / 100 ,
            prntT.sizeDelta.y / 100 , 1

        );
    }

    
}
