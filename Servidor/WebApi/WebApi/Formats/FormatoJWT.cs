using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Tokens;

namespace WebApi.Formats
{
    /// <summary>
    /// Clase que genera el token basado en la informacion dada
    /// </summary>
    public class FormatoJWT : ISecureDataFormat<AuthenticationTicket>
    {
        //Guarda el nombre del servidor necesario para la seguridad del Token
        private readonly string _issuer = string.Empty;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="issuer"> Nombre del servidor necesario para la seguridad del Token</param>
        public FormatoJWT(string issuer){_issuer = issuer;}

        public string Protect(AuthenticationTicket data)
        {
            //Si no hay datos
            if (data == null){throw new ArgumentNullException("data");}

            //Carga la audiencia del token(seguridad)
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            string symmetricKeyAsBase64 = ConfigurationManager.AppSettings["as:AudienceSecret"];
            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);
            var signingKey = new HmacSigningCredentials(keyByteArray);
            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;

            //Se genera el token y se envia al usuario
            var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims,
            issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey);
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.WriteToken(token);
            return jwt;
        }

        /// <summary>
        /// Necesario para la implementacion de los JWT
        /// </summary>
        /// <param name="protectedText"></param>
        /// <returns>Ticket de autenticacion</returns>
        public AuthenticationTicket Unprotect(string protectedText){throw new NotImplementedException();}
    }
}