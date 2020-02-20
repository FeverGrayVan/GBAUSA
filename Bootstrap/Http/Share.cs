using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application.Config;
using System.Linq;
using UnityEngine.Networking;
using System;

namespace Application.Loader.Http {

    public class Share {

        private Dictionary<string, string> routes;

        public Share() {
            routes = Routes.ShareRoutes;
        }

        public delegate void OnRequestComplete(string response);
        public delegate void OnRequestError(string response);

        public IEnumerator GetToken(int plan_id, string auth_token, OnRequestComplete complete, OnRequestError error)
        {
            string url = routes.Single(u => u.Key == "share").Value + plan_id.ToString();
            //string url = routes.Single(route => route.Key == "share").Value + plan_id.ToString();

            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + auth_token);
            request.method = UnityWebRequest.kHttpVerbGET;

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                error(request.error);
            } else {
                try {
                    String json = request.downloadHandler.text;
                    Debug.Log(json);
                    //Share response = JsonUtility.FromJson<UserResponse>(json);
                    //if (response.success) {
                    //    UserManager.instance.Login(response.data.firstname, response.data.lastname, response.data.email, response.data.state, response.data.zip, response.data.phone);
                        //complete();
                    //} else {
                        //error(response.error.message);
                    //}
                }
                catch (Exception ex) {
                    error(ex.ToString());
                }
            }
        }

        //public IEnumerator CheckToken(string token) {
        //    string url = routes.Single(route => route.Key == "openShare").Value + token;
        //}

    }

}
