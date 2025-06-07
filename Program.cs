using System;
using System.Collections.Generic;

namespace BibliotecaApp
{
    public class Program
    {
       
        static Biblioteca biblioteca = new Biblioteca();

        public static void Main(string[] args)
        {
            bool rodando = true;

            while (rodando)
            {
                Console.WriteLine("\n--- Menu Biblioteca ---");
                Console.WriteLine("1. Cadastrar Livro");
                Console.WriteLine("2. Cadastrar Usuário");
                Console.WriteLine("3. Registrar Empréstimo");
                Console.WriteLine("4. Registrar Devolução");
                Console.WriteLine("5. Listar Livros Disponíveis");
                Console.WriteLine("6. Relatórios");
                Console.WriteLine("0. Sair");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();
                switch (opcao)
                {
                    case "1": CadastrarLivro(); break;
                    case "2": CadastrarUsuario(); break;
                    case "3": RegistrarEmprestimo(); break;
                    case "4": RegistrarDevolucao(); break;
                    case "5": ListarLivrosDisponiveis(); break;
                    case "6": ExibirRelatorios(); break;
                    case "0": rodando = false; break;
                    default: Console.WriteLine("Opção inválida."); break;
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
            Console.WriteLine("Livro cadastrado com sucesso!");
        }

        static void CadastrarUsuario()
        {
            Console.Write("Nome: ");
            var nome = Console.ReadLine();
            Console.Write("Matrícula: ");
            var matricula = Console.ReadLine();

            biblioteca.CadastrarUsuario(new Usuario(nome, matricula));
            Console.WriteLine("Usuário cadastrado com sucesso!");
        }

        static void RegistrarEmprestimo()
        {
            Console.Write("ISBN do livro: ");
            var isbn = Console.ReadLine();
            Console.Write("Matrícula do usuário: ");
            var matricula = Console.ReadLine();

            if (biblioteca.RegistrarEmprestimo(isbn, matricula))
                Console.WriteLine("Empréstimo registrado!");
            else
                Console.WriteLine("Erro no empréstimo (verifique disponibilidade e dados).\n");
        }

        static void RegistrarDevolucao()
        {
            Console.Write("ISBN do livro: ");
            var isbn = Console.ReadLine();
            Console.Write("Matrícula do usuário: ");
            var matricula = Console.ReadLine();

            if (biblioteca.RegistrarDevolucao(isbn, matricula))
                Console.WriteLine("Devolução registrada!");
            else
                Console.WriteLine("Erro na devolução.\n");
        }

        static void ListarLivrosDisponiveis()
        {
            Console.WriteLine("\n--- Livros Disponíveis ---");
            foreach (var livro in biblioteca.LivrosDisponiveis())
                Console.WriteLine($"{livro.Titulo} - {livro.Autor} (ISBN: {livro.ISBN}) - Quantidade: {livro.Quantidade}");
        }

        static void ExibirRelatorios()
        {
            Console.WriteLine("\n--- Livros Emprestados ---");
            foreach (var emprestimo in biblioteca.LivrosEmprestados())
                Console.WriteLine($"{emprestimo.LivroEmprestado.Titulo} para {emprestimo.UsuarioEmprestimo.Nome} até {emprestimo.Periodo.DataDevolucao.ToShortDateString()}");

            Console.WriteLine("\n--- Usuários com Livros ---");
            foreach (var usuario in biblioteca.UsuariosComLivros())
                Console.WriteLine($"{usuario.Nome} - Matrícula: {usuario.Matricula}");
        }
    }
}
