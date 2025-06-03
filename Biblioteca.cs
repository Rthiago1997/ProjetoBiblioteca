using System.Collections.Generic;
using System.Linq;

public class Biblioteca
{
    public List<Livro> Livros { get; set; } = new List<Livro>();
    public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public List<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();

    public void CadastrarLivro(Livro livro) => Livros.Add(livro);

    public void CadastrarUsuario(Usuario usuario) => Usuarios.Add(usuario);

    public bool RegistrarEmprestimo(string isbn, string matricula)
    {
        var livro = Livros.FirstOrDefault(l => l.ISBN == isbn);
        var usuario = Usuarios.FirstOrDefault(u => u.Matricula == matricula);

        if (livro == null || usuario == null || livro.Quantidade <= 0)
            return false;

        livro.Quantidade--;
        Emprestimos.Add(new Emprestimo(livro, usuario));
        return true;
    }

    public bool RegistrarDevolucao(string isbn, string matricula)
    {
        var emprestimo = Emprestimos.FirstOrDefault(e => e.Livro.ISBN == isbn && e.Usuario.Matricula == matricula && !e.Finalizado);
        if (emprestimo == null) return false;

        emprestimo.Devolver();
        return true;
    }

    public IEnumerable<Livro> LivrosDisponiveis() => Livros.Where(l => l.Quantidade > 0);

    public IEnumerable<Emprestimo> LivrosEmprestados() => Emprestimos.Where(e => !e.Finalizado);

    public IEnumerable<Usuario> UsuariosComEmprestimos() => Emprestimos.Where(e => !e.Finalizado).Select(e => e.Usuario).Distinct();
}
