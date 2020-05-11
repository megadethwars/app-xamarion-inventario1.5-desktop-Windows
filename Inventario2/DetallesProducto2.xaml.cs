using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Inventario2.Models;
using Inventario2.Services;
namespace Inventario2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class DetallesProducto2 : ContentPage
    {
        public ModelDevice n;
        public ModelUser us;
        public DetallesProducto2(ModelDevice db)
        {
            InitializeComponent();
            this.n = db;
            

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            nameProd.Text = n.producto;
            idCodigo.Text = n.codigo;
            idcantidad.Text = n.cantidad;
            idlugar.Text = n.Lugar;
            idobserv.Text = n.observaciones;
            //idProd.Text = n.ID.ToString();
            idmarca.Text = n.marca;
            idcompra.Text = n.compra;
            idcosto.Text = n.costo;
            idorigen.Text = n.origen;
            idserie.Text = n.serie;
            iddesc.Text = n.descompostura;
            idpert.Text = n.pertenece;
            idFecha.Text = n.fecha;

            idmodelo.Text = n.modelo;
            try
            {
                //imagen.Source = "https://fotosavs.blob.core.windows.net/fotosinventario/" + n.foto;
                imagen.Source = n.foto;
            }
            catch
            {

            }

        }
        


        
        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            int aux = 2;
            var lista = Navigation.NavigationStack;
            for (int x = 0; x < lista.Count; x++)
            {
                if (lista.Count > 3)
                {
                    if (x == 2)
                    {
                        x--;
                        Navigation.RemovePage(lista[aux]);
                    }

                }
            }
            Navigation.PopAsync();
        }

        
    }
}