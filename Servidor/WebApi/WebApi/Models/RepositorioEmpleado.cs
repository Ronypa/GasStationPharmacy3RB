using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class RepositorioEmpleado
    {
        private static GasStationPharmacyEntities dataContext = new GasStationPharmacyEntities();
  
        public static EMPLEADO GetEmpleado(int cedula, string contrasena)
        {
            dataContext.Configuration.ProxyCreationEnabled = false;
            var query = from EMPLEADO in dataContext.EMPLEADOes
                        where EMPLEADO.Cedula == cedula &&  EMPLEADO.Contraseña==contrasena
                        select EMPLEADO;
            return query.SingleOrDefault();
        }
    }
}