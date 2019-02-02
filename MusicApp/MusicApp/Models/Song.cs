using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace MusicApp.Models
{
    public class Song : TableEntity
    {
        public string Name { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public int Year { get; set; }

        public Song()
        {
            this.RowKey = Guid.NewGuid().ToString();
        }
    }
}
