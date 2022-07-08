using System;
using System.Collections.Generic;
using System.Text;

namespace TeisterMask
{
    public class Constants
    {
        //Employee
        public const int USERNAME_MAX_LENGTH = 40;
        public const int USERNAME_MIN_LENGTH = 3;
        public const string PHONE_REGEX = @"^\d{3}\-\d{3}\-\d{4}$";
        public const string USERNAME_REGEX = @"^[A-Za-z0-9]{3,}$";
        public const int PHONENUMBER_MAX_LENGTH = 12;
        
        
        public const int NAME_MAX_LENGTH = 40;
        public const int NAME_MIN_LENGTH = 2;
    }
}
