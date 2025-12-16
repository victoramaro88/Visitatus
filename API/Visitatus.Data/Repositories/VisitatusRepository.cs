using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Transactions;
using System.Text.Json;
using System.Dynamic;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using Visitatus.Data.Models;

namespace Visitatus.Data.Repositories
{
    public class VisitatusRepository
    {
        #region CONSTRUTOR
        private readonly IWebHostEnvironment _hostingEnvironment;

        //-> Conexão
        private string _connVisitatus = "";

        //-> URL Externas
        private string _urlAplicacaoCertificado = "";

        //-> Bancos
        private string _bdVisitatus = "";

        //-> Tabelas
        private string _tblPresenca = "";
        private string _tblLoja = "";
        private string _tblSessao = "";
        private string _tblUsuario = "";
        private string _tblPerfilUsuario = "";
        private string _tblPotencia = "";

        public VisitatusRepository(IConfiguration Configuration, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _connVisitatus = Configuration.GetConnectionString("CONEXAO_BD")!;

            _urlAplicacaoCertificado = Configuration.GetValue<string>("UrlAplicacaoCertificado")!;

            _bdVisitatus = Configuration.GetValue<string>("DB_Visitatus")!;

            _tblPresenca = Configuration.GetValue<string>("Presenca")!;
            _tblLoja = Configuration.GetValue<string>("Loja")!;
            _tblSessao = Configuration.GetValue<string>("Sessao")!;
            _tblUsuario = Configuration.GetValue<string>("Usuario")!;
            _tblPerfilUsuario = Configuration.GetValue<string>("PerfilUsuario")!;
            _tblPotencia = Configuration.GetValue<string>("Potencia")!;

        }
        #endregion

        #region GET
        public QuantitativoPresencaModel GetQtdPresencaBySesCodi(long sesCodi)
        {
            List<PresencaModel> listaPresenca = new List<PresencaModel>();
            List<PresencaModel> listaPresencaFiltrada = new List<PresencaModel>();
            QuantitativoPresencaModel objRetorno = new QuantitativoPresencaModel();

            using (SqlConnection connection = new SqlConnection(_connVisitatus))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;
                SqlDataReader reader;

                try
                {
                    command.Parameters.Clear();
                    command.Parameters.Add("@sesCodi", SqlDbType.BigInt);
                    command.Parameters["@sesCodi"].Value = sesCodi;

                    command.CommandText = @$"
                                            SELECT
	                                            USR.usuCodi, USR.usuNome, USR.usuNCIM, PERF.perCodi, LOJ2.lojNome, LOJ2.lojNumL, POT.potSigl
                                            FROM {_bdVisitatus}.dbo.{_tblLoja} LOJ WITH(NOLOCK)
                                            JOIN {_bdVisitatus}.dbo.{_tblSessao} SES WITH(NOLOCK) ON LOJ.lojCodi = SES.lojCodi 
                                            JOIN {_bdVisitatus}.dbo.{_tblPresenca} PRE WITH(NOLOCK) ON SES.sesCodi = PRE.sesCodi
                                            JOIN {_bdVisitatus}.dbo.{_tblUsuario} USR WITH(NOLOCK) ON PRE.usuCodi = USR.usuCodi
                                            LEFT JOIN {_bdVisitatus}.dbo.{_tblPerfilUsuario} PERF WITH(NOLOCK) ON PRE.usuCodi = PERF.usuCodi AND PERF.lojCodi = LOJ.lojCodi AND PERF.perCodi IN (4, 5)
                                            JOIN {_bdVisitatus}.dbo.{_tblLoja} LOJ2 WITH(NOLOCK) ON PRE.lojCodi = LOJ2.lojCodi
                                            JOIN {_bdVisitatus}.dbo.{_tblPotencia} POT WITH(NOLOCK) ON LOJ2.potCodi = POT.potCodi 
                                            WHERE SES.sesCodi = @sesCodi
                                            ORDER BY PERF.perCodi, USR.usuNome;
                                            ";

                    reader = command.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        PresencaModel objItem;
                        while (reader.Read())
                        {
                            objItem = new PresencaModel();

                            objItem.usuCodi = reader["usuCodi"] != DBNull.Value ? long.Parse(reader["usuCodi"].ToString()!) : 0;
                            objItem.usuNome = reader["usuNome"].ToString();
                            objItem.usuNCIM = reader["usuNCIM"].ToString();
                            objItem.perCodi = reader["perCodi"] != DBNull.Value ? int.Parse(reader["perCodi"].ToString()!) : 0;
                            objItem.lojNome = reader["lojNome"].ToString();
                            objItem.lojNumL = reader["lojNumL"].ToString();
                            objItem.potSigl = reader["potSigl"].ToString();

                            listaPresenca.Add(objItem);
                        }
                        reader.Close();
                        reader.Dispose();
                    }

                    connection.Close();

                    //-> VERIFCANDO O QUANTITATIVO DE VISITANTES, MEMBROS E TOTAIS.
                    foreach (var item in listaPresenca)
                    {
                        //-> VERIFICANDO SE JÁ FOI INSERIDO A PESSOA NA CONTAGEM (VALIDAÇÃO DE PESSOA COM CADASTRO DE MEMBRO E FILIADO AO MESMO TEMPO).
                        var itemExistente = listaPresencaFiltrada.Where(l => l.usuCodi == item.usuCodi).FirstOrDefault();

                        //-> SE NÃO EXISTIR PESSOA, VALIDA E INSERE.
                        if (itemExistente == null)
                        {
                            if (item.perCodi == 0) //-> SE PERFIL == 0, É VISITANTE
                            {
                                objRetorno.qtdVisitantes += 1;
                            }
                            else //-> SENÃO, É MEMBRO
                            {
                                objRetorno.qtdMembros += 1;
                            }

                            //-> ADICIONA PARA O TOTAL GERAL
                            objRetorno.qtdTotal += 1;

                            listaPresencaFiltrada.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    connection.Close();
                    GetErro(ex.ToString());
                    throw;
                }
            }

            return objRetorno;
        }
        #endregion

        #region POST
        #endregion

        #region PUT
        #endregion

        public void GetErro(string exception)
        {
            string Folder = _hostingEnvironment.ContentRootPath + @"\Erro\erro" + (DateTime.Now).ToString().Replace("/", "_").Replace(":", ".") + ".txt";
            string erro = exception + "" + DateTime.Now;

            if (!File.Exists(Folder))
            {
                File.WriteAllText(Folder, erro);
            }
        }
    }
}
