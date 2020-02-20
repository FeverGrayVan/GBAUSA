using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using Application.Config;
using System.Linq;
using Helpers.HTTP;

namespace Application.Loader.Http {

    public class Plans {

        private Dictionary<string, string> routes;

        public Plans() {
            routes = Routes.PlanRoutes;
        }

        public delegate void OnRequestComplete(string json);
        public delegate void OnRequestError(string message);

        public IEnumerator Get(String token, int plan_id, OnRequestComplete complete, OnRequestError error) {
            
            string url = routes.Single(u => u.Key == "plans").Value + '\\' + plan_id;
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + token);
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                error(request.error);
            } else {
                String json = request.downloadHandler.text;
                complete(json);
            }
        }

        public IEnumerator Delete(String token, int plan_id, OnRequestComplete complete, OnRequestError error) {
            
            string url = routes.Single(u => u.Key == "plans").Value + '\\' + plan_id;
            UnityWebRequest request = UnityWebRequest.Delete(url);
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + token);
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                error(request.error);
            } else {
                string good = "Success";
                //String json = request.downloadHandler.text;
                complete(good);
            }
        }

        public IEnumerator GetList(String token, OnRequestComplete complete, OnRequestError error) {

            string url = routes.Single(u => u.Key == "plans").Value;
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + token);
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                Debug.Log(request);
                error(request.error);
            } else {
                String json = request.downloadHandler.text;
                complete(json);
            }
        }

        public IEnumerator Save(String token, OnRequestComplete complete, OnRequestError error, PlanRequest plan) {

            string url = routes.Single(u => u.Key == "plans").Value;
            Debug.Log("plan name in kHttp = " + plan.name);
            Debug.Log("Plan ToJson = " + JsonUtility.ToJson(plan));
            Debug.Log("URL is = " + url);
            UnityWebRequest request = UnityWebRequest.Put(url, JsonUtility.ToJson(plan));
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + token);
            request.method = UnityWebRequest.kHttpVerbPOST;
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                Debug.Log("GOT A HTTP isHttpError = " + request.isHttpError);
                Debug.Log("GOT A HTTP isNetworkError = " + request.isNetworkError);
                error(request.error);
            } else {
                String json = request.downloadHandler.text;
                complete(json);
            }
        }
    }
}


