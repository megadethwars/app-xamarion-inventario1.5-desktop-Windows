using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Inventario2.Models
{
    public class DataSourceDevices
    {

        public static ObservableCollection<ModelDevice> collection;

        static DataSourceDevices()
        {
        }
        public static void persist(List<ModelDevice> collection)
        {
            //do something here
        }

        public static void initializeData(List<ModelDevice> listdata)
        {
            collection = new ObservableCollection<ModelDevice>(listdata);
        }

    }
}
