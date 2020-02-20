using System;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers {

    public class Validation {
        /**
         * 
         * Rules
         * rules value ->
         * Array
         *         
         * length
         * type(text, numeric, any)
         * 
         * */
        private Dictionary<String, String[]> _rules;

        public Validation() {
            _rules = null;
        }

        public Validation(Dictionary<String, String[]> rules) {
            _rules = rules;
        }

        public Error Validate(Dictionary<String, String> validatable) {
            if ((_rules.Count == 0) || (_rules == null)) {
                return new Error("Rules not set", false);
            } else {
                foreach (var field in validatable) {
                    String[] rule = _rules[field.Key];
                    int rLength = Int32.Parse(rule[0]);
                    String type = rule[1];
                    if (field.Value.Length < rLength) {
                        return new Error(field.Key + " length must be at least " + rLength, false);
                    }
                    //todo: types
                }
                return new Error("", true);
            }
        }

    }

    public class Error {

        private String _message;
        public String Message {
            get {
                return _message;
            }

            set {
                _message = value;
            }
        }

        private Boolean _success;
        public Boolean Success {
            get {
                return _success;
            }
            set {
                _success = value;
            }
        }

        public Error() {
            _message = String.Empty;
            _success = false;
        }

        public Error(String message, Boolean success) {
            _message = message;
            _success = success;
        }
    }

    public static class JsonHelper {

        public static T[] FromJson<T>(string json) {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array) {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint) {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T> {
            public T[] Items;
        }

    }
}
