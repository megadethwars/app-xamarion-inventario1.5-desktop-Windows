using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventario2.Services;
using Inventario2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inventario2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarMovimiento : ContentPage
    {
        string currentIDmovement = "";
        List<ModelMovements> listamoves;
        public string tipoBusqueda;
        public EditarMovimiento()
        {
            listamoves = new List<ModelMovements>();
            InitializeComponent();
        }

        protected override  void OnAppearing()
        {
            base.OnAppearing();
          
            tipoBusqueda = pickerBuscar.SelectedItem as String;
            
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

        }

        private async void search_SearchButtonPressed(object sender, EventArgs e)
        {
            await searchmovementsAsync(search.Text);
        }

        private void postListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void searching_SearchButtonPressed(object sender, EventArgs e)
        {
            buscar();
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {

        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {

        }

        private async Task searchmovementsAsync(string id)
        {
            try
            {
                if (id != null || !id.Equals(""))
                {
                    var busqueda = await MovementService.getmovementsbyid(search.Text);

                    if (busqueda == null)
                    {
                        await DisplayAlert("Buscando", "error interno del servidor", "OK");
                        return;
                    }

                    if (busqueda[0].statuscode == 500)
                    {
                        await DisplayAlert("Buscando", "error interno del servidor", "OK");
                        return;
                    }

                    if (busqueda[0].statuscode == 404)
                    {
                        await DisplayAlert("Buscando", "productos no encontrados", "OK");
                        return;
                    }

                    if (busqueda[0].statuscode == 200)
                    {
                        if (busqueda.Count != 0)
                        {
                            postListView.ItemsSource = busqueda;
                            currentIDmovement = busqueda[0].IDmovimiento;
                        }
                          
                    }
                }
            }
            catch
            {

            }
            
        }

        private void Agregarproducto(ModelMovements producto)
        {

        }

        private void buscarproducto()
        {

        }

        private void eliminarproducto(int id)
        {

        }

        private void actualizar()
        {

        }

        private void pickerBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            tipoBusqueda = pickerBuscar.SelectedItem as string;
        }
        public async void buscar()
        {
            string cadena = "";
            if (searching.Text.Length > 3)
                cadena = searching.Text.Substring(searching.Text.Length - 3);
            var isNumeric = long.TryParse(cadena, out long n);


            if (tipoBusqueda == "producto")
            {

                var devices = await DeviceService.getdevicebyproduct(searching.Text);
                if (devices == null)
                {

                    await DisplayAlert("Buscando", "error de conexion con el servidor", "OK");
                    return;
                }

                if (devices[0].statuscode == 500)
                {
                    await DisplayAlert("Buscando", "error interno del servidor", "OK");
                    return;
                }

                if (devices[0].statuscode == 404)
                {
                    await DisplayAlert("Buscando", "producto no encontrado", "OK");
                    return;
                }

                if (devices[0].statuscode == 200 || devices[0].statuscode == 201)
                {
                    postListView.ItemsSource = devices;
                }



            }
            if (tipoBusqueda == "QR")
            {

                var devices = await DeviceService.getdevicebycode(searching.Text);
                if (devices == null)
                {

                    await DisplayAlert("Buscando", "error de conexion con el servidor", "OK");
                    return;
                }

                if (devices[0].statuscode == 500)
                {
                    await DisplayAlert("Buscando", "error interno del servidor", "OK");
                    return;
                }

                if (devices[0].statuscode == 404)
                {
                    await DisplayAlert("Buscando", "producto no encontrado", "OK");
                    return;
                }

                if (devices[0].statuscode == 200 || devices[0].statuscode == 201)
                {
                    postListView.ItemsSource = devices;
                }

            }
            if (tipoBusqueda == "modelo")
            {
                var devices = await DeviceService.getdevicebymodel(searching.Text);
                if (devices == null)
                {

                    await DisplayAlert("Buscando", "error de conexion con el servidor", "OK");
                    return;
                }

                if (devices[0].statuscode == 500)
                {
                    await DisplayAlert("Buscando", "error interno del servidor", "OK");
                    return;
                }

                if (devices[0].statuscode == 404)
                {
                    await DisplayAlert("Buscando", "producto no encontrado", "OK");
                    return;
                }

                if (devices[0].statuscode == 200 || devices[0].statuscode == 201)
                {
                    postListView.ItemsSource = devices;
                }
            }


            if (tipoBusqueda == "marca")
            {
                var devices = await DeviceService.getdevicebymarca(searching.Text);
                if (devices == null)
                {

                    await DisplayAlert("Buscando", "error de conexion con el servidor", "OK");
                    return;
                }

                if (devices[0].statuscode == 500)
                {
                    await DisplayAlert("Buscando", "error interno del servidor", "OK");
                    return;
                }

                if (devices[0].statuscode == 404)
                {
                    await DisplayAlert("Buscando", "producto no encontrado", "OK");
                    return;
                }

                if (devices[0].statuscode == 200 || devices[0].statuscode == 201)
                {
                    postListView.ItemsSource = devices;
                }
            }


            if (tipoBusqueda == "proveedor")
            {

                var devices = await DeviceService.getdevicebyprov(searching.Text);
                if (devices == null)
                {

                    await DisplayAlert("Buscando", "error de conexion con el servidor", "OK");
                    return;
                }

                if (devices[0].statuscode == 500)
                {
                    await DisplayAlert("Buscando", "error interno del servidor", "OK");
                    return;
                }

                if (devices[0].statuscode == 404)
                {
                    await DisplayAlert("Buscando", "producto no encontrado", "OK");
                    return;
                }

                if (devices[0].statuscode == 200 || devices[0].statuscode == 201)
                {
                    postListView.ItemsSource = devices;
                }
            }
            if (tipoBusqueda == "serie")
            {

                var devices = await DeviceService.getdevicebyserie(searching.Text);
                if (devices == null)
                {

                    await DisplayAlert("Buscando", "error de conexion con el servidor", "OK");
                    return;
                }

                if (devices[0].statuscode == 500)
                {
                    await DisplayAlert("Buscando", "error interno del servidor", "OK");
                    return;
                }

                if (devices[0].statuscode == 404)
                {
                    await DisplayAlert("Buscando", "producto no encontrado", "OK");
                    return;
                }

                if (devices[0].statuscode == 200 || devices[0].statuscode == 201)
                {
                    postListView2.ItemsSource = devices;
                }
            }
        }
    }
}