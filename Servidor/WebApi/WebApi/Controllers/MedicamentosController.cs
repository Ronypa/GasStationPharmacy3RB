using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GasStationPharmacy.Models;
using GasStationPharmacy.Processors;

namespace WebApi.Controllers
{
    public class MedicamentosController : ApiController
    {

        [HttpGet]
        [Route("consultarMedicamentos")]
        public HttpResponseMessage ConsultarMedicamentos()
        {
            List<Medicamento> medicamentos = ProcesadorMedicamento.ProcesoConsultarMedicamentos();
            if (medicamentos == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, medicamentos);
            return response;
        }

        [HttpPost]
        [Route("borrarMedicamento")]
        public HttpResponseMessage BorrarMedicamento(ObjGeneral obj)
        {
            if (!ProcesadorMedicamento.ProcesoBorrarMedicamento(obj.opcion))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

    }
}
