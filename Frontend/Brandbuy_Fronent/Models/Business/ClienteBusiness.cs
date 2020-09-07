using Brandbuy_Fronent.Models.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brandbuy_Fronent.Models.Business
{
    public class ClienteBusiness
    {
        public IConfiguration Configuration { get; }

        public ClienteBusiness(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public string RegistrarClientes(Cliente c)
        {
            ClienteData clienteData = new ClienteData(this.Configuration);
            var salida = "";
            salida =clienteData.ResgistraCliente(c);
            return salida;
        }

        public string IniciaSesionCliente(Cliente c)
        {
            ClienteData clienteData = new ClienteData(this.Configuration);
            var salida = "";
            salida = clienteData.IniciaSesionCliente(c);
            return salida;
        }
    }
}
