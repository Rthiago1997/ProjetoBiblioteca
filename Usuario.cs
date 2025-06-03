public class Usuario : Pessoa
{
    public string Matricula { get; set; }

    public Usuario(string nome, string matricula) : base(nome)
    {
        Matricula = matricula;
    }

    public override string ToString() => $"{Nome} - Matrícula: {Matricula}";
}
