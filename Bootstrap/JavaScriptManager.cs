using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class JavaScriptManager : MonoBehaviour {

    public static JavaScriptManager instance;

    [DllImport("__Internal")]
    public static extern void openWindowURL(string url);

    [DllImport("__Internal")]
    public static extern void openView();

    [DllImport("__Internal")]
    public static extern void cookieSet(string base64);

    [DllImport("__Internal")]
    public static extern void printView();

    [DllImport("__Internal")]
    public static extern void CopyPastePlugin(string gObj, string vName);

    private void Awake() 
    {
        if (instance != null) 
        {
            GameObject.Destroy(this.gameObject);
        } 
        else
        {
            instance = this;         
            DontDestroyOnLoad(this);
        }
    }
}
