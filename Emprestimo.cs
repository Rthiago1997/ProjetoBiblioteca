public class Emprestimo
{
    public Livro Livro { get; set; }
    public Usuario Usuario { get; set; }
    public PeriodoEmprestimo Periodo;

    public bool Finalizado => Periodo.DataDevolucao.HasValue;

    public Emprestimo(Livro livro, Usuario usuario)
    {
        Livro = livro;
        Usuario = usuario;
        Periodo.DataEmprestimo = DateTime.Now;
        Periodo.DataDevolucao = null;
    }

    public void Devolver()
    {
        if (!Finalizado)
        {
            Periodo.DataDevolucao = DateTime.Now;
            Livro.Quantidade++;
        }
    }

    public override string ToString() => $"{Livro.Titulo} para {Usuario.Nome} - Empréstimo em {Periodo.DataEmprestimo} - Devolvido: {(Finalizado ? Periodo.DataDevolucao?.ToString() : "Não")}";
}