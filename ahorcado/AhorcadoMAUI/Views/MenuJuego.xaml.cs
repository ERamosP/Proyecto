using Plugin.Maui.Audio;

namespace AhorcadoMAUI.Views;

public partial class MenuJuego : ContentPage
{
    private readonly IAudioManager audio;

    IAudioPlayer player;

    public MenuJuego(IAudioManager audioManager)
    {
        InitializeComponent();

        this.audio = audioManager;

        crearEIniciarAudio();
    }

    private async void EnviarAModoSinglePlayer(object sender, EventArgs e)
    {

        await App.Current.MainPage.Navigation.PushAsync(new SinglePlayer());
    }

    private async void crearEIniciarAudio()
    {

        player = audio.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("background_music.wav"));

        if (player.IsPlaying)
        {

            player.Stop();

            player.Play();

        }
        else
        {

            player.Play();

        }

    }

}