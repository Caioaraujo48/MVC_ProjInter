using ProjInter_MVC.Models.Enuns;

namespace ProjInter_MVC.Models;

public class JogoViewModel{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Criador { get; set; }
    public string Empresa { get; set; }
    public DateTime Lançamento { get; set; }       
    public GeneroEnum Genero { get; set; }    
    public byte[]? Foto { get; set; }
}                  