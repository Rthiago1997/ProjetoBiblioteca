using System;

namespace BibliotecaApp
{

public class Emprestimo
{
    public Livro LivroEmprestado { get; set; }
    public Usuario UsuarioEmprestimo { get; set; }
    public PeriodoEmprestimo Periodo { get; set; }
    public bool Finalizado { get; set; } = false;

    public Emprestimo(Livro livro, Usuario usuario, PeriodoEmprestimo periodo)
    {
        LivroEmprestado = livro;
        UsuarioEmprestimo = usuario;
        Periodo = periodo;
    }

    public void Finalizar()
    {
        Finalizado = true;
        LivroEmprestado.Quantidade++;
    }
}
}