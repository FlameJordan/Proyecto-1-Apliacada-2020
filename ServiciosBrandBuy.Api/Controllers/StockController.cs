using ServiciosBrandBuy.Api.Models;
using System.Web.Http;

namespace ServiciosBrandBuy.Api.Controllers
{
    public class StockController : ApiController
    {
        [HttpPost]
        public void Post(Productos productoActualizacionStock)
        {
            Data claseData = new Data();

            claseData.actualizarStockProveedor(productoActualizacionStock);
        }
    }
}
