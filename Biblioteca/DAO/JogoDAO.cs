using Biblioteca.Vos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.DAO
{
    public static class JogadorDAO
    {
        private static SqlParameter[] CriaParametros(JogadorVO jogador)
        {
            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("id", jogador.Id);
            parametros[1] = new SqlParameter("NumeroCamisa", jogador.NumeroCamisa);
            parametros[2] = new SqlParameter("Nome", jogador.Nome);
            parametros[3] = new SqlParameter("TimeId", jogador.TimeId);            
            return parametros;
        }

        public static  void Incluir (JogadorVO jogador)
        {
            using (SqlConnection conexao = ConexaoBD.GetConexao())
            {
                string sql =
                "insert into JogadorFutebol (id, NumeroCamisa, Nome, TimeId)" +
                "values (@id, @NumeroCamisa, @Nome, @TimeId)";
                Metodos.ExecutaSQL(sql, CriaParametros(jogador));
            } 
        }

        public static void Alterar(JogadorVO jogador)
        {
            using (SqlConnection conexao = ConexaoBD.GetConexao())
            {               

                string sql =
                "update JogadorFutebol set Nome = @Nome, NumeroCamisa = @NumeroCamisa, " +
                "TimeId = @TimeId where id = @id";
                Metodos.ExecutaSQL(sql, CriaParametros(jogador));
            }
        }

        public static void Excluir(int id)
        {
            using (SqlConnection conexao = ConexaoBD.GetConexao())
            {
                SqlParameter[] parametros = { new SqlParameter("id", id) };                
                string sql =
                "delete JogadorFutebol where id = @id";
                Metodos.ExecutaSQL(sql, parametros);
            }
        }

        public static JogadorVO Consulta(int id)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter ("id", id)
            };
            string sql = "select * from JogadorFutebol where id = @id";
            DataTable tabela = Metodos.ExecutaSelect(sql, parametros);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaVO(tabela.Rows[0]);
        }


        public static JogadorVO MontaVO(DataRow registro) {

            JogadorVO j = new JogadorVO();
            j.Id = Convert.ToInt32(registro["id"]);
            j.Nome = registro["Nome"].ToString();
            j.TimeId = Convert.ToInt32(registro["TimeId"]);
            j.NumeroCamisa = Convert.ToInt32(registro["NumeroCamisa"]);
            return j;
        }

        public static JogadorVO Primeiro()
        {
            string sql = "select top 1 * from JogadorFutebol order by id";
            DataTable tabela = Metodos.ExecutaSelect(sql, null);
            return ObjetoOuNull(tabela);
        }
        public static JogadorVO Ultimo()
        {
            string sql = "select top 1 * from JogadorFutebol order by id desc";
            DataTable tabela = Metodos.ExecutaSelect(sql, null);
            return ObjetoOuNull(tabela);
        }
        public static JogadorVO Proximo(int atual)
        {
            string sql = @"select top 1 * from JogadorFutebol where id > @Atual order by id ";
            SqlParameter[] p = { new SqlParameter("Atual", atual) };
            DataTable tabela = Metodos.ExecutaSelect(sql, p);
            return ObjetoOuNull(tabela);
        }
        public static JogadorVO Anterior(int atual)
        {
            string sql = @"select top 1 * from JogadorFutebol where id < @Atual order by id Nome";
            SqlParameter[] p = { new SqlParameter("Atual", atual) };
            DataTable tabela = Metodos.ExecutaSelect(sql, p);
            return ObjetoOuNull(tabela);
        }

        private static JogadorVO ObjetoOuNull(DataTable tabela)
        {
            if (tabela.Rows.Count == 0)
                return null;
            else
            {
                JogadorVO j = MontaVO(tabela.Rows[0]);
                return j;
            }
        }

    }
}
