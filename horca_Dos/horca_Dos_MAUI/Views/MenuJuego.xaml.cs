namespace AhorcadoMAUI.Views;

public partial class MenuJuego : ContentPage
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