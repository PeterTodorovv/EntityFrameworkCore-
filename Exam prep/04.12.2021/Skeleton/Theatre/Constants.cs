using System;
using System.Collections.Generic;
using System.Text;

namespace Theatre
{
    public class Constants
    {
        //Theatre
        public  const int TheatreNameMinLength = 4;
        public  const int TheatreNameMaxLength = 30;

        public const sbyte MinNumberOfHalls = 1;
        public const sbyte MaxNumberOfHalls = 10;

        public const int DirectorMinLength = 4;
        public const int DirectorMaxLength = 30;

        //Play
        public const int TitleMinLength = 4;
        public const int TitleMaxLength = 50;

        public const double MinRating = 0;
        public const double MaxRating = 10;

        public const int DescriptionMaxLength = 700;

        public const int ScreenWriterMinLength = 4;
        public const int ScreenWriterMaxLength = 30;

        //Cast
        public const int CastNameMinLength = 4;
        public const int CastNameMaxLength = 30;

        public const string PhoneNumberRegex = @"^\+44-\d{2}-\d{3}-\d{4}";

        //Ticket
        public const double MinPrice = 1;
        public const double MaxPrice = 100;

        public const sbyte MinRowNumber = 1;
        public const sbyte MaxRowNumber = 10;

    }
}
