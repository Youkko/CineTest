using Newtonsoft.Json;
using CineTest.Service.Interface;
using CineTest.Service.Enums;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using CineTest.Entities;
using System.Collections.Generic;

namespace CineTest.Service
{
    public class QueryApiService : IQueryApiService
    {

        private readonly IWebClientService _webClientService;
        private readonly string _apiKey;

        public QueryApiService(IWebClientService webClientService, string apiKey)
        {
            _webClientService = webClientService;
            _apiKey = apiKey;
        }

        public Results ListMovies(int? moviesPerPage, int? currentPage)
        {
            string token = RequestToken();
            RootObject root;
            root = _webClientService.HttpRequest<RootObject>(
                        "Movies",
                        $"List?moviesPerPage={moviesPerPage}&currentPage={currentPage}",
                        null,
                        HttpResquestMethod.GET,
                        AuthorizationType.BearerToken,
                        token
                    );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
            else
            {
                return JsonConvert.DeserializeObject<Results>(root.message.ToString());
            }
        }

        /*public IList<PilhaItem> ObterPilha()
        {
            string token = RequestToken();
            RootObject root;
            root = _webClientService.HttpRequest<RootObject>(
                        "Pilha",
                        "ObterBatch",
                        null,
                        HttpResquestMethod.GET,
                        AuthorizationType.BearerToken,
                        token
                    );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
            else
            {
                return JsonConvert.DeserializeObject<Message<PilhaItem>>(root.message.ToString()).itens.item;
            }
        }

        public IList<PilhaAcionamentoVozItem> ObterPilhaAcionamentoVoz()
        {
            string token = RequestToken();
            RootObject root;
            root = _webClientService.HttpRequest<RootObject>(
                        "Pilha",
                        "ObterAcionamentoBatch",
                        new { Voz = true },
                        HttpResquestMethod.POST,
                        AuthorizationType.BearerToken,
                        token,
                        90
                    );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
            else
            {
                return JsonConvert.DeserializeObject<Message<PilhaAcionamentoVozItem>>(root.message.ToString()).itens.item;
            }
        }

        public IList<PilhaAcionamentoDDRVozItem> ObterPilhaAcionamento()
        {
            string token = RequestToken();
            RootObject root;
            root = _webClientService.HttpRequest<RootObject>(
                        "Pilha",
                        "ObterAcionamentoBatch",
                        null,
                        HttpResquestMethod.POST,
                        AuthorizationType.BearerToken,
                        token,
                        90
                    );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
            else
            {
                return JsonConvert.DeserializeObject<Message<PilhaAcionamentoDDRVozItem>>(root.message.ToString()).itens.item;
            }
        }

        public IList<PilhaAcionamentoDDRItem> ObterPilhaAcionamentoDDR()
        {
            string token = RequestToken();
            RootObject root;
            root = _webClientService.HttpRequest<RootObject>(
                        "Pilha",
                        "ObterAcionamentoBatch",
                        new { Voz = false },
                        HttpResquestMethod.POST,
                        AuthorizationType.BearerToken,
                        token,
                        90
                    );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
            else
            {
                return JsonConvert.DeserializeObject<Message<PilhaAcionamentoDDRItem>>(root.message.ToString()).itens.item;
            }
        }

        public IList<AnexoAcionamentoItem> ObterMassaAnexosAcionamento(string token = "")
        {
            if (string.IsNullOrEmpty(token))
                token = RequestToken();

            RootObject root;
            root = _webClientService.HttpRequest<RootObject>(
                        "Email",
                        "ObterMassaAnexosAcionamento",
                        null,
                        HttpResquestMethod.POST,
                        AuthorizationType.BearerToken,
                        token,
                        90
                    );
            if (root != null)
            {
                if (root.error)
                {
                    throw new Exception(root.message.ToString());
                }
                else
                {
                    return JsonConvert.DeserializeObject<Message<AnexoAcionamentoItem>>(root.message.ToString()).itens.item;
                }
            }
            else
                throw new Exception("O WebAPI não retornou dados, ou a requisição falhou (root = null).");
        }

        public IList<Tuple<List<ConfiguracaoEmailItem>, bool>> ObterConfiguracoesEmailAcionamento(string token = "")
        {
            RootObject root;
            List<Tuple<List<ConfiguracaoEmailItem>, bool>> items = new List<Tuple<List<ConfiguracaoEmailItem>, bool>>();
            List<ConfiguracaoEmailItem> lista;
            List<string> tags = new List<string>();
            bool hasToken = !string.IsNullOrEmpty(token);

            tags.Add("AcionamentoCenarioA");
            tags.Add("AcionamentoCenarioB");
            tags.Add("AcionamentoCenarioC");
            tags.Add("AcionamentoCenarioD");
            tags.Add("AcionamentoCenarioE");

            foreach (string tag in tags)
            {
                if (!hasToken)
                    token = RequestToken();

                root = _webClientService.HttpRequest<RootObject>(
                            "Email",
                            "ConsultarEmail",
                            new { TagEmail = tag },
                            HttpResquestMethod.POST,
                            AuthorizationType.BearerToken,
                            token,
                            90
                        );

                if (root != null)
                {
                    if (root.error)
                    {
                        throw new Exception(root.message.ToString());
                    }
                    else
                    {
                        lista = new List<ConfiguracaoEmailItem>();
                        lista.AddRange(JsonConvert.DeserializeObject<Message<ConfiguracaoEmailItem>>(root.message.ToString()).itens.item);
                        if (lista.Count > 0)
                            lista[lista.Count - 1].TagEmail = tag;

                        items.Add(new Tuple<List<ConfiguracaoEmailItem>, bool>(lista, false));
                    }
                }
                else
                {
                    lista = new List<ConfiguracaoEmailItem>();
                    lista.Add(new ConfiguracaoEmailItem() { TagEmail = tag });
                    items.Add(new Tuple<List<ConfiguracaoEmailItem>, bool>(lista, true));
                }
                System.Threading.Thread.Sleep(2000);
            }
            return items;
        }

        public IList<ConfiguracoesItem> ObterConfiguracoes(string ambiente, string task)
        {
            string token = RequestToken();
            RootObject root;
            root = _webClientService.HttpRequest<RootObject>(
                        "Config",
                        "ConsultarAmbiente",
                        new { Ambiente = ambiente, Task = task },
                        HttpResquestMethod.POST,
                        AuthorizationType.BearerToken,
                        token
                    );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
            else
            {
                return JsonConvert.DeserializeObject<Message<ConfiguracoesItem>>(root.message.ToString()).itens.item;
            }
        }

        public IList<SituacaoSistema> ObterSituacaoSistema()
        {
            string token = RequestToken();
            RootObject root;
            root = _webClientService.HttpRequest<RootObject>(
                        "SituacaoSistema",
                        "ListarSituacaoSistema",
                        null,
                        HttpResquestMethod.POST,
                        AuthorizationType.BearerToken,
                        token
                    );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
            else
            {
                return JsonConvert.DeserializeObject<Message<SituacaoSistema>>(root.message.ToString()).itens.item;
            }
        }

        public IList<AcionamentoVozItem> ObterAcionamentoVoz(string token = "")
        {
            if (string.IsNullOrEmpty(token))
                token = RequestToken();

            RootObject root;
            root = _webClientService.HttpRequest<RootObject>(
                        "SituacaoSistema",
                        "ListarAcionamentoVoz",
                        null,
                        HttpResquestMethod.POST,
                        AuthorizationType.BearerToken,
                        token
                    );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
            else
            {
                return JsonConvert.DeserializeObject<Message<AcionamentoVozItem>>(root.message.ToString()).itens.item;
            }
        }

        public IList<Cronograma> ListarCronogramas()
        {
            string token = RequestToken();
            RootObject root = _webClientService.HttpRequest<RootObject>(
                "Cronograma",
                "Listar",
                null,
                HttpResquestMethod.GET,
                AuthorizationType.BearerToken,
                token
            );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
            else
            {
                return JsonConvert.DeserializeObject<Message<Cronograma>>(root.message.ToString()).itens.item;
            }
        }

        public IList<GrupoRelatorioDownload> ListarGrupoRelatorioDownload(string GrupoRelatorio, string Usuario)
        {
            string token = RequestToken();
            RootObject root = _webClientService.HttpRequest<RootObject>(
                "Varredura",
                "ConsultarGrupoRelatorioParaDownload",
                new
                {
                    GrupoRelatorio = GrupoRelatorio,
                    Usuario = Usuario,
                },
                HttpResquestMethod.POST,
                AuthorizationType.BearerToken,
                token
            );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
            else
            {
                return JsonConvert.DeserializeObject<Message<GrupoRelatorioDownload>>(root.message.ToString()).itens.item;
            }
        }

        public void AtualizarRelatorio(string IdRelatorio, string HashArquivo, string CaminhoArquivo)
        {
            string token = RequestToken();
            RootObject root = _webClientService.HttpRequest<RootObject>(
                "Varredura",
                "AtualizarRelatorio",
                new
                {
                    IdRelatorio = IdRelatorio,
                    HashArquivo = HashArquivo,
                    CaminhoArquivo = CaminhoArquivo,
                },
                HttpResquestMethod.POST,
                AuthorizationType.BearerToken,
                token
            );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
        }

        public void EnviarEmailsAcionamento(List<Tuple<EmailAnexo, List<int>>> anexos, string token = "")
        {
            if (string.IsNullOrEmpty(token))
                token = RequestToken();

            RootObject root = _webClientService.HttpRequest<RootObject>(
                "Email",
                "EnviarEmailsAcionamento",
                anexos,
                HttpResquestMethod.POST,
                AuthorizationType.BearerToken,
                token,
                120
            );
            if (root != null)
            {
                if (root.error)
                {
                    throw new Exception(root.message.ToString());
                }
            }
            else
            {
                throw new Exception("Ocorreu um erro ao enviar o e-mail para o Web API. Possivel Timeout.");
            }
        }

        public void EnviarEmail(Email email)
        {
            string token = RequestToken();
            RootObject root = _webClientService.HttpRequest<RootObject>(
                "Email",
                "EnviarEmail",
                new
                {
                    fromIFF = email.Iff,
                    Relatorio = email.Relatorio,
                    Destinatario = email.Destinatario,
                    Assunto = email.Assunto,
                    CorpoEmail = email.CorpoEmail,
                },
                HttpResquestMethod.POST,
                AuthorizationType.BearerToken,
                token
            );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
        }

        public IList<Insumos> GerarInsumos(string site)
        {
            string token = RequestToken();
            RootObject root = _webClientService.HttpRequest<RootObject>(
                "Insumo",
                "Download",
                new
                {
                    Site = site,
                },
                HttpResquestMethod.POST,
                AuthorizationType.BearerToken,
                token
            );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
            else
            {
                return JsonConvert.DeserializeObject<Message<Insumos>>(root.message.ToString()).itens.item;
            }
        }

        public void AtualizarConfiguracoesAmbiente(Configuracao configuracao)
        {
            string token = RequestToken();
            RootObject root = _webClientService.HttpRequest<RootObject>(
                "Config",
                "AtualizarConfiguracoesAmbiente",
                new
                {
                    Ambiente = configuracao.Ambiente,
                    Task = configuracao.Task,
                    TagConfiguracao = configuracao.TagConfiguracao,
                    Valor = configuracao.Valor,
                },
                HttpResquestMethod.POST,
                AuthorizationType.BearerToken,
                token
            );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
        }

        public void AtualizarPilha(IList<PilhaItem> pilha)
        {
            string pilha_s = JsonConvert.SerializeObject(pilha);
            List<PilhaItem> pilha_d = JsonConvert.DeserializeObject<List<PilhaItem>>(pilha_s);

            string token = RequestToken();
            RootObject root = _webClientService.HttpRequest<RootObject>(
                "Pilha",
                "AtualizarBatch",
                pilha,
                HttpResquestMethod.PUT,
                AuthorizationType.BearerToken,
                token
            );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
        }

        public void AtualizarPilhaAcionamento(IList<PilhaAcionamentoVozItem> pilha)
        {
            string pilha_s = JsonConvert.SerializeObject(pilha);
            List<PilhaAcionamentoVozItem> pilha_d = JsonConvert.DeserializeObject<List<PilhaAcionamentoVozItem>>(pilha_s);

            string token = RequestToken();
            RootObject root = _webClientService.HttpRequest<RootObject>(
                "Pilha",
                "AtualizarAcionamentoVozBatch",
                pilha,
                HttpResquestMethod.PUT,
                AuthorizationType.BearerToken,
                token
            );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
        }
        public void AtualizarPilhaAcionamento(IList<PilhaAcionamentoDDRItem> pilha)
        {
            string pilha_s = JsonConvert.SerializeObject(pilha);
            List<PilhaAcionamentoDDRItem> pilha_d = JsonConvert.DeserializeObject<List<PilhaAcionamentoDDRItem>>(pilha_s);

            string token = RequestToken();
            RootObject root = _webClientService.HttpRequest<RootObject>(
                "Pilha",
                "AtualizarAcionamentoDDRBatch",
                pilha,
                HttpResquestMethod.PUT,
                AuthorizationType.BearerToken,
                token
            );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
        }
*/
        public string RequestToken()
        {
            ApiToken token = _webClientService.HttpRequest<ApiToken>(
                "Login",
                "",
                new
                {
                    Key = _apiKey
                },
                HttpResquestMethod.POST
            );

            if (token.authenticated)
            {
                return token.token;
            }
            else
            {
                throw new Exception(token.message);
            }
        }
    }
}
