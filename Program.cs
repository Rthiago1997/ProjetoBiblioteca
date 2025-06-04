﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoBiblioteca {

    public class Biblioteca {

        static void main()
        {
            var biblioteca = new Biblioteca();
            bool executando = true;
        public List<Livro> Livros = new();
        public List<Usuario> Usuarios = new();
        public List<Emprestimo> Emprestimos = new();

        public void CadastrarLivro(string titulo, string autor, string isbn, int quantidade)
        {
            Livros.Add(new Livro(titulo, autor, isbn, quantidade));
        }

        public void CadastrarUsuario(string nome, string matricula)
        {
            Usuarios.Add(new Usuario(nome, matricula));
        }

        public void RegistrarEmprestimo(string isbn, string matricula)
        {
            var livro = Livros.FirstOrDefault(l => l.ISBN == isbn);
            var usuario = Usuarios.FirstOrDefault(u => u.Matricula == matricula);

            if (livro == null || usuario == null)
            {
                Console.WriteLine("Livro ou usuário não encontrado.");
                return;
            }

            if (livro.QuantidadeDisponivel <= 0)
            {
                Console.WriteLine("Livro indisponível para empréstimo.");
                return;
            }

            livro.QuantidadeDisponivel--;
            Emprestimos.Add(new Emprestimo(livro, usuario));
            Console.WriteLine("Empréstimo registrado com sucesso.");
        }

        public void RegistrarDevolucao(string isbn, string matricula)
        {
            var emprestimo = Emprestimos.FirstOrDefault(e =>
                e.LivroEmprestado.ISBN == isbn &&
                e.Usuario.Matricula == matricula &&
                e.Ativo);

            if (emprestimo != null)
            {
                emprestimo.RegistrarDevolucao();
                Console.WriteLine("Devolução registrada.");
            }
            else
            {
                Console.WriteLine("Empréstimo não encontrado.");
            }
        }

        public void ListarLivros()
        {
            foreach (var livro in Livros)
            {
                Console.WriteLine(livro);
            }
        }

        public void ExibirRelatorios()
        {
            Console.WriteLine("\n--- Livros Disponíveis ---");
            foreach (var l in Livros.Where(l => l.QuantidadeDisponivel > 0))
                Console.WriteLine(l);

            Console.WriteLine("\n--- Livros Emprestados ---");
            foreach (var e in Emprestimos.Where(e => e.Ativo))
                Console.WriteLine(e);

            Console.WriteLine("\n--- Usuários com Livros Emprestados ---");
            var usuariosComLivros = Emprestimos
                .Where(e => e.Ativo)
                .Select(e => e.Usuario)
                .Distinct();

            foreach (var u in usuariosComLivros)
                Console.WriteLine(u);
        }
    }
}
