using System;

namespace ProjetoBiblioteca;

class Program
{
    static Biblioteca biblioteca = new Biblioteca();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n--- Menu ---");
            Console.WriteLine("1. Cadastrar Livro");
            Console.WriteLine("2. Cadastrar Usuário");
            Console.WriteLine("3. Registrar Empréstimo");
            Console.WriteLine("4. Registrar Devolução");
            Console.WriteLine("5. Listar Livros");
            Console.WriteLine("6. Relatórios");
            Console.WriteLine("0. Sair");

            var opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1": CadastrarLivro(); break;
                case "2": CadastrarUsuario(); break;
                case "3": RegistrarEmprestimo(); break;
                case "4": RegistrarDevolucao(); break;
                case "5": ListarLivros(); break;
                case "6": ExibirRelatorios(); break;
                case "0": return;
                default: Console.WriteLine("Opção inválida"); break;
            }
        }
    }

    static void CadastrarLivro()
    {
        Console.Write("Título: ");
        var titulo = Console.ReadLine();
        Console.Write("Autor: ");
        var autor = Console.ReadLine();
        Console.Write("ISBN: ");
        var isbn = Console.ReadLine();
        Console.Write("Quantidade: ");
        int quantidade = int.Parse(Console.ReadLine());

        biblioteca.CadastrarLivro(new Livro(titulo, autor, isbn, quantidade));
        Console.WriteLine("Livro cadastrado com sucesso.");
    }

    static void CadastrarUsuario()
    {
        Console.Write("Nome: ");
        var nome = Console.ReadLine();
        Console.Write("Matrícula: ");
        var matricula = Console.ReadLine();

        biblioteca.CadastrarUsuario(new Usuario(nome, matricula));
        Console.WriteLine("Usuário cadastrado com sucesso.");
    }

    static void RegistrarEmprestimo()
    {
        Console.Write("ISBN do Livro: ");
        var isbn = Console.ReadLine();
        Console.Write("Matrícula do Usuário: ");
        var matricula = Console.ReadLine();

        if (biblioteca.RegistrarEmprestimo(isbn, matricula))
            Console.WriteLine("Empréstimo registrado.");
        else
            Console.WriteLine("Falha ao registrar empréstimo.");
    }

    static void RegistrarDevolucao()
    {
        Console.Write("ISBN do Livro: ");
        var isbn = Console.ReadLine();
        Console.Write("Matrícula do Usuário: ");
        var matricula = Console.ReadLine();

        if (biblioteca.RegistrarDevolucao(isbn, matricula))
            Console.WriteLine("Devolução registrada.");
        else
            Console.WriteLine("Falha ao registrar devolução.");
    }

    static void ListarLivros()
    {
        foreach (var livro in biblioteca.Livros)
            Console.WriteLine(livro);
    }

    static void ExibirRelatorios()
    {
        Console.WriteLine("\n-- Livros Disponíveis --");
        foreach (var livro in biblioteca.LivrosDisponiveis())
            Console.WriteLine(livro);

        Console.WriteLine("\n-- Livros Emprestados --");
        foreach (var emprestimo in biblioteca.LivrosEmprestados())
            Console.WriteLine(emprestimo);

        Console.WriteLine("\n-- Usuários com Livros --");
        foreach (var usuario in biblioteca.UsuariosComEmprestimos())
            Console.WriteLine(usuario);
    }
}
