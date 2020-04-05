using Inventario2.Model;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventario2.Models;
using Inventario2.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

namespace Inventario2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Confirmar : ContentPage
    {
        string p;
        public Carrito rp;
        private GeneratePDF pdf;
        private InventDB CurrentDevice;
        private bool isToggled;
        private ModelUser usuariosalida;
        public Confirmar(Carrito x)
        {
            InitializeComponent();
            rp = x;
            p = Guid.NewGuid().ToString("D");
            pdf = new GeneratePDF();
            CurrentDevice = new InventDB();
        }


        private async Task<bool> verifyuser(string user,string password)
        {
            LoginUser logus = new LoginUser();
            logus.nombre = user;
            logus.password = password;

            var status = await UserService.loginAsync(JsonConvert.SerializeObject(logus));

            if (status==null)
            {
                return false;
            }

            if (status.statuscode == 404)
            {
                await DisplayAlert("Error", "Usuario no encontrado", "Aceptar");

                return false;
            }

            if (status.statuscode == 401)
            {
                await DisplayAlert("Error", "Usuario y/o contraseña incorrecto", "Aceptar");
                return false;
            }

            if (status.statuscode == 500)
            {
                await DisplayAlert("Error", "error en el servidor(bad request)", "Aceptar");
                return  false;

            }

            if (status.statuscode == 201 || status.statuscode == 200)
            {
                //query this user
                var users = await UserService.getuserbyname(logus.nombre);

                if (users[0].statuscode == 500)
                {
                    await DisplayAlert("Error", "Error de conexion con el servidor", "Aceptar");
                    return false;
                }

                usuariosalida = users[0];
                logus.Dispose();
                users = null;
                return true;

            }

            return false;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (!isToggled)
            {
                Boolean v = true;
                bool password = false;
                if (Usuario.Text != null && Contra.Text != null)
                {
                    bool status = await verifyuser(Usuario.Text, Contra.Text);


                    //var usuarios = await App.MobileService.GetTable<Usuario>().Where(u => u.nombre == Usuario.Text).ToListAsync();
                    
                    
                    if (status)
                    {
                        isToggled = true;
                        password = true;

                        await UpdateLocations(rp.re.mv, Destino.Text);

                        for (int y = 0; y < rp.re.mv.Count(); y++)
                        {
                            try
                            {
                                rp.re.movimientos[y].ID = p;
                                rp.re.movimientos[y].IDusuario = usuariosalida.ID;
                                rp.re.movimientos[y].IDlugar = 1;
                                rp.re.movimientos[y].fotomov1 = p.Substring(15) + rp.re.mv[y].codigo + ".jpg";
                                rp.re.movimientos[y].fotomov2 = p.Substring(10) + rp.re.mv[y].codigo + "2.jpg";

                                
                                
                                
                                
                                await App.MobileService.GetTable<Movimientos>().InsertAsync(rp.re.mv[y]);
                                //UploadFile(f.GetStream());
                                //DisplayAlert("Agregado", re.mv.Count().ToString(), "Aceptar");
                                //re.mv.Clear();
                                //await Navigation.PopAsync();
                                v = true;
                                if (rp.re.f1[y] != null)
                                    UploadFile(rp.re.f1[y].GetStream(), rp.re.mv[y].foto);
                                if (rp.re.f2[y] != null)
                                    UploadFile(rp.re.f2[y].GetStream(), rp.re.mv[y].foto2);

                            }
                            catch (MobileServiceInvalidOperationException ms)
                            {
                                var response = await ms.Response.Content.ReadAsStringAsync();
                                await DisplayAlert("Error", response, "Aceptar");
                                v = false;
                                break;
                            }
                        }
                        if (v)
                        {
                            //agregar el pdf
                            rp.re.mv.Clear();
                            rp.re.f1.Clear();
                            rp.re.f2.Clear();
                            await DisplayAlert("Agregado", "Carrito Agregado correctamente", "Aceptar");
                            //await Navigation.PushAsync(new PDFMovement(p));
                            await pdf.InitPDFAsync(p);
                            ToolbarItem_Clicked(null, null);
                            //await Navigation.PopAsync();
                        }

                    }
                    else
                    {
                        return;
                    }                  
                  
                }
                else
                {
                    await DisplayAlert("Error", "Usuario o contraseña no ingresado(s)", "Aceptar");
                }
                
            }
            else
            {
                await DisplayAlert("Alerta", "Ya se han actualizado los productos de salida", "Aceptar");
            }

            

        }

        private async void UploadFile(Stream stream, string PathFoto)
        {
            var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=fotosavs;AccountKey=kS7YxHQSBtu6kDpa2sG7OVidbxcJq1Dip7+KnNjQA5SHn9N7loT2/Ul9HkdN0R5UPDWeKy0WpWQprGgnjIrbdA==;EndpointSuffix=core.windows.net");
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference("fotossalida");
            await container.CreateIfNotExistsAsync();

            var block = container.GetBlockBlobReference($"{PathFoto}");
            await block.UploadFromStreamAsync(stream);
            string url = block.Uri.OriginalString;

        }

        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            int aux = 2;
            var lista = Navigation.NavigationStack;
            for (int x = 0; x < lista.Count; x++)
            {
                if (lista.Count > 3)
                {
                    if(x==2)
                    {
                        x--;
                        Navigation.RemovePage(lista[aux]);
                    }
                   
                }
            }
            Navigation.PopAsync();
        }

        private async Task UpdateLocations(List<Movimientos> movimientos, string lugar)
        {
            foreach (Movimientos movimiento in movimientos)
            {
                try
                {
                    var tablainventario = await App.MobileService.GetTable<InventDB>().Where(u => u.codigo == movimiento.codigo).ToListAsync();
                    if (tablainventario.Count != 0)
                    {
                        CurrentDevice = tablainventario[0];
                        CurrentDevice.lugar = lugar;

                        //update
                        await App.MobileService.GetTable<InventDB>().UpdateAsync(CurrentDevice);

                    }
                }
                catch
                {

                }
            }
        }

    }
}