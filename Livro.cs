using System;

public class Livro
{
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public string ISBN { get; set; }
    private int quantidade;

    public int Quantidade
    {
        get => quantidade;
        set => quantidade = value >= 0 ? value : throw new ArgumentException("Quantidade não pode ser negativa");
    }

    public Livro(string titulo, string autor, string isbn, int quantidade)
    {
        Titulo = titulo;
        Autor = autor;
        ISBN = isbn;
        Quantidade = quantidade;
    }

    public override string ToString() => $"{Titulo} por {Autor} - ISBN: {ISBN} - Disponíveis: {Quantidade}";
}