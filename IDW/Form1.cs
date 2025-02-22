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
        private double numerador;
        private double dps;

        Ponto PontoA = new(25, 25, 80);
        Ponto PontoB = new(75, 25, 10);
        Ponto PontoC = new(75, 75, 10);
        Ponto PontoD = new(25, 75, 50);

        //FUNÇOES
        void Interpolate(double[,] mapa, List<Ponto> listaponto)
        {
            //Organizando  formula para ter valores nao pré definidos

            for (int y = 0; y < mapa.GetLength(0); y++)
            {
                for (int x = 0; x < mapa.GetLength(1); x++)
                {
                    foreach(var ponto in listaponto)
                    {
                        double dP = Math.Sqrt(Math.Pow(ponto.X - x, 2) + Math.Pow(ponto.Y - y, 2));
                        listaDistancias.Add(dP);

                        double iP = Math.Pow((1d / dP),1) ;
                        listaPesos.Add(iP);

                        numerador += (ponto.Intensidade * iP) ;

                        intensidadeCalculada = double.IsNaN(intensidadeCalculada) ? 0  : intensidadeCalculada;

                        if (dP <= 0)
                        {
                            numerador = ponto.Intensidade;
                        }

                    }
                    
                    mapa[y, x] = numerador;
                    numerador = 0;
                    
                }

            }
        }

        void interpolar2(double[,] mapa, Ponto pontoA, Ponto pontoB, Ponto pontoC, Ponto pontoD)
        {
            for (int y = 0; y < mapa.GetLength(0); y++)
            {
                for (int x = 0; x < mapa.GetLength(1); x++)
                {
                    double dPA = Math.Sqrt(Math.Pow(pontoA.X - x, 2) + Math.Pow(pontoA.Y - y, 2));
                    double dPB = Math.Sqrt(Math.Pow(pontoB.X - x, 2) + Math.Pow(pontoB.Y - y, 2));
                    double dPC = Math.Sqrt(Math.Pow(pontoC.X - x, 2) + Math.Pow(pontoC.Y - y, 2));
                    double dPD = Math.Sqrt(Math.Pow(pontoD.X - x, 2) + Math.Pow(pontoD.Y - y, 2));

                    double intensidadeCalculada;

                    double IPA = 1d / dPA;
                    double IPB = 1d / dPB;
                    double IPC = 1d / dPC;
                    double IPD = 1d / dPD;

                    double pC = pontoA.Intensidade * IPA + pontoB.Intensidade * IPB + pontoC.Intensidade * IPC + pontoD.Intensidade * IPD;
                    double pB = IPA + IPB + IPC + IPD;
                    intensidadeCalculada = pC / pB;

                    intensidadeCalculada = double.IsNaN(intensidadeCalculada) ? 0 : intensidadeCalculada;

                    if (dPA < 1)
                    {
                        intensidadeCalculada = pontoA.Intensidade;
                    }
                    if (dPB < 1)
                    {
                        intensidadeCalculada = pontoB.Intensidade;
                    }
                    if (dPC < 1)
                    {
                        intensidadeCalculada = pontoC.Intensidade;
                    }
                    if (dPD < 1)
                    {
                        intensidadeCalculada = pontoD.Intensidade;
                    }

                    mapa[y, x] = intensidadeCalculada;
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
            //Interpolate(Mapa, ListaPonto);
            interpolar2(Mapa,PontoA,  PontoB,  PontoC, PontoD);
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
            Mapa = new double[100, 100];
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

            Heatmap hm = Painel.Plot.Add.Heatmap(Mapa);
            hm.Colormap = new Thermal();
            hm.Smooth = true;
            Painel.Plot.Add.ColorBar(hm);

            //Variaveis de inicialização do Scott, nao esta aparecendo valored no cb


        }
    }

}
