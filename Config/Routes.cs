using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Application.Config {

    public static class Routes {

        public static Dictionary<string, string> AuthRoutes = new Dictionary<string, string>() {

            {
                "login",
                Settings.BASE_URL + "2d/auth/login"
            },
            {
                "register",
                Settings.BASE_URL + "2d/auth/register"
            },
            {
                "logout",
                Settings.BASE_URL + "2d/auth/logout"
            },
            {
                "me",
                Settings.BASE_URL + "2d/auth/me"
            }

        };

        /*
         * RESOURCE API      
         */
        public static Dictionary<string, string> PlanRoutes = new Dictionary<string, string>() {

            {
                "plans",
                Settings.BASE_URL + "2d/plans"
            }

        };

        /*
         * RESOURCE API
         */
        public static Dictionary<string, string> UserRoutes = new Dictionary<string, string>() {

            { "me", "2d/auth/me" }

        };

        public static Dictionary<string, string> ShareRoutes = new Dictionary<string, string>() {

            { "share", Settings.BASE_URL+"2d/plans/share/" },
            { "openShare", Settings.BASE_URL+"2d/plans/openShare/"}

        };

    }

}


