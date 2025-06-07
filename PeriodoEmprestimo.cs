
using System;

namespace BibliotecaApp
{
    
public struct PeriodoEmprestimo
{
    public DateTime DataEmprestimo { get; set; }
    public DateTime DataDevolucao { get; set; }

    public PeriodoEmprestimo(DateTime emprestimo)
    {
        DataEmprestimo = emprestimo;
        DataDevolucao = emprestimo.AddDays(7);
    }
}
}