using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MusicApp.Models;
using MusicApp.Services;

namespace MusicApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SongListView : ContentPage
	{
        public SongListView()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            SongList.ItemsSource = await TableStorageService.GetSongs();
        }

        private async void SelectSong(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem != null)
                {
                    await Navigation.PushAsync(new SongView(e.SelectedItem as Song));
                    SongList.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void AddNewSong(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SongView(new Song()));
        }
    }
}