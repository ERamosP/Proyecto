using CommunityToolkit.Maui.Views;
namespace AhorcadoMAUI.Views;

public partial class FinalPopUp : Popup
{
    public FinalPopUp(string imagen, int altura, string palabra)
    {
        InitializeComponent();

        img.Source = imagen;
        fondo.Size = new Size(300, altura);
        textoFin.Text = $"La palabra era: {palabra}";
    }

    private void OnClickTrue(object sender, EventArgs e)
    {
        Close(true);
    }

    private void OnClickFalse(object sender, EventArgs e)
    {
        Close(false);
    }
}