using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GasStationPharmacyDataAccess;


namespace WebApi.Controllers
{
    public class BDController : ApiController
    {
        public IEnumerable<USUARIO> Get() {
            Console.Write("s");
            using (GasStationPharmacyEntities entities = new GasStationPharmacyEntities()){
                return entities.USUARIOS.ToList();
            }

        }


        public USUARIO Get(string id)
        {
            using (GasStationPharmacyEntities entities = new GasStationPharmacyEntities())
            {
                return entities.USUARIOS.FirstOrDefault(d => d.email==id);
            }

        }

    }
}
