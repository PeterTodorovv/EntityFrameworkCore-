namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
	{
		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
			StringBuilder sb = new StringBuilder();
            GamesImportDto[] gamesExportDtos = JsonConvert.DeserializeObject<GamesImportDto[]>(jsonString);
			List<Game> games = new List<Game>();
			List<Developer> developers = new List<Developer>();
			List<Genre> genres = new List<Genre>();
			List<Tag> tags = new List<Tag>();

			foreach(var gameDto in gamesExportDtos)
            {
                if (!IsValid(gameDto))
                {
					sb.AppendLine(Constants.Error_message);
					continue;
                }

				bool isReleaseDateValid = DateTime.TryParseExact(gameDto.ReleaseDate, "yyyy-MM-dd",
					CultureInfo.InvariantCulture, DateTimeStyles.None ,out DateTime releaseDate);

                if (!isReleaseDateValid)
                {
					sb.AppendLine(Constants.Error_message);
					continue;
				}

				if(gameDto.Tags.Length == 0)
                {
					sb.AppendLine(Constants.Error_message);
					continue;
				}

				Game game = new Game()
				{
					Name = gameDto.Name,
					Price = gameDto.Price,
					ReleaseDate = releaseDate
				};

				Developer developer = developers.FirstOrDefault(d => d.Name == gameDto.Developer);
				if(developer == null)
                {
					developer = new Developer() { Name = gameDto.Developer};
					developers.Add(developer);
                }
				game.Developer = developer;

				Genre genre = genres.FirstOrDefault(d => d.Name == gameDto.Genre);
				if(genre == null)
                {
					genre = new Genre() { Name = gameDto.Genre };
					genres.Add(genre);
                }
				game.Genre = genre;

				foreach (string tagName in gameDto.Tags)
				{
					if (String.IsNullOrEmpty(tagName))
					{
						continue;
					}

					Tag gameTag = tags
						.FirstOrDefault(t => t.Name == tagName);

					if (gameTag == null)
					{
						Tag newGameTag = new Tag()
						{
							Name = tagName
						};

						tags.Add(newGameTag);
						game.GameTags.Add(new GameTag()
						{
							Game = game,
							Tag = newGameTag
						});
					}
					else
					{
						game.GameTags.Add(new GameTag()
						{
							Game = game,
							Tag = gameTag
						});
					}
				}
				if (tags.Count == 0)
                {
					sb.AppendLine(Constants.Error_message);
					continue;
				}

				games.Add(game);
				sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
            }
			context.Games.AddRange(games);
			context.SaveChanges();

			return sb.ToString().TrimEnd();
		}

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
			StringBuilder sb = new StringBuilder();
            UsersImportDto[] usersImports = JsonConvert.DeserializeObject<UsersImportDto[]>(jsonString);
			List<User> users = new List<User>();

            foreach (var userDto in usersImports)
            {
                if (!IsValid(userDto))
                {
					sb.AppendLine(Constants.Error_message);
					continue;
				}

				if(userDto.Cards.Count == 0)
                {
					sb.AppendLine(Constants.Error_message);
					continue;
				}

				User user = new User()
				{
					FullName = userDto.FullName,
					Username = userDto.Username,
					Email = userDto.Email,
					Age = userDto.Age
				};

                foreach (var cardDto in userDto.Cards)
                {
                    if (!IsValid(cardDto))
                    {
						sb.AppendLine(Constants.Error_message);
						continue;
					}

					Card card = new Card()
					{
						Number = cardDto.Number,
						Cvc = cardDto.CVC,
						Type = (CardType) Enum.Parse(typeof(CardType),cardDto.Type, true)
					};

					user.Cards.Add(card);
                }

				if(user.Cards.Count == 0)
                {
					sb.AppendLine(Constants.Error_message);
					continue;
				}

				users.Add(user);
				sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
            }

			context.Users.AddRange(users);
			context.SaveChanges();

			return sb.ToString().TrimEnd();

		}

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
			StringBuilder sb = new StringBuilder();
			XmlRootAttribute root = new XmlRootAttribute("Purchases");
			XmlSerializer serializer = new XmlSerializer(typeof(PurchasesImportDto[]), root);

			StringReader reader = new StringReader(xmlString);

			PurchasesImportDto[] purchasesImports = (PurchasesImportDto[])serializer.Deserialize(reader);

			List<Purchase> purchases = new List<Purchase>();

			foreach(var purchaseDto in purchasesImports)
            {
                if (!IsValid(purchaseDto))
                {
					sb.AppendLine(Constants.Error_message);
					continue;
				}

				bool isDateValid = DateTime.TryParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm",
					CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validDate);

				if (!isDateValid)
                {
					sb.AppendLine(Constants.Error_message);
					continue;
				}

				var card = context.Cards.FirstOrDefault(c => c.Number == purchaseDto.Card);
				
				if (card == null)
                {
					sb.AppendLine(Constants.Error_message);
					continue;
				}

				var game = context.Games.FirstOrDefault(g => g.Name == purchaseDto.Title);

				if(game == null)
                {
					sb.AppendLine(Constants.Error_message);
					continue;
				}

				Purchase purchase = new Purchase()
				{
					Type = (PurchaseType)Enum.Parse(typeof(PurchaseType), purchaseDto.Type, true),
					ProductKey = purchaseDto.Key,
					Date = validDate,
					Card = card,
					Game = game
				};

				purchases.Add(purchase);
				sb.AppendLine($"Imported {game.Name} for {card.User.Username}");
            }

			context.Purchases.AddRange(purchases);
			context.SaveChanges();

			return sb.ToString().TrimEnd();
		}

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}