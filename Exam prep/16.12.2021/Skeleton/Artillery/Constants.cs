using System;
using System.Collections.Generic;
using System.Text;

namespace Artillery
{
    public class Constants
    {
        //Country class
        public const int COUNTRY_NAME_MIN_LENGTH = 4;
        public const int COUNTRY_NAME_MAX_LENGTH = 60;
        public const int ARMY_SIZE_MAX_VALUE = 10_000_000;
        public const int ARMY_SIZE_MIN_VALUE = 50_000;

        //Manufacturer class
        public const int MANIFACTURER_NAME_MIN_LENGTH = 4;
        public const int MANIFACTURER_NAME_MAX_LENGTH = 40;
        public const int FOUNDED_MIN_LENGTH = 10;
        public const int FOUNDED_MAX_LENGTH = 100;

        //Shell class
        public const int CALIBER_MIN_LENGTH = 4;
        public const int CALIBER_MAX_LENGTH = 30;
        public const double SHELL_MIN_WEIGHT = 2;
        public const double SHELL_MAX_WEIGHT = 1_680;

        //Gun
        public const int GUN_MIN_WEIGHT = 100;
        public const int GUN_MAX_WEiGHT = 1_350_000;
        public const double BARREL_MIN_LENGTH = 2;
        public const double BARREL_MAX_LENGTH = 35;
        public const int MIN_RANGE = 1;
        public const int MAX_RANGE = 100_000;
    }
}
