using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDW
{
    class Ponto(int x, int y, double intensidade)
    {

        private int _x = x;
        private int _y = y;
        private double _intensidade = intensidade;


        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        public double Intensidade { get => _intensidade; set => _intensidade = value; }
    }
}
