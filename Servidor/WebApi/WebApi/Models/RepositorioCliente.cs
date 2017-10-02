using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class RepositorioCliente
    {
        private static GasStationPharmacyEntities dataContext = new GasStationPharmacyEntities();

        public static CLIENTE GetCliente(int cedula, string contrasena)
        {
            dataContext.Configuration.ProxyCreationEnabled = false;
            var query = from CLIENTE in dataContext.CLIENTEs
                        where CLIENTE.Cedula == cedula && CLIENTE.Contraseña == contrasena
                        select CLIENTE;
            return query.SingleOrDefault();
        }
    }
}