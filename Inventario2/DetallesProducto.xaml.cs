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
    
    public partial class DetallesProducto : ContentPage
    {
        public ModelDevice n;
        public ModelUser us;
        public DetallesProducto(ModelDevice db,ModelUser u)
        {
            InitializeComponent();
            this.n = db;
            us = u;

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
        private async void Button_Clicked(object sender, EventArgs e)
        {
            string res = await DisplayActionSheet("¡Estas a punto de eliminar un Producto!, ¿Deseas continuar?", "Cancelar", "Eliminar Producto");
            switch (res)
            {
                case "Eliminar Producto":
                    //Eliminar empleado
                    try
                    {
                        if (n.IDlugar != 1)
                        {
                            await DisplayAlert("Error", "No se puede eliminar un producto fuera del almacen", "Aceptar");
                            return;
                        }

                        //await App.MobileService.GetTable<InventDB>().DeleteAsync(n);
                        var status = await DeleteDevice(n.ID);
                        if (status)
                        {
                            try
                            {
                                var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=inventarioavs;AccountKey=wO8R0xJGc9+VleJHkKEL2AHLmZOEUvLcZg0M1KaMNI2lB9Jd27SShyHhlgeCGEQLOs7SCgYffIx4OI6TBABFPg==;EndpointSuffix=core.windows.net");
                                var client = account.CreateCloudBlobClient();
                                var container = client.GetContainerReference("fotosinventario");
                                await container.CreateIfNotExistsAsync();
                                var block = container.GetBlockBlobReference($"{n.foto}");
                                await block.DeleteIfExistsAsync();
                                
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            await DisplayAlert("Hecho", "Producto borrado exitosamente", "Aceptar");
                            await Navigation.PopAsync();
                        }
                        
                    }
                    catch (MobileServiceInvalidOperationException ms)
                    {

                        Console.WriteLine(ms.Message);
                       await  DisplayAlert("Error", "Error al borrar el producto", "Aceptar");

                    }
                        break;
                    
            }
        }


        private async Task<bool> DeleteDevice(int id)
        {
            try
            {
                var deluser = await DeviceService.deleteDevice(id);

                if (deluser == null)
                {
                    await DisplayAlert("Error", "Error de conexion al servidor", "Aceptar");
                    return false;
                }

                if (deluser.statuscode == 500)
                {
                    await DisplayAlert("Error", "Error interno en el servidor", "Aceptar");
                    return false;
                }

                if (deluser.statuscode == 404)
                {
                    await DisplayAlert("Error", "No encontrado", "Aceptar");
                    return false;
                }

                if (deluser.statuscode == 409)
                {
                    await DisplayAlert("Error", "Conflicto al borrar Usuario", "Aceptar");
                    return false;
                }

                if (deluser.statuscode == 200 || deluser.statuscode == 201)
                {
                    
                    return true;
                }


                //var tablainventario = await App.MobileService.GetTable<InventDB>().Where(u => u.codigo == movimiento.codigo).ToListAsync();

            }
            catch
            {
                return false;
            }
            return false;
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

        void Button_Clicked_1(System.Object sender, System.EventArgs e)
        {
            if(us.IDtipoUsuario == 1 || us.IDtipoUsuario==2 )
                Navigation.PushAsync(new EditarProducto(this));
        }
    }
}