﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventario2.Services;
using Inventario2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
namespace Inventario2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarMovimiento : ContentPage
    {
        string currentIDmovement = "";
        List<ModelMovements> listamoves;
        List<ModelMovements> listaidmmovementsDel;
        List<ModelDevice> listadevices;

        public string tipoBusqueda;
        public EditarMovimiento()
        {
            listamoves = new List<ModelMovements>();
            listaidmmovementsDel = new List<ModelMovements>();
            listadevices = new List<ModelDevice>();
            
            InitializeComponent();
        }

        protected override void OnAppearing()
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
            var but = (Button)sender;
            ModelMovements movement = (ModelMovements)but.BindingContext;
            Grid grid = (Grid)but.Parent;
            //registrar id para eliminar
            if (!listaidmmovementsDel.Contains(movement))
            {
                listaidmmovementsDel.Add(movement);
            }

            DataSourceMovements.collection.Remove(movement);
        }

        private void searching_SearchButtonPressed(object sender, EventArgs e)
        {
            buscar();
        }

        private void btdeletedevice(object sender, EventArgs e)
        {
            var but = (Button)sender;
            ModelDevice device = (ModelDevice)but.BindingContext;
            
            //registrar id para eliminar
            if (listadevices.Contains(device))
            {
                listadevices.Remove(device);
            }

            DataSourceDevices.collection.Remove(device);
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {

        }

        private void btAceptar(object sender, EventArgs e)
        {
            actualizar();
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

                            DataSourceMovements.initializeData(busqueda);
                            postListView.ItemsSource = DataSourceMovements.collection;

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
            eliminarHistorial(listaidmmovementsDel);
        }

        private async void eliminarHistorial(List<ModelMovements> lista)
        {
            try
            {
                foreach (ModelMovements movement in lista)
                {
                    var res = await MovementService.deletemovement(movement.ID);

                    if (res.statuscode == 200 || res.statuscode == 201)
                    {
                        var device = await DeviceService.getdevicebyid(movement.IDdevice);

                        if (device[0].statuscode == 200 || device[0].statuscode == 201)
                        {
                            device[0].IDlugar = 1;
                            device[0].IDmov = "";
                            await DeviceService.putdevice(device[0].ID, JsonConvert.SerializeObject(device[0]));
                        }


                    }


                }
            }
            catch (Exception ex)
            {

            }

        }

        private void pickerBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            tipoBusqueda = pickerBuscar.SelectedItem as string;
        }


        private void btacceptDevice(object sender, EventArgs e)
        {
            var but = (Button)sender;
            ModelDevice device = (ModelDevice)but.BindingContext;
            //registrar id para eliminar
            if (listadevices.Contains(device))
            {
                listadevices.Add(device);
            }
          
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
                    DataSourceDevices.initializeData(devices);
                    postListView2.ItemsSource = DataSourceDevices.collection;
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
                    DataSourceDevices.initializeData(devices);
                    postListView2.ItemsSource = DataSourceDevices.collection;
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
                    DataSourceDevices.initializeData(devices);
                    postListView2.ItemsSource = DataSourceDevices.collection;
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
                    DataSourceDevices.initializeData(devices);
                    postListView2.ItemsSource = DataSourceDevices.collection;
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
                    DataSourceDevices.initializeData(devices);
                    postListView2.ItemsSource = DataSourceDevices.collection;
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
                    DataSourceDevices.initializeData(devices);
                    postListView2.ItemsSource = DataSourceDevices.collection;
                }
            }
        }
    }
}