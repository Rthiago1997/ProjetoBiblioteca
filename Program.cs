using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoBiblioteca{


// Programa principal
class Program
{
    static void Main(string[] args)
    {
        var biblioteca = new Biblioteca();
        biblioteca.ExibirMenu();
    }
}

// Struct para representar o período de empréstimo
public struct PeriodoEmprestimo
{
    public DateTime DataEmprestimo { get; }
    public DateTime DataDevolucaoPrevista { get; }
    
    public PeriodoEmprestimo(DateTime dataEmprestimo)
    {
        DataEmprestimo = dataEmprestimo;
        DataDevolucaoPrevista = dataEmprestimo.AddDays(14); // 2 semanas de prazo
    }
}

// Classe base Pessoa
public class Pessoa
{
    public string Nome { get; protected set; }
    
    public Pessoa(string nome)
    {
        Nome = nome;
    }
}

// Classe Usuario que herda de Pessoa
public class Usuario : Pessoa
{
    public string Matricula { get; private set; }
    public List<Livro> LivrosEmprestados { get; private set; }
    
    public Usuario(string nome, string matricula) : base(nome)
    {
        Matricula = matricula;
        LivrosEmprestados = new List<Livro>();
    }
    
    public void AdicionarLivroEmprestado(Livro livro)
    {
        LivrosEmprestados.Add(livro);
    }
    
    public void RemoverLivroEmprestado(Livro livro)
    {
        LivrosEmprestados.Remove(livro);
    }
}

// Classe Livro
public class Livro
{
    private int _quantidadeDisponivel;
    
    public string Titulo { get; private set; }
    public string Autor { get; private set; }
    public string ISBN { get; private set; }
    public int QuantidadeDisponivel 
    { 
        get => _quantidadeDisponivel;
        private set
        {
            if (value < 0)
            {
                throw new ArgumentException("Quantidade não pode ser negativa");
            }
            _quantidadeDisponivel = value;
        }
    }
    
    public Livro(string titulo, string autor, string isbn, int quantidade)
    {
        Titulo = titulo;
        Autor = autor;
        ISBN = isbn;
        QuantidadeDisponivel = quantidade;
    }
    
    public void DecrementarQuantidade()
    {
        QuantidadeDisponivel--;
    }
    
    public void IncrementarQuantidade()
    {
        QuantidadeDisponivel++;
    }
}

// Classe Emprestimo
public class Emprestimo
{
    public Livro Livro { get; private set; }
    public Usuario Usuario { get; private set; }
    public PeriodoEmprestimo Periodo { get; private set; }
    public bool Finalizado { get; private set; }
    
    public Emprestimo(Livro livro, Usuario usuario, DateTime dataEmprestimo)
    {
        Livro = livro;
        Usuario = usuario;
        Periodo = new PeriodoEmprestimo(dataEmprestimo);
        Finalizado = false;
        
        livro.DecrementarQuantidade();
        usuario.AdicionarLivroEmprestado(livro);
    }
    
    public void FinalizarEmprestimo()
    {
        if (!Finalizado)
        {
            Finalizado = true;
            Livro.IncrementarQuantidade();
            Usuario.RemoverLivroEmprestado(Livro);
        }
    }
}

// Classe Biblioteca (sistema principal)
public class Biblioteca
{
    private List<Livro> _livros;
    private List<Usuario> _usuarios;
    private List<Emprestimo> _emprestimos;
    
    public Biblioteca()
    {
        _livros = new List<Livro>();
        _usuarios = new List<Usuario>();
        _emprestimos = new List<Emprestimo>();
    }
    
    public void CadastrarLivro(string titulo, string autor, string isbn, int quantidade)
    {
        var livro = new Livro(titulo, autor, isbn, quantidade);
        _livros.Add(livro);
    }
    
    public void CadastrarUsuario(string nome, string matricula)
    {
        var usuario = new Usuario(nome, matricula);
        _usuarios.Add(usuario);
    }
    
    public void RegistrarEmprestimo(string isbn, string matricula)
    {
        var livro = _livros.FirstOrDefault(l => l.ISBN == isbn);
        var usuario = _usuarios.FirstOrDefault(u => u.Matricula == matricula);
        
        if (livro == null)
        {
            Console.WriteLine("Livro não encontrado!");
            return;
        }
        
        if (usuario == null)
        {
            Console.WriteLine("Usuário não encontrado!");
            return;
        }
        
        if (livro.QuantidadeDisponivel <= 0)
        {
            Console.WriteLine("Não há exemplares disponíveis deste livro!");
            return;
        }
        
        var emprestimo = new Emprestimo(livro, usuario, DateTime.Now);
        _emprestimos.Add(emprestimo);
        
        Console.WriteLine($"Empréstimo registrado com sucesso! Devolução prevista para: {emprestimo.Periodo.DataDevolucaoPrevista.ToShortDateString()}");
    }
    
    public void RegistrarDevolucao(string isbn, string matricula)
    {
        var emprestimo = _emprestimos.FirstOrDefault(e => 
            !e.Finalizado && 
            e.Livro.ISBN == isbn && 
            e.Usuario.Matricula == matricula);
        
        if (emprestimo == null)
        {
            Console.WriteLine("Empréstimo não encontrado ou já finalizado!");
            return;
        }
        
        emprestimo.FinalizarEmprestimo();
        Console.WriteLine("Devolução registrada com sucesso!");
    }
    
    public void ListarLivrosDisponiveis()
    {
        Console.WriteLine("\n=== LIVROS DISPONÍVEIS ===");
        foreach (var livro in _livros.Where(l => l.QuantidadeDisponivel > 0))
        {
            Console.WriteLine($"{livro.Titulo} - {livro.Autor} (ISBN: {livro.ISBN}) - Disponíveis: {livro.QuantidadeDisponivel}");
        }
    }
    
    public void ListarLivrosEmprestados()
    {
        Console.WriteLine("\n=== LIVROS EMPRESTADOS ===");
        var emprestimosAtivos = _emprestimos.Where(e => !e.Finalizado);
        
        if (!emprestimosAtivos.Any())
        {
            Console.WriteLine("Nenhum livro emprestado no momento.");
            return;
        }
        
        foreach (var emp in emprestimosAtivos)
        {
            Console.WriteLine($"{emp.Livro.Titulo} - Emprestado para: {emp.Usuario.Nome} (Matrícula: {emp.Usuario.Matricula})");
            Console.WriteLine($"   Data empréstimo: {emp.Periodo.DataEmprestimo.ToShortDateString()}");
            Console.WriteLine($"   Data devolução: {emp.Periodo.DataDevolucaoPrevista.ToShortDateString()}");
        }
    }
    
    public void ListarUsuariosComLivros()
    {
        Console.WriteLine("\n=== USUÁRIOS COM LIVROS EMPRESTADOS ===");
        var usuariosComLivros = _usuarios.Where(u => u.LivrosEmprestados.Count > 0);
        
        if (!usuariosComLivros.Any())
        {
            Console.WriteLine("Nenhum usuário com livros emprestados no momento.");
            return;
        }
        
        foreach (var usuario in usuariosComLivros)
        {
            Console.WriteLine($"{usuario.Nome} (Matrícula: {usuario.Matricula}) - Livros:");
            foreach (var livro in usuario.LivrosEmprestados)
            {
                Console.WriteLine($"   {livro.Titulo} (ISBN: {livro.ISBN})");
            }
        }
    }
    
    public void ExibirMenu()
    {
        while (true)
        {
            Console.WriteLine("\n=== SISTEMA DE BIBLIOTECA ===");
            Console.WriteLine("1. Cadastrar livro");
            Console.WriteLine("2. Cadastrar usuário");
            Console.WriteLine("3. Registrar empréstimo");
            Console.WriteLine("4. Registrar devolução");
            Console.WriteLine("5. Listar livros disponíveis");
            Console.WriteLine("6. Listar livros emprestados");
            Console.WriteLine("7. Listar usuários com livros");
            Console.WriteLine("8. Sair");
            Console.Write("Escolha uma opção: ");
            
            var opcao = Console.ReadLine();
            
            switch (opcao)
            {
                case "1":
                    CadastrarLivroMenu();
                    break;
                case "2":
                    CadastrarUsuarioMenu();
                    break;
                case "3":
                    RegistrarEmprestimoMenu();
                    break;
                case "4":
                    RegistrarDevolucaoMenu();
                    break;
                case "5":
                    ListarLivrosDisponiveis();
                    break;
                case "6":
                    ListarLivrosEmprestados();
                    break;
                case "7":
                    ListarUsuariosComLivros();
                    break;
                case "8":
                    return;
                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }
        }
    }
    
    private void CadastrarLivroMenu()
    {
        Console.Write("Título: ");
        var titulo = Console.ReadLine();
        
        Console.Write("Autor: ");
        var autor = Console.ReadLine();
        
        Console.Write("ISBN: ");
        var isbn = Console.ReadLine();
        
        Console.Write("Quantidade: ");
        if (int.TryParse(Console.ReadLine(), out int quantidade))
        {
            CadastrarLivro(titulo, autor, isbn, quantidade);
            Console.WriteLine("Livro cadastrado com sucesso!");
        }
        else
        {
            Console.WriteLine("Quantidade inválida!");
        }
    }
    
    private void CadastrarUsuarioMenu()
    {
        Console.Write("Nome: ");
        var nome = Console.ReadLine();
        
        Console.Write("Matrícula: ");
        var matricula = Console.ReadLine();
        
        CadastrarUsuario(nome, matricula);
        Console.WriteLine("Usuário cadastrado com sucesso!");
    }
    
    private void RegistrarEmprestimoMenu()
    {
        Console.Write("ISBN do livro: ");
        var isbn = Console.ReadLine();
        
        Console.Write("Matrícula do usuário: ");
        var matricula = Console.ReadLine();
        
        RegistrarEmprestimo(isbn, matricula);
    }
    
    private void RegistrarDevolucaoMenu()
    {
        Console.Write("ISBN do livro: ");
        var isbn = Console.ReadLine();
        
        Console.Write("Matrícula do usuário: ");
        var matricula = Console.ReadLine();
        
        RegistrarDevolucao(isbn, matricula);
    }
}
}