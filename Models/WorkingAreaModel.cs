using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application.Models;
using Application.Loader;

public class WorkingAreaModel : MonoBehaviour {

    public static List<GameObject> droppedItems;

    public void Start() {
        droppedItems = new List<GameObject>();

    }

    public static void RemoveItem(GameObject item) => droppedItems.Remove(item);

    public static void AppendItem(GameObject item) => droppedItems.Add(item);

    public static int ItemsCount() => droppedItems.Count;

    public void OnMouseDown()
    {
        //Debug.Log("Go go WorkingAreaModel");
    }
}
