using System;
using Helpers.HTTP;
using System.Collections.Generic;
using UnityEngine;
using Application.Loader;
using System.Linq;

namespace Application.Models
{
    public class Plan
    {
        public string plan_name;
        public int plan_id;
        public int user_id;
        public List<PlanDetails> details;
        public bool isSaved;
        public Vector3 areaScale;


        public string testing;

        public Plan()
        {
            this.details = new List<PlanDetails>();
            this.isSaved = false;
        }

        public Plan(string name, int plan_id, int user_id)
        {
            this.plan_id = plan_id;
            this.plan_name = name;
            this.user_id = user_id;
            details = new List<PlanDetails>();
            isSaved = false;
        }

        public Plan(string name, int plan_id, int user_id, Helpers.HTTP.PlanDetails[] details)
        {
            this.plan_id = plan_id;
            this.plan_name = name;
            this.user_id = user_id;
            this.details = new List<PlanDetails>();
            isSaved = false;

            FillDetails(details);
        }

        public void FromResponse(PlanResponse planResponse)
        {
            this.plan_id = planResponse.id;
            this.user_id = planResponse.user_id;
            this.plan_name = planResponse.name;
            details = new List<PlanDetails>();
            //isSaved = false;
           
            foreach(PlanDetails item in details){
                 Debug.Log("Details dance = ");
            }
            //FillDetails(planResponse.plan_details);
        }

        public void FillDetails(string name, int plan_id, int user_id, Helpers.HTTP.PlanDetails[] details)
        {
            this.plan_id = plan_id;
            this.plan_name = name;
            this.user_id = user_id;
            this.details = new List<PlanDetails>();
            isSaved = false;

            Debug.Log("DETAILS = " + this.details);

            FillDetails(details);
        }

        public void FillDetails(Helpers.HTTP.PlanDetails[] details)
        {

            this.details = new List<PlanDetails>();
            for (int i = 0; i < details.Length; i++)
            {
                this.details.Add(new PlanDetails(details[i]));
            }
        }

        public void AppendDetails(PlanDetails details)
        {
            Debug.Log("I CAUGHT "+details.item_name);
            this.details.Add(details);
        }

        public void SetScale(Vector3 scale)
        {
            Debug.Log("SETSCALE INSTANTIATED . SCALE = " + scale);
                this.areaScale = scale;
                Debug.Log("THIS AREA SCALE = " + this.areaScale);
                this.testing = ("oh im saving yeah");
                
        }

        ////todo: Move to plan controller
        //[Obsolete("Save_v2 is deprecated, movde to PlanController.")]
        //public void Save_v2(string plan_name) {
        //    if (!isSaved) {
        //        this.plan_name = plan_name;
        //        PlanRequset plan = new PlanRequset {
        //            plan_details = new ItemData[details.Count],
        //            name = this.plan_name
        //        };

        //        foreach (PlanDetails det in details) {
        //            plan.plan_details.Append(det.ToItemData());
        //        }

        //        StartCoroutine(HttpManager.instance.plans.Save(HttpManager.instance.token,
        //        (string json) => {
        //            UnityEngine.Debug.Log(json);
        //        },
        //        (string error) => {
        //            //
        //        }, plan));
        //    }
        //}


        ////todo: move to plan controller
        //[Obsolete("Save is deprecated, please use Save_v2 instead.")]
        //public void Save() {

        //    List<GameObject> items = WorkingAreaModel.droppedItems;
        //    PlanRequset plan = new PlanRequset {
        //        plan_details = new ItemData[items.Count],
        //        name = "Temp_name"
        //    };

        //    if (items.Count > 0) {
        //        int i = 0;
        //        foreach (GameObject item in items) {

        //            plan.plan_details[i] = new ItemData();

        //            plan.plan_details[i].name = item.transform.name;
        //            plan.plan_details[i].id = 1;
        //            plan.plan_details[i].position.x = item.transform.position.x;
        //            plan.plan_details[i].position.y = item.transform.position.y;

        //            plan.plan_details[i].rotation.x = item.transform.rotation.x;
        //            plan.plan_details[i].rotation.y = item.transform.rotation.y;
        //            plan.plan_details[i].rotation.z = item.transform.rotation.z;

        //            plan.plan_details[i].scale.x = item.transform.localScale.x;
        //            plan.plan_details[i].scale.y = item.transform.localScale.y;

        //            i++;
        //        }

        //        UnityEngine.Debug.Log(HttpManager.instance.token);

        //        StartCoroutine(HttpManager.instance.plans.Save(HttpManager.instance.token,
        //        (string json) => {
        //            //
        //        },
        //        (string error) => {
        //            //
        //        }, plan));
        //        //return new Plan();
        //    } else {
        //        //return new Plan();
        //    }

        //}
    }
}