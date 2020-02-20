using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers.HTTP;

namespace Application.Models {

    public class PlanDetails {

        public int id;
        public int plan_id;

        public float position_x;
        public float position_y;

        public float rotation_z;

        public float sDelta_x;
        public float sDelta_y;

        public string item_name;
        public string item_misc;

        public PlanDetails(float position_x, float position_y, float rotation_z, float sDelta_x, float sDelta_y, string item_name, string item_misc) 
        {
            this.position_x = position_x;
            this.position_y = position_y;

            this.rotation_z = rotation_z;

            this.sDelta_x = sDelta_x;
            this.sDelta_y = sDelta_y;

            this.item_name = item_name;
            this.item_misc = item_misc;

            Debug.Log("Message from the PlanDetails of "+item_name+" constructor (7 arguments)");
        }

        public PlanDetails(Helpers.HTTP.PlanDetails details) 
        {
            this.id = details.id;
            this.plan_id = details.plan_id;

            this.position_x = details.position_x;
            this.position_y = details.position_y;

            this.rotation_z = details.rotation_z;

            this.sDelta_x = details.sDelta_x;
            this.sDelta_y = details.sDelta_y;

            this.item_name = details.item_name;
            this.item_misc = details.item_misc;

            Debug.Log("Message from the PlanDetailsHTTP of "+details.item_name+" constructor (7 arguments)");
        }

        public ItemData ToItemData() 
        {
            ItemData data = new ItemData();

            Position dataPosition = new Position();
            SizeDelta dataSizeDelta = new SizeDelta();

            dataPosition.x = this.position_x;
            dataPosition.y = this.position_y;
            dataPosition.z = this.rotation_z;

            dataSizeDelta.x = this.sDelta_x;
            dataSizeDelta.y = this.sDelta_y;

            data.position = dataPosition;
            data.sDelta = dataSizeDelta;

            data.ItemName = this.item_name;
            data.ItemMisc = this.item_misc;
           
            Debug.Log("Message from the ItemData of "+data.ItemName+" constructor (ItemData)");

            return data;
        }
    }

}
