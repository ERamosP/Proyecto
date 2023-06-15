using CommunityToolkit.Maui.Views;
using Entidades;

namespace AhorcadoMAUI.Views;

public partial class SeleccionPopUp : Popup
{
	public SeleccionPopUp(List<clsPalabra> palabras)
	{
		InitializeComponent();

		btn1.Text = palabras[0].nombre; 
 
		btn2.Text = palabras[1].nombre; 

		btn3.Text = palabras[2].nombre;

        fondo.Size = new Size(300, 300);

    }

	private void palabraSeleccionada1(object sender, EventArgs e) {

		Close(btn1.Text);
	}

    private void palabraSeleccionada2(object sender, EventArgs e)
    {

        Close(btn2.Text);

    }

    private void palabraSeleccionada3(object sender, EventArgs e)
    {
        Close(btn3.Text);

    }


}