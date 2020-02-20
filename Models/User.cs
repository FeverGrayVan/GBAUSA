using System;

namespace Application.Models {


    public class User {

        public string firstname;
        public string lastname;
        public string email;
        public string state;
        public int zip;
        public string phone;
        public int id;

        public User() {

        }

        public User(String _firstname, String _lastname, String _email, String _state, int _zip, String _phone, int _id) {
            firstname = _firstname;
            lastname = _lastname;
            email = _email;
            state = _state;
            zip = _zip;
            phone = _phone;
            id = _id;
        }



    }
}


