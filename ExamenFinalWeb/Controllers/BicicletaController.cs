using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BAPI.Models;
using RestSharp;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;

namespace ExamenFinalWeb.Controllers
{
    public class BicicletaController : Controller
    {
        RestClient client = new RestClient("https://localhost:7249/api/");
        // GET: BicicletaController
        public ActionResult Index()
        {
            var request = new RestRequest("Bicicletas", Method.Get);
            var response = client.Execute(request);
            List<Bicicleta> bicicletas = JsonConvert.DeserializeObject<List<Bicicleta>>(response.Content);
            return View(bicicletas);
        }

        // GET: BicicletaController/Details/5
        public ActionResult Details(int id)
        {
            var request = new RestRequest("Bicicletas/" + id, Method.Get);
            var response = client.Execute(request);
            Bicicleta bicicleta = JsonConvert.DeserializeObject<Bicicleta>(response.Content);
            return View(bicicleta);
        }

        // GET: BicicletaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BicicletaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection, Bicicleta newbicicleta, IFormFile Imagen)
        {
            try
            {
                if (Imagen != null && Imagen.Length > 0)
                {

                    var fileName = Path.GetFileName(Imagen.FileName);

                    // get the path where the file should be saved
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imagenes", fileName);
                    newbicicleta.Imagen = Imagen.FileName;
                    // check if the file already exists
                    if (System.IO.File.Exists(filePath))
                    {
                        int i = 1;
                        string fileNameOnly = Path.GetFileNameWithoutExtension(filePath);
                        string extension = Path.GetExtension(filePath);
                        while (System.IO.File.Exists(filePath))
                        {
                            fileName = string.Format("{0}({1})", fileNameOnly, i++);
                            newbicicleta.Imagen = fileName+ extension;
                            filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imagenes", fileName + extension);
                        }
                    }
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Imagen.CopyToAsync(stream);
                    }
                }
                
                var request = new RestRequest("Bicicletas", Method.Post);
                string jsonBody = JsonConvert.SerializeObject(newbicicleta);
                request.AddJsonBody(jsonBody);
                var response = client.Execute(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BicicletaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BicicletaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Bicicleta newbicicleta, IFormFile Imagen)
        {
            try
            {
                if (Imagen != null && Imagen.Length > 0)
                {

                    var fileName = Path.GetFileName(Imagen.FileName);

                    // get the path where the file should be saved
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imagenes", fileName);
                    newbicicleta.Imagen = Imagen.FileName;
                    // check if the file already exists
                    if (System.IO.File.Exists(filePath))
                    {
                        int i = 1;
                        string fileNameOnly = Path.GetFileNameWithoutExtension(filePath);
                        string extension = Path.GetExtension(filePath);
                        while (System.IO.File.Exists(filePath))
                        {
                            fileName = string.Format("{0}({1})", fileNameOnly, i++);
                            newbicicleta.Imagen = fileName + extension;
                            filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imagenes", fileName + extension);
                        }
                    }
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Imagen.CopyToAsync(stream);
                    }
                }
                var request = new RestRequest("Bicicletas/" + id, Method.Put);
                string jsonBody = JsonConvert.SerializeObject(newbicicleta);
                request.AddJsonBody(jsonBody);
                var response = client.Execute(request);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BicicletaController/Delete/5
        public ActionResult Delete(int id)
        {
            var request = new RestRequest("Bicicletas/" + id, Method.Get);
            var response = client.Execute(request);
            Bicicleta bicicleta = JsonConvert.DeserializeObject<Bicicleta>(response.Content);
            return View(bicicleta);
        }

        // POST: BicicletaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var request = new RestRequest("Bicicletas/" + id, Method.Delete);
                var response = client.Execute(request);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
