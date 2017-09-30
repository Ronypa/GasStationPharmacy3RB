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

        public IEnumerable<USUARIO> Get()
        {
            using (GasStationPharmacyEntities entities = new GasStationPharmacyEntities())
            {
                return entities.USUARIOS.ToList();
            }

        }


        public USUARIO Get(string id)
        {
            using (GasStationPharmacyEntities entities = new GasStationPharmacyEntities())
            {
                return entities.USUARIOS.FirstOrDefault(d => d.email == id);
            }

        }


    }
}
