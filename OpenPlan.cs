using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Application.Loader;
using System;
using System.Runtime.InteropServices;

public class OpenPlan : MonoBehaviour
{
    public Dropdown preListGarage, postListGarage;
    public InputField width, height;
    public Sprite[] sprites = new Sprite[11];
    public Image showImage;
    public bool monitor = false;
    public void Start()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    public void ShowImage()
    {
        var num = preListGarage.value;
        showImage.sprite = sprites[num];
    }
    public void OpenWorkScene(int sceneIndex) {
        
        int indexValue = preListGarage.value;
        string valueDD = preListGarage.options[indexValue].text;
        PlanManager.instance.SetCurrentGarage(valueDD);
        SceneManager.LoadScene(sceneIndex);
    }
    public void OpenNewCreatePlane(int index)
    {
        if (postListGarage.options[postListGarage.value].text == "Monitor Barn")
        {
            
            monitor = true;
        }
        PlanManager.instance.SetCurrentGarage(
            "New Plane_" + postListGarage.options[postListGarage.value].text + "_" + height.text + "x" + width.text,
            postListGarage.options[postListGarage.value].text,
            Convert.ToSingle(width.text),
            Convert.ToSingle(height.text),
            monitor
            );       

        SceneManager.LoadScene(index);
    }
    
    void Update()
    {
        LimitInput(width);
        LimitInput(height);
    }

    public void LimitInput(InputField field)
    {
        float inputValue = Convert.ToSingle(field.text);
        if (inputValue > 100) { inputValue = 100;}
        field.text = inputValue.ToString();
    }
}
