using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int[] xAviao = new int[2000];
        int[] yAviao = new int[2000];
        int qtdAvioes = 0;
        Color[] cor = new Color[2000];
        Random rnd = new Random();
        bool[] aviaoAtivo = new bool[2000];

        float[] tempoGasto1 = new float[2000];
        float[] tempoGasto2 = new float[2000];
        float[] tempoGasto3 = new float[2000];
        float[] tempoGasto4 = new float[2000];
        float[] tempoGasto5 = new float[2000];
        float[] tempoGasto6 = new float[2000];
        float[] tempoGasto7 = new float[2000];
        float[] tempoGasto8 = new float[2000];
        float[] tempoTotal = new float[2000];

        bool[] hangar = { true, true, true, true, true, true, true, true, true, true };
        string[] relatorio = { "", "", "", "", "", "", "", "", "", "" };
        int pousando = 0;
        int decolando = 0;

        Object locker1 = new Object();
        Object locker2 = new Object();
        Object locker3 = new Object();
        Object locker4 = new Object();
        Object locker5 = new Object();

        public Form1()
        {
            InitializeComponent();     

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label1.Text = DateTime.Now.ToString("HH:mm:ss");
            if (qtdAvioes > 0)
            {
                this.label12.Text = "Voando: " + (tempoGasto1[qtdAvioes - 1] / 1000).ToString() + " s";
                this.label13.Text = "Pousando: " + (tempoGasto3[qtdAvioes - 1] / 1000).ToString() + " s";
                this.label14.Text = "Pista Hangar: " + (tempoGasto4[qtdAvioes - 1] / 1000).ToString() + " s";
                this.label15.Text = "Hangar: " + (tempoGasto6[qtdAvioes - 1] / 1000).ToString() + " s";
                this.label16.Text = "Decolando: " + (tempoGasto8[qtdAvioes - 1] / 1000).ToString() + " s";
            }

            this.textBox1.Text = ("Relatório Hangar 1\nNº      |   Hora\n" + relatorio[0]).Replace("\n", "\r\n");
            this.textBox2.Text = ("Relatório Hangar 2\nNº      |   Hora\n" + relatorio[1]).Replace("\n", "\r\n");
            this.textBox3.Text = ("Relatório Hangar 3\nNº      |   Hora\n" + relatorio[2]).Replace("\n", "\r\n");
            this.textBox4.Text = ("Relatório Hangar 4\nNº      |   Hora\n" + relatorio[3]).Replace("\n", "\r\n");
            this.textBox5.Text = ("Relatório Hangar 5\nNº      |   Hora\n" + relatorio[4]).Replace("\n", "\r\n");
            this.textBox6.Text = ("Relatório Hangar 6\nNº      |   Hora\n" + relatorio[5]).Replace("\n", "\r\n");
            this.textBox7.Text = ("Relatório Hangar 7\nNº      |   Hora\n" + relatorio[6]).Replace("\n", "\r\n");
            this.textBox8.Text = ("Relatório Hangar 8\nNº      |   Hora\n" + relatorio[7]).Replace("\n", "\r\n");
            this.textBox9.Text = ("Relatório Hangar 9\nNº      |   Hora\n" + relatorio[8]).Replace("\n", "\r\n");
            this.textBox10.Text = ("Relatório Hangar 10\nNº      |   Hora\n" + relatorio[9]).Replace("\n", "\r\n");
            
            funcDesenha();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            qtdAvioes++;
            xAviao[qtdAvioes-1] = 1008;
            yAviao[qtdAvioes-1] = 100;
            cor[qtdAvioes - 1] = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            aviaoAtivo[qtdAvioes - 1] = true;
            new Thread(() => aviao(qtdAvioes-1)).Start();
         }

        public void funcDesenha()
        {
            Bitmap imgFundo = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics objDesenha = Graphics.FromImage(imgFundo);
            Pen varLapis = new Pen(Color.Black, 4);
            Pen lapisAviao = new Pen(Color.Black, 2);
            
            Pen dashHangar = new Pen(Color.Blue, 4);
            Pen dashPartida = new Pen(Color.Green, 4);
            Pen dashPista = new Pen(Color.Black, 4);

            Font varFont = new Font("Microsoft Sans Serif", 12);
            Brush brushTexto = new SolidBrush(Color.Black);
            Brush brushFundo = new SolidBrush(Color.White);
            
            dashHangar.DashPattern = new float[] {3.0F, 3.0F};
            dashPartida.DashPattern = new float[] {3.0F, 3.0F};
            dashPista.DashPattern = new float[] {3.0F, 3.0F};

            //desenho das linhas de partida e de chegada
            objDesenha.DrawLine(dashPartida, 968, 90, 968, 150);
            objDesenha.DrawLine(dashPartida, 550, 90, 550, 150);

            // desenho da pista de pouso
            objDesenha.DrawLine(varLapis, 120, 90, 550, 90);
            objDesenha.DrawLine(varLapis, 180, 150, 550, 150);
            objDesenha.DrawLine(varLapis, 120, 150, 120, 90);
            objDesenha.DrawLine(dashPista, 120, 150, 180, 150);

            // desenho do lugar de espera para a decolagem
            objDesenha.DrawLine(dashPista, 550, 150, 550, 210);
            objDesenha.DrawLine(dashPista, 490, 150, 490, 210);
            objDesenha.DrawLine(dashPista, 490, 210, 550, 210);

            // desenho pista de acesso aos hangares
            objDesenha.DrawLine(varLapis, 120, 150, 120, 300);
            objDesenha.DrawLine(varLapis, 180, 150, 180, 240);
            objDesenha.DrawLine(varLapis, 120, 300, 180, 300);

            // desenho dos hangares
            objDesenha.DrawLine(dashHangar, 180, 240, 180, 300);
            objDesenha.DrawLine(dashHangar, 240, 240, 240, 300);
            objDesenha.DrawLine(dashHangar, 300, 240, 300, 300);
            objDesenha.DrawLine(dashHangar, 360, 240, 360, 300);
            objDesenha.DrawLine(dashHangar, 420, 240, 420, 300);
            objDesenha.DrawLine(dashHangar, 480, 240, 480, 300);
            objDesenha.DrawLine(dashHangar, 540, 240, 540, 300);
            objDesenha.DrawLine(dashHangar, 600, 240, 600, 300);
            objDesenha.DrawLine(dashHangar, 660, 240, 660, 300);
            objDesenha.DrawLine(dashHangar, 720, 240, 720, 300);
            objDesenha.DrawLine(dashHangar, 780, 240, 780, 300);
            objDesenha.DrawLine(dashHangar, 180, 240, 780, 240);
            objDesenha.DrawLine(dashHangar, 180, 300, 780, 300);

            // desenho dos aviões
            for (int i = qtdAvioes-1; i >= 0; i--)
            {
                if (aviaoAtivo[i])
                {

                    Brush varBrush = new SolidBrush(cor[i]);
                    PointF[] pointsAviao = new PointF[] {new PointF(0,0)};

                    if (yAviao[i] < 110) {
                        pointsAviao = new PointF[] {
                            new PointF(xAviao[i]-41,yAviao[i]+20),
                            new PointF(xAviao[i]-35,yAviao[i]+23),
                            new PointF(xAviao[i]-23,yAviao[i]+24),
                            new PointF(xAviao[i]-10,yAviao[i]+36),
                            new PointF(xAviao[i]-13,yAviao[i]+23),
                            new PointF(xAviao[i]+4,yAviao[i]+23),
                            new PointF(xAviao[i]+11,yAviao[i]+29),
                            new PointF(xAviao[i]+10,yAviao[i]+20),
                            new PointF(xAviao[i]+11,yAviao[i]+11),
                            new PointF(xAviao[i]+4,yAviao[i]+17),
                            new PointF(xAviao[i]-13,yAviao[i]+17),
                            new PointF(xAviao[i]-10,yAviao[i]+4),
                            new PointF(xAviao[i]-23,yAviao[i]+16),
                            new PointF(xAviao[i]-35,yAviao[i]+17),
                            new PointF(xAviao[i]-41,yAviao[i]+20)
                        };
                    }

                    if (xAviao[i] <= 170 && yAviao[i] >= 110 && yAviao[i] < 250) {
                        pointsAviao = new PointF[] {
                            new PointF(xAviao[i]-20,yAviao[i]+53),
                            new PointF(xAviao[i]-17,yAviao[i]+47),
                            new PointF(xAviao[i]-16,yAviao[i]+35),
                            new PointF(xAviao[i]-4,yAviao[i]+22),
                            new PointF(xAviao[i]-17,yAviao[i]+25),
                            new PointF(xAviao[i]-17,yAviao[i]+8),
                            new PointF(xAviao[i]-11,yAviao[i]+1),
                            new PointF(xAviao[i]-20,yAviao[i]+2),
                            new PointF(xAviao[i]-29,yAviao[i]+1),
                            new PointF(xAviao[i]-23,yAviao[i]+8),
                            new PointF(xAviao[i]-23,yAviao[i]+25),
                            new PointF(xAviao[i]-36,yAviao[i]+22),
                            new PointF(xAviao[i]-24,yAviao[i]+35),
                            new PointF(xAviao[i]-23,yAviao[i]+47),
                            new PointF(xAviao[i]-20,yAviao[i]+53)
                        };
                    }


                    if (yAviao[i] >= 240) {
                        pointsAviao = new PointF[] {
                            new PointF(xAviao[i]+6,yAviao[i]+20),
                            new PointF(xAviao[i],yAviao[i]+23),
                            new PointF(xAviao[i]-12,yAviao[i]+24),
                            new PointF(xAviao[i]-25,yAviao[i]+36),
                            new PointF(xAviao[i]-22,yAviao[i]+23),
                            new PointF(xAviao[i]-39,yAviao[i]+23),
                            new PointF(xAviao[i]-46,yAviao[i]+29),
                            new PointF(xAviao[i]-45,yAviao[i]+20),
                            new PointF(xAviao[i]-46,yAviao[i]+11),
                            new PointF(xAviao[i]-39,yAviao[i]+17),
                            new PointF(xAviao[i]-22,yAviao[i]+17),
                            new PointF(xAviao[i]-25,yAviao[i]+4),
                            new PointF(xAviao[i]-12,yAviao[i]+16),
                            new PointF(xAviao[i],yAviao[i]+17),
                            new PointF(xAviao[i]+6,yAviao[i]+20)
                        };
                    }

                    if (xAviao[i] == 540 && yAviao[i] >= 110) {
                        pointsAviao = new PointF[] {
                            new PointF(xAviao[i]-20,yAviao[i]-6),
                            new PointF(xAviao[i]-17,yAviao[i]),
                            new PointF(xAviao[i]-16,yAviao[i]+12),
                            new PointF(xAviao[i]-4,yAviao[i]+25),
                            new PointF(xAviao[i]-17,yAviao[i]+22),
                            new PointF(xAviao[i]-17,yAviao[i]+39),
                            new PointF(xAviao[i]-11,yAviao[i]+46),
                            new PointF(xAviao[i]-20,yAviao[i]+45),
                            new PointF(xAviao[i]-29,yAviao[i]+46),
                            new PointF(xAviao[i]-23,yAviao[i]+39),
                            new PointF(xAviao[i]-23,yAviao[i]+22),
                            new PointF(xAviao[i]-36,yAviao[i]+25),
                            new PointF(xAviao[i]-24,yAviao[i]+12),
                            new PointF(xAviao[i]-23,yAviao[i]),
                            new PointF(xAviao[i]-20,yAviao[i]-6)
                        };
                    }


                    PointF[] fundoTexto = new PointF[] {
                        new PointF(xAviao[i],yAviao[i]-25),
                        new PointF(xAviao[i]-40,yAviao[i]-25),
                        new PointF(xAviao[i]-40,yAviao[i]-5),
                        new PointF(xAviao[i],yAviao[i]-5)
                    };

                    objDesenha.FillPolygon(varBrush, pointsAviao);
                    objDesenha.FillPolygon(brushFundo, fundoTexto);
                    objDesenha.DrawPolygon(lapisAviao, pointsAviao);
                    objDesenha.DrawString((i + 1).ToString("D4"), varFont, brushTexto, xAviao[i]-40, yAviao[i]-25);


                }
            }
            pictureBox1.BackgroundImage = imgFundo;
        }

        public void aviao(int nAviao)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // deslocamento no eixo x para a esquerda (aviões voando)
            for (int i = 1008; i >= 590; i = i - 4)
            {
                xAviao[nAviao] = i;
                Thread.Sleep(92);
            }
            
            tempoGasto1[nAviao] = stopwatch.ElapsedMilliseconds;
            tempoTotal[nAviao] = tempoGasto1[nAviao];

            // locker para que uma thread não incremente a variável pousando junto com outra thread
            lock (locker1){pousando++;}

            // locker para que pouse um avião na pista de cada vez
            lock (locker2)
            {
                while (true)
                {
                    if (decolando == 0)
                    {
                        tempoGasto2[nAviao] = stopwatch.ElapsedMilliseconds - tempoTotal[nAviao];
                        tempoTotal[nAviao] = tempoTotal[nAviao] + tempoGasto2[nAviao];

                        // deslocamento no eixo x para a esquerda (pouso)
                        for (int i = 590; i >= 170; i = i - 20)
                        {
                            xAviao[nAviao] = i;
                            Thread.Sleep(32);
                        }

                        tempoGasto3[nAviao] = stopwatch.ElapsedMilliseconds - tempoTotal[nAviao];
                        tempoTotal[nAviao] = tempoTotal[nAviao] + tempoGasto3[nAviao];

                        lock (locker1) { pousando--; }
                        break;
                    }
                }
            }

            // deslocamento para baixo no eixo y (pista de acesso aos hangares)
            for (int i = 100; i <= 250; i = i + 10)
            {
                yAviao[nAviao] = i;
                Thread.Sleep(50);
            }

            tempoGasto4[nAviao] = stopwatch.ElapsedMilliseconds - tempoTotal[nAviao];
            tempoTotal[nAviao] = tempoTotal[nAviao] + tempoGasto4[nAviao];

            int nHangar = 0;
            bool esperando = true;

            // locker para que um avião estacione de cada vez
            lock (locker3)
            {
                while (esperando)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (hangar[i])
                        {
                            hangar[i] = false;
                            esperando = false;
                            nHangar = i + 1;
                            xAviao[nAviao] = 170 + 60 * (i + 1); // o avião vai para o hangar livre
                            relatorio[i] = (nAviao + 1).ToString("D4") + "  |  " + DateTime.Now.ToString("HH:mm:ss") + "\n" + relatorio[i];
                            break;
                        }
                    }
                }
            }
            tempoGasto5[nAviao] = stopwatch.ElapsedMilliseconds - tempoTotal[nAviao];
            tempoTotal[nAviao] = tempoTotal[nAviao] + tempoGasto5[nAviao];

            Thread.Sleep(60000);
            tempoGasto6[nAviao] = stopwatch.ElapsedMilliseconds - tempoTotal[nAviao];
            tempoTotal[nAviao] = tempoTotal[nAviao] + tempoGasto6[nAviao];

            // o avião vai para o lugar de espera para a decolagem
            xAviao[nAviao] = 540;
            yAviao[nAviao] = 160;

            hangar[nHangar - 1] = true;

            // locker para que um avião inicie a decolagem de cada vez, pois apesar de poder
            //haver dois aviões na pista de decolagem é ideal que um avião espere o outro avião
            //chegar pelo menos no meio da pista para acessar a pista de decolagem
            lock (locker4)
            {
                while (true)
                {
                    if (pousando == 0 && decolando < 2)
                    {                           
                        // locker para que uma thread não incremente a variável decolando junto com outra thread
                        lock (locker5) {decolando++;}

                        tempoGasto7[nAviao] = stopwatch.ElapsedMilliseconds - tempoTotal[nAviao];
                        tempoTotal[nAviao] = tempoTotal[nAviao] + tempoGasto7[nAviao];

                        // deslocamento para cima no eixo y (entrada na pista de decolagem)                           
                        for (int i = 160; i >= 100; i = i - 5)
                        {
                            yAviao[nAviao] = i;
                            Thread.Sleep(40);
                        }

                        // deslocamento para a esquerda no eixo x (até o meio da pista de decolagem)
                        for (int i = 540; i >= 400; i = i - 5)
                        {
                            xAviao[nAviao] = i;
                            Thread.Sleep(15);
                        }
                        break;

                    }
                }
            }

            // deslocamento para a esquerda no eixo x (decolagem)
            for (int i = 400; i >= 120; i = i - 5)
            {
                xAviao[nAviao] = i;
                Thread.Sleep(1);
            }

            tempoGasto8[nAviao] = stopwatch.ElapsedMilliseconds - tempoTotal[nAviao];

            lock (locker5){decolando--;}

            // deslocamento para a esquerda no eixo x (aviões voando até sumir da tela)
            for (int i = 120; i >= 0; i = i - 5)
            {
                xAviao[nAviao] = i;
                Thread.Sleep(5);
            }

            aviaoAtivo[nAviao] = false;        

        }

    }

}
