using AhorcadoMAUI.Views;
using Plugin.Maui.Audio;

namespace AhorcadoMAUI
{
    public partial class App : Application
    {

        public App(IAudioManager audio, IAudioManager audio2, IAudioManager audio3, IAudioManager audio4, IAudioManager audio5)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MenuJuego(audio,audio2, audio3, audio4, audio5));
        }
    }
}