namespace AhorcadoMAUI.Views;

public partial class ViewResultado : ContentPage
{

	public string RutaImagen;

	public ViewResultado()
	{
        InitializeComponent();
    }

	public ViewResultado(string imagen)
	{
		InitializeComponent();

		imagenFinal.Source = imagen;

	}

	public async void ReiniciarJuego(object sender, EventArgs e) { 
	
		await App.Current.MainPage.Navigation.PopAsync();
	
	}

	public async void ReiniciarPrograma(object sender, EventArgs e) {

		await App.Current.MainPage.Navigation.PopToRootAsync();
	
	
	}

}