using System;
using System.Collections.Generic;
using System.Linq;

#region Livro.cs
public class Livro
{
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public string ISBN { get; set; }

    private int quantidade;
    public int Quantidade
    {
        get { return quantidade; }
        set
        {
            if (value < 0)
                throw new ArgumentException("A quantidade de livros não pode ser negativa.");
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

    public override string ToString()
    {
        return $"{Titulo} - {Autor} - ISBN: {ISBN} - Quantidade: {Quantidade}";
    }
}
#endregion

#region Usuario.cs
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
        return $"{Nome} - Matrícula: {Matricula}";
    }
}
#endregion

#region PeriodoEmprestimo.cs
public struct PeriodoEmprestimo
{
    public DateTime DataEmprestimo { get; set; }
    public DateTime? DataDevolucao { get; set; }
}
#endregion

#region Emprestimo.cs
public class Emprestimo
{
    public Livro Livro { get; set; }
    public Usuario Usuario { get; set; }
    public PeriodoEmprestimo Periodo { get; set; }

    public bool Ativo => Periodo.DataDevolucao == null;

    public Emprestimo(Livro livro, Usuario usuario)
    {
        Livro = livro;
        Usuario = usuario;
        Periodo = new PeriodoEmprestimo
        {
            DataEmprestimo = DateTime.Now,
            DataDevolucao = null
        };
    }

    public void Devolver()
    {
        Periodo.DataDevolucao = DateTime.Now;
        Livro.Quantidade++;
    }

    public override string ToString()
    {
        string status = Ativo ? "Em aberto" : $"Devolvido em {Periodo.DataDevolucao.Value}";
        return $"{Livro.Titulo} emprestado para {Usuario.Nome} em {Periodo.DataEmprestimo.ToShortDateString()} - {status}";
    }
}
#endregion

#region Biblioteca.cs
public class Biblioteca
{
    public List<Livro> Livros { get; } = new();
    public List<Usuario> Usuarios { get; } = new();
    public List<Emprestimo> Emprestimos { get; } = new();

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
        var emprestimo = Emprestimos.FirstOrDefault(e =>
            e.Livro.ISBN == isbn && e.Usuario.Matricula == matricula && e.Ativo);

        if (emprestimo == null)
            return false;

        emprestimo.Devolver();
        return true;
    }

    public List<Livro> LivrosDisponiveis() => Livros.Where(l => l.Quantidade > 0).ToList();
    public List<Emprestimo> LivrosEmprestados() => Emprestimos.Where(e => e.Ativo).ToList();
    public List<Usuario> UsuariosComLivros() =>
        Emprestimos.Where(e => e.Ativo)
                   .Select(e => e.Usuario)
                   .Distinct()
                   .ToList();
}
#endregion

#region Program.cs
class Program
{
    static Biblioteca biblioteca = new();

    static void Main()
    {
        int opcao;
        do
        {
            Console.WriteLine("\n--- MENU ---");
            Console.WriteLine("1. Cadastrar Livro");
            Console.WriteLine("2. Cadastrar Usuário");
            Console.WriteLine("3. Registrar Empréstimo");
            Console.WriteLine("4. Registrar Devolução");
            Console.WriteLine("5. Listar Livros");
            Console.WriteLine("6. Relatórios");
            Console.WriteLine("0. Sair");
            Console.Write("Escolha uma opção: ");
            int.TryParse(Console.ReadLine(), out opcao);
            Console.WriteLine();

            switch (opcao)
            {
                case 1: CadastrarLivro(); break;
                case 2: CadastrarUsuario(); break;
                case 3: RegistrarEmprestimo(); break;
                case 4: RegistrarDevolucao(); break;
                case 5: ListarLivros(); break;
                case 6: ExibirRelatorios(); break;
                case 0: Console.WriteLine("Saindo..."); break;
                default: Console.WriteLine("Opção inválida!"); break;
            }
        } while (opcao != 0);
    }

    static void CadastrarLivro()
    {
        Console.Write("Título: ");
        string titulo = Console.ReadLine();
        Console.Write("Autor: ");
        string autor = Console.ReadLine();
        Console.Write("ISBN: ");
        string isbn = Console.ReadLine();
        Console.Write("Quantidade: ");
        int.TryParse(Console.ReadLine(), out int qtd);

        try
        {
            var livro = new Livro(titulo, autor, isbn, qtd);
            biblioteca.CadastrarLivro(livro);
            Console.WriteLine("Livro cadastrado com sucesso.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao cadastrar livro: {ex.Message}");
        }
    }

    static void CadastrarUsuario()
    {
        Console.Write("Nome: ");
        string nome = Console.ReadLine();
        Console.Write("Matrícula: ");
        string matricula = Console.ReadLine();

        var usuario = new Usuario(nome, matricula);
        biblioteca.CadastrarUsuario(usuario);
        Console.WriteLine("Usuário cadastrado com sucesso.");
    }

    static void RegistrarEmprestimo()
    {
        Console.Write("ISBN do livro: ");
        string isbn = Console.ReadLine();
        Console.Write("Matrícula do usuário: ");
        string matricula = Console.ReadLine();

        if (biblioteca.RegistrarEmprestimo(isbn, matricula))
            Console.WriteLine("Empréstimo registrado.");
        else
            Console.WriteLine("Erro: Livro não disponível ou dados incorretos.");
    }

    static void RegistrarDevolucao()
    {
        Console.Write("ISBN do livro: ");
        string isbn = Console.ReadLine();
        Console.Write("Matrícula do usuário: ");
        string matricula = Console.ReadLine();

        if (biblioteca.RegistrarDevolucao(isbn, matricula))
            Console.WriteLine("Devolução registrada.");
        else
            Console.WriteLine("Erro: Empréstimo não encontrado.");
    }

    static void ListarLivros()
    {
        Console.WriteLine("Lista de Livros:");
        foreach (var livro in biblioteca.Livros)
            Console.WriteLine(livro);
    }

    static void ExibirRelatorios()
    {
        Console.WriteLine("\n--- Relatórios ---");

        Console.WriteLine("\nLivros Disponíveis:");
        foreach (var livro in biblioteca.LivrosDisponiveis())
            Console.WriteLine(livro);

        Console.WriteLine("\nLivros Emprestados:");
        foreach (var emp in biblioteca.LivrosEmprestados())
            Console.WriteLine(emp);

        Console.WriteLine("\nUsuários com livros emprestados:");
        foreach (var usuario in biblioteca.UsuariosComLivros())
            Console.WriteLine(usuario);
    }
}
#endregion
