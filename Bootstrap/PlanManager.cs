using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Application.Models;

namespace Application.Loader {

    public class PlanManager : MonoBehaviour {

        public static PlanManager instance = null;
        public List<Garage> garagesTypes;
        public Garage currentGarage;
        public Plan currentPlan;

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

        public Garage SetCurrentGarage(string garageName) {
            currentGarage = garagesTypes.Single(garage => garage.name == garageName);
            return currentGarage;
        }
        public Garage SetCurrentGarage(string garageName,string key, float height, float width, bool monitor)
        {
            currentGarage = new Garage(width, height, garageName, key);
            
            return currentGarage;
        }

        private void InitializeManager() 
        {
            currentPlan = new Plan();
            garagesTypes = new List<Garage>();

            garagesTypes.Add(new Garage(20f, 18f, "Car Garage (18x20)", "One Car Garage"));
            garagesTypes.Add(new Garage(25f, 22f, "Car Garage (22x25)", "Two Car Garage"));
            garagesTypes.Add(new Garage(35f, 24f, "Car Garage (24x35)", "Three Car Garage"));
            garagesTypes.Add(new Garage(45f, 20f, "RV Garage (20x45)", "RV Garage"));
            garagesTypes.Add(new Garage(30f, 24f, "Garage/Workshop (24x30)", "Garage/Workshop"));
            garagesTypes.Add(new Garage(40f, 30f, "Garage/Carport Hybrid (30x40)", "Garage/Carport"));
            garagesTypes.Add(new Garage(60f, 30f, "Agricultural (30x60)", "Agricultural"));
            garagesTypes.Add(new Garage(25f, 24f, "Horse Barn (24x25)", "Horse Barn"));
            garagesTypes.Add(new Garage(50f, 36f, "Monitor Barn (36x50)", "Monitor Barn"));
            garagesTypes.Add(new Garage(80f, 40f, "Commercial/Industrial (40x80)", "Commercial/Industrial"));
            garagesTypes.Add(new Garage(30f, 22f, "Boat Storage (22x30)", "Boat Storage"));
        }
    }

    public class Garage {

        public float width;
        public float height;

        public string name;
        public string key;
        

        public Garage() {

        }

        public Garage(float width, float height, string name, string key) {
            this.width = width*1.2f;
            this.height = height*1.2f;

            this.name = name;
            this.key = key;

        }

    }

}


