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
using System.Threading; //Se agrego esta libreria para Threading
using System.Diagnostics; // Para el dispatcher y timers 

namespace SemestralX
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Stopwatch stopwatch; //Toma el tiempo de ejecución del programa
        TimeSpan tiempoAnterior; //Timespan guarda rangos de tiempo

        List<Enemigos> enemigos = new List<Enemigos>();
        int puntos = 0;
        enum EstadoJuego { GamePlay, GameOver,GameStart };
        EstadoJuego estadoActual = EstadoJuego.GameStart;
        enum Direccion { Derecha, Izquierda, Ninguna };  //Para aclarar la direccion del jugador
        Direccion direccionJugador = Direccion.Ninguna; //Inicializar

        double velG = 130;

        public MainWindow()
        {
            InitializeComponent();
            canvasGamePlay.Focus();

            stopwatch = new Stopwatch();
            stopwatch.Start();
            tiempoAnterior = stopwatch.Elapsed;

            enemigos.Add(new Enemigos(imgE));
            enemigos.Add(new Enemigos(imgE2));
            

            ThreadStart threadStart = new ThreadStart(actualizar);
            //2..Inicializar el Thread - Dar valores e instrucciones
            Thread threadMoverEnemigos = new Thread(threadStart);
            //3..Ejecutar el Thread
            threadMoverEnemigos.Start();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            canvasStart.Visibility = Visibility.Collapsed;
            canvasGamePlay.Visibility = Visibility.Visible;
            this.estadoActual = EstadoJuego.GamePlay;
            canvasGamePlay.Focus();
        }

        void moverjugador(TimeSpan deltaTime)
        {
            double LeftNaveActual = Canvas.GetLeft(imgG);
            
            switch (direccionJugador)
            {
             
                case Direccion.Izquierda: //Para que no salga por la izquierda

                    if (LeftNaveActual - (velG * deltaTime.TotalSeconds) >= 0)
                    {
                        Canvas.SetLeft(imgG, LeftNaveActual - (velG * deltaTime.TotalSeconds));
                    }
                    break;
                case Direccion.Derecha:
                    double nuevaPosicion = LeftNaveActual + (velG * deltaTime.TotalSeconds);
                    if (nuevaPosicion + imgG.Height <= 720)
                    {
                        Canvas.SetLeft(imgG, nuevaPosicion);
                    }

                    break;
                case Direccion.Ninguna:
                    break;
            }
        }

        void actualizar()
        {   //Invoke lleva de parametro una función
            while (true)
            {
                Dispatcher.Invoke(
                 () => //Se creo una función nueva dentro de otra. La => es para indicar que es otra función
                 {
                     var tiempoActuali = stopwatch.Elapsed;
                     var deltaTime = tiempoActuali - tiempoAnterior;



                     if (estadoActual == EstadoJuego.GamePlay)
                     {

                         //moverjugador
                         //Se agrega al parametro utilizado 
                         moverjugador(deltaTime);
                         movimientoEnemigos(deltaTime);
                         canvasGamePlay.Focus();
                         //Intersecciones :(
                         foreach (Enemigos enemigos in enemigos)
                         {
                             double xG = Canvas.GetLeft(imgG);
                             double yG = Canvas.GetTop(imgG);
                             double xEnemigos = Canvas.GetLeft(enemigos.Imagen);
                             double yEnemigos = Canvas.GetTop(enemigos.Imagen);

                             if (xEnemigos + enemigos.Imagen.Width >= xG && xEnemigos <= xG + imgG.Width && yEnemigos + enemigos.Imagen.Height >= yG && yEnemigos <= yG + imgG.Height)
                             {
                                 estadoActual = EstadoJuego.GameOver;
                                 canvasGamePlay.Visibility = Visibility.Collapsed;
                                 canvasGameOver.Visibility = Visibility.Visible;
                                
                             }
                             
                            
                         }
                     }
                     tiempoAnterior = tiempoActuali;
                 }
                );
            }
        }

        void movimientoEnemigos(TimeSpan deltaTime)
        {
           
            double velocidadE1 = 150;
            double velocidadE2 = 150;
            
            velocidadE1 += 20 * deltaTime.TotalSeconds;
            velocidadE2 += 12 * deltaTime.TotalSeconds;
            

            double leftE = Canvas.GetTop(imgE);
            // se mueve 120 pixeles por segundo
            Canvas.SetTop(imgE, leftE + (velocidadE1 * deltaTime.TotalSeconds));
            if (Canvas.GetTop(imgE) >= 450)
            {
                Canvas.SetTop(imgE, -100);
            }
            double leftEnem2 = Canvas.GetTop(imgE2);
            // se mueve 120 pixeles por segundo
            Canvas.SetTop(imgE2, leftEnem2 + (velocidadE2 * deltaTime.TotalSeconds));
            if (Canvas.GetTop(imgE2) >= 450)
            {
                Canvas.SetTop(imgE2, -100);
            }


            foreach (Enemigos enem in enemigos)
            {
               if (Canvas.GetTop((imgE)) <= 170 && Canvas.GetTop(imgE) >= 169.99)
               {
                   puntos = puntos + 100;
                   lblScore.Text = puntos.ToString();
                   lblScoreF.Text = puntos.ToString();
                }
                if (Canvas.GetTop((imgE2)) <= 170 && Canvas.GetTop(imgE2) >= 170)
               {
                   puntos = puntos + 150;
                   lblScore.Text = puntos.ToString();
                   lblScoreF.Text = puntos.ToString();
               }
            }

            

        }
        private void canvasGamePlay_KeyDown(object sender, KeyEventArgs e)
        {
            if (estadoActual == EstadoJuego.GamePlay)
            {
               

                if (e.Key == Key.Left)
                {
                    direccionJugador = Direccion.Izquierda;
                }

                if (e.Key == Key.Right)
                {
                    direccionJugador = Direccion.Derecha;
                }
            }
        }

        private void canvasGamePlay_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Left && direccionJugador == Direccion.Izquierda)
            {
                direccionJugador = Direccion.Ninguna;
            }

            if (e.Key == Key.Right && direccionJugador == Direccion.Derecha)
            {
                direccionJugador = Direccion.Ninguna;
            }
        }
    }
}
