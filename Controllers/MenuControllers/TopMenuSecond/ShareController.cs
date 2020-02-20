using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application.Loader;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;




public class ShareController : MonoBehaviour {

    public System.Net.Mime.TransferEncoding TransferEncoding { get; set; }
    public GameObject toolTip;
    public Texture2D mainTex;
    public GameObject area;
    public Image imgTest;

    public void OpenSharePopup() {
        ModalMagaer.instance.OpenShare();
    }
    public void GetURL()
    {
        var textMsg = GameObject.Find("textMsg:InputField").GetComponent<InputField>();
        textMsg.text = GameObject.Find("TopMenu").GetComponent<TopMenuController>().url_help;
    }
    
    public void Share() {
        if (UserManager.instance.isLoggedIn)
        {
            toolTip = Resources.Load<GameObject>("Prefabs/ToolTipPref");
            var email = GameObject.Find("emailShare:InputField").GetComponent<InputField>();
            var textMsg = GameObject.Find("textMsg:InputField").GetComponent<InputField>();
            try
            {
                if (email.text == "")
                {
                    toolTip.GetComponent<ToolTipController>().setMessage("Please write Email");
                    GameObject.Instantiate(toolTip);
                }
                else
                {
                    var form = UserManager.instance;
                    
                   
                    MailAddress from = new MailAddress("garage2d.share.gba.usa@gmail.com", "GBA" );  //form.user.firstname);
                    MailAddress to = new MailAddress(email.text);
                    MailMessage m = new MailMessage(from, to);
                    m.Subject = "GBA USA - 2D Planner:EmailNotify";
                    //m.BodyEncoding
                    m.IsBodyHtml = true;
                    m.Body = GameObject.Find("TopMenu").GetComponent<TopMenuController>().url_help;
                    //m.Body = textMsg.text;
                    
                    //Attachment att = new Attachment(((System.IO.Directory.GetCurrentDirectory() + "/ScreenshotCache/Screenshot.png")),"image/png");
                    //m.Attachments.Add(att);
                    //Attachment att = new Attachment(GameObject.Find("TopMenu").GetComponent<TopMenuController>().url_help);
                    //att.TransferEncoding = TransferEncoding.;
                    //m.Attachments.Add(att);

                    SmtpClient smtp = new SmtpClient("smtp.gmail.com",587);
                    
                    smtp.EnableSsl = true;
                    //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtp.UseDefaultCredentials = false;
                    //Debug.Log(m);
                    smtp.Credentials = new System.Net.NetworkCredential("garage2d.share.gba.usa@gmail.com", "godblessUSA2019") as ICredentialsByHost;
                    ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors){
                        return true;
                    };
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    //smtp.Timeout = 10;
                    //Debug.Log(smtp.Exception);
                //Debug.Log(to);
                //Debug.Log(smtp.Credentials);
                    smtp.Send(m);

                }
            }
            catch (System.Exception ex)
            {
                
                print(ex.Message);
                
            }
        }
        else
            ModalMagaer.instance.OpenAuth();
        //StartCoroutine(HttpManager.instance.share.GetToken(
        //    PlanManager.instance.currentPlan.plan_id,
        //   HttpManager.instance.token,
        //        (string json) => {
        //            Debug.Log(json);
        //        },
        //        (string error) => {
        //            Debug.Log(error);
        //        }
        //        ));
        //Close();
    }

    public void ExportToJpg()
    {
        area = GameObject.Find("Work Area");
        var tmp = GameObject.Instantiate(area.gameObject);
        tmp.transform.SetParent(imgTest.transform);
        tmp.transform.localScale = new Vector3(1, 1, 1);
        tmp.transform.localPosition = new Vector3(0, 0, 0);
        imgTest.gameObject.SetActive(true);

        print("ScreenShot");
        StartCoroutine("TakeScreen");

    }

    public void ScreenShotTest()
    {
        //Debug.Log((System.IO.Directory.GetCurrentDirectory() + "/ScreenshotCache/Screenshot.png"));
        //ScreenCapture.CaptureScreenshot((System.IO.Directory.GetCurrentDirectory() + "/ScreenshotCache/Screenshot.png"));
        //public_html/wp-content/themes/gba-usa/garage/ScreenshotCache
        ScreenCapture.CaptureScreenshot("public_html/wp-content/themes/gba-usa/garage/ScreenshotCache/Screenshot.png");

    }

    



    
    public IEnumerator TakeScreen()
    {
        yield return new WaitForEndOfFrame();
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
        
        byte[] textureBytes = mainTex.EncodeToJPG();
        //string encodedText = System.Convert.ToBase64String(textureBytes);
        
        
     
        

    }

    public void Close() {
        Destroy(GameObject.Find("SharePopup(Clone)"));
    }

}
