using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drop : MonoBehaviour, IDropHandler
{
    public ComandManager cmd;
    private WorkSpaceController workSpace;

    public void Start()
    {
        cmd = GameObject.Find("Main Camera").GetComponent<ComandManager>();
        workSpace = GameObject.Find("Area").GetComponent<WorkSpaceController>();
    }

    public void OnDrop(PointerEventData data)
    {
        Sprite dropSprite = GetDropSprite(data);
        if (dropSprite != null)
        {
            var dropObj = (GameObject)Instantiate(Resources.Load("Prefabs/ObjItem"));
            var itemObj = dropObj.GetComponent<ObjItemController>();

            dropObj.transform.position = new Vector2(data.position.x ,data.position.y);
            Sprite sp = Sprite.Create(dropSprite.texture, dropSprite.textureRect, new Vector2(0.5f, 0.5f));
            itemObj.image.texture = sp.texture;

            dropObj.name = "ObjItem("+sp.texture.name+")";
            dropObj.transform.SetParent(GameObject.Find("Area").transform);
            dropObj.transform.SetAsLastSibling();
            itemObj.image.rectTransform.sizeDelta = ManualSizes(dropSprite.texture.name);
            itemObj.image.transform.localScale = new Vector3(1,1,1);
            dropObj.transform.localScale = new Vector3(1,1,1);
            
            cmd.AddCmd(dropObj, Operation.Create);
        }
    }

    public Vector2 ManualSizes(string textureName)
    {
        Vector2 itemRectDelta = new Vector2(0,0);
        switch(textureName)
        {
            case "Window 1":
            itemRectDelta = new Vector2(30,5);
            break;
            case "Camper":
            itemRectDelta = new Vector2(114,396);
            break;
            case "Red Pickup":
            case "Orange Pickup":
            case "Silver Pickup":
            itemRectDelta = new Vector2(82,233);
            break;
            case "Cow":
            itemRectDelta = new Vector2(29,93);
            break;
            case "Horse":
            case "Red Cruising Motorcycle":
            case "Blue Cruising Motorcycle":
            itemRectDelta = new Vector2(36,91);
            break;
            case "Motorcycle":
            case "Air Compressor":
            case "Small Loader":
            itemRectDelta = new Vector2(41,100);
            break;
            case "Large Loader":
            itemRectDelta = new Vector2(46,100);
            break;
            case "Rowboat":
            itemRectDelta = new Vector2(48,180);
            break;
            case "Canoe":
            itemRectDelta = new Vector2(33,168);
            break;
            case "Coupe":
            itemRectDelta = new Vector2(65,168);
            break;
            case "Snowmobile":
            itemRectDelta = new Vector2(48,120);
            break;
            case "Water Trough":
            itemRectDelta = new Vector2(100,51);
            break;
            case "Motorboat":
            itemRectDelta = new Vector2(60,240);
            break;
            case "Sports Car":
            itemRectDelta = new Vector2(77,183);
            break;
            case "SUV":
            itemRectDelta = new Vector2(80,208);
            break;
            case "Red Bicycle":
            itemRectDelta = new Vector2(18,68);
            break;
            case "Rolling Tool Chest":
            case "Tool Chest":
            itemRectDelta = new Vector2(18,26);
            break;
            case "Lathe":
            case "Drill Press":
            itemRectDelta = new Vector2(60,100);
            break;
            case "Table Saw":
            itemRectDelta = new Vector2(40,60);
            break;
            case "Pressure Washer":
            itemRectDelta = new Vector2(20,40);
            break;
            case "Workbench":
            itemRectDelta = new Vector2(36,96);
            break;
            case "10'x10'Stall   (Left Entrance)":
            case "10'x10'Stall   (Right Entrance)":
            itemRectDelta = new Vector2(120,120);
            break;
            case "12'x12'Stall   (Left Entrance)":
            case "12'x12'Stall   (Right Entrance)":
            itemRectDelta = new Vector2(144,144);
            break;
            case "Goat":
            itemRectDelta = new Vector2(17,57);
            break;
            case "Pig":
            itemRectDelta = new Vector2(18,61);
            break;
            case "Bale of Hay (Square)":
            itemRectDelta = new Vector2(18,18);
            break;
            case "Bale of Hay (Rectangular)":
            itemRectDelta = new Vector2(18,36);
            break;
            case "Combine":
            itemRectDelta = new Vector2(360,276);
            break;
            case "Manure Spreader":
            itemRectDelta = new Vector2(79,276);
            break;
            case "Grain Truck":
            itemRectDelta = new Vector2(96,360);
            break;
            case "Tractor with front loader":
            itemRectDelta = new Vector2(95,240);
            break;
            case "Grain Wagon":
            itemRectDelta = new Vector2(156,468);
            break;
            case "Tractor":
            itemRectDelta = new Vector2(86,150);
            break;
            case "Roll Up Door":
            itemRectDelta = new Vector2(72,5);
            break;
            case "Door 1":
            itemRectDelta = new Vector2(50,45);
            break;
            case "Lawn Mower":
            itemRectDelta = new Vector2(24,42);
            break;
            case "Sedan":
            itemRectDelta = new Vector2(65,192);
            break;
            case "ATV":
            itemRectDelta = new Vector2(48,83);
            break;
        }
        return itemRectDelta;
    }

    public Sprite GetDropSprite(PointerEventData data)
    {
        var originalObj = data.pointerDrag;
        if (originalObj == null)
            return null;

        var dragMe = originalObj.GetComponent<Drag>();
        if (dragMe == null)
            return null;

        var srcImage = originalObj.GetComponent<Image>();
        if (srcImage == null)
            return null;

        return srcImage.sprite;
    }
}