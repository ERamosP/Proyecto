using AhorcadoMAUI.ViewModels;
using Plugin.Maui.Audio;

namespace AhorcadoMAUI.Views;

public partial class MultiPlayer : ContentPage
{
    private readonly IAudioManager audio;
    private readonly IAudioManager audio2;
    private readonly IAudioManager audio3;
    private readonly IAudioManager audio4;
    private readonly IAudioManager audio5;

    IAudioPlayer player;

    public MultiPlayer()
	{
		InitializeComponent();
	}


    public MultiPlayer(IAudioManager audio, IAudioManager audio2, IAudioManager audio3, IAudioManager audio4, IAudioManager audio5)
    {
        InitializeComponent();

        this.audio = audio;

        this.audio2 = audio2;
        
        this.audio3 = audio3;
        
        this.audio4 = audio4;
        
        this.audio5 = audio5;

        this.BindingContext = new clsMultiplayerVM(audio, audio2, audio3, audio4, audio5);
    }

    protected async override void OnAppearing()
    {
        player = audio.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("background_music.wav"));

        player.Play();

        player.Loop = true;

        base.OnAppearing();
    }

    protected async override void OnDisappearing()
    {

        player.Stop();

        base.OnDisappearing();
    }

}