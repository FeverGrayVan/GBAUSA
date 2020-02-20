using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComandManager : MonoBehaviour
{
    //public List<UndoObject> cmd;
    public Stack<UndoObject> cmd;//=new Stack<UndoObject>();
    public int count;

    public void Start()
    {        
        cmd = new Stack<UndoObject>();
        count = cmd.Count;
        print("List undo created");
    }
    public void Update()
    {
        //count = cmd.Count;
    }
    public void AddCmd(GameObject obj, Operation opt)
    {
        if (cmd != null)
        {
            print(obj.name+" "+opt);
        cmd.Push(new UndoObject(obj,opt));
        print("List undo adds creation of : "+obj.name);
        }
        
        //count = cmd.Count;
    }

    public void AddCmd(GameObject obj, Operation opt, Vector2 tr)
    {
        if (cmd != null)
        {
            print(obj.name+" "+opt);
        cmd.Push(new UndoObject(obj,opt,tr));
        print("List undo adds: "+obj.name);
        }
        
        //count = cmd.Count;
    }
    public void AddCmd(GameObject obj, Operation opt, float rot)
    {   
        if (cmd != null)
        {
            print(obj.name+" "+opt);
        cmd.Push(new UndoObject(obj,opt,rot));
        print("List undo adds rotation : "+obj.name);
        }
        
        //count = cmd.Count;
    }
    public void AddCmd(GameObject obj, Operation opt, Vector2 tr, Vector2 rect)
    {   
        if (cmd != null)
        {
                print(obj.name+" "+opt);
        cmd.Push(new UndoObject(obj,opt,tr,rect));
        print("List undo adds resize rect: "+obj.name);
        }
        
        //count = cmd.Count;
    }
    public void DeleteCmd(GameObject obj, Operation opt)
    {
        //UndoObject tmp = new UndoObject(obj,opt);
        //count = cmd.Count;
        //cmd.Pop();
    }

}
