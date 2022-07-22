namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Play> plays = new List<Play>();

            XmlRootAttribute root = new XmlRootAttribute("Plays");
            XmlSerializer serializer = new XmlSerializer(typeof(PlayImportDto[]), root);

            StringReader reader = new StringReader(xmlString);

            PlayImportDto[] playImports = (PlayImportDto[])serializer.Deserialize(reader);

            foreach (var playDto in playImports)
            {
                if (!IsValid(playDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isValidDuration = TimeSpan.TryParseExact(playDto.Duration, @"hh\:mm\:ss"
                    , CultureInfo.InvariantCulture,  out TimeSpan Duration);

                if (!isValidDuration)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(Duration.TotalHours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Genre genre = new Genre();
                bool isValidEnum = Enum.TryParse(typeof(Genre), playDto.Genre, out object p);

                if (!isValidEnum)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play play = new Play()
                {
                    Title = playDto.Title,
                    Duration = Duration,
                    Rating = playDto.Rating,
                    Genre = (Genre)Enum.Parse(typeof(Genre), playDto.Genre),
                    Description = playDto.Description,
                    Screenwriter = playDto.Screenwriter
                };

                plays.Add(play);
                sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre.ToString(), play.Rating.ToString()));
            }

            context.Plays.AddRange(plays);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Cast> casts = new List<Cast>();

            XmlRootAttribute root = new XmlRootAttribute("Casts");
            XmlSerializer serializer = new XmlSerializer(typeof(CastImportDto[]), root);

            StringReader reader = new StringReader(xmlString);

            CastImportDto[] castImports = (CastImportDto[])serializer.Deserialize(reader);

            foreach (var castDto in castImports)
            {
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isValidMainCharacter = bool.TryParse(castDto.IsMainCharacter, out bool isMainCharacter);

                if (!isValidMainCharacter)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Cast cast = new Cast()
                {
                    FullName = castDto.FullName,
                    IsMainCharacter = isMainCharacter,
                    PhoneNumber = castDto.PhoneNumber,
                    PlayId = castDto.PlayId,
                    Play = context.Plays.Find(castDto.PlayId)
                };

                sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, cast.IsMainCharacter ? "main" : "lesser"));
                casts.Add(cast);
            }

            context.Casts.AddRange(casts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<Theatre> theatres = new List<Theatre>();

            ProjectionsImportDto[] projectionsImportDtos = JsonConvert.DeserializeObject<ProjectionsImportDto[]>(jsonString);

            foreach (var projectionDto in projectionsImportDtos)
            {
                if (!IsValid(projectionDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre theatre = new Theatre()
                {
                    Name = projectionDto.Name,
                    NumberOfHalls = projectionDto.NumberOfHalls,
                    Director = projectionDto.Director,
                };

                foreach(var ticketDto in projectionDto.tickets)
                {
                    if (!IsValid(ticketDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Ticket ticket = new Ticket()
                    {
                        Price = ticketDto.Price,
                        RowNumberr = ticketDto.RowNumber,
                        Play = context.Plays.Find(ticketDto.PlayId),
                        Theatre = theatre
                    };

                    theatre.Tickets.Add(ticket);
                }

                theatres.Add(theatre);
                sb.AppendLine(String.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count.ToString()));
            }

            context.Theatres.AddRange(theatres);
            context.SaveChanges();

            return sb.ToString();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
