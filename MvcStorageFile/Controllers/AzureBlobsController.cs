using Microsoft.AspNetCore.Mvc;
using MvcStorage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStorage.Controllers
{
    public class AzureBlobsController : Controller
    {
        ServiceStorageBlobs ServiceBlobs;

        public AzureBlobsController(ServiceStorageBlobs serviceblobs)
        {
            this.ServiceBlobs = serviceblobs;
        }

        public async Task<IActionResult> Index()
        {
            return View(await this.ServiceBlobs.GetContainersAsync()); 
        }

        public IActionResult CreateContainer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateContainer(string containername)
        {
            await this.ServiceBlobs.CreateContainerAsync(containername);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteContainer(String containername)
        {
            await this.ServiceBlobs.DeleteContainerAsync(containername);
            return RedirectToAction("Index");
        }
    }
}
