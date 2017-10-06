using WebApi.Formats;
using WebApi.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Configuration;
using System.Web.Http;

namespace WebApi
{
    /// <summary>
    /// Clase que activa la funcionalidad de los tokens 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configura el servidor para realizar autenticacion por tokens
        /// </summary>
        /// <param name="app">Informacion del servidor</param>
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            ConfigureTokenGeneration(app);//Metodo que genera los tokens
            ConfigureTokenConsumption(app);//Metodo para leer los tokens
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        /// <summary>
        /// Metodo para procesar la solicitud de la generacion de un token
        /// </summary>
        /// <param name="app">Configuracion del servidor</param>
        private void ConfigureTokenGeneration(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),//Direccion para solicitar el token
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(1),//La duracion del token
                Provider = new AuthorizationProvider(),//Validacion del logeo 
                AccessTokenFormat = new FormatoJWT("GasStationPharmacy")//Nombre del servidor
            };
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }

        /// <summary>
        /// Metodo para validar un token cuando el usuario hace una solicitud
        /// </summary>
        /// <param name="app">Configuracion del servidor</param>
        private void ConfigureTokenConsumption(IAppBuilder app) {
            
            //Cuestiones de seguridad para hacer el token
            var issuer = "GasStationPharmacy";
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            //Realiza la verificacion del token
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },//Se verifica la audiencia del token
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)}
                }
            );
        }
    }
}