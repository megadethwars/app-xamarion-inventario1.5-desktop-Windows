using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.FilePicker;
using Syncfusion.XlsIO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inventario2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Inventario : ContentPage
    {
        Plugin.FilePicker.Abstractions.FileData f;
        //string codigo;
        public List<InventDB> users;
        public string stringcode;
        public int cont = 0;
        public string tipoBusqueda;
        public Inventario()
        {
            InitializeComponent();
            
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            search.Text = stringcode;
            tipoBusqueda = pickerBuscar.SelectedItem as String;
            search.Focus();
            if (cont == 1)
            {
                var busqueda = await App.MobileService.GetTable<InventDB>().Where(u => u.codigo == search.Text).ToListAsync();
                if (busqueda.Count != 0)
                    postListView.ItemsSource = busqueda;
                else
                    DependencyService.Get<IMessage>().ShortAlert("Producto no Encontrado");
                cont = 0;
            }
            else
            {
                var usuario = await App.MobileService.GetTable<InventDB>().ToListAsync();
                postListView.ItemsSource = usuario;
            }
            
            
        }

        
        private void PostListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedPost = postListView.SelectedItem as InventDB;
            if (selectedPost != null)
                Navigation.PushAsync(new DetallesProducto(selectedPost));
        }

        public async void buscar()
        {
            string cadena = "";
            if (search.Text.Length > 3)
                cadena = search.Text.Substring(search.Text.Length - 3);
            var isNumeric = long.TryParse(cadena, out long n);


            if (tipoBusqueda=="Nombre")
            {
                //SQLiteConnection conn = new SQLiteConnection(App.DtabaseLocation);
                //conn.CreateTable<InventDB>();
                //var users1 = conn.Query<InventDB>("select * from InventDB where Nombre= ?", search.Text);
                //conn.Close();
                var users1 = await App.MobileService.GetTable<InventDB>().Where(u => u.nombre == search.Text).ToListAsync();
                if (users1.Count != 0)
                {
                    //DisplayAlert("Buscando", "encontrado", "OK");
                    postListView.ItemsSource = users1;
                }
                else
                {
                    DisplayAlert("Buscando", "Producto no encontrado", "Aceptar");
                    var usuarios = await App.MobileService.GetTable<InventDB>().ToListAsync();

                    postListView.ItemsSource = usuarios;
                }
            }
            if(tipoBusqueda=="QR")
            {

                var users1 = await App.MobileService.GetTable<InventDB>().Where(u => u.codigo == search.Text).ToListAsync();
                if (users1.Count != 0)
                {
                    //DisplayAlert("Buscando", "encontrado", "OK");
                    postListView.ItemsSource = users1;
                }
                else
                {
                    DisplayAlert("Buscando", " no encontrado" + search.Text, "OK");
                    var usuarios = await App.MobileService.GetTable<InventDB>().ToListAsync();

                    postListView.ItemsSource = usuarios;
                }
            }
            if (tipoBusqueda == "Marca")
            {

                var users1 = await App.MobileService.GetTable<InventDB>().Where(u => u.marca == search.Text).ToListAsync();
                if (users1.Count != 0)
                {
                    //DisplayAlert("Buscando", "encontrado", "OK");
                    postListView.ItemsSource = users1;
                }
                else
                {
                    DisplayAlert("Buscando", " no encontrado" + search.Text, "OK");
                    var usuarios = await App.MobileService.GetTable<InventDB>().ToListAsync();

                    postListView.ItemsSource = usuarios;
                }
            }
            if (tipoBusqueda == "Modelo")
            {

                var users1 = await App.MobileService.GetTable<InventDB>().Where(u => u.modelo == search.Text).ToListAsync();
                if (users1.Count != 0)
                {
                    //DisplayAlert("Buscando", "encontrado", "OK");
                    postListView.ItemsSource = users1;
                }
                else
                {
                    DisplayAlert("Buscando", " no encontrado" + search.Text, "OK");
                    var usuarios = await App.MobileService.GetTable<InventDB>().ToListAsync();

                    postListView.ItemsSource = usuarios;
                }
            }
            if (tipoBusqueda == "Proveedor")
            {

                var users1 = await App.MobileService.GetTable<InventDB>().Where(u => u.proveedor == search.Text).ToListAsync();
                if (users1.Count != 0)
                {
                    //DisplayAlert("Buscando", "encontrado", "OK");
                    postListView.ItemsSource = users1;
                }
                else
                {
                    DisplayAlert("Buscando", " no encontrado" + search.Text, "OK");
                    var usuarios = await App.MobileService.GetTable<InventDB>().ToListAsync();

                    postListView.ItemsSource = usuarios;
                }
            }
            if (tipoBusqueda == "Serie")
            {

                var users1 = await App.MobileService.GetTable<InventDB>().Where(u => u.serie == search.Text).ToListAsync();
                if (users1.Count != 0)
                {
                    //DisplayAlert("Buscando", "encontrado", "OK");
                    postListView.ItemsSource = users1;
                }
                else
                {
                    DisplayAlert("Buscando", " no encontrado" + search.Text, "OK");
                    var usuarios = await App.MobileService.GetTable<InventDB>().ToListAsync();

                    postListView.ItemsSource = usuarios;
                }
            }
        }
        private  void SearchBar(object sender, EventArgs e)
        {
            buscar();
        }

        private void Scan(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Escanear(this));
        }
        public async void Checar()
        {
            
            f = await CrossFilePicker.Current.PickFile();

            if (f != null)
            {
                var t = DisplayAlert("Excel",f.FileName,"aceptar");
                
                    ActivityIndicator activityIndicator = new ActivityIndicator { IsRunning = true, IsVisible = true, IsEnabled = true };
                    ExcelEngine excelEngine = new ExcelEngine();
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2013;


                    IWorkbook workbook = application.Workbooks.Open(f.GetStream());

                    //Access first worksheet from the workbook.
                    IWorksheet worksheet = workbook.Worksheets[1];
                    List<InventDB> list = null;
                    
                    //Filas.
                    for (int x = 2; x < 1514; x++)
                    {
                        //columnas

                        var strs = worksheet.GetText(x, 2);
                        list = await App.MobileService.GetTable<InventDB>().Where(u => u.codigo == strs).ToListAsync();
                        if (list.Count != 0)
                        {
                            list[0].compra = worksheet.GetText(x, 8);
                            list[0].costo = worksheet.GetText(x, 6);
                            list[0].descompostura = worksheet.GetText(x, 11);
                            list[0].marca = worksheet.GetText(x, 4);
                            list[0].modelo = worksheet.GetText(x, 5);
                            list[0].nombre = worksheet.GetText(x, 3);
                            list[0].observaciones = worksheet.GetText(x, 9);
                            list[0].origen = worksheet.GetText(x, 7);
                            list[0].pertenece = worksheet.GetText(x, 10);
                            list[0].lugar = "Almacen";
                            list[0].serie = worksheet.GetText(x, 1);
                            await App.MobileService.GetTable<InventDB>().UpdateAsync(list[0]);
                        }
                        else
                        {
                            var id = Guid.NewGuid().ToString();
                            InventDB n = new InventDB
                            {

                                codigo = strs,
                                serie = worksheet.GetText(x, 1),
                                compra = worksheet.GetText(x, 8),
                                costo = worksheet.GetText(x, 6),
                                descompostura = worksheet.GetText(x, 11),
                                marca = worksheet.GetText(x, 4),
                                modelo = worksheet.GetText(x, 5),
                                nombre = worksheet.GetText(x, 3),
                                observaciones = worksheet.GetText(x, 9),
                                origen = worksheet.GetText(x, 7),
                                pertenece = worksheet.GetText(x, 10),
                                ID = id,
                                lugar = "Almacen"
                            };
                            await App.MobileService.GetTable<InventDB>().InsertAsync(n);
                        }

                    }
                    activityIndicator.IsRunning = false;
                
            }
            
        }

        private async void MenuOp(object sender, EventArgs e)
        { //Despegar menu de  3 opciones Ingresar, Retirar, Detalles
            string res = await DisplayActionSheet("Opciones", "Cancelar", null, "Agregar Nuevo Producto", "Reingresar Producto", "Salida","Actualizar BD");
            switch (res)
            {
                case "Agregar Nuevo Producto":
                    //Abrir vista/pagina Detalles del Producto
                    Navigation.PushAsync(new NuevoProducto());
                    
                    break;
                case "Reingresar Producto":
                    //Abrir vista/pagina Ingresar Producto
                    Navigation.PushAsync(new IngresarProducto(  ));
                    break;
                case "Salida":
                    //Abrir vista/pagina Retirar Producto
                    Navigation.PushAsync(new RetirarProducto(this));
                    break;
                case "Actualizar BD":
                    Navigation.PushAsync(new UpdateBD());
                    break;



            }
        }

        private void pickerBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            tipoBusqueda = pickerBuscar.SelectedItem as string;
        }

        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }

        async void search_TextChanged(Object sender, TextChangedEventArgs e)
        {
            if(search.Text=="")
            {
                var usuario = await App.MobileService.GetTable<InventDB>().ToListAsync();
                postListView.ItemsSource = usuario;
            }
        }
    }
}