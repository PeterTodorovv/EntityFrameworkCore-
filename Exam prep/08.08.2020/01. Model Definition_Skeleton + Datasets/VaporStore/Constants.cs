using System;
using System.Collections.Generic;
using System.Text;

namespace VaporStore
{
    public class Constants
    {
        //User
        public const int Username_max_length = 20;
        public const int Username_min_length = 3;
        public const int Age_max_value = 103;
        public const int Age_min_value = 3;
        public const string FullName_regex = @"^[A-Z]{1}[a-z]+\s[A-Z]{1}[a-z]+$";

        //Card
        public const int Cvc_length = 3;
        public const string Card_number_regex = @"^\d{4}\s\d{4}\s\d{4}\s\d{4}$"; 

        //Game import
        public const string Error_message = "Invalid Data";

        //Purchase
        public const string Key_regex = @"^[A-Z|\d]{4}-[A-Z|\d]{4}-[A-Z|\d]{4}$";
    }
}
