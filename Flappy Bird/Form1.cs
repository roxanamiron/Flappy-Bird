using Flappy_Bird.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flappy_Bird
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //declaram 2 variabile de tip Lista prin care pasarea o sa treaca printre ele
        List<int> Pipe1 = new List<int>();
        List<int> Pipe2 = new List<int>();
        int PipeWidth = 55;
        //diferente dintre cele 2 conducte
        int PipeDiffrentY=140; 
        //diferenta dintre conductele sus/jos
        int PipeDiffrentX = 180;
        bool Start = true;
        bool running;
        int step = 5; //viteza pasarii
        int OriginalX, OriginalY; // stocheaza pozitia pasarii(sus-jis, stg-dreapta)
        bool ResetPipes = false;
        int points;
        bool inPipe = false; //daca luam puncte, daca ne aflam in aceea distanta intre cele 2 pipe-uri
        int score;
        int ScoreDiffrent;

        private void Die()
        {
            running = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            button1.Visible = true;
            button1.Enabled = true;

            ReadAndShowScore();
            points = 0;
            pictureBox1.Location = new Point(OriginalX, OriginalY);
            ResetPipes = true;
            Pipe1.Clear();
        }

        private void ReadAndShowScore()
        {
            using (StreamReader reader = new StreamReader("Score.ini"))
            {
                score = int.Parse(reader.ReadToEnd());  //convertim stringul in inteager
                reader.Close();
                if(int.Parse(label1.Text) == 0 || int.Parse(label1.Text) > 0)
                {
                    ScoreDiffrent = score - int.Parse(label1.Text) + 1; //vrem sa nea cat mai trebuie sa mai zboare pasarea
                                                                       //ca sa ajunga la scorul cel mai mare 
                }
                //daca scorul este mai mic
                if(score < int.Parse(label1.Text))
                {
                    MessageBox.Show(string.Format("Felicitari ai depasit scorul de {0}. Noul scor este {1}", score, label1.Text),
                        "Flappy Bird", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    using (StreamWriter writer = new StreamWriter("Score.ini"))
                    {
                        writer.Write(label1.Text);
                        writer.Close();
                    }
                }
                //daca scorul m.mare decat label
                if(score > int.Parse(label1.Text))
                {
                    MessageBox.Show(string.Format("Mai aveai nevoie de {0} puncte, ca sa depasesti scorul de {1}", 
                      ScoreDiffrent, score),"Flappy Bird", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //daca scorul exact = cu scorul actual
                if(score == int.Parse(label1.Text))
                {
                    MessageBox.Show(string.Format("Scorul tau este {0}, incearca sa depasesti acest scor", score, "Flappy Bird",
                        MessageBoxButtons.OK, MessageBoxIcon.Information));
                }
            }
           
        }

        private void StarGame()
        {
            ResetPipes = false;
            timer1.Enabled = true;
            timer2.Enabled = true;
            timer3.Enabled = true;
            Random random = new Random();
            int num = random.Next(40, this.Height - this.PipeDiffrentY);
            int num1 = num + this.PipeDiffrentY;
            Pipe1.Clear();
            Pipe1.Add(this.Width);
            Pipe1.Add(num);
            Pipe1.Add(this.Width);
            Pipe1.Add(num1);

            num = random.Next(40, (this.Height - PipeDiffrentY));
            num1 = num + PipeDiffrentY;
            Pipe2.Clear();
            Pipe2.Add(this.Width + PipeDiffrentX);
            Pipe2.Add(num);
            Pipe2.Add(this.Width + PipeDiffrentX);
            Pipe2.Add(num1);

            button1.Visible = false;
            button1.Enabled = false;
            running = true;
            Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();  // revalideaza functia de rescriere a unui control
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StarGame();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //creeze conducte aleatoriu
            if(Pipe1[0] + PipeWidth <= 0 || Start == true)
            {
                Random rnd = new Random();
                int px = this.Width;
                int py = rnd.Next(40, (this.Height - PipeDiffrentY));
                var p2x = px;
                var p2y = py + PipeDiffrentY;
                Pipe1.Clear();
                Pipe1.Add(px);
                Pipe1.Add(py);
                Pipe1.Add(p2x);
                Pipe1.Add(p2y);
            }
            else
            {
                Pipe1[0] = Pipe1[0] - 2;
                Pipe1[2] = Pipe1[2] - 2;
            }
            if (Pipe2[0] + PipeWidth <= 0)
            {
                Random rnd = new Random();
                int px = this.Width;
                int py = rnd.Next(40, (this.Height - PipeDiffrentY));
                var p2x = px;
                var p2y = py + PipeDiffrentY;
                int[] p1 = { px, py, p2x, p2y };
                Pipe2.Clear();
                Pipe2.Add(px);
                Pipe2.Add(py);
                Pipe2.Add(p2x);
                Pipe2.Add(p2y);
            }
            else
            {
                Pipe2[0] = Pipe2[0] - 2;
                Pipe2[2] = Pipe2[2] - 2;
            }
            if(Start == true)
            {
                Start = false;                
            }

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //desenam conductele, 2 sus si 2 jos
            if(!ResetPipes && Pipe1.Any() && Pipe2.Any())
            {
                //prima de sus
                e.Graphics.FillRectangle(Brushes.DarkKhaki, new Rectangle(Pipe1[0], 0, PipeWidth, Pipe1[1]));
                e.Graphics.FillRectangle(Brushes.DarkKhaki, new Rectangle(Pipe1[0] - 10, Pipe1[3]- PipeDiffrentY, 65, 15));

                //prima de jos
                e.Graphics.FillRectangle(Brushes.DarkKhaki, new Rectangle(Pipe1[2], Pipe1[3], PipeWidth, this.Height - Pipe1[3]));
                e.Graphics.FillRectangle(Brushes.DarkKhaki, new Rectangle(Pipe1[2] - 10, Pipe1[3], 65, 15));

                //a2a de sus
                e.Graphics.FillRectangle(Brushes.DarkKhaki, new Rectangle(Pipe2[0], 0, PipeWidth, Pipe2[1]));
                e.Graphics.FillRectangle(Brushes.DarkKhaki, new Rectangle(Pipe2[0] - 10, Pipe2[3] - PipeDiffrentY, 65, 15));

                //a 2 a de jos
                e.Graphics.FillRectangle(Brushes.DarkKhaki, new Rectangle(Pipe2[2], Pipe2[3], PipeWidth, this.Height - Pipe2[3]));
                e.Graphics.FillRectangle(Brushes.DarkKhaki, new Rectangle(Pipe2[2] - 10, Pipe2[3], 65, 15));
            }
        }

        private void CheckForPoint()
        {
            Rectangle rec = pictureBox1.Bounds;
            Rectangle rec1 = new Rectangle(Pipe1[2] + 20, Pipe1[3] - PipeDiffrentY, 15, PipeDiffrentY);
            Rectangle rec2 = new Rectangle(Pipe2[2] + 20, Pipe2[3] - PipeDiffrentY, 15, PipeDiffrentY);
            Rectangle intersect1 = Rectangle.Intersect(rec, rec1);
            Rectangle intersect2 = Rectangle.Intersect(rec, rec2);
            if(!ResetPipes | Start)
            {
                if(intersect1 != Rectangle.Empty || intersect2 != Rectangle.Empty)
                {
                    if(!inPipe)
                    {
                        points++;
                        SoundPlayer sp = new SoundPlayer(Flappy_Bird.Properties.Resources.points);
                        sp.Play();
                        inPipe = true;
                    }
                }
                else
                {
                    inPipe = false;
                }
            }
        }

        private void CheckForCollision()
        {
            Rectangle rec = pictureBox1.Bounds;
            Rectangle rec1 = new Rectangle(Pipe1[0], 0, PipeWidth, Pipe1[1]);
            Rectangle rec2 = new Rectangle(Pipe1[2], Pipe1[3], PipeWidth, this.Height - Pipe1[3]);
            Rectangle rec3 = new Rectangle(Pipe2[0], 0, PipeWidth, Pipe2[1]);
            Rectangle rec4 = new Rectangle(Pipe2[2], Pipe2[3], PipeWidth,this.Height - Pipe2[3]);
            Rectangle intersect1 = Rectangle.Intersect(rec, rec1);
            Rectangle intersect2 = Rectangle.Intersect(rec, rec2);
            Rectangle intersect3 = Rectangle.Intersect(rec, rec3);
            Rectangle intersect4 = Rectangle.Intersect(rec, rec4);

            if(!ResetPipes || Start )
            {
                if(intersect1 != Rectangle.Empty | intersect2 != Rectangle.Empty | intersect3 != Rectangle.Empty | intersect4 != Rectangle.Empty)
                {
                    SoundPlayer sp = new SoundPlayer(Flappy_Bird.Properties.Resources.Splat);
                    Die();
                }
            }

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Space:
                    step = -5;
                    pictureBox1.Image = Resources.flappy_straight;
                    break;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            //pasarea este in continua cadere
            pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + step);
            if(pictureBox1.Location.Y < 0)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, 0);
            }
            if(pictureBox1.Location.Y + pictureBox1.Height > this.ClientSize.Height)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, this.ClientSize.Height - pictureBox1.Height);
            }
            CheckForCollision();
            if(running)
            {
                CheckForPoint();
            }
            label1.Text = Convert.ToString(points);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Space:
                    step = 5;
                    pictureBox1.Image = Resources.flappy_down;
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OriginalX = pictureBox1.Location.X;
            OriginalY = pictureBox1.Location.Y;
            //creeam fiserul cu scorul
            if(!File.Exists("Score.ini"))
            {
                File.Create("Score.ini").Dispose();
            }
        }
    }
}
