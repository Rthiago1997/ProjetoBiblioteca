public class Pessoa
{
    public string Nome { get; set; }

    public Pessoa(string nome)
    {
        Nome = nome;
    }
}

public class Usuario : Pessoa
{
    public string Matricula { get; set; }

    public Usuario(string nome, string matricula) : base(nome)
    {
        Matricula = matricula;
    }

    public override string ToString()
    {
        return $"{Nome} (Matrícula: {Matricula})";
    }
}
