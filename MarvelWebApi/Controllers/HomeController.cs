using MarvelWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MarvelWebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SelecionaPersonagem(string nomePersonagem)
        {
            List<Personagem> personagem;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    string ts = DateTime.Now.Ticks.ToString();
                    string publicKey = "";
                    string privateKey = "";
                    string baseUrl = "http://gateway.marvel.com/v1/public/";
                    string hash = GerarHash(ts, publicKey, privateKey);

                    HttpResponseMessage response = client.GetAsync(
                        baseUrl +
                        $"characters?ts={ts}&apikey={publicKey}&hash={hash}&" +
                        $"nameStartsWith={Uri.EscapeUriString(nomePersonagem)}").Result;

                    response.EnsureSuccessStatusCode();
                    string conteudo =
                        response.Content.ReadAsStringAsync().Result;

                    dynamic resultado = JsonConvert.DeserializeObject(conteudo);

                    personagem = new List<Personagem>();
                    if (resultado.data.results.Count == 0)
                    {
                        personagem.Add(new Personagem() { MsgErro = "Nenhum personagem encontrado com esse nome." });
                    }
                    else
                    {
                        foreach (var item in resultado.data.results)
                        {
                            personagem.Add(new Personagem() { Nome = item.name, Descricao = item.description, UrlImagem = item.thumbnail.path + "." + item.thumbnail.extension, UrlWiki = item.urls[1].url });
                        }
                    }
                }

                return PartialView("_Personagem", personagem);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GerarHash(
            string ts, string publicKey, string privateKey)
        {
            byte[] bytes =
                Encoding.UTF8.GetBytes(ts + privateKey + publicKey);
            var gerador = MD5.Create();
            byte[] bytesHash = gerador.ComputeHash(bytes);
            return BitConverter.ToString(bytesHash)
                .ToLower().Replace("-", String.Empty);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}