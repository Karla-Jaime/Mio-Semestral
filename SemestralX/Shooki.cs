using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SemestralX
{
    public class Shooki
    {
        

        public enum Direccion { Izquierda, Derecha};
        Direccion DireccionActual { get; set; }

        double PosicionX { get; set; }

    }
}
