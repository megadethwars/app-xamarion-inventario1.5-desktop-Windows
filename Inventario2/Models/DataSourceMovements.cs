using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Inventario2.Models
{
    public class DataSourceMovements
    {
        public static ObservableCollection<ModelMovements> collection;

        static DataSourceMovements()
        {
        }
        public static void persist(List<ModelMovements> collection)
        {
            //do something here
        }

        public static void initializeData(List<ModelMovements> listdata)
        {
            collection = new ObservableCollection<ModelMovements>(listdata);
        }

    }
}
