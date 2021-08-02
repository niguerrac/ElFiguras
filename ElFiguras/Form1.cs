using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElFiguras
{
    public partial class Form1 : Form
    {
        Graphics papel;
        //Variables figuras
        enum Figura { Cuadrado, Hexagono, Circulo, Triangulo };
        Figura figura = Figura.Cuadrado;

        SolidBrush brush = new SolidBrush(Color.Yellow);
        int x=80;
        int y=80;

        int y_objetivo = 0;
        int x_objetivo = 0;
        float radius = 40;

        //Variables Mov
        float radioMov = 20;
        float h=0;
        float k=0;

        int multpiplicador= 1;

        Timer t = new Timer();
        Timer tCirculo = new Timer();
        Timer tCuadrado = new Timer();


        public Form1()
        {
            InitializeComponent();

            tCuadrado.Interval = 10;
            tCuadrado.Tick += new EventHandler(tCuadrado_Tick);

            tCirculo.Interval = 10;
            tCirculo.Tick += new EventHandler(tCirculo_Tick);

            t.Interval = 10;
            t.Tick += new EventHandler(t_Tick);

            y_objetivo = y;
            x_objetivo = x;

            papel = pictureBox1.CreateGraphics();

            label_x.Text = "X: " + x.ToString();
            label_y.Text = "Y: " + y.ToString();

            string[] txtData = {"Dimencion",radius.ToString(),"ColorR",brush.Color.R.ToString(), "ColorG", brush.Color.G.ToString(), "ColorB",brush.Color.B.ToString()
                    ,"Movimiento_punto_A_X",x.ToString(),"Movimiento_punto_A_Y",y.ToString(),"Movimiento_punto_B_X",x_objetivo.ToString(),"Movimiento_punto_C_Y",y_objetivo.ToString()};

            string docPath =
          Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            File.WriteAllLines(docPath+@"\Figura.fig", txtData);
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            multpiplicador = (int)numericUpDown1.Value;
        }



        private void label1_Click(object sender, EventArgs e)
        {

        }
       
        //########################
        #region Dibujar_figuras
        public void DibujarForma()
        {
            
            switch (figura){
                case  Figura.Cuadrado:
                    papel.FillRectangle(brush, x-(radius), y-(radius), radius*2, radius*2);
                    break;
                case Figura.Hexagono:
                    var shape = new PointF[6];


                    for (int a = 0; a < 6; a++)
                    {
                        shape[a] = new PointF(
                            x + radius * (float)Math.Cos(a * 60 * Math.PI / 180f),
                            y + radius * (float)Math.Sin(a * 60 * Math.PI / 180f));
                    }

                    papel.FillPolygon(brush, shape);
                    break;
                case Figura.Circulo:
                    papel.FillEllipse(brush, x - radius, y - radius,
                      radius + radius, radius + radius);
                    break;
                case Figura.Triangulo:
                    var Trian = new PointF[3];
                    Trian[0] = new PointF(x, y - radius);
                    Trian[1] = new PointF(x-radius, y + radius);
                    Trian[2] = new PointF(x + radius, y + radius);

                    papel.FillPolygon(brush, Trian);
                    break;
            }
        }
        private void btnCuadrado_Click(object sender, EventArgs e)
        {
            figura = Figura.Cuadrado;
            papel.Clear(Color.White);
            DibujarForma();
        }
        private void btnHexagono_Click(object sender, EventArgs e)
        {
            figura = Figura.Hexagono;
            papel.Clear(Color.White);
            DibujarForma();
        }

        private void btnCirculo_Click(object sender, EventArgs e)
        {
            figura = Figura.Circulo;
            papel.Clear(Color.White);
            DibujarForma();
        }

        private void btnTriangulo_Click(object sender, EventArgs e)
        {
            figura = Figura.Triangulo;
            papel.Clear(Color.White);
            DibujarForma();
        }
        #endregion
        //########################
        #region Movimientos
        private void btnSubir_Click(object sender, EventArgs e)
        {
            y_objetivo -= multpiplicador;
            t.Stop();
            tCirculo.Stop();
            tCuadrado.Stop();
            t.Start();
        }
        private void btnIzquierda_Click(object sender, EventArgs e)
        {
            x_objetivo -= multpiplicador;
            t.Stop();
            tCirculo.Stop();
            tCuadrado.Stop();
            t.Start();
        }
        private void btnBajar_Click(object sender, EventArgs e)
        {
            y_objetivo += multpiplicador;
            t.Stop();
            tCirculo.Stop();
            tCuadrado.Stop();
            t.Start();
        }

        private void btnDerecha_Click(object sender, EventArgs e)
        {
            x_objetivo += multpiplicador;
            t.Stop();
            tCirculo.Stop();
            tCuadrado.Stop();
            t.Start();
        }
        private void t_Tick(object sender, EventArgs e)
        {

            //x++;
            if (y_objetivo < y)
            {
                y--;
            }
            if (y_objetivo > y)
            {
                y++;
            }
            if (x_objetivo < x)
            {
                x--;
            }
            if (x_objetivo > x)
            {
                x++;
            }
            label_x.Text = "X: " + x.ToString();
            label_y.Text = "Y: " + y.ToString();

            papel.Clear(Color.White);
            DibujarForma();

            if (x == x_objetivo && y == y_objetivo)
                t.Stop();
        }
        private void btnCircular_Click(object sender, EventArgs e)
        {
            h = x;
            k = y + radioMov* multpiplicador ;

            CambioSigno = false;
            tCirculo.Stop();
            tCuadrado.Stop();
            t.Stop();
            tCirculo.Start();
        }
        bool CambioSigno = false;
        private void tCirculo_Tick(object sender, EventArgs e)
        {

            if (!CambioSigno)
            {
                if (x < h + radioMov * multpiplicador)
                {
                    x++;
                    y = (int)(-Math.Sqrt(radioMov * multpiplicador * radioMov * multpiplicador - Math.Pow(x - h, 2)) + k);
                }
                else { CambioSigno = true; }
            }

            if (CambioSigno)
            {
                if (x > h - radioMov * multpiplicador)
                {
                    x--;
                    y = (int)(Math.Sqrt(radioMov * multpiplicador * radioMov * multpiplicador - Math.Pow(x - h, 2)) + k);
                }
                else { CambioSigno = false; }
            }
           
            label_x.Text = "X: " + x.ToString();
            label_y.Text = "Y: " + y.ToString();
            y_objetivo = y;
            x_objetivo = x;

            papel.Clear(Color.White);
            DibujarForma();
            if (x == h && y == k-radioMov* multpiplicador)
                tCirculo.Stop();
        }

        private void btnMovCuadrado_Click(object sender, EventArgs e)
        {
            h = x;
            k = y;
            CambioSigno = false;
            tCirculo.Stop();
            tCuadrado.Stop();
            t.Stop();
            tCuadrado.Start();
        }
        private void tCuadrado_Tick(object sender, EventArgs e)
        {
            
            if (!CambioSigno)
            {
                if (x < h +radioMov* multpiplicador  * 2)
                    x++;
                else
                {
                    if (y < k +radioMov* multpiplicador * 2)
                        y++;
                    else
                        CambioSigno = true;
                }
            }
            if (CambioSigno)
            {
                if (x > h)
                    x--;
                else
                {
                    if (y > k)
                        y--;
                    else
                        CambioSigno = false;
                }
            }

            label_x.Text = "X: " + x.ToString();
            label_y.Text = "Y: " + y.ToString();
            y_objetivo = y;
            x_objetivo = x;
            papel.Clear(Color.White);
            DibujarForma();
            if (x == h && y == k)
                tCuadrado.Stop();
        }
        #endregion
        private void btnCargar_Click(object sender, EventArgs e)
        {
            t.Stop();
            tCirculo.Stop();
            tCuadrado.Stop();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Seleccion Figura";
            string docPath =
          Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog1.InitialDirectory = docPath;
            openFileDialog1.Filter = "All files (*.fig*)|*.fig*|All files (*.fig*)|*.fig*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    string[] text = File.ReadAllLines(file);

                    if (text.Length == 16)
                    {
                        radius = float.Parse(text[1], CultureInfo.InvariantCulture.NumberFormat);
                        Color color = new Color();
                        color = Color.FromArgb(Int32.Parse(text[3]),Int32.Parse(text[5]) , Int32.Parse(text[7]));
                        brush.Color = color;
                        x = Int32.Parse(text[9]);
                        y = Int32.Parse(text[11]);
                        x_objetivo = Int32.Parse(text[13]);
                        y_objetivo = Int32.Parse(text[15]);
                        papel.Clear(Color.White);
                        DibujarForma();
                        
                        t.Start();
                    }
                }
                catch (IOException)
                {
                }
            }

        }
    }
}
