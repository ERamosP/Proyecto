namespace horca_Dos_MAUI.Views;

public class MenuJuego : ContentPage
{
	public MenuJuego()
	{
		InitializeComponent();
	}

    private async void EnviarAModoSinglePlayer(object sender, EventArgs e)
    {

        await App.Current.MainPage.Navigation.PushAsync(new SinglePlayer());
    }



}