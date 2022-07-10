using System;
using System.Collections.Generic;
using System.Text;

namespace TeisterMask
{
    public class Constants
    {

        //Employee
        public const int UsernameMaxLength = 40;
        public const int UsernameMinLength = 3;
        public const string UsernameRegex = @"^[A-z|\d]+$";
        public const string PhoneRegex = @"\d{3}-\d{3}-\d{4}";

        //Project
        public const int NameMaxLength = 40;
        public const int NameMinLength = 2;

    }
}
