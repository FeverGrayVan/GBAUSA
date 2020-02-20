using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace Application.Loader {

    public class ModalMagaer : MonoBehaviour 
    {
        public static ModalMagaer instance;

        private string authPrefab;
        private string printPrefab;
        private string sharePrefab;
        private string savePrefab;

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
                InitializeManager();
            }
        }

        private void InitializeManager() 
        {
            authPrefab = "Prefabs/AuthModal";
            printPrefab = "Prefabs/PrintPopup";
            sharePrefab = "Prefabs/SharePopup";
            savePrefab = "Prefabs/SavePopup";
        }

        private void loadPopup(String prefName) 
        {
            GameObject popup = Resources.Load(prefName) as GameObject;
            if (popup == null) 
            {
                Debug.Log("Error loading prefab");
            } 
            else 
            {
                GameObject popupCloned = Instantiate(popup, new Vector3(100, 100, 2), Quaternion.identity.normalized);
                popupCloned.transform.SetParent(GameObject.Find("Canvas").transform);
                popupCloned.transform.localPosition = new Vector3(0, 0, 2);
            }
        }

        public void OpenAuth() => loadPopup(authPrefab);
        public void OpenPrint() => loadPopup(printPrefab);
        public void OpenShare() => loadPopup(sharePrefab);
        public void OpenSave() => loadPopup(savePrefab);

    }

}


