using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using Helpers.HTTP;
using Application.Models;
using System.Text;
using Application.Loader.Http;
using Application.Config;

namespace Application.Loader {

    public class HttpManager : MonoBehaviour 
    {
        public static HttpManager instance = null;

        public Auth auth;
        public Plans plans;
        public Share share;
        public String token;

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
            auth = new Auth();
            plans = new Plans();
            share = new Share();
        }
    }
}
