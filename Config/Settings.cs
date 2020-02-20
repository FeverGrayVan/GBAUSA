using System;

namespace Application.Config {

    public static class Settings {

        public static Boolean LOCAL = false;

        private const String DEBUG_BASE_URL = "https://localhost:8001/api/";
        private const String PRODUCTION_BASE_URL = "https://garage2d-api.project-release.info/api/";
        //private const String PRODUCTION_BASE_URL = "https://garage2d-api.project-release.info/api/";
        private const String PROD_URL_ASSETS = "https://gba-usa.project-release.info/garage-2d/";
        //"https://localhost/wp-content/themes/gba-usa/garage/"; //"https://garage2d.project-release.info/";

        public static String ASSET_URL {
            get {
                return PROD_URL_ASSETS;
            }
        }

        public static String BASE_URL {
            get {
                if (LOCAL) {
                    return DEBUG_BASE_URL;
                } else {
                    return PRODUCTION_BASE_URL;
                }
            }
        }

    }

}
