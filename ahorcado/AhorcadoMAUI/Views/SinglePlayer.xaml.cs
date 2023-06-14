using AhorcadoMAUI.ViewModels;
using Plugin.Maui.Audio;

namespace AhorcadoMAUI.Views;

public partial class SinglePlayer : ContentPage
{

    private readonly IAudioManager audio;
    private readonly IAudioManager audio2;
    private readonly IAudioManager audio3;
    private readonly IAudioManager audio4;
    private readonly IAudioManager audio5;

    public SinglePlayer()
	{
		InitializeComponent();
	}

    public SinglePlayer(IAudioManager audioManager, IAudioManager audioManager2, IAudioManager audioManager3, IAudioManager audioManager4, IAudioManager audioManager5)
    {
        InitializeComponent();


        this.audio = audioManager;

        this.audio2 = audioManager2;

        this.audio3 = audioManager3;

        this.audio4 = audioManager4;

        this.audio5 = audioManager5;

        this.BindingContext = new clsSinglePlayerVM(audio,audio2,audio3,audio4,audio5);
        
    }

}