using GasStationPharmacy.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GasStationPharmacy.Processors;

namespace GasStationPharmacy.Controllers
{
    /// <summary>
    /// Controlador que acepta peticiones http sobre la tabla de empleados
    /// </summary>
    public class EmpleadosController : ApiController
    {

        /// <summary>
        /// Consulta todos los empleados
        /// </summary>
        /// <returns>Una lista con todos los empleados y un http status de de ok si se 
        /// proceso la solicitud o unauthorized en caso contrario
        /// </returns>
        [HttpGet]
        [Route("consultarEmpleados")]
        public HttpResponseMessage ConsultarEmpleado()
        {
            List<Empleado> empleados = ProcesadorEmpleado.ProcesoConsultarEmpleados();
            if (empleados == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de empleados 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, empleados);
            return response;
        }


        [HttpPost]
        [Route("borrarEmpleados")]
        public HttpResponseMessage BorrarEmpleado([FromBody]string cedula)
        {
            if (!ProcesadorEmpleado.ProcesoBorrarEmpleado(int.Parse(cedula)))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            //Encontro la lista de clientes 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}