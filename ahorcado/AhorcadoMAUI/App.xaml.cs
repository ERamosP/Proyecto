using AhorcadoMAUI.Views;

namespace AhorcadoMAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MenuJuego());
        }
    }
}