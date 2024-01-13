using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace service_layer
{
    internal class Program
    {
        private static SLManager sLManager;
        static  void Main(string[] args)
        {
            

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;

            try
            {
                ConectarSL();
                CrearSocioDeNegocio();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR : {ex.Message}");
            }
            finally
            { 
                Console.ReadKey();
            }
        }

        private static void CrearSocioDeNegocio()
        {
            try
            {
                var socioNegocio = new { CardCode = "CL70393849", CardName = "SEMILLERO 2023", FederalTaxID = "70393849" };

                string responseSocio = sLManager.Post("BusinessPartners", socioNegocio);
                Console.WriteLine($"SOCIO DE NEGOCIO CREADO EXITOSAMENTE!");
                Console.WriteLine(responseSocio);
            }
            catch (Exception ex)
            {
                throw new Exception($"[CREACIÓN DE SOCIO] - {ex.Message}");
            }
        }

        private static void ConectarSL()
        {
            try
            {
                string serviceLayerUrl = "https://192.168.1.14:50000/b1s/v1";
                LoginData loginData = new LoginData()
                {
                    CompanyDB = "02_LARAMA_PRODUCCION",
                    UserName = "DESARROLLO02",
                    Password = "Pisco.2030"
                };

                sLManager = new SLManager(serviceLayerUrl, loginData);
                Console.WriteLine($"[CONEXIÓN SL] - SESSION ID: {sLManager.GetSessionId()}");
            }
            catch (Exception ex)
            {
                throw new Exception($"[CONEXIÓN SL] - {ex.Message}");
            }
        }
    }
}
