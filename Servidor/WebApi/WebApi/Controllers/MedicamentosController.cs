using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GasStationPharmacy.Models;
using GasStationPharmacy.Processors;

namespace WebApi.Controllers
{
    public class MedicamentosController : ApiController
    {
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todos los medicamentos de la base de datos 
        /// </summary>
        /// <returns>HTTP ok con la lista de medicamentos</returns>
        [Authorize(Roles = "Administrador,Cliente")]
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
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, medicamentos);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Borra un medicamento
        /// </summary>
        /// <param name="obj">nombre del medicamento a borrar</param>
        /// <returns>HTTP ok si lo borra</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("borrarMedicamento")]
        public HttpResponseMessage BorrarMedicamento(ObjGeneral obj)
        {
            if (!ProcesadorMedicamento.ProcesoBorrarMedicamento(obj.opcion))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            } 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Agrega un medicamento nuevo
        /// </summary>
        /// <param name="medicamento">informacion del medicamento a agregar</param>
        /// <returns>HTTP ok si lo crea</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("agregarMedicamento")]
        public HttpResponseMessage AgregarMedicamento(Medicamento medicamento)
        {
            if (medicamento == null || !ProcesadorMedicamento.ProcesoAgregarMedicamento(medicamento))
            {
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Actualiza un medicamento
        /// </summary>
        /// <param name="medicamento">informacion del medicamento a actualizar</param>
        /// <returns>HTTP ok si lo actualiza</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("actualizarMedicamento")]
        public HttpResponseMessage ActualizarMedicamento(Medicamento medicamento)
        {
            if (!ProcesadorMedicamento.ProcesoActualizarMedicamento(medicamento))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}