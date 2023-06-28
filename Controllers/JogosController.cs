using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjInter_MVC.Models;

namespace ProjInter_MVC.Controllers;

public class JogosController : Controller
{
    public string uriBase = "http://arielchazzeio.somee.com/JogosFav/Jogos/";

        [HttpGet]
        public async Task<ActionResult> IndexAsync()
        {
            try
            {

                string uriComplementar = "GetAll";//Conter o nome do método
                HttpClient httpClient = new HttpClient(); // Fará toda a transição de requisição
                string token = HttpContext.Session.GetString("SessionTokenJogo");//Token recuperado da Sessão
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); //Requisição com o token para carregamento do header

                HttpResponseMessage response = await httpClient.GetAsync(uriBase + uriComplementar); //Guarda a resposta da Requisição
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<JogoViewModel> listaPersonagens = await Task.Run(() =>
                    JsonConvert.DeserializeObject<List<JogoViewModel>>(serialized));

                    return View(listaPersonagens);

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
        [HttpPost]
        public async Task<ActionResult> CreateAsync(JogoViewModel p)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenJogo");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonConvert.SerializeObject(p));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uriBase, content);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Mensagem"] = string.Format("Jogo {0}, Id {1} salvo com sucesso!", p.Nome, serialized);
                    return RedirectToAction("Index");
                }
                else
                    throw new System.Exception(serialized);
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Create");
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> DetailsAsync(int? id)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenJogo");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await httpClient.GetAsync(uriBase + id.ToString());
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JogoViewModel p = await Task.Run(() =>
                    JsonConvert.DeserializeObject<JogoViewModel>(serialized));
                    return View(p);
                }
                else
                    throw new System.Exception(serialized);
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public async Task<ActionResult> EditAsync(int? id)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenJogo");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await httpClient.GetAsync(uriBase + id.ToString());

                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JogoViewModel p = await Task.Run(() =>
                    JsonConvert.DeserializeObject<JogoViewModel>(serialized));
                    return View(p);
                }
                else
                    throw new System.Exception(serialized);
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<ActionResult> EditAsync(JogoViewModel p)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenJogo");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var content = new StringContent(JsonConvert.SerializeObject(p));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                
                HttpResponseMessage response = await httpClient.PutAsync(uriBase, content);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Mensagem"] = string.Format("Jogo {0}, Genero {1} atualizado com sucesso!", p.Nome, p.Genero);

                    return RedirectToAction("Index");
                }
                else
                    throw new System.Exception(serialized);
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string token = HttpContext.Session.GetString("SessionTokenJogo");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await httpClient.DeleteAsync(uriBase + id.ToString());
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Mensagem"] = string.Format("Jogo Id {0} removido com sucesso!", id);
                    return RedirectToAction("Index");
                }
                else
                   throw new System.Exception(serialized); 
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

       /* [HttpPost]
        public async Task<ActionResult> EnviarFoto(JogoViewModel u)
        {
            try
            {
                if (Request.Form.Files.Count == 0)
                    throw new System.Exception("Selecione o arquivo");
                else
                {
                    var file =Request.Form.Files[0];
                    var fileName = Path.GetFileName(file.FileName);
                    string nomeArquivoSemExtensao = Path.GetFileNameWithoutExtension(fileName);
                    var extensao = Path.GetExtension(fileName);

                    if (extensao != ".jpg" && extensao != "jpeg" && extensao != ".png")
                        throw new System.Exception("O arquivo selecionado não é uma foto");
                    
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        u.Foto = ms.ToArray();
                    }
                    HttpClient httpClient = new HttpClient();
                        string token = HttpContext.Session.GetString("SessionTokenJogo");
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                        string uriComplementar = "AtualizarFoto";
                        var content = new StringContent(JsonConvert.SerializeObject(u));
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        HttpResponseMessage response = await httpClient.PutAsync(uriBase + uriComplementar, content);
                        string serialized = await response.Content.ReadAsStringAsync();

                        if(response.StatusCode == System.Net.HttpStatusCode.OK)
                            TempData["Mensagem"] = "Foto enviada com sucesso";
                        else
                            throw new System.Exception(serialized);   
                }
            }
            catch(System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> BaixarFoto()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string login = HttpContext.Session.GetString("SessionUsername");
                string uriComplementar = $"GetByLogin/{login}";
                HttpResponseMessage response = await httpClient.GetAsync(uriBase + uriComplementar);
                string serialized = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JogoViewModel viewModel = await 
                        Task.Run(() => 
                    JsonConvert.DeserializeObject<JogoViewModel>(serialized));

                    string contentType = System.Net.Mime.MediaTypeNames.Application.Octet;

                    byte[] fileBytes = viewModel.Foto;
                    string fileName = $"Foto{viewModel.Nome}_{DateTime.Now:ddMMyyyyHHmmss}.png"; //
                    return File(fileBytes, contentType, fileName);
                }  
                else    
                    throw new System.Exception(serialized);
            }
            catch(System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }*/
}