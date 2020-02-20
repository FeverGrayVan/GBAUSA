using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Helpers.HTTP;
using Application.Config;
using System.Linq;

namespace Application.Loader.Http {

    public class Auth {

        private Dictionary<string, string> routes;

        public Auth() {
            routes = Routes.AuthRoutes;
        }

        public delegate void OnRequestComplete();
        public delegate void OnRequestError(string message);

        public IEnumerator SignIn(string email, string password, OnRequestComplete complete, OnRequestError error) {

            string url = routes.Single(route => route.Key == "login").Value;
            WWWForm data = new WWWForm();
            data.AddField("email", email);
            data.AddField("password", password);

            UnityWebRequest request = UnityWebRequest.Post(url, data);
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                error(request.error);
            } else {
                try {
                    String json = request.downloadHandler.text;
                    AuthResponse response = JsonUtility.FromJson<AuthResponse>(json);
                    if (response.success) {
                        HttpManager.instance.token = response.data.access_token;
                        complete();
                    } else {
                        error(response.error.message);
                    }
                }
                catch (Exception ex) {
                    error(ex.ToString());
                }
            }
        }

        public IEnumerator Logout(string token, OnRequestComplete complete, OnRequestError error) {

            string url = routes.Single(route => route.Key == "logout").Value;

            UnityWebRequest request = UnityWebRequest.Post(url, "");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + token);
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                error(request.error);
            } else {
                try {
                    String json = request.downloadHandler.text;
                    LogoutResponse response = JsonUtility.FromJson<LogoutResponse>(json);
                    if (response.success) {
                        complete();
                    } else {
                        error(response.error.message);
                    }
                }
                catch (Exception ex) {
                    error(ex.ToString());
                }
            }
        }

        public IEnumerator Me(string token, OnRequestComplete complete, OnRequestError error) {

            string url = routes.Single(route => route.Key == "me").Value;
            Debug.Log("ME AUTH TOKE = " + token );
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + token);
            Debug.Log("Auth token = " + token);
            request.method = UnityWebRequest.kHttpVerbGET;
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                error(request.error);
            } else {
                try {
                    String json = request.downloadHandler.text;
                    Debug.Log("JsonResponseLogin =" + json);
                    UserResponse response = JsonUtility.FromJson<UserResponse>(json);
                    if (response.success) {
                        UserManager.instance.Login(response.data.firstname, response.data.lastname, response.data.email, response.data.state, response.data.zip, response.data.phone, response.data.id);
                        complete();
                    } else {
                        error(response.error.message);
                    }
                }
                catch (Exception ex) {
                    error(ex.ToString());
                }
            }
        }

        public IEnumerator Register(string email, string password,
            string firstname, string lastname, string state, int zip,
            string phone, OnRequestComplete complete, OnRequestError error)
        {

            string url = routes.Single(route => route.Key == "register").Value;

            WWWForm regData = new WWWForm();
            regData.AddField("email", email);
            regData.AddField("password", password);
            regData.AddField("firstname", firstname);
            regData.AddField("lastname", lastname);
            regData.AddField("state", state);
            regData.AddField("zip", zip);
            regData.AddField("phone", phone);

            UnityWebRequest request = UnityWebRequest.Post(url, regData);
            request.SetRequestHeader("Accept", "application/json");
            //request.SetRequestHeader("Content-Type", "application/json");
            //request.method = UnityWebRequest.kHttpVerbPOST;
            Debug.Log(request);
            Debug.Log("url = " + url);
            Debug.Log("regData = " + regData);
            //request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();
            //String json = request.downloadHandler.data;
            //Debug.Log(request.downloadHandler.text);

            if (request.isNetworkError || request.isHttpError)
            {
                error(request.error);
                error(request.downloadHandler.text);
            }
            else
            {
                try
                {
                    String json = request.downloadHandler.text;
                    Debug.Log("Register Repsonse JSON = " +json);
                    RegisterResponse response = JsonUtility.FromJson<RegisterResponse>(json);
                    if (response.success) {
                        UserManager.instance.Login(
                            response.data.user.firstname,
                            response.data.user.lastname, 
                            response.data.user.email, 
                            response.data.user.state, 
                            response.data.user.zip, 
                            response.data.user.phone, 
                            response.data.user.id);
                        HttpManager.instance.token = response.data.token;
                        complete();
                    } else {
                        error(json);
                        //error(response.error.message);
                    }
                }
                catch (Exception ex) {
                    Debug.Log("Found exception ");// + ex);
                    error(ex.ToString());
                }
            }

        }


    }

}


