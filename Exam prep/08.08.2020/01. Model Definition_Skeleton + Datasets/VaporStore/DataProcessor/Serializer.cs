namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{

			var genres = context.Genres.Where(gr => genreNames.Contains(gr.Name))
				.ToArray()
				.Select(gr => new
                {
					Id = gr.Id,
					Genre = gr.Name,
					Games = gr.Games.Where(g => g.Purchases.Count > 0).Select(g => new
                    {
						Id = g.Id,
						Title = g.Name,
						Developer = g.Developer.Name,
						Tags = string.Join(", ", g.GameTags.Select(gt => gt.Tag.Name).ToArray()),
						Players = g.Purchases.Count
                    }).OrderByDescending(g => g.Players).ThenBy(g => g.Id),
					TotalPlayers = gr.Games.Sum(g => g.Purchases.Count)
                }).OrderByDescending(gr => gr.TotalPlayers).ThenBy(gr => gr.Id)
				.ToArray();

			string json = JsonConvert.SerializeObject(genres, Formatting.Indented);
			return json;
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
			XmlRootAttribute root = new XmlRootAttribute("Users");
			XmlSerializer serializer = new XmlSerializer(typeof(UserPurchasesExportDto[]), root);

			UserPurchasesExportDto[] userPurchasesExports = context.Users
				.Where(u => u.Cards.Any(c => c.Purchases.Any()))
				.Select(u => new UserPurchasesExportDto()
				{
					Username = u.Username,
					Purchases = context.Purchases
					.Where(p => p.Card.User.Username == u.Username)
						.OrderBy(p => p.Date)
				}).ToArray();

		}
	}
}