using Biblioteca;
using Biblioteca.Exceptions;
using Biblioteca.DAO;
using Biblioteca.Vos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 

namespace CadJogos1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }

        private void TrataErro(Exception erro)
        {
            if (erro is FormatException)
            {                
                Metodos.Mensagem("Campo numérico inválido!", TipoMensagemEnum.erro);
            }
            else if (erro is ValidacaoException)
            {
                Metodos.Mensagem(erro.Message, TipoMensagemEnum.erro);
            }
            else if (erro is SqlException)
            {
                Metodos.Mensagem("Ocorreu um erro no banco de dados. Detalhes: \r\n" +
                erro.Message, TipoMensagemEnum.erro);
            }
            else if (erro is Exception)
            {
                Metodos.Mensagem("Ocorreu um erro desconhecido!", TipoMensagemEnum.erro);
            }
        }


        private void PreencheTela(JogadorVO j)
        {
            if (j != null)
            {
                txtId.Text = j.Id.ToString();
                txtNome.Text = j.Nome;
                txtNumCamisa.Text = j.NumeroCamisa.ToString();                
                txtTimeId.Text = j.TimeId.ToString();
            }
            else
                LimpaCampos(this);
        }
        private void LimpaCampos(Control objeto)
        {
            if (objeto is TextBox || objeto is MaskedTextBox)
                objeto.Text = "";
            else
                foreach (Control o in objeto.Controls)
                    LimpaCampos(o);
        }

        private void AlteraParaModo(EnumModoOperacao modo)
        {
            txtNome.Enabled = txtTimeId.Enabled = txtNumCamisa.Enabled =
            btnGravar.Enabled = btnCancelar.Enabled = (modo != EnumModoOperacao.Navegacao);            
            button1.Enabled = (modo == EnumModoOperacao.Navegacao);
            btnAnterior.Enabled =
            btnProximo.Enabled =
            btnAlterar.Enabled =
            btnExcluir.Enabled = (modo == EnumModoOperacao.Navegacao) &&
            txtId.Text.Length > 0;
            btnPrimeiro.Enabled = (modo == EnumModoOperacao.Navegacao);
            btnUltimo.Enabled = (modo == EnumModoOperacao.Navegacao);
            if (modo == EnumModoOperacao.Inclusao)
            {
                txtId.Enabled = true;
                LimpaCampos(this);
                txtId.Focus();
            }
            else
                txtId.Enabled = false;
        }

        private bool Validacoes()
        {
            if (Metodos.ValidaInt(txtId.Text) == false)
            {
                Metodos.Mensagem("Digite apenas números no campo ID.",
                TipoMensagemEnum.erro);
                return false;
            }
            if (Metodos.ValidaDouble(txtTimeId.Text) == false)
            {
                Metodos.Mensagem("Digite apenas números no campo TimeId.",
                TipoMensagemEnum.erro);
                return false;
            }            
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AlteraParaModo(EnumModoOperacao.Inclusao);
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            AlteraParaModo(EnumModoOperacao.Alteracao);
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (Metodos.ValidaInt(txtId.Text) == false)
            {
                Metodos.Mensagem("Digite apenas números no campo ID.",
                TipoMensagemEnum.alerta);
                return;
            }
            if (Metodos.Mensagem("Confirma?", TipoMensagemEnum.pergunta))
            {
                try
                {
                    JogadorDAO.Excluir(Convert.ToInt32(txtId.Text));
                    btnPrimeiro.PerformClick();
                }
                catch (Exception erro)
                {
                    TrataErro(erro);
                }
            }
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            var j = JogadorDAO.Consulta(Convert.ToInt32(txtId.Text));
            if (j != null)
            {
                txtId.Text = j.Id.ToString();
                txtNome.Text = j.Nome;
                txtTimeId.Text = j.TimeId.ToString();
                txtNumCamisa.Text = j.NumeroCamisa.ToString();                
            }
        }

        private void btnPrimeiro_Click(object sender, EventArgs e)
        {
            JogadorVO j = JogadorDAO.Primeiro();
            PreencheTela(j);
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            JogadorVO j = JogadorDAO.Proximo(Convert.ToInt32(txtId.Text));
            if (j != null)
                PreencheTela(j);
        }

        private void btnProximo_Click(object sender, EventArgs e)
        {
            JogadorVO j = JogadorDAO.Anterior(Convert.ToInt32(txtId.Text));
            if (j != null)
                PreencheTela(j);
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            JogadorVO j = JogadorDAO.Ultimo();
            PreencheTela(j);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnPrimeiro.PerformClick();
            AlteraParaModo(EnumModoOperacao.Navegacao);
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            if (!Validacoes())
                return;
            try
            {
                JogadorVO j = new JogadorVO();
                j.Id = Convert.ToInt32(txtId.Text);
                j.Nome = txtNome.Text;
                j.NumeroCamisa= Convert.ToInt32(txtNumCamisa.Text);                
                j.TimeId = Convert.ToInt32(txtTimeId.Text);
                if (txtId.Enabled)
                    JogadorDAO.Incluir(j);
                else
                    JogadorDAO.Alterar(j);
                AlteraParaModo(EnumModoOperacao.Navegacao);
            }
            catch (Exception erro)
            {
                TrataErro(erro);
            }
        }
    }
}
