using CommunityToolkit.Maui.Views;
namespace AhorcadoMAUI.Views;

public partial class FinalPopUp : Popup
{
    public FinalPopUp(string imagen)
    {
        InitializeComponent();

        img.Source = imagen;

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