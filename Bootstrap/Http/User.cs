using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace Application.Loader.Http {

    public class User {

        private Dictionary<string, string> routes;

        public delegate void OnRequestComplete();
        public delegate void OnRequestError(string message);

        public User() {

        }

        //moved to Auth.cs
        //DEPRECATED
        //public IEnumerator Me(string token, OnRequestComplete complete, OnRequestError error) {

        //    string url = String.Empty;

        //    UnityWebRequest request = UnityWebRequest.Get(url);
        //    request.SetRequestHeader("Accept", "application/json");
        //    request.SetRequestHeader("Authorization", "Bearer " + token);
        //    request.method = UnityWebRequest.kHttpVerbGET;
        //    yield return request.SendWebRequest();

        //    if (request.isNetworkError || request.isHttpError) {
        //        error(request.error);
        //    } else {
        //        try {
        //            String json = request.downloadHandler.text;
        //            UserResponse response = JsonUtility.FromJson<UserResponse>(json);
        //            if (response.success) {
        //                UserManager.instance.Login(response.data.firstname, response.data.lastname, response.data.email, response.data.state, response.data.zip, response.data.phone);
        //                complete();
        //            } else {
        //                error(response.error.message);
        //            }
        //        }
        //        catch (Exception ex) {
        //            error(ex.ToString());
        //        }
        //    }
        //}

    }

}

