using Microsoft.AspNetCore.Mvc;
using MvcStorage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStorage.Controllers
{
    public class QueueController : Controller
    {
        ServiceQueueBus ServiceBus;

        public QueueController(ServiceQueueBus servicebus)
        {
            this.ServiceBus = servicebus;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(String mensaje
            , String accion)
        {
            if (accion.ToLower() == "mensaje")
            {
                await this.ServiceBus.SendMessage(mensaje);
            }else if (accion.ToLower() == "batch")
            {
                await this.ServiceBus.SendBatchMessages();
            }else if (accion.ToLower() == "recibir")
            {
                await this.ServiceBus.ReceiveMessages();
            }
            return View();
        }
    }
}
