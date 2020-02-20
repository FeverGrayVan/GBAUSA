using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application.Loader;
using UnityEngine.UI;
using System.Runtime.InteropServices;
//using System.Drawing.Printing;

public class PrintController : MonoBehaviour
{

    public GameObject mainGameObject;
    private Texture2D mainTex;
    public Image mainImage;
    public Image imgTest;
    public Transform area;


    public void PrintBtnClicks()
    {
        if (UserManager.instance.isLoggedIn)
        {
            StartCoroutine("Screen");
        }
        else
        {
            ModalMagaer.instance.OpenAuth();
            mainGameObject.SetActive(false);
        }
    }
    public void OpenPrint()
    {
        
    }
    public IEnumerator Screen()
    {       
        yield return new WaitForEndOfFrame();
        mainGameObject.SetActive(false);
        var tmp = GameObject.Instantiate(area.gameObject);
        tmp.transform.SetParent(imgTest.transform);
        tmp.transform.localScale = new Vector3(1, 1, 1);
        tmp.transform.localPosition = new Vector3(0, 0, 0);
        imgTest.gameObject.SetActive(true);

        mainTex = ScreenCapture.CaptureScreenshotAsTexture(2);
        mainTex.name = "ScreenShot";

        Sprite sp = Sprite.Create(mainTex, new Rect(0, 0, mainTex.width, mainTex.height), new Vector2(0, 0));
        imgTest.sprite = sp;

        var childs = imgTest.gameObject.GetComponentsInChildren<Transform>();
        if (childs.Length != 0)
        {
            foreach (Transform item in childs)
            {
                //print(item.gameObject.name);
                if (item.gameObject.name != "TestImage")
                    Destroy(item.gameObject);
            }
        }

        mainImage.sprite = sp;
        imgTest.gameObject.SetActive(false);
        mainGameObject.SetActive(true);
    }

    public void Cancel_onClick() {
        mainGameObject.SetActive(false);
    }

}
