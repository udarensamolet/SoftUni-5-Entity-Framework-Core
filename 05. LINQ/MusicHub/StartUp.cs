namespace MusicHub
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            int producerId = int.Parse(Console.ReadLine());
            Console.WriteLine(ExportAlbumsInfo(context, producerId));

            int duration = int.Parse(Console.ReadLine());
            Console.WriteLine(ExportSongsAboveDuration(context, duration));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context
                .Albums
                .Where(a => a.ProducerId.Value == producerId)
                //.Include(a => a.Producer)
                //.Include(a => a.Songs)
                //.ThenInclude(s => s.Writer)
                .ToList()
                .Select(a => new
                {
                    AlbumName = a.Name,
                    AlbumReleaseDate = a.ReleaseDate,
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs
                        .Select(s => new
                        {
                            SongName = s.Name,
                            SongPrice = s.Price,
                            WriterName = s.Writer.Name
                        })
                        .OrderByDescending(s => s.SongName)
                        .ThenBy(s => s.WriterName)
                        .ToList(),
                    TotalPrice = a.Price,
                })
                .OrderByDescending(a => a.TotalPrice)
                .ToList();

            string releaseDateFormat = "MM/dd/yyyy";
            var sb = new StringBuilder();
            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {album.AlbumReleaseDate.ToString(releaseDateFormat, CultureInfo.InvariantCulture)}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                if (album.Songs.Any())
                {
                    sb.AppendLine($"-Songs:");
                    int counter = 1;
                    foreach(var song in album.Songs)
                    {
                        sb.AppendLine($"---#{counter++}");
                        sb.AppendLine($"---SongName: {song.SongName}");
                        sb.AppendLine($"---Price: {song.SongPrice:f2}");
                        sb.AppendLine($"---Writer: {song.WriterName}");
                    }
                }
                sb.AppendLine($"-AlbumPrice: {album.TotalPrice:f2}");
            }
            return sb.ToString().Trim();

        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context
                .Songs
                .ToList()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    SongName = s.Name,
                    SongPerformerFullNames = s.SongPerformers
                        .Select(sp => new
                        {
                            PerformerFullName = $"{sp.Performer.FirstName} {sp.Performer.LastName}"
                        })
                        .OrderBy(sp => sp.PerformerFullName)
                        .ToList(),
                    SongWriter = s.Writer.Name,
                    SongAlbumProducer = s.Album.Producer.Name,
                    SongDuration = s.Duration
                })
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.SongWriter)
                .ToList();

            var sb = new StringBuilder();
            int counter = 1;
            foreach (var song in songs) 
            {
                sb.AppendLine($"-Song #{counter++}");
                sb.AppendLine($"---SongName: {song.SongName}");
                sb.AppendLine($"---Writer: {song.SongWriter}");
                if (song.SongPerformerFullNames.Any())
                {
                    foreach (var performer in song.SongPerformerFullNames)
                    {
                        sb.AppendLine($"---Performer: {performer.PerformerFullName}");
                    }
                }
                sb.AppendLine($"---AlbumProducer: {song.SongAlbumProducer}");
                sb.AppendLine($"---Duration: {song.SongDuration}");
            }
            return sb.ToString().Trim();
        }
    }
}