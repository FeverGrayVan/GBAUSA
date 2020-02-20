using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Application.Models;

namespace Helpers.HTTP {

    [Serializable]
    public class PlanRequest
    {
        public ItemData[] plan_details;
        public string name;
    }

    [Serializable]
    public class ItemData 
    {
        public Position position;
        public SizeDelta sDelta;
        public string ItemName;
        public string ItemMisc;
        
        public ItemData() 
        {
            position = new Position();
            sDelta = new SizeDelta();
            ItemName = null;
            ItemMisc = null;
        }
    }

    [Serializable]
    public class Position 
    {
        public float x;
        public float y;
        public float z;

        public Position() 
        {
            x = 0f;
            y = 0f;
            z = 0f;
        }
    }

    [Serializable]
    public class SizeDelta 
    {
        public float x;
        public float y;

        public SizeDelta() 
        {
            x = 0f;
            y = 0f;
        }
    }


    // [Serializable]
    // public class Rotation {
    //     public float x;
    //     public float y;
    //     public float z;

    //     public Rotation() {
    //         x = 0f;
    //         y = 0f;
    //         z = 0f;
    //     }
    // }

    

    // [Serializable]
    // public class RectT {
    //     public float x = 227f;
    //     public float y = 227f;

    //     public RectT() {
    //         x = 228f;
    //         y = 228f;
    //     }
    // }

}