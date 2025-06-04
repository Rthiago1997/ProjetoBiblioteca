using System;

public class Livro
{
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public string ISBN { get; set; }
    private int quantidadeDisponivel;

    public int QuantidadeDisponivel
    {
        get => quantidadeDisponivel;
        set => quantidadeDisponivel = value >= 0 ? value : 0;
    }

    public Livro(string titulo, string autor, string isbn, int quantidade)
    {
        Titulo = titulo;
        Autor = autor;
        ISBN = isbn;
        QuantidadeDisponivel = quantidade;
    }

    public override string ToString()
    {
        return $"{Titulo} - {Autor} (ISBN: {ISBN}) | Disponíveis: {QuantidadeDisponivel}";
    }
}
