using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiWeb.Models;

namespace ApiWeb.Controllers
{
    public class LoginController : ApiController
    {
        [Authorize]
        public IEnumerable<EMPLEADO> Get()
        {
            using (GasStationPharmacyEntities entities = new GasStationPharmacyEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                return entities.EMPLEADOes.ToList();
            }

        }


        public COMPAÑIA Get(string id)
        {
            using (GasStationPharmacyEntities entities = new GasStationPharmacyEntities())
            {
                return entities.COMPAÑIA.FirstOrDefault(d => d.Nombre == id);
            }

        }


    }
}
