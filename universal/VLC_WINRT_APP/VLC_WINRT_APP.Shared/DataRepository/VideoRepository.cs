﻿using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using VLC_WINRT_APP.Model.Video;

namespace VLC_WINRT_APP.DataRepository
{
    public class VideoRepository : IDataRepository
    {
        private static readonly string _dbPath = Path.Combine(
                    Windows.Storage.ApplicationData.Current.LocalFolder.Path,
                    "mediavlcVideos.sqlite");

        public VideoRepository()
        {
            Initialize();
        }

        public void Initialize()
        {
            using (var db = new SQLiteConnection(_dbPath))
            {
                db.CreateTable<VideoItem>();
            }
        }

        public void Drop()
        {
            throw new System.NotImplementedException();
        }

        public async Task<VideoItem> GetFromPath(String path)
        {
            var conn = new SQLiteAsyncConnection(_dbPath);
            var req = conn.Table<VideoItem>().Where(x => x.FilePath == path);
            var res = await req.ToListAsync().ConfigureAwait(false);
            return res.FirstOrDefault();
        }

        public Task Insert(VideoItem item)
        {
            var conn = new SQLiteAsyncConnection(_dbPath);
            return conn.InsertAsync(item);
        }

        public Task Update(VideoItem video)
        {
            var connection = new SQLiteAsyncConnection(_dbPath);
            return connection.UpdateAsync(video);
        }

        public Task<List<VideoItem>> GetLastViewed(int nbElements)
        {
            var connection = new SQLiteAsyncConnection(_dbPath);
            var req = connection.Table<VideoItem>().OrderByDescending(x => x.LastWatched).Take(nbElements);
            return req.ToListAsync();
        }
    }
}