using System;

namespace BibliotecaApp
{

public class Livro
{
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public string ISBN { get; set; }
    private int quantidade;

    public int Quantidade
    {
        get => quantidade;
        set
        {
            if (value < 0)
                throw new ArgumentException("Quantidade não pode ser negativa.");
            quantidade = value;
        }
    }

    public Livro(string titulo, string autor, string isbn, int quantidade)
    {
        Titulo = titulo;
        Autor = autor;
        ISBN = isbn;
        Quantidade = quantidade;
    }

    public bool Disponivel() => Quantidade > 0;
}

}
