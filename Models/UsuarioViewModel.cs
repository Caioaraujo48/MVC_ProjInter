namespace ProjInter_MVC.Models;

public class UsuarioViewModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordString { get; set; }
    //public List<Jogo> JogosFavoritos{ get; set; }
    public string Perfil { get; set; }
    public string Email { get; set; }
    public string Token{ get; set; } = string.Empty;
}