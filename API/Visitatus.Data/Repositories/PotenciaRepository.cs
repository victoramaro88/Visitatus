using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitatus.Data.Models;

namespace Visitatus.Data.Repositories
{
    public class PotenciaRepository
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

        public PotenciaRepository(IConfiguration Configuration, IWebHostEnvironment hostingEnvironment)
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
        public List<PotenciaModel> GetPotencia(int potCodi)
        {
            List<PotenciaModel> listaPotencia = new List<PotenciaModel>();

            using (SqlConnection connection = new SqlConnection(_connVisitatus))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;
                SqlDataReader reader;

                try
                {
                    command.Parameters.Clear();
                    command.Parameters.Add("@potCodi", SqlDbType.Int);
                    command.Parameters["@potCodi"].Value = potCodi;

                    command.CommandText = @$"
                                                SELECT 
	                                                potCodi, potNome, potRegu, potStat, potSigl
                                                FROM {_bdVisitatus}.dbo.{_tblPotencia} WITH(NOLOCK)
                                            ";

                    if(potCodi > 0)
                    {
                        command.CommandText += $@"
                                                WHERE potCodi = 5;
                                                ";
                    }

                    reader = command.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        PotenciaModel objItem;
                        while (reader.Read())
                        {
                            objItem = new PotenciaModel();

                            objItem.potCodi = reader["potCodi"] != DBNull.Value ? int.Parse(reader["potCodi"].ToString()!) : 0;
                            objItem.potNome = reader["potNome"].ToString();
                            objItem.potRegu = reader["potRegu"] != DBNull.Value ? bool.Parse(reader["potRegu"].ToString()!) : default;
                            objItem.potStat = reader["potStat"] != DBNull.Value ? bool.Parse(reader["potStat"].ToString()!) : default;
                            objItem.potSigl = reader["potSigl"].ToString();

                            listaPotencia.Add(objItem);
                        }
                        reader.Close();
                        reader.Dispose();
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    connection.Close();
                    GetErro(ex.ToString());
                    throw;
                }
            }

            return listaPotencia;
        }
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
