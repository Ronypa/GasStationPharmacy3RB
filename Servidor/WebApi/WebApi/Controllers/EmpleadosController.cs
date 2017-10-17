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
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Consulta todos los empleados
        /// </summary>
        /// <returns>Una lista con todos los empleados y un http status de de ok si se 
        /// proceso la solicitud o unauthorized en caso contrario
        /// </returns>
        [Authorize(Roles = "Administrador, Cliente")]
        [HttpPost]
        [Route("consultarEmpleados")]
        public HttpResponseMessage ConsultarEmpleado(ObjGeneral compania)
        {
            List<EmpleadoRecibo> empleados = ProcesadorEmpleado.ProcesoConsultarEmpleados(compania.opcion);
            if (empleados == null)
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return responseError;
            }
            //Encontro la lista de empleados 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, empleados);
            return response;
        }

        /// <summary>
        /// Realiza el proceso de agregar un cliente
        /// </summary>
        /// <param name="cliente">Modelo del cliente a agregar</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("agregarEmpleado")]
        public HttpResponseMessage AgregarEmpleado(EmpleadoRecibo empleado)
        {
            if (empleado == null || !ProcesadorEmpleado.ProcesoAgregarEmpleado(empleado))
            {
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        /// <summary>
        /// Realiza el proceso de actualizar los datos de un empleado
        /// </summary>
        /// <param name="empleado">Modelo del cliente a agregar</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("actualizarEmpleado")]
        public HttpResponseMessage ActualizarEmpleado(EmpleadoRecibo empleado)
        {
            if (empleado == null || !ProcesadorEmpleado.ProcesoActualizarEmpleado(empleado))
            {
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Borrado de un empleado
        /// </summary>
        /// <param name="cedula">cedula del empleado a borrar</param>
        /// <returns>HTTP OK si lo borrra</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [Route("borrarEmpleados")]
        public HttpResponseMessage BorrarEmpleado([FromBody]string cedula)
        {
            if (!ProcesadorEmpleado.ProcesoBorrarEmpleado(int.Parse(cedula)))
            {//No se proceso bien la solicitud
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseError;
            } 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}