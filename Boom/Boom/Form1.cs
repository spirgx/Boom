using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Boom
{
    public partial class Form1 : Form
    {
        private const int Row = 10;
        private const int Col = 10;
        private const int Booms = 10;
        Label[,] Grid = new Label[Row, Col];
        String[,] Data = new String[Row, Col];
        String[,] View = new String[Row, Col];
        Boolean[,] Mark = new Boolean[Row, Col];

        
        public Form1()
        {
            InitializeComponent();
        }

        private void InitialGame()
        {
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    Data[i, j] = "";
                    View[i, j] = "";
                }
            }

            Random rnd1 = new Random(Guid.NewGuid().GetHashCode());
            Random rnd2 = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < Booms; i++)
            {
                int r = 0, c = 0;
                while (Data[r = rnd1.Next(0, Row), c = rnd2.Next(0, Col)] == "●");
                Data[r, c] = "●";
            }

            CountBooms();            

        }

        private void CountBooms()
        {
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    if (Data[i, j] != "●") NineGrid(i, j);
                }
            }

        }

        private void NineGrid(int i,int j)
        {
            int count = 0;
            for (int r = -1; r <= 1; r++)
            {
                for (int c = -1; c <= 1 ; c++)
                {
                    if (i + r < 0 || i + r >= Row ||  j + c < 0 || j + c >= Col) continue;
                    if (Data[i + r, j + c] == "●") count++;
                }
            }
            Data[i, j] = count.ToString();
        }

        private void DisplayView()
        {
            for (int i = 0; i < Row; i++)
                for (int j = 0; j < Col; j++)
                    Grid[i, j].Text = View[i, j];
        }

        private void DisplayData()
        {
            for (int i = 0; i < Row; i++)
                for (int j = 0; j < Col; j++)
                    Grid[i, j].Text = Data[i, j];
        }

        private void Form1_Load(object sender, EventArgs e)
        {        

            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    Grid[i, j] = new Label();
                    Grid[i, j].Name = "Label" + i.ToString("D2") + j.ToString("D2");
                    Grid[i, j].AutoSize = false;
                    Grid[i, j].BackColor = Color.CornflowerBlue;
                    Grid[i, j].ForeColor = Color.Black;
                    Grid[i, j].TextAlign = ContentAlignment.MiddleCenter;
                    Grid[i, j].Height = 30;
                    Grid[i, j].Width = 30;
                    Grid[i, j].Location = new Point(40 * j + 40, 40 * i + 40);
                    Grid[i, j].MouseClick += new MouseEventHandler(control_RightMouseClick);
                    this.Controls.Add(Grid[i, j]);
                }
            }

            this.Height = (Row + 1) * 40 + 70;
            this.Width = (Col+1) * 40 + 50;

            InitialGame();
            DisplayView();
        }

        private void control_RightMouseClick(object sender, MouseEventArgs e)
        {
            String name = ((Label)sender).Name;
            int i = int.Parse(name.Substring(name.Length - 4, 2));
            int j = int.Parse(name.Substring(name.Length - 2));

            if (e.Button == MouseButtons.Right)
            {
                if (View[i, j] == "M")
                {
                    ((Label)sender).BackColor = Color.CornflowerBlue;
                    ((Label)sender).Text = "";
                    View[i, j] = "";
                    Mark[i, j] = false;
                }
                else
                {
                    ((Label)sender).BackColor = Color.Green;
                    ((Label)sender).Text = "M";
                    View[i, j] = "M";
                    Mark[i, j] = true;
                }
            }

            if (e.Button == MouseButtons.Left)
            {
                ((Label)sender).BackColor = Color.Gray;
                if (Data[i, j] == "●" && View[i, j] != "M")
                {
                    DisplayData();
                    Mark[i, j] = true;
                    ((Label)sender).ForeColor = Color.Red;
                    this.BackColor = Color.Red;
                    MessageBox.Show("You lose.");
                    return;
                }
                if (Data[i, j] == "0")
                    RecursionZero(i, j);
                else
                {
                    View[i, j] = Data[i, j];
                    Mark[i, j] = true;
                }
            }

            DisplayView();
            CheckWin();
            DisplayView();
        }

        private void CheckWin()
        {
            int count = 0;
            int markBoomsCount = 0;
            int markCount = 0;

            for (int i = 0; i < Row; i++)
                for (int j = 0; j < Col; j++)
                    if (Mark[i, j] == true) count++;


            if (count == Row * Col)
            {
                for (int i = 0; i < Row; i++)
                {
                    for (int j = 0; j < Col; j++)
                    {
                        if (View[i, j] == "M")
                            markCount++;                        
                        if (Data[i, j] == "●" && View[i, j] == "M")
                            markBoomsCount++;
                        if (Data[i, j] != "●" && View[i, j] == "M")
                            View[i, j] = "✖";
                    }
                }
                if (markBoomsCount == Booms && markCount == Booms)
                    MessageBox.Show("You win");
                else
                    MessageBox.Show("Mark Booms Error.");
            }
        }

        private void RecursionZero(int i, int j)
        {
            if (i < 0 || i >= Row || j < 0 || j >= Col) return;

            if (Data[i, j] != "0")
            {
                View[i, j] = Data[i, j];
                Grid[i, j].BackColor = Color.Gray;
                Mark[i, j] = true; 
                return;
            }

            Grid[i, j].BackColor = Color.Gray;
            Data[i, j] = "";
            Mark[i, j] = true;
            View[i, j] = Data[i, j];

            RecursionZero(i - 1, j - 1);
            RecursionZero(i - 1, j);
            RecursionZero(i - 1, j + 1);

            RecursionZero(i, j - 1);
            RecursionZero(i, j + 1);

            RecursionZero(i + 1, j - 1);
            RecursionZero(i + 1, j);
            RecursionZero(i + 1, j + 1);
        }

    }
}
