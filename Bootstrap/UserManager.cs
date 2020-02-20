using UnityEngine;
using System.Collections;
using Application.Models;
using System;

namespace Application.Loader {

    public class UserManager : MonoBehaviour {

        public static UserManager instance = null;
        public bool isLoggedIn = false;
        public User user = null;

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

        private void InitializeManager() {
            user = new User();
            //user.firstname = "default_user_name";
            //user.lastname = "default_user_surname";
        }

        public void Login(String _firstname, String _lastname, String _email, String _state, int _zip, String _phone, int _id) {
            user = new User {
                firstname = _firstname,
                lastname = _lastname,
                email = _email,
                state = _state,
                zip = _zip,
                phone = _phone,
                id = _id
            };
            isLoggedIn = true;
        }

        public void Logout() {
            user = null;
            isLoggedIn = false;
        }

    }

}


