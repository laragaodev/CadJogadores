using Biblioteca.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Biblioteca
{
    public class Metodos
    {

        public static bool ValidaInt(string valor)
        {
            int valorConvertido;
            return int.TryParse(valor, out valorConvertido);
        }

        public static bool ValidaDouble(string valor)
        {
            double valorConvertido;
            return double.TryParse(valor, out valorConvertido);
        }

        public static bool ValidaData(string data)
        {
            DateTime valorConvertido;
            return DateTime.TryParse(data, out valorConvertido);
        }

        public static bool Mensagem(string mensagem, TipoMensagemEnum tipoDaMensagem)
        {
            if (tipoDaMensagem == TipoMensagemEnum.alerta)
            {
                MessageBox.Show(mensagem, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return true;
            }
            
            else if (tipoDaMensagem == TipoMensagemEnum.erro)
            {
                MessageBox.Show(mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else if (tipoDaMensagem == TipoMensagemEnum.informacao)
            {
                MessageBox.Show(mensagem, "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            else
            {
                if (MessageBox.Show(mensagem, "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes)
                    return true;
                else
                    return false;
            }
        }

        

        public static void ExecutaSQL(string sql, SqlParameter[] parametros)
        {
            using (SqlConnection conexao = ConexaoBD.GetConexao())
            {
                using (SqlCommand comando = new SqlCommand(sql, conexao))
                {
                    if (parametros != null)
                    {
                        comando.Parameters.AddRange(parametros);
                    }
                    comando.ExecuteNonQuery();
                }
            }
        }
        public static DataTable ExecutaSelect(string sql, SqlParameter[] parametros)
        {
            using (SqlConnection conexao = ConexaoBD.GetConexao())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conexao))
                {
                    if (parametros != null)
                    {
                        adapter.SelectCommand.Parameters.AddRange(parametros);
                    }
                    DataTable tabela = new DataTable();
                    adapter.Fill(tabela);
                    return tabela;
                }
            }

        }
    }

        
}
