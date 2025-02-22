using ScottPlot;
using ScottPlot.Colormaps;
using ScottPlot.Plottables;
using ScottPlot.TickGenerators.Financial;
using System.Drawing;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IDW
{
    public partial class Form1 : Form
    {
        private List<double[]> valoresAdiconados = new List<double[]>();
        private List<Ponto> ListaPonto = new List<Ponto>();
        private List<double> listaDistancias = new List<double>();
        private List<double> listaPesos = new List<double>();
        private double[,] Mapa = new double[100, 100]; // Pelos visto vou ter que mudar o tamanho , se quiser que o painel seja fluido
        private int indexValoresAdicionados;
        private double intensidadeCalculada;


        //FUNÇOES
        void Interpolate(double[,] mapa, List<Ponto> listaponto)
        {
            //Organizando  formula para ter valores nao pré definidos

            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
                {

                    foreach(var ponto in listaponto)
                    {
                        double dP = Math.Sqrt(Math.Pow(ponto.X - x, 2) + Math.Pow(ponto.Y - y, 2));
                        listaDistancias.Add(dP);

                        double iP = 1d / dP;
                        listaPesos.Add(iP);

                        intensidadeCalculada = (ponto.Intensidade * iP) + intensidadeCalculada;

                        intensidadeCalculada = double.IsNaN(intensidadeCalculada) ? 0 : intensidadeCalculada;

                        if (dP < 1)
                        {
                            intensidadeCalculada = ponto.Intensidade;
                        }

                    }
                    double resultado = intensidadeCalculada / listaPesos.Sum();
                    mapa[y, x] = intensidadeCalculada;
                    intensidadeCalculada = 0;
                    
                }

            }
        }
        void CriaPontos()
        {
            ListaPonto.Clear();
            foreach (var ponto in valoresAdiconados){
                Ponto novoPonto = new Ponto((int)ponto[0], (int)ponto[1], ponto[2]);
                ListaPonto.Add(novoPonto);

            }
        }
        void CriaGrafico()
        {
            
            Painel.Plot.Add.Heatmap(Mapa);
            Interpolate(Mapa, ListaPonto);
            
            Painel.Refresh();
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
            CriaGrafico();
        }

        private void btnEnviarValores_Click(object sender, EventArgs e)
        {


            if (txbEixoX.Text == "" || txbEixoY.Text == "" || txbIntensidade.Text == "")
            {
                MessageBox.Show("Adicione Valores", "Atenção");
            }
            else
            {
                if (Int32.Parse(txbEixoX.Text) > 100 || Int32.Parse(txbEixoY.Text) > 100 || Int32.Parse(txbEixoX.Text) < 0 || Int32.Parse(txbEixoY.Text) < 0)
                {
                    MessageBox.Show("X ou Y maiores que 100 ou menores que 0");
                }
                else
                {
                    valoresAdiconados.Add([Int32.Parse(txbEixoX.Text), Int32.Parse(txbEixoY.Text), Double.Parse(txbIntensidade.Text, CultureInfo.InvariantCulture)]);
                    
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

        private void Form1_Load(object sender, EventArgs e)
        {
            lsvValoresAdicionados.Columns.Add("Nome",55, System.Windows.Forms.HorizontalAlignment.Center);
            lsvValoresAdicionados.Columns.Add("X", 55, System.Windows.Forms.HorizontalAlignment.Center);
            lsvValoresAdicionados.Columns.Add("Y", 55, System.Windows.Forms.HorizontalAlignment.Center);
            lsvValoresAdicionados.Columns.Add("Intensidade", 115, System.Windows.Forms.HorizontalAlignment.Center);

            //Variaveis de inicialização do Scott, nao esta aparecendo valored no cb
            Heatmap hm = Painel.Plot.Add.Heatmap(Mapa);
            hm.Colormap = new Thermal();
            hm.Smooth = true;
            Painel.Plot.Add.ColorBar(hm);
        }
    }

}
