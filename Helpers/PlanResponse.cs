using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Application.Models;

namespace Helpers.HTTP {

    [Serializable]
    public class PlanResponse {

        public int id;
        public int user_id;
        public string name;
        public string created_at;
        public string updated_at;
        public ItemData[] plan_details;

        public Plan ToPlan() 
        {
            return new Plan();
        }

        public PlanRequest ToPlanRequest() 
        {
            return new PlanRequest();
        }

    }

    [Serializable]
    public class PlanDetails 
    {
        public string created_at;
        public string updated_at;

        public int id;
        public int plan_id;

        public float position_x;
        public float position_y;

        public float rotation_z;

        public float sDelta_x;
        public float sDelta_y;

        public string item_name;
        public string item_misc;
    }
}

