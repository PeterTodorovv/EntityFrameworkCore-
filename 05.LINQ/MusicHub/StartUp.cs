namespace MusicHub
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            Console.WriteLine(ExportSongsAboveDuration(context, 240));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            Album[] albums = context.Albums.ToArray().Select(a => a).Where(a => a.ProducerId == producerId).OrderByDescending(a => a.Price).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate:MM/dd/yyyy}");
                sb.AppendLine($"-ProducerName: {album.Producer.Name}");
                sb.AppendLine($"-Songs:");

                int songNumber = 1;
                foreach(var song in album.Songs.OrderByDescending(s => s.Name).ThenBy(s => s.Writer))
                {
                    sb.AppendLine($"---#{songNumber}");
                    sb.AppendLine($"---SongName: {song.Name}");
                    sb.AppendLine($"---Price: {song.Price:f2}");
                    sb.AppendLine($"---Writer: {song.Writer.Name}");
                    songNumber++;
                }
                sb.AppendLine($"-AlbumPrice: {album.Price:f2}");
            }


            return sb.ToString().Trim();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder sb = new StringBuilder();

            var songs = context.Songs.Select(s => new
            {
                SongName = s.Name,
                Writer = s.Writer.Name,
                Performer = String.Join(' ', s.SongPerformers.Select(p => $"{p.Performer.FirstName} {p.Performer.LastName}").FirstOrDefault()),
                AlbumProducer = s.Album.Producer.Name,
                Duration = s.Duration,
                DurationInSeconds = int.Parse(s.Duration.Minutes.ToString()) * 60 + int.Parse(s.Duration.Seconds.ToString())
            }
            )
            .ToArray()
            .Where(s => s.DurationInSeconds > duration)
            .OrderBy(s => s.SongName).ThenBy(s => s.Writer).ThenBy(s => s.Performer).ToArray();


            int songNumber = 1;
            foreach(var song in songs)
            {
                sb.AppendLine($"-Song #{songNumber}");
                sb.AppendLine($"---SongName: {song.SongName}");
                sb.AppendLine($"---Writer: {song.Writer}");
                sb.AppendLine($"---Performer: {song.Performer}");
                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.Duration:c}");

                songNumber++;
            }

            return sb.ToString();
        }
    }
}
