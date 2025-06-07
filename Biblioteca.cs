using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaApp
{
    
public class Biblioteca
{
    public List<Livro> Livros { get; set; } = new List<Livro>();
    public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public List<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();

    public void CadastrarLivro(Livro livro) => Livros.Add(livro);
    public void CadastrarUsuario(Usuario usuario) => Usuarios.Add(usuario);

    public bool RegistrarEmprestimo(string isbn, string matricula)
    {
        var livro = Livros.FirstOrDefault(l => l.ISBN == isbn && l.Disponivel());
        var usuario = Usuarios.FirstOrDefault(u => u.Matricula == matricula);

        if (livro != null && usuario != null)
        {
            livro.Quantidade--;
            Emprestimos.Add(new Emprestimo(livro, usuario, new PeriodoEmprestimo(DateTime.Now)));
            return true;
        }
        return false;
    }

    public bool RegistrarDevolucao(string isbn, string matricula)
    {
        var emprestimo = Emprestimos.FirstOrDefault(e => e.LivroEmprestado.ISBN == isbn && e.UsuarioEmprestimo.Matricula == matricula && !e.Finalizado);
        if (emprestimo != null)
        {
            emprestimo.Finalizar();
            return true;
        }
        return false;
    }

    public List<Livro> LivrosDisponiveis() => Livros.Where(l => l.Disponivel()).ToList();
    public List<Emprestimo> LivrosEmprestados() => Emprestimos.Where(e => !e.Finalizado).ToList();
    public List<Usuario> UsuariosComLivros() => Emprestimos.Where(e => !e.Finalizado).Select(e => e.UsuarioEmprestimo).Distinct().ToList();
}

}