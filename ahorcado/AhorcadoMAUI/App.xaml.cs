using AhorcadoMAUI.Views;
using Plugin.Maui.Audio;

namespace AhorcadoMAUI
{
    public partial class App : Application
    {

        public App(IAudioManager audio)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MenuJuego(audio));
        }
    }
}