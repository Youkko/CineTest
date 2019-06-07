using CineTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineTest.Service.Interface
{
    public interface IQueryApiService
    {
        Results ListMovies(int? moviesPerPage, int? currentPage);

        /*IList<PilhaItem> ObterPilha();
        IList<PilhaAcionamentoVozItem> ObterPilhaAcionamentoVoz();
        IList<PilhaAcionamentoDDRItem> ObterPilhaAcionamentoDDR();
        IList<PilhaAcionamentoDDRVozItem> ObterPilhaAcionamento();
        IList<SituacaoSistema> ObterSituacaoSistema();
        IList<Cronograma> ListarCronogramas();
        IList<AcionamentoVozItem> ObterAcionamentoVoz(string token = "");
        IList<Tuple<List<ConfiguracaoEmailItem>, bool>> ObterConfiguracoesEmailAcionamento(string token = "");
        IList<ConfiguracoesItem> ObterConfiguracoes(string ambiente, string task);
        IList<GrupoRelatorioDownload> ListarGrupoRelatorioDownload(string GrupoRelatorio, string Usuario);
        IList<AnexoAcionamentoItem> ObterMassaAnexosAcionamento(string token = "");
        void AtualizarRelatorio(string IdRelatorio, string HashArquivo, string CaminhoArquivo);
        void EnviarEmailsAcionamento(List<Tuple<EmailAnexo, List<int>>> anexos, string token = "");
        void EnviarEmail(Email email);
        IList<Insumos> GerarInsumos(string site);
        void AtualizarConfiguracoesAmbiente(Configuracao configuracao);
        void AtualizarPilha(IList<PilhaItem> pilha);
        void AtualizarPilhaAcionamento(IList<PilhaAcionamentoVozItem> pilha);
        void AtualizarPilhaAcionamento(IList<PilhaAcionamentoDDRItem> pilha);*/
        string RequestToken();
    }
}