using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Form1 : Form
    {
        //todo
        //kolorki poprawic
        //dodac promien
        int Map_width, Map_height, NumberOfStates, StateCounter = 0;
        private Graphics g1;
        Grid Map;
        float RecWidth;
        float RecHeight;
        private static System.Timers.Timer aTimer;
        int BC, Columns, Rows, Neighbourhood, Radius, MC_iterations;
        double DeltaT;
        bool flag = false;
        List<Color> colorTable;


        public Form1()
        {

            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void UserClick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            PointF coordinates = me.Location;
            int i = -1, j = -1;

            while (coordinates.X > 0)
            {
                coordinates.X -= RecWidth;
                i++;
            }
            while (coordinates.Y > 0)
            {
                coordinates.Y -= RecHeight;
                j++;
            }

            if (StateCounter < NumberOfStates)
            {
                StateCounter++;
                Map.Map[j, i].SetState(StateCounter);
                DrawCells(Map.Map);
            }


        }

        private void Start_button_Click(object sender, EventArgs e)
        {
            setColorTable();
            try
            {
                Map_width = int.Parse(textBox1.Text);
                Map_height = int.Parse(textBox2.Text);
                NumberOfStates = int.Parse(State_box.Text);
                Neighbourhood = Neigbourhood_box.SelectedIndex;
                BC = comboBox2.SelectedIndex;
                if (Map_width < 0 || Map_height < 0 || NumberOfStates < 0)
                    throw new System.FormatException();
                for (int i = 0; i < 1; i++)
                {
                    Map.UpdateVector_periodical_Hexagonal_random();
                    DrawCells(Map.Map);
                }



            }
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Nieprawidłowe wartości - tylko liczby naturalne");
                return;
            }

        }

        private void DrawCells(Cell[,] Map)
        {
            pictureBox1.Refresh();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g1 = Graphics.FromImage(this.pictureBox1.Image);
            Pen pen = new Pen(Color.Black, 1);
            SolidBrush brush = new SolidBrush(Color.Red);
            float x = 0, y = 0;

            RecWidth = (float)pictureBox1.Width / (float)Map_width;
            RecHeight = (float)pictureBox1.Height / (float)Map_height;

            if (RecWidth > RecHeight)
            {
                RecWidth = RecHeight;
            }
            else
            {
                RecHeight = RecWidth;
            }

            //int transition = 255 / NumberOfStates;

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].GetState() != 0)
                    {
                        for (int k = 1; k < NumberOfStates + 1; k++)
                        {
                            if (Map[i, j].GetState() == k)
                            {
                                brush.Color = colorTable[k];
                            }
                        }
                        //brush.Color = Color.FromArgb(Map[i, j].getCellState() * transition / 2, Map[i, j].getCellState() * transition / 3, Map[i, j].getCellState() * transition / 4);
                    }
                    else
                    {
                        brush.Color = Color.White;
                    }
                    g1.FillRectangle(brush, x, y, RecWidth, RecHeight);

                    if (Mesh_box.Checked == true)
                    {
                        g1.DrawRectangle(pen, x, y, RecWidth, RecHeight);
                    }

                    x += RecWidth;
                }

                x = 0;
                y += RecHeight;
            }
        }


        private void DrawEnergy(int[,] Map)
        {
            pictureBox1.Refresh();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g1 = Graphics.FromImage(this.pictureBox1.Image);
            Pen pen = new Pen(Color.Black, 1);
            SolidBrush brush = new SolidBrush(Color.Red);
            float x = 0, y = 0;

            RecWidth = (float)pictureBox1.Width / (float)Map_width;
            RecHeight = (float)pictureBox1.Height / (float)Map_height;

            if (RecWidth > RecHeight)
            {
                RecWidth = RecHeight;
            }
            else
            {
                RecHeight = RecWidth;
            }
            int transition;
            if (Neighbourhood == 6)
            {
                transition = 1;
            }
            else
            {
                transition = 255 / NumberOfStates;
            }


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {

                    if (Map[i, j] != 0)
                    {
                        brush.Color = Color.FromArgb(Map[i, j] * transition, 0, 0);

                    }

                    else
                    {
                        brush.Color = Color.White;
                    }

                    g1.FillRectangle(brush, x, y, RecWidth, RecHeight);
                    if (Mesh_box.Checked == true)
                    {
                        g1.DrawRectangle(pen, x, y, RecWidth, RecHeight);
                    }
                    x += RecWidth;
                }

                x = 0;
                y += RecHeight;
            }
            //g1.Dispose();
        }

        private void DrawDislocations(Cell[,] Map)
        {
            pictureBox1.Refresh();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g1 = Graphics.FromImage(this.pictureBox1.Image);
            Pen pen = new Pen(Color.Black, 1);
            SolidBrush brush = new SolidBrush(Color.Red);
            float x = 0, y = 0;

            RecWidth = (float)pictureBox1.Width / (float)Map_width;
            RecHeight = (float)pictureBox1.Height / (float)Map_height;

            if (RecWidth > RecHeight)
            {
                RecWidth = RecHeight;
            }
            else
            {
                RecHeight = RecWidth;
            }
            double max_dislocation = GetMaxDislocation();
            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {

                    if (Map[i, j].GetRecrystalizationState() ==true)
                    {
                        brush.Color = Color.FromArgb((int)(Map[i,j].GetDislocationDensity()/max_dislocation *255), 0, 0);

                    }
                    else
                    {
                        brush.Color = Color.Green;
                    }

                    g1.FillRectangle(brush, x, y, RecWidth, RecHeight);
                    if (Mesh_box.Checked == true)
                    {
                        g1.DrawRectangle(pen, x, y, RecWidth, RecHeight);
                    }
                    x += RecWidth;
                }

                x = 0;
                y += RecHeight;
            }
            //g1.Dispose();
        }

        double GetMaxDislocation()
        {
            double max = 0;

            for (int i =0; i < Map_height; i++)
            {
                for(int j =0; j<Map_width; j++)
                {
                    if(Map.Map[i,j].GetDislocationDensity()>max)
                    {
                        max = Map.Map[i, j].GetDislocationDensity();
                    }
                }
            }
            return max;
        }

        void setColorTable()
        {
            NumberOfStates = int.Parse(State_box.Text);
            colorTable = new List<Color>();
            int R, G, B;
            Random rand = new Random();
            colorTable.Add(Color.White);
            for (int i = 1; i < NumberOfStates + 1; i++)
            {
                R = rand.Next(256);
                G = rand.Next(256);
                B = rand.Next(256);

                if (!colourExist(R, G, B, colorTable))
                {
                    colorTable.Add(Color.FromArgb(R, G, B));
                }
                else
                {
                    while (colourExist(R, G, B, colorTable))
                    {
                        R = rand.Next(256);
                        G = rand.Next(256);
                        B = rand.Next(256);
                    }
                }
            }
        }

        bool colourExist(int R, int G, int B, List<Color> colorTable)
        {
            bool exist = false;

            for (int i = 0; i < colorTable.Count; i++)
            {
                if (colorTable[i] == Color.FromArgb(R, G, B))
                {
                    exist = true;
                }
            }
            return exist;
        }

        void updateColorTable(int NumberOfStatesBeforeRecrystalization)
        {
            int R, G, B;
            Random rand = new Random();
            for (int i = NumberOfStatesBeforeRecrystalization; i < NumberOfStates + 1; i++)
            {
                R = rand.Next(256);
                G = rand.Next(256);
                B = rand.Next(256);

                if (!colourExist(R, G, B, colorTable))
                {
                    colorTable.Add(Color.FromArgb(R, G, B));
                }
                else
                {
                    while (colourExist(R, G, B, colorTable))
                    {
                        R = rand.Next(256);
                        G = rand.Next(256);
                        B = rand.Next(256);
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Homogenous_button_Click(object sender, EventArgs e)
        {
            int HomogenousCounter = 1;
            setColorTable();
            pictureBox1.Refresh();
            try
            {
                Map_width = int.Parse(textBox1.Text);
                Map_height = int.Parse(textBox2.Text);
                NumberOfStates = int.Parse(State_box.Text);
                Columns = int.Parse(Column_box.Text);
                Rows = int.Parse(Row_box.Text);
                BC = comboBox2.SelectedIndex;
                Neighbourhood = Neigbourhood_box.SelectedIndex;
                if (Neighbourhood == 6)
                    Radius = int.Parse(Radius_box.Text);
                if (Map_width < 0 || Map_height < 0 || NumberOfStates < 0 || Columns < 0 || Rows < 0)
                    throw new System.FormatException();
            }

            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Nieprawidłowe wartości - tylko liczby naturalne lub wprowadz liczbe rzędów i kolumn");
                return;
            }
            MC_button.Enabled = false;
            StateCounter = 0;
            Map = new Grid(Map_width, Map_height, NumberOfStates, BC, Neighbourhood, Radius);

            int deltaRows = Map_height / Rows;
            int deltaColumns = Map_width / Columns;
            if (deltaRows == 0)
            {
                deltaRows = 1;
                System.Windows.Forms.MessageBox.Show("za mała mapa");
            }
            if (deltaColumns == 0)
            {
                deltaRows = 1;
                System.Windows.Forms.MessageBox.Show("za mała mapa");
            }

            for (int i = 0; i < Rows; ++i)
            {
                for (int j = 0; j < Columns; ++j)
                {
                    if (HomogenousCounter < NumberOfStates + 1)
                        Map.Map[(i * deltaRows), (j * deltaColumns)].SetState(HomogenousCounter);
                    HomogenousCounter++;
                    StateCounter++;
                }
            }


            DrawCells(Map.Map);
        }

        private void Stop_button_Click(object sender, EventArgs e)
        {
            aTimer.Enabled = false;
            MC_button.Enabled = false;
        }

        private void Radius_button_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
            setColorTable();
            try
            {
                Map_width = int.Parse(textBox1.Text);
                Map_height = int.Parse(textBox2.Text);
                NumberOfStates = int.Parse(State_box.Text);
                BC = comboBox2.SelectedIndex;
                Neighbourhood = Neigbourhood_box.SelectedIndex;
                if (Neighbourhood == 6)
                    Radius = int.Parse(Radius_box.Text);
                if (Map_width < 0 || Map_height < 0 || NumberOfStates < 0)
                    throw new System.FormatException();
                if (NumberOfStates * Radius > 2 * Math.Sqrt(Map_height * Map_height + Map_width * Map_width))
                    throw new Exception();
            }
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Nieprawidłowe wartości - tylko liczby naturalne ");
                return;
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Za duzy promien w stosunku do ilosci ziaren, może powodować crash programu");
                return;
            }
            MC_button.Enabled = false;
            StateCounter = 0;
            Map = new Grid(Map_width, Map_height, NumberOfStates, BC, Neighbourhood, Radius);

            int[,] Coordinates = new int[NumberOfStates, 2];
            Random rand = new Random();
            int x, y;
            int RadiusCounter = 1;

            while (RadiusCounter < NumberOfStates + 1)
            {
                x = rand.Next(0, Map_height);
                y = rand.Next(0, Map_width);

                if (CheckRadius(x, y, Coordinates, RadiusCounter - 1, Radius))
                {
                    Map.Map[x, y].SetState(RadiusCounter);
                    Coordinates[RadiusCounter - 1, 0] = x;
                    Coordinates[RadiusCounter - 1, 1] = y;
                    RadiusCounter++;
                    StateCounter++;
                }

            }

            DrawCells(Map.Map);
        }


        private void Random_button_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            pictureBox1.Refresh();
            setColorTable();
            try
            {
                Map_width = int.Parse(textBox1.Text);
                Map_height = int.Parse(textBox2.Text);
                NumberOfStates = int.Parse(State_box.Text);
                BC = comboBox2.SelectedIndex;
                Neighbourhood = Neigbourhood_box.SelectedIndex;
                if (Neighbourhood == 6)
                    Radius = int.Parse(Radius_box.Text);
                if (Map_width < 0 || Map_height < 0 || NumberOfStates < 0)
                    throw new System.FormatException();
            }
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Nieprawidłowe wartości - tylko liczby naturalne");
                return;
            }
            MC_button.Enabled = false;
            StateCounter = 0;
            int RandomCounter = 1;

            Map = new Grid(Map_width, Map_height, NumberOfStates, BC, Neighbourhood, Radius);

            while (RandomCounter < NumberOfStates + 1)
            {
                int p1 = rand.Next(0, Map_height), p2 = rand.Next(0, Map_width);
                if (RandomCounter < NumberOfStates + 1)
                {
                    if (Map.Map[p1, p2].GetState() == 0)
                    {
                        Map.Map[p1, p2].SetState(RandomCounter);
                        RandomCounter++;
                        StateCounter++;
                    }

                }
            }



            DrawCells(Map.Map);
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Cells_button_Click(object sender, EventArgs e)
        {
            DrawCells(Map.Map);

        }

        private void Dislocation_button_Click(object sender, EventArgs e)
        {
            DrawDislocations(Map.Map);
        }


        private void Energy_button_Click(object sender, EventArgs e)
        {
            DrawEnergy(Map.energyMap);
        }

        private void Recrystalization_button_Click(object sender, EventArgs e)
        {
            double A, B, Percent;
            try
            {
                DeltaT = double.Parse(DeltaT_box.Text);
                A = double.Parse(A_box.Text);
                B = double.Parse(B_box.Text);
                Percent = double.Parse(Percent_box.Text);

                if (DeltaT < 0.0 && Percent <= 0.0)
                    throw new System.FormatException();
            }
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Nieprawidłowe wartości");
                return;
            }
            int NumberOfStatesBeforeRecrystalization;
            for (int i = 0; i < 1; i++)
            {
                if(i == 0)
                { 
                NumberOfStatesBeforeRecrystalization = NumberOfStates + 1;
                }
                else
                {
                    NumberOfStatesBeforeRecrystalization = NumberOfStates;
                }
                Map.Recrystalization(DeltaT, Percent, A, B);
                NumberOfStates = Map.GetNumberOfStatesAfterRecrystalization();
                updateColorTable(NumberOfStatesBeforeRecrystalization);

            }
            DrawCells(Map.Map);
        }
        private void MC_button_Click(object sender, EventArgs e)
        {
            double kt = 1;
            try
            {
                MC_iterations = int.Parse(MC_Iterations_box.Text);
                kt = double.Parse(kt_box.Text);
            }
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Nieprawidłowe wartości - tylko liczby naturalne");
                return;
            }
            for (int i = 0; i < MC_iterations; i++)
            {
                Map.MonteCarlo(kt);
            }
            DrawCells(Map.Map);
        }

        private void Simulation_button_Click(object sender, EventArgs e)
        {
            MC_button.Enabled = false;
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 400;
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

        }

        private void Empty_box_Click(object sender, EventArgs e)
        {
            setColorTable();
            pictureBox1.Refresh();
            try
            {
                Map_width = int.Parse(textBox1.Text);
                Map_height = int.Parse(textBox2.Text);
                NumberOfStates = int.Parse(State_box.Text);
                BC = comboBox2.SelectedIndex;
                Neighbourhood = Neigbourhood_box.SelectedIndex;
                if (Neighbourhood == 6)
                    Radius = int.Parse(Radius_box.Text);

                if (Map_width < 0 || Map_height < 0 || NumberOfStates < 0)
                    throw new System.FormatException();
            }
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Nieprawidłowe wartości - tylko liczby naturalne");
                return;
            }
            MC_button.Enabled = false;
            StateCounter = 0;
            Map = new Grid(Map_width, Map_height, NumberOfStates, BC, Neighbourhood, Radius);
            DrawCells(Map.Map);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {




            if (BC == 0)
            {
                switch (Neighbourhood)
                {
                    case 0:
                        Map.UpdateVector_periodical_Neumann();
                        break;
                    case 1:
                        Map.UpdateVector_periodical_Moore();
                        break;
                    case 2:
                        Map.UpdateVector_periodical_Pentagonal();
                        break;
                    case 3:
                        Map.UpdateVector_periodical_Hexagonal_left();
                        break;
                    case 4:
                        Map.UpdateVector_periodical_Hexagonal_right();
                        break;
                    case 5:
                        Map.UpdateVector_periodical_Hexagonal_random();
                        break;
                    case 6:
                        Map.UpdateVector_periodical_Radius();
                        break;




                }

            }
            else if (BC == 1)
            {
                switch (Neighbourhood)
                {
                    case 0:
                        Map.UpdateVector_absorbing_Neumann();
                        break;
                    case 1:
                        Map.UpdateVector_absorbing_Moore();
                        break;
                    case 2:
                        Map.UpdateVector_absorbing_Pentagonal();
                        break;
                    case 3:
                        Map.UpdateVector_absorbing_Hexagonal_left();
                        break;
                    case 4:
                        Map.UpdateVector_absorbing_Hexagonal_right();
                        break;
                    case 5:
                        Map.UpdateVector_absorbing_Hexagonal_random();
                        break;
                    case 6:
                        Map.UpdateVector_absorbing_Radius();
                        break;
                }
            }


            BeginInvoke(new Action(() =>
            {
                DrawCells(Map.Map);
            }));


            if (CheckIfZero())
            {
                aTimer.Enabled = false;
                BeginInvoke(new Action(() =>
                {
                    MC_button.Enabled = true;
                }));

            }




        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        public bool CheckIfZero()
        {
            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map.Map[i, j].GetState() == 0)
                        return false;
                }

            }
            return true;
        }

        bool CheckRadius(int x, int y, int[,] Coordinates, int RadiusCounter, int Radius)
        {
            for (int i = 0; i < RadiusCounter; i++)
            {
                if (Math.Sqrt((x - Coordinates[i, 0]) * (x - Coordinates[i, 0]) + (y - Coordinates[i, 1]) * (y - Coordinates[i, 1])) > Radius)
                    continue;
                else
                {
                    return false;
                }
            }
            return true;
        }

    }
}
