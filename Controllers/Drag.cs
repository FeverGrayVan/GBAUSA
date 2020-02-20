using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerClickHandler
{
    public bool dragOnSurfaces = true;
    public Image[] objects;
    public RawImage[] objects2;

    private Dictionary<int, GameObject> m_DraggingIcons = new Dictionary<int, GameObject>();
    private Dictionary<int, RectTransform> m_DraggingPlanes = new Dictionary<int, RectTransform>();

    public ComandManager cmd;
    private GameObject canvas;
    public WorkSpaceController workSpace;
    private Drop drop;

    public void Start()
    {
        cmd = GameObject.Find("Main Camera").GetComponent<ComandManager>();
        canvas = GameObject.Find("Canvas");
        workSpace = GameObject.Find("Area").GetComponent<WorkSpaceController>();
        drop = GameObject.Find("Area").GetComponent<Drop>();
        SetTextLabel();
    }

    public void SetTextLabel()
    {
        Text textLabel = gameObject.transform.GetChild(0).GetComponentInChildren<Text>();
        if(textLabel != null){
            textLabel.text = this.gameObject.transform.parent.name;
        }else{
            Debug.Log("Found null object preview");
        }             
    }

    public void OnPointerClick(PointerEventData data)
    {
        var dropObj = (GameObject)Instantiate(Resources.Load("Prefabs/ObjItem"));
        var itemObj = dropObj.GetComponent<ObjItemController>();

        var originalObj = data.pointerDrag;
        var srcImage = originalObj.GetComponent<Image>();

        Sprite sp = srcImage.sprite;
        itemObj.image.texture = sp.texture;

        dropObj.name = "ObjItem("+sp.texture.name+")";
        dropObj.transform.SetParent(GameObject.Find("Area").transform); 

        itemObj.image.rectTransform.sizeDelta = drop.ManualSizes(sp.texture.name);
        itemObj.image.transform.localScale = new Vector3(1,1,1); 

        dropObj.transform.localScale = new Vector3(1,1,1);
        dropObj.transform.localPosition = new Vector2(0,0);
        
        cmd.AddCmd(dropObj, Operation.Create);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        m_DraggingIcons[eventData.pointerId] = new GameObject("icon");

        m_DraggingIcons[eventData.pointerId].transform.SetParent(canvas.transform, false);
        m_DraggingIcons[eventData.pointerId].transform.SetAsLastSibling();

        var image = m_DraggingIcons[eventData.pointerId].AddComponent<Image>();
        var group = m_DraggingIcons[eventData.pointerId].AddComponent<CanvasGroup>();
        group.blocksRaycasts = false;

        image.sprite = GetComponent<Image>().sprite;
        image.sprite.name = GetComponent<Image>().sprite.name;

        float width = image.sprite.texture.width, height = image.sprite.texture.height, target = 1;
        if (width > height)
        {
            target = (100 / width);
        }
        else
        {
            target = (100 / height);
        }
        width = Mathf.Round(width * target);
        height = Mathf.Round(height * target);
        image.rectTransform.sizeDelta = new Vector2(width, height);

        if (dragOnSurfaces)
        {
            m_DraggingPlanes[eventData.pointerId] = transform as RectTransform;
        }
        else
        {
            m_DraggingPlanes[eventData.pointerId] = canvas.transform as RectTransform;
        }

        workSpace.AreaItemsRaycast(false, true);
        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_DraggingIcons[eventData.pointerId] != null)
            SetDraggedPosition(eventData);
        workSpace.AreaItemsRaycast(false, true);   
    }

    private void SetDraggedPosition(PointerEventData eventData)
    {
        if (dragOnSurfaces && eventData.pointerEnter != null && eventData.pointerEnter.transform as RectTransform != null)
            m_DraggingPlanes[eventData.pointerId] = eventData.pointerEnter.transform as RectTransform;
        
        var rt = m_DraggingIcons[eventData.pointerId].GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlanes[eventData.pointerId], eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = m_DraggingPlanes[eventData.pointerId].rotation;
        }
        Debug.Log("transporting");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_DraggingIcons[eventData.pointerId] != null)
        {    
            Destroy(m_DraggingIcons[eventData.pointerId]);
            m_DraggingIcons[eventData.pointerId] = null;
        }
        workSpace.ToolSwitch("MouseTool");
    }
}
