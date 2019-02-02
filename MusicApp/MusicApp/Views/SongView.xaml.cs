using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MusicApp.Models;
using MusicApp.Services;

namespace MusicApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SongView : ContentPage
	{
        Song song;

        public SongView(Song song)
        {
            InitializeComponent();

            this.BindingContext = song;
            this.song = song;

            if (string.IsNullOrWhiteSpace(song.PartitionKey))
            {
                ToolbarItems.RemoveAt(1);
                song.PartitionKey = Guid.NewGuid().ToString();
            }
        }

        private async void SaveSong(object sender, EventArgs e)
        {
            try
            {
                if (await TableStorageService.SaveSong(song))
                {
                    await DisplayAlert("Success!", "Song saved!", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error!", "Song NOT saved!", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", "There was an error!", "OK");
            }
        }

        private async void DeleteSong(object sender, EventArgs e)
        {
            try
            {
                if (await DisplayAlert("Warning!", "Do you really want to delete it?", "Yes", "No"))
                {
                    if (await TableStorageService.DeleteSong(song))
                    {
                        await DisplayAlert("Success!", "Song deleted!", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error!", "Song NOT deleted!", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", "There was an error!", "OK");
            }
        }

    }
}