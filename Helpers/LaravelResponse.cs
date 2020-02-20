using System;
using Application.Loader;

namespace Helpers.HTTP {

    [Serializable]
    public class LaravelResponse {

        public bool success;
        public LaravelError error;

        public LaravelResponse() {

        }
    }

    [Serializable]
    public class AuthResponse : LaravelResponse {

        public AuthData data;

        public AuthResponse() {

        }
    }

    [Serializable]
    public struct AuthData {
        public String access_token;
        public String token_type;
        public String expires_in;
    }

    [Serializable]
    public class UserResponse : LaravelResponse {

        public UserData data;

        public UserResponse() {

        }
    }

    [Serializable]
    public struct UserData {
        public int id;
        public string firstname;
        public string lastname;
        public string email;
        public string state;
        public int zip;
        public string phone;

        public string created_at;
        public string updated_at;
    }

    [Serializable]
    public class RegisterResponse : LaravelResponse {

        public RegisterData data;

        public RegisterResponse() {

        }

    }

    [Serializable]
    public struct RegisterData {
        public UserData user;
        public String token;
    }

    [Serializable]
    public class LogoutResponse : LaravelResponse {
        public string data;
    }

    [Serializable]
    public class LaravelError {

        public string message;
        public string file;
        public string line;
        public string code;

    }




}
