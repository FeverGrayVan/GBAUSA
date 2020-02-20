using UnityEngine;
using System.Collections;

public interface IComand
{
    void Execute();
    void UnExecute();
    
}
public enum Operation
{
    Drag,
    Resize,
    Rotate,
    Delete,
    None,
    Create
}
public class UndoObject
{
    GameObject item;
    Operation opt;
    Vector2 trans;
    Vector2 rect;
    float rot;

    public UndoObject()
    {
        Item = new GameObject();
        Opt = Operation.None;
    }
    public UndoObject(GameObject obj, Operation op)
    {
        Item = obj;
        Opt = op;
        
    }
    public UndoObject(GameObject obj, Operation op,float rot)
    {
        Item = obj;
        Opt = op;
        Rot = rot;
    }
    public UndoObject(GameObject obj, Operation op,Vector2 tr)
    {
        Item = obj;
        Opt = op;
        Trans = tr;
    }
    public UndoObject(GameObject obj, Operation op,Vector2 tr, Vector2 rect)
    {
        Item = obj;
        Opt = op;
        Trans = tr;
        Rect = rect; 
    }

    public GameObject Item { get => item; set => item = value; }
    public Operation Opt { get => opt; set => opt = value; }
    public Vector2 Trans { get => trans; set => trans = value; }

    public Vector2 Rect { get => rect; set => rect = value; }
    public float Rot { get => rot; set => rot = value; }
}
