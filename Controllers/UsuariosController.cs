using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjInter_MVC.Models;

namespace ProjInter_MVC.Controllers;

public class UsuariosController : Controller
{
    public string uriBase = "http://arielchazzeio.somee.com/JogosFav/Usuarios/";

    [HttpGet]
    public ActionResult Index()
    {
        return View("CadastrarUsuario");
    }
    
    [HttpPost]
        public async Task<ActionResult> RegistrarAsync(UsuarioViewModel u)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string uriComplementar = "Registrar";

                var content = new StringContent(JsonConvert.SerializeObject(u));
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uriBase + uriComplementar, content);

                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Mensagem"] =
                        string.Format("Usuário {0} Registrado com sucesso! Faça o login para acessar.", u.Username);
                    return View("AutenticarUsuario");
                }
                else
                {
                    throw new System.Exception(serialized);
                }
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult IndexLogin()
        {
            return View("AutenticarUsuario");
        }

        [HttpPost]
        public async Task<ActionResult> AutenticarAsync(UsuarioViewModel u)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string uriComplementar = "Autenticar";

                var content = new StringContent(JsonConvert.SerializeObject(u));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uriBase + uriComplementar, content);

                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    UsuarioViewModel uLogado = JsonConvert.DeserializeObject<UsuarioViewModel>(serialized);
                    HttpContext.Session.SetString("SessionTokenUsuario", uLogado.Token);
                    HttpContext.Session.SetString("SessionUsername", u.Username);
                    TempData["Mensagem"] = string.Format("Bem-vindo {0}!!!", uLogado.Username);
                    return RedirectToAction("Index", "Personagens");
                }
                else
                {
                    throw new System.Exception(serialized);
                }
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return IndexLogin();
            }
        }
        [HttpGet]
        public async Task<ActionResult> IndexInformacoesAsync()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                //Novo: Recuperação informação da sessão
                string login = HttpContext.Session.GetString("SessionUsername");
                string uriComplementar =
                $"GetByLogin/{login}";
                HttpResponseMessage response = await httpClient.GetAsync(uriBase +
                uriComplementar);
                string serialized = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    UsuarioViewModel u = await Task.Run(() =>
                    JsonConvert.DeserializeObject<UsuarioViewModel>(serialized));
                    return View(u);
                }
                else
                {
                    throw new System.Exception(serialized);
                }
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
}