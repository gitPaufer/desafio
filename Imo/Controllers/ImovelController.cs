using Imo.Data;
using Imo.Models;
using Imo.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Graph;
using Newtonsoft.Json.Linq;
using Microsoft.Graph.Models;
using System.Text.Json;
using Newtonsoft.Json;

namespace Imo.Controllers
{
    public class ImovelController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://localhost:5140/api/Imovel");
                var response = await client.SendAsync(request);
                var imovelDados = await response.Content.ReadAsStringAsync();
                var imoveis = JsonConvert.DeserializeObject<Imovel[]>(imovelDados);

                var listaImoveis = imoveis.ToList();

                return View(listaImoveis);
            }
        }
        public async Task<IActionResult> IndexImovel(string tipoNegocio, string tipoImovel)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri($"http://localhost:5140/api/Imovel?tipoNegocio={tipoNegocio}&tipoImovel={tipoImovel}");
                var response = await client.SendAsync(request);
                var imovelDados = await response.Content.ReadAsStringAsync();
                var imoveis = JsonConvert.DeserializeObject<Imovel[]>(imovelDados);

                var listaImoveis = imoveis.ToList();

                return View(listaImoveis);
            }
        }

        public async Task<IActionResult> ViewImovel(Guid id)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://localhost:5140/api/Imovel/" + id);
                var response = await client.SendAsync(request);

                var imovelDados = await response.Content.ReadAsStringAsync();
                var imovel = JsonConvert.DeserializeObject<Imovel>(imovelDados);

                return View(imovel);

            }
            return RedirectToAction("Index");
        }


        private Guid idImovelSelectionado;
        private string pathImage;
        private readonly ImoContext mvcImoContext;
        private readonly IWebHostEnvironment mvcWebHostEnvironment;
        public ImovelController(ImoContext mvcImoContext, IWebHostEnvironment mvcWebHostEnvironment)
        {
            this.mvcImoContext = mvcImoContext;
            this.mvcWebHostEnvironment = mvcWebHostEnvironment;
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddImovelViewModel addImovelRequest)
        {
            var imovel = new Imovel()
            {
                Id = Guid.NewGuid(),
                Nome = addImovelRequest.Nome,
                Tipo = addImovelRequest.Tipo,
                Localizacao = addImovelRequest.Localizacao,
                Preco = addImovelRequest.Preco,
                Descricao = addImovelRequest.Descricao,
                ContactoFone = addImovelRequest.ContactoFone,
                ContactoEmail = addImovelRequest.ContactoEmail,
                TipoNegocio = addImovelRequest.TipoNegocio,
                Date = DateTime.Now,
            };
            await mvcImoContext.Imovels.AddAsync(imovel);
            await mvcImoContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Imovel_Foto model)
        {
            var imovel = await mvcImoContext.Imovel_Fotos.FindAsync(model.Id);
            if (imovel != null)
            {
                mvcImoContext.Imovel_Fotos.Remove(imovel);
                await mvcImoContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var imovel = await mvcImoContext.Imovels.FirstOrDefaultAsync(x => x.Id == id);
            idImovelSelectionado = id;

            if (imovel != null)
            {
                var viewlModel = new UpdateImovelViewModel()
                {
                    Id = imovel.Id,
                    Nome = imovel.Nome,
                    Tipo = imovel.Tipo,
                    Localizacao = imovel.Localizacao,
                    Preco = imovel.Preco,
                    Descricao = imovel.Descricao,
                    ContactoFone = imovel.ContactoFone,
                    ContactoEmail = imovel.ContactoEmail,
                    TipoNegocio = imovel.TipoNegocio

                };
                return await Task.Run(() => View("View", viewlModel));
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> View(UpdateImovelViewModel model)
        {
            var imovel = await mvcImoContext.Imovels.FindAsync(model.Id);

            if (imovel != null)
            {
                imovel.Nome = model.Nome;
                imovel.Tipo = model.Tipo;
                imovel.Descricao = model.Descricao;
                imovel.ContactoEmail = model.ContactoEmail;
                imovel.ContactoFone = model.ContactoFone;
                imovel.Preco = model.Preco;
                imovel.Localizacao = model.Localizacao;
                imovel.Date = model.Date;
                imovel.TipoNegocio = model.TipoNegocio;

                await mvcImoContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateImovelViewModel model)
        {
            var imovel = await mvcImoContext.Imovels.FindAsync(model.Id);
            if (imovel != null)
            {
                mvcImoContext.Imovels.Remove(imovel);
                await mvcImoContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        //public IActionResult UploadImagem(IEnumerable<IFormFile> files)
        public async Task<IActionResult> UploadImagem(IEnumerable<IFormFile> files, string idImovel)
        {
            IFormFile imagemCarregada = files.FirstOrDefault();
            if (imagemCarregada != null)
            {
                pathImage = mvcWebHostEnvironment.WebRootPath;


                string pathSaveImage = pathImage + "\\image\\";
                string newNameImage = imagemCarregada.FileName;

                if (!Directory.Exists(pathSaveImage))
                {
                    Directory.CreateDirectory(pathSaveImage);
                }

                using (var stream = System.IO.File.Create(pathSaveImage + newNameImage))
                {
                    imagemCarregada.CopyToAsync(stream);
                }


                MemoryStream ms = new MemoryStream();
                imagemCarregada.OpenReadStream().CopyTo(ms);

                Imovel_Foto file = new Imovel_Foto()
                {
                    Id = Guid.NewGuid(),
                    IdImovel = idImovel,
                    DescricaoFoto = imagemCarregada.FileName,
                    PathFoto = pathSaveImage + "\\" + imagemCarregada.FileName,
                    ContentType = imagemCarregada.ContentType,
                    Date = DateTime.Now,
                    IdUser = 1
                };

                mvcImoContext.Imovel_Fotos.Add(file);
                mvcImoContext.SaveChanges();
            }

            return RedirectToAction("Image");
        }
        public IActionResult Visualizar(Guid id)
        {
            ViewBag.IdImovel = id;
            var arquivosBanco = mvcImoContext.Imovel_Fotos.FirstOrDefault(a => a.Id == id);

            return File(arquivosBanco.PathFoto, arquivosBanco.DescricaoFoto);
        }

        public IActionResult Image(string idImovel)
        {
            ViewBag.IdImovel = idImovel;
            var files = mvcImoContext.Imovel_Fotos.ToList();
            return View(files);
        }

        //public IActionResult ViewImagemImovel(Guid id)
        //{
        //    var imovel = mvcImoContext.Imovel_Fotos.Include(i => i.PathFoto).FirstOrDefault(x => x.Id == id);

        //    if (imovel != null)
        //    {
        //        return View(imovel);
        //    }

        //    return RedirectToAction("Index");
        //}

    }
}
