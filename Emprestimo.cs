using System;

public struct PeriodoEmprestimo
{
    public DateTime DataEmprestimo;
    public DateTime? DataDevolucao;
}

public class Emprestimo
{
    public Livro LivroEmprestado { get; set; }
    public Usuario Usuario { get; set; }
    public PeriodoEmprestimo Periodo { get; set; }

    public bool Ativo => Periodo.DataDevolucao == null;

    public Emprestimo(Livro livro, Usuario usuario)
    {
        LivroEmprestado = livro;
        Usuario = usuario;
        Periodo = new PeriodoEmprestimo { DataEmprestimo = DateTime.Now, DataDevolucao = null };
    }

    public void RegistrarDevolucao()
    {
        Periodo.DataDevolucao = DateTime.Now;
        LivroEmprestado.QuantidadeDisponivel++;
    }

    public override string ToString()
    {
        string status = Ativo ? "EM ABERTO" : $"DEVOLVIDO EM {Periodo.DataDevolucao.Value.ToShortDateString()}";
        return $"Livro: {LivroEmprestado.Titulo} | Usuário: {Usuario.Nome} | Empréstimo: {Periodo.DataEmprestimo.ToShortDateString()} | {status}";
    }
}
