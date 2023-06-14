using Plugin.Maui.Audio;

namespace AhorcadoMAUI.Views;

public partial class MenuJuego : ContentPage
{
    private readonly IAudioManager audio;
    private readonly IAudioManager audio2;
    private readonly IAudioManager audio3;
    private readonly IAudioManager audio4;
    private readonly IAudioManager audio5;

    IAudioPlayer player;

    public MenuJuego(IAudioManager audioManager, IAudioManager audioManager2, IAudioManager audioManager3, IAudioManager audioManager4, IAudioManager audioManager5)
    {
        InitializeComponent();

        this.audio = audioManager;

        this.audio2 = audioManager2;

        this.audio3 = audioManager3;

        this.audio4 = audioManager4;

        this.audio5 = audioManager5;


    }

    private async void EnviarAModoSinglePlayer(object sender, EventArgs e)
    {

        await App.Current.MainPage.Navigation.PushAsync(new SinglePlayer(audio,audio2,audio3,audio4,audio5));
    }

    protected async override void OnAppearing()
    {
        player = audio.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("background_music.wav"));

        player.Play();

        base.OnAppearing();
    }

    protected async override void OnDisappearing()
    {

        player.Stop();

        base.OnDisappearing();
    }


}