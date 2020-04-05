using System;
using System.Collections.Generic;
using System.Text;
using Inventario2.Models;
using System.Threading.Tasks;

namespace Inventario2.Services
{
    public class MovementService
    {
        public static async Task<StatusMessage> postmovement(string objeto)
        {
            try
            {
                var status = await HttpMethods.Post(Global.url + "postmove", objeto);
                return status;
            }
            catch
            {
                return null;
            }


        }
    }
}
