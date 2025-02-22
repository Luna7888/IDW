using ScottPlot;
using ScottPlot.Colormaps;
using ScottPlot.TickGenerators.Financial;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IDW
{
    public partial class Form1 : Form
    {
        private List<double[]> valoresAdiconados = new List<double[]>();
        private int indexValoresAdicionados;


        //FUNÇOES
        void CriaPontos()
        {

        }

        private void PreencheListView(ListView listview, string nome, string x, string y, string intensidade)
        {
            ListViewItem item = new ListViewItem(new[] { nome, x, y, intensidade });
            listview.Items.Add(item);
        }



        public Form1()
        {
            InitializeComponent();
        }



        //EVENTOS TXB
        private void txbEixoX_KeyPress(object sender, KeyPressEventArgs e)
        {
            Key.IntNumber(e);
        }

        private void txbEixoY_KeyPress(object sender, KeyPressEventArgs e)
        {
            Key.IntNumber(e);
        }

        private void txbIntensidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            Key.IntNumber_Ponto(e, sender);
        }

        //EVENTOS BTN
        private void btnCriarGrafico_Click(object sender, EventArgs e)
        {
            CriaPontos();
        }

        private void btnEnviarValores_Click(object sender, EventArgs e)
        {


            if (txbEixoX.Text == "" || txbEixoY.Text == "" || txbIntensidade.Text == "")
            {
                MessageBox.Show("Adicione Valores", "Atenção");
            }
            else
            {
                if(Int32.Parse(txbEixoX.Text) > 100 || Int32.Parse(txbEixoY.Text) > 100  || Int32.Parse(txbEixoX.Text) < 0 || Int32.Parse(txbEixoY.Text) < 0)
                {
                    MessageBox.Show("X ou Y maiores que 100 ou menores que 0");
                }
                else
                {
                    valoresAdiconados.Add([Int32.Parse(txbEixoX.Text), Int32.Parse(txbEixoY.Text), Double.Parse(txbIntensidade.Text)]);

                    txbEixoX.Text = "";
                    txbEixoY.Text = "";
                    txbIntensidade.Text = "";
                    lsvValoresAdicionados.Items.Clear();

                    indexValoresAdicionados = 0;
                    foreach (var values in valoresAdiconados)
                    {
                        indexValoresAdicionados++;
                        PreencheListView(lsvValoresAdicionados, $"Ponto {indexValoresAdicionados}", values[0].ToString(), values[1].ToString(), values[2].ToString());
                    }
                }

            }
        }
    }

}
