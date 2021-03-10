using Microsoft.AspNetCore.Mvc;
using MvcStorage.Models;
using MvcStorage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStorage.Controllers
{
    public class AzureTablesController : Controller
    {
        ServiceStorageTables ServiceTables;

        public AzureTablesController(ServiceStorageTables servicetables)
        {
            this.ServiceTables = servicetables;
        }

        public async Task<IActionResult> Index()
        {
            List<String> empresas = await this.ServiceTables.GetEmpresasAsync();
            ViewData["EMPRESAS"] = empresas;
            return View(await this.ServiceTables.GetClientsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Index(String empresa)
        {
            ViewData["EMPRESAS"] = await this.ServiceTables.GetEmpresasAsync();
            return View(await 
                this.ServiceTables.GetClientesEmpresaAsync(empresa));
        }

        public IActionResult CreateClient()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient(Cliente cliente)
        {
            await this.ServiceTables.CreateClientAsync(cliente.IdCliente
                , cliente.Nombre, cliente.Edad, cliente.Empresa);
            return RedirectToAction("Index");
        }
    }
}
