using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application.Models;
using UnityEngine.UI;
using System.Linq;
using Application.Loader;

public class ObjectModel : Model
{
    public bool active = false;
    public string image;
    public Texture2D texture;
    public Vector2 size;
    public Image ImageItem;

    public ObjectModel(string image)
    {
        this.image = image;
    }

    public void Disactivate()
    {
        GameObject item = gameObject.transform.Find("Obj").gameObject;
        ObjectSettings o_sett = gameObject.GetComponent<ObjectSettings>();
        GameObject orig_item = o_sett.original_object;
        orig_item.SetActive(true);
        orig_item.transform.position = item.transform.position;
        orig_item.transform.rotation = item.transform.rotation;
        orig_item.transform.localScale = gameObject.transform.localScale;
        orig_item.GetComponent<RectTransform>().sizeDelta = item.GetComponent<RectTransform>().sizeDelta;
        orig_item.GetComponent<ObjectModel>().active = false;
        orig_item.GetComponent<ObjectModel>().size = item.GetComponent<RectTransform>().sizeDelta;
        gameObject.SetActive(false);
        Destroy(gameObject);
        UpdateModel(orig_item);
    }

    public void Activate()
    {
        GameObject objSet = Instantiate(Resources.Load("Prefabs/ObjSet")) as GameObject;
        ObjectSettings oset = objSet.GetComponent<ObjectSettings>();
        ObjectModel o_model = objSet.AddComponent<ObjectModel>();
        o_model.active = true;
        o_model.texture = texture;
        objSet.transform.SetParent(gameObject.transform.parent);
        objSet.transform.position = gameObject.transform.position;
        objSet.transform.rotation = gameObject.transform.rotation;

        objSet.GetComponent<RectTransform>().sizeDelta = new Vector2(this.size.x + 150, this.size.y + 125);
        objSet.transform.localScale = gameObject.transform.localScale;

        objSet.name = gameObject.name + "_sett";
        oset.o_name = gameObject.name + "_sett";
        oset.original_object = gameObject;
        oset.texture = texture;
        oset.UpdateSprite(gameObject.transform.localScale, this.size);
        gameObject.SetActive(false);
        active = true;
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
            "ObjectItemToPlan");
    }

    public static void DisactivateAll()
    {
        List<GameObject> items = WorkingAreaModel.droppedItems;
        foreach (GameObject item in items)
        {
            if (item.GetComponent<ObjectModel>().active)
            {
                item.GetComponent<ObjectModel>().Disactivate();
            }
        }
    }

    public void UpdateModel(GameObject original)
    {
        try
        {
            PlanDetails detailsToUpdate =
                PlanManager.instance.currentPlan.details.FirstOrDefault(detail => detail.item_name == gameObject.name);
            detailsToUpdate.position_x = original.transform.position.x;
            detailsToUpdate.position_y = original.transform.position.y;
            detailsToUpdate.rotation_z = original.transform.rotation.z;
            detailsToUpdate.sDelta_x = original.GetComponent<RectTransform>().sizeDelta.x;
            detailsToUpdate.sDelta_y = original.GetComponent<RectTransform>().sizeDelta.y;
            //detailsToUpdate.img_width = 300;
            //detailsToUpdate.img_width = 300;
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
}