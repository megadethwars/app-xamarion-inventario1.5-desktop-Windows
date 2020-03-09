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
    public partial class UpdateBD : ContentPage
    {
        Plugin.FilePicker.Abstractions.FileData f;
        public UpdateBD()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            f = await CrossFilePicker.Current.PickFile();
            nombre.Text = f.FileName;
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            if (f != null)
            {
                

                
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2013;


                IWorkbook workbook = application.Workbooks.Open(f.GetStream());

                //Access first worksheet from the workbook.
                IWorksheet worksheet = workbook.Worksheets[1];
                List<InventDB> list = null;
               
                //Filas.
                
                for (int x = 2; x < 1577; x++)
                {
                    //columnas

                    var strs = worksheet.GetText(x, 1);
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
                    porcent.Text = worksheet.GetText(x, 1)+"  "+(x * 100 / 1576).ToString()+"%";

                }
                
            }
        }
    }
}