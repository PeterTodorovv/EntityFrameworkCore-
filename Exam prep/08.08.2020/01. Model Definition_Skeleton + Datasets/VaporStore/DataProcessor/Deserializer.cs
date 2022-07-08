namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Text.Json.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Deserializer
	{
		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
			StringBuilder sb = new StringBuilder();
            GamesExportDto[] gamesExportDtos = JsonConvert.DeserializeObject<GamesExportDto[]>(jsonString);
			List<Game> games = new List<Game>();

			foreach(var gameDto in gamesExportDtos)
            {
                if (!IsValid(gameDto))
                {
					sb.AppendLine(Constants.Error_message);
					continue;
                }

				if(gameDto.Price < 0)
                {
					sb.AppendLine(Constants.Error_message);
					continue;
				}

                if (String.IsNullOrEmpty(gameDto.Name))
                {
					sb.AppendLine(Constants.Error_message);
					continue;
				}

                if (String.IsNullOrEmpty(gameDto.ReleaseDate))
                {
					sb.AppendLine(Constants.Error_message);
					continue;
				}

                if (String.IsNullOrEmpty(gameDto.Developer))
                {
					sb.AppendLine(Constants.Error_message);
					continue;
				}

                if (String.IsNullOrEmpty(gameDto.Genre))
                {
					sb.AppendLine(Constants.Error_message);
					continue;
				}

                if (gameDto.Tags.Length <= 0)
                {
					sb.AppendLine(Constants.Error_message);
					continue;
				}
            }
		}

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
			throw new NotImplementedException();
		}

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
			throw new NotImplementedException();
		}

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}