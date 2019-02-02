using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using MusicApp.Helpers;
using MusicApp.Models;

namespace MusicApp.Services
{
    public static class TableStorageService
    {
        static CloudStorageAccount cloudStorageAccount;
        static CloudTableClient tableClient;
        static CloudTable songsTable;

        private static async Task ConnectToTable()
        {
            cloudStorageAccount = CloudStorageAccount.Parse(Constants.StorageAccountConnectionString);
            tableClient = cloudStorageAccount.CreateCloudTableClient();
            songsTable = tableClient.GetTableReference(Constants.SongsTableName);
            await songsTable.CreateIfNotExistsAsync();
        }

        public static async Task<List<Song>> GetSongs()
        {
            await ConnectToTable();

            TableContinuationToken continuationToken = null;
            var songs = new List<Song>();
            
            try
            {
                do
                {
                    var result = await songsTable.ExecuteQuerySegmentedAsync(new TableQuery<Song>(), continuationToken);
                    continuationToken = result.ContinuationToken;

                    if (result.Results.Count > 0)
                        songs.AddRange(result.Results);
                } while (continuationToken != null);
            }
            catch (Exception ex)
            {

            }

            return songs;
        }

        public static async Task<Song> GetSong(string partitionKey, string rowKey)
        {
            try
            {
                await ConnectToTable();
                var operation = TableOperation.Retrieve<Song>(partitionKey, rowKey);
                var query = await songsTable.ExecuteAsync(operation);
                return query.Result as Song;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public static async Task<bool> SaveSong(Song song)
        {
            try
            {
                await ConnectToTable();
                var operation = TableOperation.InsertOrMerge(song);
                var upsert = await songsTable.ExecuteAsync(operation);
                return (upsert.HttpStatusCode == 204);
            }
            catch(Exception ex)
            {

            }

            return false;
        }

        public static async Task<bool> DeleteSong(Song song)
        {
            try
            {
                await ConnectToTable();
                var operation = TableOperation.Delete(song);
                var delete = await songsTable.ExecuteAsync(operation);
                return (delete.HttpStatusCode == 204);
            }
            catch (Exception ex)
            {

            }

            return false;
        }
    }
}
