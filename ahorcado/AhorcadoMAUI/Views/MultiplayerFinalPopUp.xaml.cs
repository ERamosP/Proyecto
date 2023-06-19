using CommunityToolkit.Maui.Views;
namespace AhorcadoMAUI.Views;

public partial class MultiplayerFinalPopUp : Popup
{
    public MultiplayerFinalPopUp(string imagen, int altura, string jugador)
    {
        InitializeComponent();

        img.Source = imagen;
        fondo.Size = new Size(300, altura);
        textoFin.Text = $"{jugador}";
    }

    private void OnClickFalse(object sender, EventArgs e)
    {
        Close(false);
    }
}