using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication5
{
    class Cell
    {
        double mass_x, mass_y;
        int state;
        double DislocationDensity = 0;
        bool IsRecrystalised = false;
        static Random rand = new Random();

            public Cell()
        {
            mass_x = rand.NextDouble();
            mass_y = rand.NextDouble();
            state = 0;
        }
        public Cell(int state)
        {
            this.state = state;
            mass_x = rand.NextDouble();
            mass_y = rand.NextDouble();
        }
        public Cell(Cell cell)
        {
            this.state = cell.state;
            this.mass_x = cell.mass_x;
            this.mass_y = cell.mass_y;
            this.DislocationDensity = cell.DislocationDensity;
            this.IsRecrystalised = cell.IsRecrystalised;
        }

        public void CloneCell(Cell cell)
        {
            this.state = cell.state;
            this.mass_x = cell.mass_x;
            this.mass_y = cell.mass_y;
            this.DislocationDensity = cell.DislocationDensity;
            this.IsRecrystalised = cell.IsRecrystalised;
        }

        public void SetState(int state)
        {
            this.state = state;
        }
        public void IncrementDislocationDensity(double DislocationDensity)
        {
            this.DislocationDensity += DislocationDensity;
        }
        public void SetDislocationDensity(double DislocationDensity)
        {
            this.DislocationDensity = DislocationDensity;
        }
        public bool GetRecrystalizationState()
        {
            return IsRecrystalised;
        }
        public double GetDislocationDensity()
        {
            return DislocationDensity;
        }
        public void SetRecrystalisationState(bool State)
        {
            this.IsRecrystalised = State;
        }
        public int GetState()
        {
            return state;
        }
        public double GetMass_x()
        {
            return mass_x;
        }
        public double GetMass_y()
        {
            return mass_y;
        }
        public void SetMass_x(double mass_x)
        {
            this.mass_x = mass_x;
        }
        public void SetMass_y(double mass_y)
        {
            this.mass_y = mass_y;
        }
    }
}
