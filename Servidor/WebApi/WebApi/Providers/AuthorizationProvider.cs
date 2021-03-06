﻿using GasStationPharmacy.Processors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GasStationPharmacy.Models;


namespace WebApi.Providers
{
    /// <summary>
    /// Clase que valida el logeo de un usuario
    /// </summary>
    public class AuthorizationProvider : OAuthAuthorizationServerProvider
    {

        /// <summary>
        /// Extrae la infromacion de logeo de los usuarios ya sean empleados o clientes
        /// </summary>
        /// <param name="context">Informacion sobre la solicitud de logeo, tiene el 
        /// password, nombre de usuario y tipo de solicitud(empleado o cliente)</param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //Se extrae la informacion del context
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {context.TryGetFormCredentials(out clientId, out clientSecret);}
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Se valida la solicitud de logeo y si es correcta se envia a generar el token
        /// </summary>
        /// <param name="context">Informacion sobre la solicitud de logeo, tiene el 
        /// password, nombre de usuario y tipo de solicitud(empleado o cliente)</param>
        /// <returns></returns>
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var identity = new ClaimsIdentity("JWT");
            var admin = "false";
            ObjGeneral compania = null;
            //Cuando se está intentando logear un cliente
            if (context.ClientId=="cliente") {
                if (!ProcesadorCliente.ProcesoLogearCliente(int.Parse(context.UserName), context.Password))
                {
                    context.SetError("Login incorrecto", "Usuario y/o contraseña incorrecto");
                    return Task.FromResult<object>(null);
                }
                else {
                    identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Cliente"));
                    
                    //Se genera el ticket del token si el logeo es correcto
                    var ticket = new AuthenticationTicket(identity, null);
                    context.Validated(ticket);
                    return Task.FromResult<object>(null);
                }
            }

            //Cuando se esta intentando logear un empleado
            else if (context.ClientId == "empleado") {
                List<string> roles = ProcesadorEmpleado.ProcesoLogearEmpleado(int.Parse(context.UserName), context.Password);
                if (roles==null)
                {
                    context.SetError("Login incorrecto", "Usuario y/o contraseña incorrecto");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                    foreach (string rol in roles)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, rol));
                        if (rol == "Administrador") {
                            admin = "true";
                        }
                    }
                }
                compania = ProcesadorEmpleado.ProcesoConsultarCompañia(int.Parse(context.UserName));
                var props = new AuthenticationProperties(new Dictionary<string, string>
                    { {"admin", admin},{"compañia",compania.opcion}, { "sucursal", compania.opcion2 } });
                //Se genera el ticket del token si el logeo es correcto
                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
                return Task.FromResult<object>(null);
            }
            //Cuando es una solicitud incorrecta
            else {
                context.SetError("Login incorrecto", "Id de solicitud incorrecto");
                return Task.FromResult<object>(null);
            }
        }
    }
}