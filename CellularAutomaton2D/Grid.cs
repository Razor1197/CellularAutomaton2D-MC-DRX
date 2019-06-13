using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication5
{
    class Grid
    {
        public Cell[,] Map;
        int Map_height, Map_width;
        int[] StatesArray;
        public int NumberOfStates;
        int ActualState;
        Cell[,] previousIteration;
        Cell zero = new Cell(0);
        public double[,] DensityMap;
        public int[,] energyMap;
        int Radius, BC, neighbourhoodType;
        double DeltaDislocationDensity = 0, DislocationDensity = 0, RecrystalizationTimeStep = 0, DislocationDensityCritical;
        WriteToFile writeToFile = new WriteToFile("C:\\Users\\Korne\\source\\repos\\Cellular Automton 2D\\WindowsFormsApplication5\\bin\\Debug\\dislocationDensitySteps.txt");

        static Random rand;

        public Grid(int Map_width, int Map_height, int NumberOfStates, int BC, int neighbourhoodType, int Radius)
        {
            // because state 0 is also a state so if we want 5 states then array size is 6 (NumberOfStates+1)
            this.NumberOfStates = NumberOfStates + 1;
            this.Map_height = Map_height;
            this.Map_width = Map_width;
            this.Radius = Radius;
            this.BC = BC;
            this.neighbourhoodType = neighbourhoodType;
            this.DislocationDensityCritical = 0.8;
            this.DensityMap = new double[Map_height,Map_width];
            previousIteration = new Cell[Map_height, Map_width];
            energyMap = new int[Map_height, Map_width];
            rand = new Random();
            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    energyMap[i, j] = 0;
                }
            }
            StatesArray = new int[this.NumberOfStates];
            for (int i = 0; i < NumberOfStates; i++)
            {
                StatesArray[i] = 0;
            }
            Map = new Cell[Map_height, Map_width];

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    Map[i, j] = new Cell();
                }
            }
            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    previousIteration[i, j] = new Cell();
                }
            }

        }

        void updatePreviousIteration()
        {
            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    previousIteration[i, j].CloneCell(Map[i, j]);
                }
            }
        }
        /*
        public void UpdateVector(int BC, int Neighbourhood)
        {
            int[] neighbourhood = new int[4];
            updatePreviousIteration();

            if (BC == 0)
            {
                for (int i = 0; i < Map_height; i++)
                {
                    for (int j = 0; j < Map_width; j++)
                    {
                        if (Map[i, j].GetState() == 0)
                        {
                            //von neuman neighbourhood
                            //corners
                            if (j == 0 && i == 0)
                            {

                                neighbourhood[0] = previousIteration[Map_height - 1, 0];
                                neighbourhood[1] = previousIteration[0, Map_width - 1];
                                neighbourhood[2] = previousIteration[0, 1];
                                neighbourhood[3] = previousIteration[1, 0];
                            }
                            else if (j == Map_width - 1 && i == 0)
                            {

                                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                                neighbourhood[1] = previousIteration[0, Map_width - 2];
                                neighbourhood[2] = previousIteration[0, 0];
                                neighbourhood[3] = previousIteration[1, Map_width - 1];
                            }
                            else if (j == 0 && i == Map_height - 1)
                            {
                                neighbourhood[0] = previousIteration[Map_height - 2, 0];
                                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];
                                neighbourhood[2] = previousIteration[Map_height - 1, 1];
                                neighbourhood[3] = previousIteration[0, 0];
                            }

                            else if (j == Map_width - 1 && i == Map_height - 1)
                            {
                                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 2];
                                neighbourhood[2] = previousIteration[Map_height - 1, 0];
                                neighbourhood[3] = previousIteration[0, Map_width - 1];
                            }
                            //edges 
                            else if (j == 0 && i != 0 && i != (Map_height - 1))
                            {
                                neighbourhood[0] = previousIteration[i - 1, j];
                                neighbourhood[1] = previousIteration[i, Map_width - 1];
                                neighbourhood[2] = previousIteration[i, j + 1];
                                neighbourhood[3] = previousIteration[i + 1, j];
                            }

                            else if (j != 0 && i == 0 && j != (Map_width - 1))
                            {
                                neighbourhood[0] = previousIteration[Map_height - 1, j];
                                neighbourhood[1] = previousIteration[i, j - 1];
                                neighbourhood[2] = previousIteration[i, j + 1];
                                neighbourhood[3] = previousIteration[i + 1, j];
                            }

                            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                            {
                                neighbourhood[0] = previousIteration[i - 1, j];
                                neighbourhood[1] = previousIteration[i, j - 1];
                                neighbourhood[2] = previousIteration[i, 0];
                                neighbourhood[3] = previousIteration[i + 1, j];
                            }

                            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                            {
                                neighbourhood[0] = previousIteration[i - 1, j];
                                neighbourhood[1] = previousIteration[i, j - 1];
                                neighbourhood[2] = previousIteration[i, j + 1];
                                neighbourhood[3] = previousIteration[0, j];
                            }

                            //rest
                            else
                            {
                                neighbourhood[0] = previousIteration[i - 1, j];
                                neighbourhood[1] = previousIteration[i, j - 1];
                                neighbourhood[2] = previousIteration[i, j + 1];
                                neighbourhood[3] = previousIteration[i + 1, j];
                            }



                            //////////////////////////////////////////////////////
                            for (int k = 0; k < 4; k++)
                            {
                                for (int l = 1; l < NumberOfStates; l++)
                                {
                                    if (neighbourhood[k] == l)
                                        StatesArray[l]++;
                                }
                            }


                            ActualState = StatesArray[0];
                            for (int k = 1; k < NumberOfStates; k++)
                            {
                                if (StatesArray[k] > ActualState)
                                {
                                    ActualState = k;
                                }
                            }
                            Map[i, j].SetState(ActualState);


                            for (int l = 0; l < NumberOfStates; l++)
                            {
                                StatesArray[l] = 0;
                            }
                        }

                    }


                }
            }

            else if (BC == 1)
            {
                for (int i = 0; i < Map_height; i++)
                {
                    for (int j = 0; j < Map_width; j++)
                    {
                        if (Map[i, j].GetState() == 0)
                        {
                            //corners
                            if (j == 0 && i == 0)
                            {

                                neighbourhood[0] = 0;
                                neighbourhood[1] = 0;
                                neighbourhood[2] = previousIteration[0, 1];
                                neighbourhood[3] = previousIteration[1, 0];
                            }
                            else if (j == Map_width - 1 && i == 0)
                            {

                                neighbourhood[0] = 0;
                                neighbourhood[1] = previousIteration[0, Map_width - 2];
                                neighbourhood[2] = 0;
                                neighbourhood[3] = previousIteration[1, Map_width - 1];
                            }
                            else if (j == 0 && i == Map_height - 1)
                            {
                                neighbourhood[0] = previousIteration[Map_height - 2, 0];
                                neighbourhood[1] = 0;
                                neighbourhood[2] = previousIteration[Map_height - 1, 1];
                                neighbourhood[3] = 0;
                            }

                            else if (j == Map_width - 1 && i == Map_height - 1)
                            {
                                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 2];
                                neighbourhood[2] = 0;
                                neighbourhood[3] = 0;
                            }
                            //edges 
                            else if (j == 0 && i != 0 && i != (Map_height - 1))
                            {
                                neighbourhood[0] = previousIteration[i - 1, j];
                                neighbourhood[1] = 0;
                                neighbourhood[2] = previousIteration[i, j + 1];
                                neighbourhood[3] = previousIteration[i + 1, j];
                            }

                            else if (j != 0 && i == 0 && j != (Map_width - 1))
                            {
                                neighbourhood[0] = 0;
                                neighbourhood[1] = previousIteration[i, j - 1];
                                neighbourhood[2] = previousIteration[i, j + 1];
                                neighbourhood[3] = previousIteration[i + 1, j];
                            }

                            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                            {
                                neighbourhood[0] = previousIteration[i - 1, j];
                                neighbourhood[1] = previousIteration[i, j - 1];
                                neighbourhood[2] = 0;
                                neighbourhood[3] = previousIteration[i + 1, j];
                            }

                            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                            {
                                neighbourhood[0] = previousIteration[i - 1, j];
                                neighbourhood[1] = previousIteration[i, j - 1];
                                neighbourhood[2] = previousIteration[i, j + 1];
                                neighbourhood[3] = 0;
                            }

                            //rest
                            else
                            {
                                neighbourhood[0] = previousIteration[i - 1, j];
                                neighbourhood[1] = previousIteration[i, j - 1];
                                neighbourhood[2] = previousIteration[i, j + 1];
                                neighbourhood[3] = previousIteration[i + 1, j];
                            }
                            //////////////////////////////////////////////////////

                            for (int k = 0; k < 4; k++)
                            {
                                for (int l = 1; l < NumberOfStates; l++)
                                {
                                    if (neighbourhood[k] == l)
                                        StatesArray[l]++;
                                }
                            }


                            ActualState = StatesArray[0];
                            for (int k = 1; k < NumberOfStates; k++)
                            {
                                if (StatesArray[k] > ActualState)
                                {
                                    ActualState = k;
                                }
                            }
                            Map[i, j].SetState(ActualState);


                            for (int l = 0; l < NumberOfStates; l++)
                            {
                                StatesArray[l] = 0;
                            }

                        }
                    }
                }


            }
        }
        */
        public Cell[] Periodical_Neumann(int i, int j)
        {
            Cell[] neighbourhood = new Cell[4];

            if (j == 0 && i == 0)
            {

                neighbourhood[0] = previousIteration[Map_height - 1, 0];
                neighbourhood[1] = previousIteration[0, Map_width - 1];
                neighbourhood[2] = previousIteration[0, 1];
                neighbourhood[3] = previousIteration[1, 0];
            }
            else if (j == Map_width - 1 && i == 0)
            {

                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[0, Map_width - 2];
                neighbourhood[2] = previousIteration[0, 0];
                neighbourhood[3] = previousIteration[1, Map_width - 1];
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, 0];
                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[2] = previousIteration[Map_height - 1, 1];
                neighbourhood[3] = previousIteration[0, 0];
            }

            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[2] = previousIteration[Map_height - 1, 0];
                neighbourhood[3] = previousIteration[0, Map_width - 1];
            }
            //edges 
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, Map_width - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[Map_height - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, 0];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[0, j];
            }

            //rest
            else
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            return neighbourhood;
        }
        public void UpdateVector_periodical_Neumann()
        {
            Cell[] neighbourhood;

            updatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].GetState() == 0)
                    {
                        neighbourhood = Periodical_Neumann(i, j);

                        //////////////////////////////////////////////////////
                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 1; l < NumberOfStates; l++)
                            {
                                if (neighbourhood[k].GetState() == l)
                                    StatesArray[l]++;
                            }
                        }


                        ActualState = StatesArray[0];
                        for (int k = 1; k < NumberOfStates; k++)
                        {
                            if (StatesArray[k] > ActualState)
                            {
                                ActualState = k;
                            }
                        }
                        Map[i, j].SetState(ActualState);


                        for (int l = 0; l < NumberOfStates; l++)
                        {
                            StatesArray[l] = 0;
                        }
                    }

                }


            }
        }

        public Cell[] Absorbing_Neumann(int i, int j)
        {
            Cell[] neighbourhood = new Cell[4];

            //corners
            if (j == 0 && i == 0)
            {

                neighbourhood[0] = zero;
                neighbourhood[1] = zero;
                neighbourhood[2] = previousIteration[0, 1];
                neighbourhood[3] = previousIteration[1, 0];
            }
            else if (j == Map_width - 1 && i == 0)
            {

                neighbourhood[0] = zero;
                neighbourhood[1] = previousIteration[0, Map_width - 2];
                neighbourhood[2] = zero;
                neighbourhood[3] = previousIteration[1, Map_width - 1];
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, 0];
                neighbourhood[1] = zero;
                neighbourhood[2] = previousIteration[Map_height - 1, 1];
                neighbourhood[3] = zero;
            }

            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[2] = zero;
                neighbourhood[3] = zero;
            }
            //edges 
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = zero;
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = zero;
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = zero;
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = zero;
            }

            //rest
            else
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }
            return neighbourhood;
        }
        public void UpdateVector_absorbing_Neumann()
        {
            Cell[] neighbourhood;

            updatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].GetState() == 0)
                    {
                        neighbourhood = Absorbing_Neumann(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 1; l < NumberOfStates; l++)
                            {
                                if (neighbourhood[k].GetState() == l)
                                    StatesArray[l]++;
                            }
                        }


                        ActualState = StatesArray[0];
                        for (int k = 1; k < NumberOfStates; k++)
                        {
                            if (StatesArray[k] > ActualState)
                            {
                                ActualState = k;
                            }
                        }
                        Map[i, j].SetState(ActualState);


                        for (int l = 0; l < NumberOfStates; l++)
                        {
                            StatesArray[l] = 0;
                        }
                    }

                }


            }



        }

        public Cell[] Periodical_Moore(int i, int j)
        {
            Cell[] neighbourhood = new Cell[8];

            if (j == 0 && i == 0)
            {
                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, 0];
                neighbourhood[2] = previousIteration[Map_height - 1, 1];

                neighbourhood[3] = previousIteration[0, Map_width - 1];
                neighbourhood[4] = previousIteration[0, 1];

                neighbourhood[5] = previousIteration[1, Map_width - 1];
                neighbourhood[6] = previousIteration[1, 0];
                neighbourhood[7] = previousIteration[1, 1];
            }
            else if (j == Map_width - 1 && i == 0)
            {
                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[2] = previousIteration[Map_height - 1, 0];

                neighbourhood[3] = previousIteration[0, Map_width - 2];
                neighbourhood[4] = previousIteration[0, 0];

                neighbourhood[5] = previousIteration[1, Map_width - 2];
                neighbourhood[6] = previousIteration[1, Map_width - 1];
                neighbourhood[7] = previousIteration[1, 0];
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 2, 0];
                neighbourhood[2] = previousIteration[Map_height - 2, 1];

                neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[4] = previousIteration[Map_height - 1, 1];

                neighbourhood[5] = previousIteration[0, Map_width - 1];
                neighbourhood[6] = previousIteration[0, 0];
                neighbourhood[7] = previousIteration[0, 1];
            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[2] = previousIteration[Map_height - 2, 0];

                neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[4] = previousIteration[Map_height - 1, 0];

                neighbourhood[5] = previousIteration[0, Map_width - 2];
                neighbourhood[6] = previousIteration[0, Map_width - 1];
                neighbourhood[7] = previousIteration[0, 0];
            }
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = previousIteration[i, Map_width - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[i + 1, Map_width - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }
            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[Map_height - 1, j - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, j];
                neighbourhood[2] = previousIteration[Map_height - 1, j + 1];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }
            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, 0];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, 0];

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, 0];
            }
            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[0, j - 1];
                neighbourhood[6] = previousIteration[0, j];
                neighbourhood[7] = previousIteration[0, j + 1];
            }

            else
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }


            return neighbourhood;
        }
        public void UpdateVector_periodical_Moore()
        {
            Cell[] neighbourhood;
            updatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].GetState() == 0)
                    {

                        neighbourhood = Periodical_Moore(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 8; k++)
                        {
                            for (int l = 1; l < NumberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].GetState() == l)
                                {
                                    StatesArray[l]++;
                                }
                            }
                        }

                        ActualState = StatesArray[0];
                        for (int k = 1; k < NumberOfStates; k++)
                        {
                            if (StatesArray[k] > ActualState)
                            {
                                ActualState = k;
                            }
                        }


                        Map[i, j].SetState(ActualState);

                        for (int k = 0; k < NumberOfStates; k++)
                        {
                            StatesArray[k] = 0;
                        }
                    }
                }
            }

        }

        public Cell[] Absorbing_Moore(int i, int j)
        {
            Cell[] neighbourhood = new Cell[8];

            if (j == 0 && i == 0)
            {
                neighbourhood[0] = zero;
                neighbourhood[1] = zero;
                neighbourhood[2] = zero;
                neighbourhood[3] = zero;
                neighbourhood[4] = previousIteration[0, 1];

                neighbourhood[5] = zero;
                neighbourhood[6] = previousIteration[1, 0];
                neighbourhood[7] = previousIteration[1, 1];
            }
            else if (j == Map_width - 1 && i == 0)
            {
                neighbourhood[0] = zero;
                neighbourhood[1] = zero;
                neighbourhood[2] = zero;

                neighbourhood[3] = previousIteration[0, Map_width - 2];
                neighbourhood[4] = zero;

                neighbourhood[5] = previousIteration[1, Map_width - 2];
                neighbourhood[6] = previousIteration[1, Map_width - 1];
                neighbourhood[7] = zero;
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = zero;
                neighbourhood[1] = previousIteration[Map_height - 2, 0];
                neighbourhood[2] = previousIteration[Map_height - 2, 1];

                neighbourhood[3] = zero;
                neighbourhood[4] = previousIteration[Map_height - 1, 1];

                neighbourhood[5] = zero;
                neighbourhood[6] = zero;
                neighbourhood[7] = zero;
            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[2] = zero;

                neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[4] = zero;

                neighbourhood[5] = zero;
                neighbourhood[6] = zero;
                neighbourhood[7] = zero;
            }

            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = zero;
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = zero;
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = zero;
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }

            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = zero;
                neighbourhood[1] = zero;
                neighbourhood[2] = zero;

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }

            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = zero;

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = zero;

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = zero;
            }

            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = zero;
                neighbourhood[6] = zero;
                neighbourhood[7] = zero;
            }

            else
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }

            return neighbourhood;
        }
        public void UpdateVector_absorbing_Moore()
        {

            Cell[] neighbourhood = new Cell[8];
            updatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].GetState() == 0)
                    {
                        neighbourhood = Absorbing_Moore(i, j);


                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 8; k++)
                        {
                            for (int l = 1; l < NumberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].GetState() == l)
                                {
                                    StatesArray[l]++;
                                }
                            }
                        }

                        ActualState = StatesArray[0];
                        for (int k = 1; k < NumberOfStates; k++)
                        {
                            if (StatesArray[k] > ActualState)
                            {
                                ActualState = k;
                            }
                        }


                        Map[i, j].SetState(ActualState);

                        for (int k = 0; k < NumberOfStates; k++)
                        {
                            StatesArray[k] = 0;
                        }
                    }
                }
            }
        }

        public Cell[] Periodical_Hexagonal_left(int i, int j)
        {
            Cell[] neighbourhood = new Cell[6];


            if (j == 0 && i == 0)
            {

                neighbourhood[0] = previousIteration[Map_height - 1, 0];
                neighbourhood[1] = previousIteration[Map_height - 1, 1];

                neighbourhood[2] = previousIteration[0, Map_width - 1];
                neighbourhood[3] = previousIteration[0, 1];

                neighbourhood[4] = previousIteration[1, Map_width - 1];
                neighbourhood[5] = previousIteration[1, 0];

            }
            else if (j == Map_width - 1 && i == 0)
            {

                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, 0];

                neighbourhood[2] = previousIteration[0, Map_width - 2];
                neighbourhood[3] = previousIteration[0, 0];

                neighbourhood[4] = previousIteration[1, Map_width - 2];
                neighbourhood[5] = previousIteration[1, Map_width - 1];

            }
            else if (j == 0 && i == Map_height - 1)
            {

                neighbourhood[0] = previousIteration[Map_height - 2, 0];
                neighbourhood[1] = previousIteration[Map_height - 2, 1];

                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[3] = previousIteration[Map_height - 1, 1];

                neighbourhood[4] = previousIteration[0, Map_width - 1];
                neighbourhood[5] = previousIteration[0, 0];

            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {

                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 2, 0];

                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[3] = previousIteration[Map_height - 1, 0];

                neighbourhood[4] = previousIteration[0, Map_width - 2];
                neighbourhood[5] = previousIteration[0, Map_width - 1];

            }
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = previousIteration[i, Map_width - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, Map_width - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {

                neighbourhood[0] = previousIteration[Map_height - 1, j];
                neighbourhood[1] = previousIteration[Map_height - 1, j + 1];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, 0];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, 0];

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[0, j - 1];
                neighbourhood[5] = previousIteration[0, j];

            }

            else
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }


            return neighbourhood;
        }
        public void UpdateVector_periodical_Hexagonal_left()
        {
            Cell[] neighbourhood;
            updatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].GetState() == 0)
                    {
                        neighbourhood = Periodical_Hexagonal_left(i, j);

                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < NumberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].GetState() == l)
                                {
                                    StatesArray[l]++;
                                }
                            }
                        }

                        ActualState = StatesArray[0];
                        for (int k = 1; k < NumberOfStates; k++)
                        {
                            if (StatesArray[k] > ActualState)
                            {
                                ActualState = k;
                            }
                        }


                        Map[i, j].SetState(ActualState);

                        for (int k = 0; k < NumberOfStates; k++)
                        {
                            StatesArray[k] = 0;
                        }
                    }
                }
            }

        }

        public Cell[] Absorbing_Hexagonal_left(int i, int j)
        {
            Cell[] neighbourhood = new Cell[6];

            if (j == 0 && i == 0)
            {

                neighbourhood[0] = zero;
                neighbourhood[1] = zero;

                neighbourhood[2] = zero;
                neighbourhood[3] = previousIteration[0, 1];

                neighbourhood[4] = zero;
                neighbourhood[5] = previousIteration[1, 0];

            }
            else if (j == Map_width - 1 && i == 0)
            {

                neighbourhood[0] = zero;
                neighbourhood[1] = zero;

                neighbourhood[2] = previousIteration[0, Map_width - 2];
                neighbourhood[3] = zero;

                neighbourhood[4] = previousIteration[1, Map_width - 2];
                neighbourhood[5] = previousIteration[1, Map_width - 1];

            }
            else if (j == 0 && i == Map_height - 1)
            {

                neighbourhood[0] = previousIteration[Map_height - 2, 0];
                neighbourhood[1] = previousIteration[Map_height - 2, 1];

                neighbourhood[2] = zero;
                neighbourhood[3] = previousIteration[Map_height - 1, 1];

                neighbourhood[4] = zero;
                neighbourhood[5] = zero;

            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {

                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = zero;

                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[3] = zero;

                neighbourhood[4] = zero;
                neighbourhood[5] = zero;

            }
            //edges
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = zero;
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = zero;
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {

                neighbourhood[0] = zero;
                neighbourhood[1] = zero;

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = zero;

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = zero;

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = zero;
                neighbourhood[5] = zero;

            }

            else
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }


            return neighbourhood;
        }
        public void UpdateVector_absorbing_Hexagonal_left()
        {
            Cell[] neighbourhood = new Cell[6];
            updatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].GetState() == 0)
                    {
                        neighbourhood = Absorbing_Hexagonal_left(i, j);

                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < NumberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].GetState() == l)
                                {
                                    StatesArray[l]++;
                                }
                            }
                        }

                        ActualState = StatesArray[0];
                        for (int k = 1; k < NumberOfStates; k++)
                        {
                            if (StatesArray[k] > ActualState)
                            {
                                ActualState = k;
                            }
                        }


                        Map[i, j].SetState(ActualState);

                        for (int k = 0; k < NumberOfStates; k++)
                        {
                            StatesArray[k] = 0;
                        }
                    }
                }
            }

        }

        public Cell[] Periodical_Hexagonal_right(int i, int j)
        {
            Cell[] neighbourhood = new Cell[6];

            if (j == 0 && i == 0)
            {
                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, 0];


                neighbourhood[2] = previousIteration[0, Map_width - 1];
                neighbourhood[3] = previousIteration[0, 1];


                neighbourhood[4] = previousIteration[1, 0];
                neighbourhood[5] = previousIteration[1, 1];
            }
            else if (j == Map_width - 1 && i == 0)
            {
                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];


                neighbourhood[2] = previousIteration[0, Map_width - 2];
                neighbourhood[3] = previousIteration[0, 0];


                neighbourhood[4] = previousIteration[1, Map_width - 1];
                neighbourhood[5] = previousIteration[1, 0];
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 2, 0];


                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[3] = previousIteration[Map_height - 1, 1];


                neighbourhood[4] = previousIteration[0, 0];
                neighbourhood[5] = previousIteration[0, 1];
            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];


                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[3] = previousIteration[Map_height - 1, 0];


                neighbourhood[4] = previousIteration[0, Map_width - 1];
                neighbourhood[5] = previousIteration[0, 0];
            }
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[i - 1, j];


                neighbourhood[2] = previousIteration[i, Map_width - 1];
                neighbourhood[3] = previousIteration[i, j + 1];


                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }
            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[Map_height - 1, j - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, j];


                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];


                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }
            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];


                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, 0];


                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, 0];
            }
            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];


                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];


                neighbourhood[4] = previousIteration[0, j];
                neighbourhood[5] = previousIteration[0, j + 1];
            }

            else
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];


                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];


                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }

            return neighbourhood;
        }
        public void UpdateVector_periodical_Hexagonal_right()
        {
            Cell[] neighbourhood = new Cell[6];
            updatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].GetState() == 0)
                    {
                        neighbourhood = Periodical_Hexagonal_right(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < NumberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].GetState() == l)
                                {
                                    StatesArray[l]++;
                                }
                            }
                        }

                        ActualState = StatesArray[0];
                        for (int k = 1; k < NumberOfStates; k++)
                        {
                            if (StatesArray[k] > ActualState)
                            {
                                ActualState = k;
                            }
                        }


                        Map[i, j].SetState(ActualState);

                        for (int k = 0; k < NumberOfStates; k++)
                        {
                            StatesArray[k] = 0;
                        }
                    }
                }
            }
        }

        public Cell[] Absorbing_Hexagonal_right(int i, int j)
        {
            Cell[] neighbourhood = new Cell[6];

            if (j == 0 && i == 0)
            {
                neighbourhood[0] = zero;
                neighbourhood[1] = zero;

                neighbourhood[2] = zero;
                neighbourhood[3] = previousIteration[0, 1];

                neighbourhood[4] = previousIteration[1, 0];
                neighbourhood[5] = previousIteration[1, 1];
            }
            else if (j == Map_width - 1 && i == 0)
            {
                neighbourhood[0] = zero;
                neighbourhood[1] = zero;

                neighbourhood[2] = previousIteration[0, Map_width - 2];
                neighbourhood[3] = zero;

                neighbourhood[4] = previousIteration[1, Map_width - 1];
                neighbourhood[5] = zero;
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = zero;
                neighbourhood[1] = previousIteration[Map_height - 2, 0];

                neighbourhood[2] = zero;
                neighbourhood[3] = previousIteration[Map_height - 1, 1];

                neighbourhood[4] = zero;
                neighbourhood[5] = zero;
            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];

                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[3] = zero;

                neighbourhood[4] = zero;
                neighbourhood[5] = zero;
            }

            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = zero;
                neighbourhood[1] = previousIteration[i - 1, j];

                neighbourhood[2] = zero;
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }

            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = zero;
                neighbourhood[1] = zero;

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }

            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = zero;

                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = zero;
            }

            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = zero;
                neighbourhood[5] = zero;
            }

            else
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }


            return neighbourhood;
        }
        public void UpdateVector_absorbing_Hexagonal_right()
        {

            Cell[] neighbourhood = new Cell[6];
            updatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].GetState() == 0)
                    {
                        neighbourhood = Absorbing_Hexagonal_right(i, j);

                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < NumberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].GetState() == l)
                                {
                                    StatesArray[l]++;
                                }
                            }
                        }

                        ActualState = StatesArray[0];
                        for (int k = 1; k < NumberOfStates; k++)
                        {
                            if (StatesArray[k] > ActualState)
                            {
                                ActualState = k;
                            }
                        }


                        Map[i, j].SetState(ActualState);

                        for (int k = 0; k < NumberOfStates; k++)
                        {
                            StatesArray[k] = 0;
                        }
                    }
                }
            }
        }

        public Cell[] Periodical_Hexagonal_random(int i, int j)
        {
            Cell[] neighbourhood = new Cell[6];

            switch (rand.Next(2))
            {

                case 0:
                    if (j == 0 && i == 0)
                    {
                        neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                        neighbourhood[1] = previousIteration[Map_height - 1, 0];


                        neighbourhood[2] = previousIteration[0, Map_width - 1];
                        neighbourhood[3] = previousIteration[0, 1];


                        neighbourhood[4] = previousIteration[1, 0];
                        neighbourhood[5] = previousIteration[1, 1];
                    }
                    else if (j == Map_width - 1 && i == 0)
                    {
                        neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                        neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];


                        neighbourhood[2] = previousIteration[0, Map_width - 2];
                        neighbourhood[3] = previousIteration[0, 0];


                        neighbourhood[4] = previousIteration[1, Map_width - 1];
                        neighbourhood[5] = previousIteration[1, 0];
                    }
                    else if (j == 0 && i == Map_height - 1)
                    {
                        neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                        neighbourhood[1] = previousIteration[Map_height - 2, 0];


                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 1];
                        neighbourhood[3] = previousIteration[Map_height - 1, 1];


                        neighbourhood[4] = previousIteration[0, 0];
                        neighbourhood[5] = previousIteration[0, 1];
                    }
                    else if (j == Map_width - 1 && i == Map_height - 1)
                    {
                        neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                        neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];


                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                        neighbourhood[3] = previousIteration[Map_height - 1, 0];


                        neighbourhood[4] = previousIteration[0, Map_width - 1];
                        neighbourhood[5] = previousIteration[0, 0];
                    }
                    else if (j == 0 && i != 0 && i != (Map_height - 1))
                    {
                        neighbourhood[0] = previousIteration[i - 1, Map_width - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];


                        neighbourhood[2] = previousIteration[i, Map_width - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];


                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }
                    else if (j != 0 && i == 0 && j != (Map_width - 1))
                    {
                        neighbourhood[0] = previousIteration[Map_height - 1, j - 1];
                        neighbourhood[1] = previousIteration[Map_height - 1, j];


                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];


                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }
                    else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];


                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, 0];


                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, 0];
                    }
                    else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];


                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];


                        neighbourhood[4] = previousIteration[0, j];
                        neighbourhood[5] = previousIteration[0, j + 1];
                    }

                    else
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];


                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];


                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }
                    break;
                case 1:
                    if (j == 0 && i == 0)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 1, 0];
                        neighbourhood[1] = previousIteration[Map_height - 1, 1];

                        neighbourhood[2] = previousIteration[0, Map_width - 1];
                        neighbourhood[3] = previousIteration[0, 1];

                        neighbourhood[4] = previousIteration[1, Map_width - 1];
                        neighbourhood[5] = previousIteration[1, 0];

                    }
                    else if (j == Map_width - 1 && i == 0)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                        neighbourhood[1] = previousIteration[Map_height - 1, 0];

                        neighbourhood[2] = previousIteration[0, Map_width - 2];
                        neighbourhood[3] = previousIteration[0, 0];

                        neighbourhood[4] = previousIteration[1, Map_width - 2];
                        neighbourhood[5] = previousIteration[1, Map_width - 1];

                    }
                    else if (j == 0 && i == Map_height - 1)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 2, 0];
                        neighbourhood[1] = previousIteration[Map_height - 2, 1];

                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 1];
                        neighbourhood[3] = previousIteration[Map_height - 1, 1];

                        neighbourhood[4] = previousIteration[0, Map_width - 1];
                        neighbourhood[5] = previousIteration[0, 0];

                    }
                    else if (j == Map_width - 1 && i == Map_height - 1)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                        neighbourhood[1] = previousIteration[Map_height - 2, 0];

                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                        neighbourhood[3] = previousIteration[Map_height - 1, 0];

                        neighbourhood[4] = previousIteration[0, Map_width - 2];
                        neighbourhood[5] = previousIteration[0, Map_width - 1];

                    }
                    else if (j == 0 && i != 0 && i != (Map_height - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, Map_width - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, Map_width - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j != 0 && i == 0 && j != (Map_width - 1))
                    {

                        neighbourhood[0] = previousIteration[Map_height - 1, j];
                        neighbourhood[1] = previousIteration[Map_height - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, 0];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, 0];

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[0, j - 1];
                        neighbourhood[5] = previousIteration[0, j];

                    }

                    else
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    break;
            }

            return neighbourhood;
        }
        public void UpdateVector_periodical_Hexagonal_random()
        {
            Cell[] neighbourhood = new Cell[6];
            updatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].GetState() == 0)
                    {
                        neighbourhood = Periodical_Hexagonal_random(i, j);

                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < NumberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].GetState() == l)
                                {
                                    StatesArray[l]++;
                                }
                            }
                        }

                        ActualState = StatesArray[0];
                        for (int k = 1; k < NumberOfStates; k++)
                        {
                            if (StatesArray[k] > ActualState)
                            {
                                ActualState = k;
                            }
                        }


                        Map[i, j].SetState(ActualState);

                        for (int k = 0; k < NumberOfStates; k++)
                        {
                            StatesArray[k] = 0;
                        }
                    }
                }
            }
        }

        public Cell[] Absorbing_Hexagonal_random(int i, int j)
        {
            Cell[] neighbourhood = new Cell[6];
            Random rand = new Random();
            switch (rand.Next(2))
            {

                case 0:
                    if (j == 0 && i == 0)
                    {

                        neighbourhood[0] = zero;
                        neighbourhood[1] = zero;

                        neighbourhood[2] = zero;
                        neighbourhood[3] = previousIteration[0, 1];

                        neighbourhood[4] = zero;
                        neighbourhood[5] = previousIteration[1, 0];

                    }
                    else if (j == Map_width - 1 && i == 0)
                    {

                        neighbourhood[0] = zero;
                        neighbourhood[1] = zero;

                        neighbourhood[2] = previousIteration[0, Map_width - 2];
                        neighbourhood[3] = zero;

                        neighbourhood[4] = previousIteration[1, Map_width - 2];
                        neighbourhood[5] = previousIteration[1, Map_width - 1];

                    }
                    else if (j == 0 && i == Map_height - 1)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 2, 0];
                        neighbourhood[1] = previousIteration[Map_height - 2, 1];

                        neighbourhood[2] = zero;
                        neighbourhood[3] = previousIteration[Map_height - 1, 1];

                        neighbourhood[4] = zero;
                        neighbourhood[5] = zero;

                    }
                    else if (j == Map_width - 1 && i == Map_height - 1)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                        neighbourhood[1] = zero;

                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                        neighbourhood[3] = zero;

                        neighbourhood[4] = zero;
                        neighbourhood[5] = zero;

                    }
                    //edges
                    else if (j == 0 && i != 0 && i != (Map_height - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = zero;
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = zero;
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j != 0 && i == 0 && j != (Map_width - 1))
                    {

                        neighbourhood[0] = zero;
                        neighbourhood[1] = zero;

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = zero;

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = zero;

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = zero;
                        neighbourhood[5] = zero;

                    }

                    else
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    break;
                case 1:
                    if (j == 0 && i == 0)
                    {
                        neighbourhood[0] = zero;
                        neighbourhood[1] = zero;

                        neighbourhood[2] = zero;
                        neighbourhood[3] = previousIteration[0, 1];

                        neighbourhood[4] = previousIteration[1, 0];
                        neighbourhood[5] = previousIteration[1, 1];
                    }
                    else if (j == Map_width - 1 && i == 0)
                    {
                        neighbourhood[0] = zero;
                        neighbourhood[1] = zero;

                        neighbourhood[2] = previousIteration[0, Map_width - 2];
                        neighbourhood[3] = zero;

                        neighbourhood[4] = previousIteration[1, Map_width - 1];
                        neighbourhood[5] = zero;
                    }
                    else if (j == 0 && i == Map_height - 1)
                    {
                        neighbourhood[0] = zero;
                        neighbourhood[1] = previousIteration[Map_height - 2, 0];

                        neighbourhood[2] = zero;
                        neighbourhood[3] = previousIteration[Map_height - 1, 1];

                        neighbourhood[4] = zero;
                        neighbourhood[5] = zero;
                    }
                    else if (j == Map_width - 1 && i == Map_height - 1)
                    {
                        neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                        neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];

                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                        neighbourhood[3] = zero;

                        neighbourhood[4] = zero;
                        neighbourhood[5] = zero;
                    }

                    else if (j == 0 && i != 0 && i != (Map_height - 1))
                    {
                        neighbourhood[0] = zero;
                        neighbourhood[1] = previousIteration[i - 1, j];

                        neighbourhood[2] = zero;
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }

                    else if (j != 0 && i == 0 && j != (Map_width - 1))
                    {
                        neighbourhood[0] = zero;
                        neighbourhood[1] = zero;

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }

                    else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = zero;

                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = zero;
                    }

                    else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = zero;
                        neighbourhood[5] = zero;
                    }

                    else
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }
                    break;
            }

            return neighbourhood;
        }
        public void UpdateVector_absorbing_Hexagonal_random()
        {
            Cell[] neighbourhood = new Cell[6];
            updatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].GetState() == 0)
                    {
                        neighbourhood = Absorbing_Hexagonal_random(i, j);

                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < NumberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].GetState() == l)
                                {
                                    StatesArray[l]++;
                                }
                            }
                        }

                        ActualState = StatesArray[0];
                        for (int k = 1; k < NumberOfStates; k++)
                        {
                            if (StatesArray[k] > ActualState)
                            {
                                ActualState = k;
                            }
                        }


                        Map[i, j].SetState(ActualState);

                        for (int k = 0; k < NumberOfStates; k++)
                        {
                            StatesArray[k] = 0;
                        }
                    }
                }
            }
        }

        public Cell[] Periodical_Pentagonal(int i, int j)
        {
            Cell[] neighbourhood = new Cell[5];
            Random rand = new Random();
            switch (rand.Next(0, 3))
            {
                case 0:
                    {
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, 0];
                            neighbourhood[2] = previousIteration[0, Map_width - 1];
                            neighbourhood[3] = previousIteration[1, Map_width - 1];
                            neighbourhood[4] = previousIteration[1, 0];

                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[2] = previousIteration[0, Map_width - 2];
                            neighbourhood[3] = previousIteration[1, Map_width - 2];
                            neighbourhood[4] = previousIteration[1, Map_width - 1];

                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 2, 0];
                            neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[3] = previousIteration[0, Map_width - 1];
                            neighbourhood[4] = previousIteration[0, 0];

                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[3] = previousIteration[0, Map_width - 2];
                            neighbourhood[4] = previousIteration[0, Map_width - 1];

                        }
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, Map_width - 1];
                            neighbourhood[3] = previousIteration[i + 1, Map_width - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];
                        }
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, j - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];
                        }
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];
                        }
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[0, j - 1];
                            neighbourhood[4] = previousIteration[0, j];
                        }

                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];
                        }

                        break;
                    }
                case 1:
                    {
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, 0];
                            neighbourhood[1] = previousIteration[Map_height - 1, 1];
                            neighbourhood[2] = previousIteration[0, 1];
                            neighbourhood[3] = previousIteration[1, 0];
                            neighbourhood[4] = previousIteration[1, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, 0];
                            neighbourhood[2] = previousIteration[0, 0];
                            neighbourhood[3] = previousIteration[1, Map_width - 1];
                            neighbourhood[4] = previousIteration[1, 0];
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, 0];
                            neighbourhood[1] = previousIteration[Map_height - 2, 1];
                            neighbourhood[2] = previousIteration[Map_height - 1, 1];
                            neighbourhood[3] = previousIteration[0, 0];
                            neighbourhood[4] = previousIteration[0, 1];
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 2, 0];
                            neighbourhood[2] = previousIteration[Map_height - 1, 0];
                            neighbourhood[3] = previousIteration[0, Map_width - 1];
                            neighbourhood[4] = previousIteration[0, 0];
                        }
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, j];
                            neighbourhood[1] = previousIteration[Map_height - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, 0];
                            neighbourhood[2] = previousIteration[i, 0];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, 0];
                        }
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[0, j];
                            neighbourhood[4] = previousIteration[0, j + 1];
                        }

                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        break;
                    }
                case 2:
                    {
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[0, Map_width - 1];
                            neighbourhood[1] = previousIteration[0, 1];
                            neighbourhood[2] = previousIteration[1, Map_width - 1];
                            neighbourhood[3] = previousIteration[1, 0];
                            neighbourhood[4] = previousIteration[1, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[0, Map_width - 2];
                            neighbourhood[1] = previousIteration[0, 0];
                            neighbourhood[2] = previousIteration[1, Map_width - 2];
                            neighbourhood[3] = previousIteration[1, Map_width - 1];
                            neighbourhood[4] = previousIteration[1, 0];
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, 1];
                            neighbourhood[2] = previousIteration[0, Map_width - 1];
                            neighbourhood[3] = previousIteration[0, 0];
                            neighbourhood[4] = previousIteration[0, 1];
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 1, 0];
                            neighbourhood[2] = previousIteration[0, Map_width - 2];
                            neighbourhood[3] = previousIteration[0, Map_width - 1];
                            neighbourhood[4] = previousIteration[0, 0];
                        }
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i, Map_width - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[i + 1, Map_width - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, 0];
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, 0];
                        }
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[0, j - 1];
                            neighbourhood[3] = previousIteration[0, j];
                            neighbourhood[4] = previousIteration[0, j + 1];
                        }

                        else
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }

                        break;
                    }
                case 3:
                    {
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, 0];
                            neighbourhood[2] = previousIteration[Map_height - 1, 1];
                            neighbourhood[3] = previousIteration[0, Map_width - 1];
                            neighbourhood[4] = previousIteration[0, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[2] = previousIteration[Map_height - 1, 0];
                            neighbourhood[3] = previousIteration[0, Map_width - 2];
                            neighbourhood[4] = previousIteration[0, 0];
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 2, 0];
                            neighbourhood[2] = previousIteration[Map_height - 2, 1];
                            neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[4] = previousIteration[Map_height - 1, 1];
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[2] = previousIteration[Map_height - 2, 0];

                            neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[4] = previousIteration[Map_height - 1, 0];
                        }
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, Map_width - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, j - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, j];
                            neighbourhood[2] = previousIteration[Map_height - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, 0];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, 0];
                        }
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }

                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }

                        break;
                    }
                default:
                    break;
            }

            return neighbourhood;
        }
        public void UpdateVector_periodical_Pentagonal()
        {
            Cell[] neighbourhood = new Cell[5];
            updatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {

                    if (Map[i, j].GetState() == 0)
                    {
                        neighbourhood = Periodical_Pentagonal(i, j);

                        for (int k = 0; k < 5; k++)
                        {
                            for (int l = 1; l < NumberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].GetState() == l)
                                {
                                    StatesArray[l]++;
                                }
                            }
                        }

                        ActualState = StatesArray[0];
                        for (int k = 1; k < NumberOfStates; k++)
                        {
                            if (StatesArray[k] > ActualState)
                            {
                                ActualState = k;
                            }
                        }

                        Map[i, j].SetState(ActualState);
                        for (int k = 0; k < NumberOfStates; k++)
                        {
                            StatesArray[k] = 0;
                        }

                    }
                }
            }

        }
        public Cell[] Absorbing_Pentagonal(int i, int j)
        {
            Cell[] neighbourhood = new Cell[5];
            Random rand = new Random();
            switch (rand.Next(0, 3))
            {
                case 0:
                    {
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = zero;
                            neighbourhood[1] = zero;
                            neighbourhood[2] = zero;
                            neighbourhood[3] = zero;
                            neighbourhood[4] = previousIteration[1, 0];

                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = zero;
                            neighbourhood[1] = zero;
                            neighbourhood[2] = previousIteration[0, Map_width - 2];
                            neighbourhood[3] = previousIteration[1, Map_width - 2];
                            neighbourhood[4] = previousIteration[1, Map_width - 1];

                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = zero;
                            neighbourhood[1] = previousIteration[Map_height - 2, 0];
                            neighbourhood[2] = zero;
                            neighbourhood[3] = zero;
                            neighbourhood[4] = zero;

                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[3] = zero;
                            neighbourhood[4] = zero;

                        }
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = zero;
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = zero;
                            neighbourhood[3] = zero;
                            neighbourhood[4] = previousIteration[i + 1, j];

                        }
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = zero;
                            neighbourhood[1] = zero;
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];

                        }
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];

                        }
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = zero;
                            neighbourhood[4] = zero;

                        }

                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];

                        }
                        break;

                    }
                case 1:
                    {
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = zero;
                            neighbourhood[1] = zero;
                            neighbourhood[2] = previousIteration[0, 1];
                            neighbourhood[3] = previousIteration[1, 0];
                            neighbourhood[4] = previousIteration[1, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = zero;
                            neighbourhood[1] = zero;
                            neighbourhood[2] = zero;
                            neighbourhood[3] = previousIteration[1, Map_width - 1];
                            neighbourhood[4] = zero;
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, 0];
                            neighbourhood[1] = previousIteration[Map_height - 2, 1];
                            neighbourhood[2] = previousIteration[Map_height - 1, 1];
                            neighbourhood[3] = zero;
                            neighbourhood[4] = zero;
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[1] = zero;
                            neighbourhood[2] = zero;
                            neighbourhood[3] = zero;
                            neighbourhood[4] = zero;
                        }

                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }

                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = zero;
                            neighbourhood[1] = zero;
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }

                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = zero;
                            neighbourhood[2] = zero;
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = zero;
                        }

                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = zero;
                            neighbourhood[4] = zero;
                        }


                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }

                        break;
                    }
                case 2:
                    {
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = zero;
                            neighbourhood[1] = previousIteration[0, 1];
                            neighbourhood[2] = zero;
                            neighbourhood[3] = previousIteration[1, 0];
                            neighbourhood[4] = previousIteration[1, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[0, Map_width - 2];
                            neighbourhood[1] = zero;
                            neighbourhood[2] = previousIteration[1, Map_width - 2];
                            neighbourhood[3] = previousIteration[1, Map_width - 1];
                            neighbourhood[4] = zero;
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = zero;
                            neighbourhood[1] = previousIteration[Map_height - 1, 1];
                            neighbourhood[2] = zero;
                            neighbourhood[3] = zero;
                            neighbourhood[4] = zero;
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[1] = zero;
                            neighbourhood[2] = zero;
                            neighbourhood[3] = zero;
                            neighbourhood[4] = zero;
                        }

                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = zero;
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = zero;
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }

                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }

                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = zero;
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = zero;
                        }

                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = zero;
                            neighbourhood[3] = zero;
                            neighbourhood[4] = zero;
                        }


                        else
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }

                        break;
                    }
                case 3:
                    {
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = zero;
                            neighbourhood[1] = zero;
                            neighbourhood[2] = zero;
                            neighbourhood[3] = zero;
                            neighbourhood[4] = previousIteration[0, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = zero;
                            neighbourhood[1] = zero;
                            neighbourhood[2] = zero;
                            neighbourhood[3] = previousIteration[0, Map_width - 2];
                            neighbourhood[4] = zero;
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = zero;
                            neighbourhood[1] = previousIteration[Map_height - 2, 0];
                            neighbourhood[2] = previousIteration[Map_height - 2, 1];
                            neighbourhood[3] = zero;
                            neighbourhood[4] = previousIteration[Map_height - 1, 1];
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[2] = zero;
                            neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[4] = zero;

                        }

                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = zero;
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = zero;
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }

                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = zero;
                            neighbourhood[1] = zero;
                            neighbourhood[2] = zero;
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }

                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = zero;
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = zero;
                        }

                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }


                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }

                        break;
                    }
                default:
                    break;
            }

            return neighbourhood;
        }
        public void UpdateVector_absorbing_Pentagonal()
        {
            Cell[] neighbourhood = new Cell[5];
            updatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {

                    if (Map[i, j].GetState() == 0)
                    {

                        neighbourhood = Absorbing_Pentagonal(i, j);
                        for (int k = 0; k < 5; k++)
                        {
                            for (int l = 1; l < NumberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].GetState() == l)
                                {
                                    StatesArray[l]++;
                                }
                            }
                        }

                        ActualState = StatesArray[0];
                        for (int k = 1; k < NumberOfStates; k++)
                        {
                            if (StatesArray[k] > ActualState)
                            {
                                ActualState = k;
                            }
                        }

                        Map[i, j].SetState(ActualState);
                        for (int k = 0; k < NumberOfStates; k++)
                        {
                            StatesArray[k] = 0;
                        }

                    }
                }
            }


        }

        public List<Cell> Periodical_Radius(int x, int y)
        {
            int startx = x - Radius;
            int starty = y - Radius;
            int endx = x + Radius;
            int endy = y + Radius;
            //int neighbourhood;
            List<Cell> neighbourhood = new List<Cell>();

            for (int i = startx; i <= endx; i++)
            {
                for (int j = starty; j <= endy; j++)
                {
                    if (CheckRadius(x, y, i, j))
                    {
                        neighbourhood.Add(previousIteration[Math.Abs((i + Map_height) % Map_height), Math.Abs((j + Map_width) % Map_width)]);
                    }

                }
            }
            return neighbourhood;
        }

        public List<Cell> Absorbing_Radius(int x, int y)
        {
            int startx = x - Radius;
            int starty = y - Radius;
            int endx = x + Radius;
            int endy = y + Radius;
            //int neighbourhood;
            List<Cell> neighbourhood = new List<Cell>();

            for (int i = startx; i <= endx; i++)
            {
                for (int j = starty; j <= endy; j++)
                {
                    if (i >= 0 && i < Map_height && j >= 0 && j < Map_width)
                    {
                        if (CheckRadius(x, y, i, j))
                        {

                            neighbourhood.Add(previousIteration[i, j]);

                        }
                    }
                }
            }
            return neighbourhood;
        }

        public void UpdateVector_periodical_Radius()
        {
            List<Cell> neighbourhood;
            updatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {

                    if (Map[i, j].GetState() == 0)
                    {

                        neighbourhood = Periodical_Radius(i, j);

                        for (int k = 0; k < neighbourhood.Count; k++)
                        {
                            for (int l = 1; l < NumberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].GetState() == l)
                                {
                                    StatesArray[l]++;
                                }
                            }
                        }

                        ActualState = StatesArray[0];
                        for (int k = 1; k < NumberOfStates; k++)
                        {
                            if (StatesArray[k] > ActualState)
                            {
                                ActualState = k;
                            }
                        }

                        Map[i, j].SetState(ActualState);
                        for (int k = 0; k < NumberOfStates; k++)
                        {
                            StatesArray[k] = 0;
                        }

                    }
                }
            }
        }

        public void UpdateVector_absorbing_Radius()
        {
            List<Cell> neighbourhood;
            updatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {

                    if (Map[i, j].GetState() == 0)
                    {

                        neighbourhood = Absorbing_Radius(i, j);

                        for (int k = 0; k < neighbourhood.Count; k++)
                        {
                            for (int l = 1; l < NumberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].GetState() == l)
                                {
                                    StatesArray[l]++;
                                }
                            }
                        }

                        ActualState = StatesArray[0];
                        for (int k = 1; k < NumberOfStates; k++)
                        {
                            if (StatesArray[k] > ActualState)
                            {
                                ActualState = k;
                            }
                        }

                        Map[i, j].SetState(ActualState);
                        for (int k = 0; k < NumberOfStates; k++)
                        {
                            StatesArray[k] = 0;
                        }

                    }
                }
            }
        }

        bool CheckIfOnBorder(int x, int y)
        {
            Cell[] neighbourhood;
            List<Cell> neighbourhoodRadius;

            if (BC == 0)
            {
                switch (neighbourhoodType)
                {
                    case 0:
                        neighbourhood = Periodical_Neumann(x, y);
                        for (int i = 0; i < neighbourhood.Length; i++)
                        {
                            if (neighbourhood[i].GetState() != Map[x, y].GetState())
                                return true;
                        }
                        break;
                    case 1:
                        neighbourhood = Periodical_Moore(x, y);
                        for (int i = 0; i < neighbourhood.Length; i++)
                        {
                            if (neighbourhood[i].GetState() != Map[x, y].GetState())
                                return true;
                        }
                        break;
                    case 2:
                        neighbourhood = Periodical_Pentagonal(x, y);
                        for (int i = 0; i < neighbourhood.Length; i++)
                        {
                            if (neighbourhood[i].GetState() != Map[x, y].GetState())
                                return true;
                        }
                        break;
                    case 3:
                        neighbourhood = Periodical_Hexagonal_left(x, y);
                        for (int i = 0; i < neighbourhood.Length; i++)
                        {
                            if (neighbourhood[i].GetState() != Map[x, y].GetState())
                                return true;
                        }
                        break;
                    case 4:
                        neighbourhood = Periodical_Hexagonal_right(x, y);
                        for (int i = 0; i < neighbourhood.Length; i++)
                        {
                            if (neighbourhood[i].GetState() != Map[x, y].GetState())
                                return true;
                        }
                        break;
                    case 5:
                        neighbourhood = Periodical_Hexagonal_random(x, y);
                        for (int i = 0; i < neighbourhood.Length; i++)
                        {
                            if (neighbourhood[i].GetState() != Map[x, y].GetState())
                                return true;
                        }
                        break;
                    case 6:
                        neighbourhoodRadius = Periodical_Radius(x, y);
                        for (int i = 0; i < neighbourhoodRadius.Count; i++)
                        {
                            if (neighbourhoodRadius[i].GetState() != Map[x, y].GetState())
                                return true;
                        }
                        break;

                }
            }
            else if (BC == 1)
            {
                switch (neighbourhoodType)
                {
                    case 0:
                        neighbourhood = Absorbing_Neumann(x, y);
                        for (int i = 0; i < neighbourhood.Length; i++)
                        {
                            if (neighbourhood[i].GetState() != Map[x, y].GetState() && neighbourhood[i].GetState() != 0)
                                return true;
                        }
                        break;
                    case 1:
                        neighbourhood = Absorbing_Moore(x, y);
                        for (int i = 0; i < neighbourhood.Length; i++)
                        {
                            if (neighbourhood[i].GetState() != Map[x, y].GetState() && neighbourhood[i].GetState() != 0)
                                return true;
                        }
                        break;
                    case 2:
                        neighbourhood = Absorbing_Pentagonal(x, y);
                        for (int i = 0; i < neighbourhood.Length; i++)
                        {
                            if (neighbourhood[i].GetState() != Map[x, y].GetState() && neighbourhood[i].GetState() != 0)
                                return true;
                        }
                        break;
                    case 3:
                        neighbourhood = Absorbing_Hexagonal_left(x, y);
                        for (int i = 0; i < neighbourhood.Length; i++)
                        {
                            if (neighbourhood[i].GetState() != Map[x, y].GetState() && neighbourhood[i].GetState() != 0)
                                return true;
                        }
                        break;
                    case 4:
                        neighbourhood = Absorbing_Hexagonal_right(x, y);
                        for (int i = 0; i < neighbourhood.Length; i++)
                        {
                            if (neighbourhood[i].GetState() != Map[x, y].GetState() && neighbourhood[i].GetState() != 0)
                                return true;
                        }
                        break;
                    case 5:
                        neighbourhood = Absorbing_Hexagonal_random(x, y);
                        for (int i = 0; i < neighbourhood.Length; i++)
                        {
                            if (neighbourhood[i].GetState() != Map[x, y].GetState() && neighbourhood[i].GetState() != 0)
                                return true;
                        }
                        break;
                    case 6:
                        neighbourhoodRadius = Absorbing_Radius(x, y);
                        for (int i = 0; i < neighbourhoodRadius.Count; i++)
                        {
                            if (neighbourhoodRadius[i].GetState() != Map[x, y].GetState() && neighbourhoodRadius[i].GetState() != 0)
                                return true;
                        }
                        break;

                }
            }
            return false;
        }
        bool CheckRadius(int circlex, int circley, int x, int y)
        {
            //x = x - 1;
            // y = y - 1;
            //Math.Sqrt((x - circlex) * (x - circlex) + ((y - circley) * ((y - circley)) <= Radius )
            if (Math.Sqrt((((double)Math.Abs(x) + Map[(x + Map_height) % Map_height, (y + Map_width) % Map_width].GetMass_x()) - circlex) * (((double)Math.Abs(x) + Map[(x + Map_height) % Map_height, (y + Map_width) % Map_width].GetMass_x()) - circlex) + (((double)Math.Abs(y) + Map[(x + Map_height) % Map_height, (y + Map_width) % Map_width].GetMass_y()) - circley) * (((double)Math.Abs(y) + Map[(x + Map_height) % Map_height, (y + Map_width) % Map_width].GetMass_y()) - circley)) <= (double)Radius)
                return true;
            else
            {
                return false;
            }


        }

        public void MonteCarlo(double kt)
        {
            bool[,] boolmap = new bool[Map_height, Map_width];
            Random rand = new Random();
            int x, y;
            int limit = Map_height * Map_width;
            int counter = 0;
            int energy = 0;
            Cell[] neighbourhood;
            List<Cell> neighbourhoodRadius;
            int randomNeighbourState, randomNeighbourEnergy = 0;
            updatePreviousIteration();

            while (counter < limit)
            {
                x = rand.Next(0, Map_height);
                y = rand.Next(0, Map_width);

                if (boolmap[x, y] == false)
                {
                    boolmap[x, y] = true;
                    counter++;
                    if (BC == 0)
                    {
                        switch (neighbourhoodType)
                        {
                            case 0:
                                neighbourhood = Periodical_Neumann(x, y);
                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != Map[x, y].GetState())
                                    {
                                        energy++;
                                    }
                                }
                                energyMap[x, y] = energy;

                                randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();

                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != randomNeighbourState)
                                    {
                                        randomNeighbourEnergy++;
                                    }
                                }

                                if (randomNeighbourEnergy <= energy)
                                {
                                    Map[x, y].SetState(randomNeighbourState);
                                    previousIteration[x, y].SetState(randomNeighbourState);
                                    energyMap[x, y] = randomNeighbourEnergy;

                                }
                                else
                                {
                                    if (rand.NextDouble() <= Math.Exp(-(randomNeighbourEnergy - energy) / kt))
                                    {
                                        Map[x, y].SetState(randomNeighbourState);
                                        previousIteration[x, y].SetState(randomNeighbourState);
                                        energyMap[x, y] = randomNeighbourEnergy;
                                    }

                                }
                                break;
                            case 1:
                                neighbourhood = Periodical_Moore(x, y);
                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != Map[x, y].GetState())
                                    {
                                        energy++;
                                    }
                                }
                                energyMap[x, y] = energy;

                                randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();

                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != randomNeighbourState)
                                    {
                                        randomNeighbourEnergy++;
                                    }
                                }

                                if (randomNeighbourEnergy <= energy)
                                {
                                    Map[x, y].SetState(randomNeighbourState);
                                    previousIteration[x, y].SetState(randomNeighbourState);
                                    energyMap[x, y] = randomNeighbourEnergy;

                                }
                                else
                                {
                                    if (rand.NextDouble() <= Math.Exp(-(randomNeighbourEnergy - energy) / kt))
                                    {
                                        Map[x, y].SetState(randomNeighbourState);
                                        previousIteration[x, y].SetState(randomNeighbourState);
                                        energyMap[x, y] = randomNeighbourEnergy;
                                    }

                                }
                                break;
                            case 2:
                                neighbourhood = Periodical_Pentagonal(x, y);
                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != Map[x, y].GetState())
                                    {
                                        energy++;
                                    }
                                }
                                energyMap[x, y] = energy;

                                randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();

                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != randomNeighbourState)
                                    {
                                        randomNeighbourEnergy++;
                                    }
                                }

                                if (randomNeighbourEnergy <= energy)
                                {
                                    Map[x, y].SetState(randomNeighbourState);
                                    previousIteration[x, y].SetState(randomNeighbourState);
                                    energyMap[x, y] = randomNeighbourEnergy;

                                }
                                else
                                {
                                    if (rand.NextDouble() <= Math.Exp(-(randomNeighbourEnergy - energy) / kt))
                                    {
                                        Map[x, y].SetState(randomNeighbourState);
                                        previousIteration[x, y].SetState(randomNeighbourState);
                                        energyMap[x, y] = randomNeighbourEnergy;
                                    }

                                }
                                break;
                            case 3:
                                neighbourhood = Periodical_Hexagonal_left(x, y);
                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != Map[x, y].GetState())
                                    {
                                        energy++;
                                    }
                                }
                                energyMap[x, y] = energy;

                                randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();

                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != randomNeighbourState)
                                    {
                                        randomNeighbourEnergy++;
                                    }
                                }

                                if (randomNeighbourEnergy <= energy)
                                {
                                    Map[x, y].SetState(randomNeighbourState);
                                    previousIteration[x, y].SetState(randomNeighbourState);
                                    energyMap[x, y] = randomNeighbourEnergy;

                                }
                                else
                                {
                                    if (rand.NextDouble() <= Math.Exp(-(randomNeighbourEnergy - energy) / kt))
                                    {
                                        Map[x, y].SetState(randomNeighbourState);
                                        previousIteration[x, y].SetState(randomNeighbourState);
                                        energyMap[x, y] = randomNeighbourEnergy;
                                    }

                                }
                                break;
                            case 4:
                                neighbourhood = Periodical_Hexagonal_right(x, y);
                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != Map[x, y].GetState())
                                    {
                                        energy++;
                                    }
                                }
                                energyMap[x, y] = energy;

                                randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();

                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != randomNeighbourState)
                                    {
                                        randomNeighbourEnergy++;
                                    }
                                }

                                if (randomNeighbourEnergy <= energy)
                                {
                                    Map[x, y].SetState(randomNeighbourState);
                                    previousIteration[x, y].SetState(randomNeighbourState);
                                    energyMap[x, y] = randomNeighbourEnergy;

                                }
                                else
                                {
                                    if (rand.NextDouble() <= Math.Exp(-(randomNeighbourEnergy - energy) / kt))
                                    {
                                        Map[x, y].SetState(randomNeighbourState);
                                        previousIteration[x, y].SetState(randomNeighbourState);
                                        energyMap[x, y] = randomNeighbourEnergy;
                                    }

                                }
                                break;
                            case 5:
                                neighbourhood = Periodical_Hexagonal_random(x, y);
                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != Map[x, y].GetState())
                                    {
                                        energy++;
                                    }
                                }
                                energyMap[x, y] = energy;

                                randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();

                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != randomNeighbourState)
                                    {
                                        randomNeighbourEnergy++;
                                    }
                                }

                                if (randomNeighbourEnergy <= energy)
                                {
                                    Map[x, y].SetState(randomNeighbourState);
                                    previousIteration[x, y].SetState(randomNeighbourState);
                                    energyMap[x, y] = randomNeighbourEnergy;

                                }
                                else
                                {
                                    if (rand.NextDouble() <= Math.Exp(-(randomNeighbourEnergy - energy) / kt))
                                    {
                                        Map[x, y].SetState(randomNeighbourState);
                                        previousIteration[x, y].SetState(randomNeighbourState);
                                        energyMap[x, y] = randomNeighbourEnergy;
                                    }

                                }
                                break;
                            case 6:
                                neighbourhoodRadius = Periodical_Radius(x, y);
                                for (int i = 0; i < neighbourhoodRadius.Count; i++)
                                {
                                    if (neighbourhoodRadius[i].GetState() != Map[x, y].GetState())
                                    {
                                        energy++;
                                    }
                                }
                                energyMap[x, y] = energy;
                                randomNeighbourState = neighbourhoodRadius[rand.Next(0, neighbourhoodRadius.Count)].GetState();

                                for (int i = 0; i < neighbourhoodRadius.Count; i++)
                                {
                                    if (neighbourhoodRadius[i].GetState() != randomNeighbourState)
                                    {
                                        randomNeighbourEnergy++;
                                    }
                                }

                                if (randomNeighbourEnergy <= energy)
                                {
                                    Map[x, y].SetState(randomNeighbourState);
                                    previousIteration[x, y].SetState(randomNeighbourState);
                                    energyMap[x, y] = randomNeighbourEnergy;

                                }
                                else
                                {
                                    if (rand.NextDouble() <= Math.Exp(-(randomNeighbourEnergy - energy) / kt))
                                    {
                                        Map[x, y].SetState(randomNeighbourState);
                                        previousIteration[x, y].SetState(randomNeighbourState);
                                        energyMap[x, y] = randomNeighbourEnergy;
                                    }

                                }
                                break;
                        }

                    }
                    else if (BC == 1)
                    {
                        switch (neighbourhoodType)
                        {
                            case 0:
                                neighbourhood = Absorbing_Neumann(x, y);
                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != Map[x, y].GetState() && neighbourhood[i].GetState() != 0)
                                    {
                                        energy++;
                                    }
                                }
                                energyMap[x, y] = energy;
                                randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();
                                while (randomNeighbourState == 0)
                                {
                                    randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();
                                }

                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != randomNeighbourState && neighbourhood[i].GetState() != 0)
                                    {
                                        randomNeighbourEnergy++;
                                    }
                                }

                                if (randomNeighbourEnergy <= energy)
                                {
                                    Map[x, y].SetState(randomNeighbourState);
                                    previousIteration[x, y].SetState(randomNeighbourState);
                                    energyMap[x, y] = randomNeighbourEnergy;

                                }
                                else
                                {
                                    if (rand.NextDouble() <= Math.Exp(-(randomNeighbourEnergy - energy) / kt))
                                    {
                                        Map[x, y].SetState(randomNeighbourState);
                                        previousIteration[x, y].SetState(randomNeighbourState);
                                        energyMap[x, y] = randomNeighbourEnergy;
                                    }

                                }
                                break;
                            case 1:
                                neighbourhood = Absorbing_Moore(x, y);
                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != Map[x, y].GetState() && neighbourhood[i].GetState() != 0)
                                    {
                                        energy++;
                                    }
                                }
                                energyMap[x, y] = energy;
                                randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();
                                while (randomNeighbourState == 0)
                                {
                                    randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();
                                }

                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != randomNeighbourState && neighbourhood[i].GetState() != 0)
                                    {
                                        randomNeighbourEnergy++;
                                    }
                                }

                                if (randomNeighbourEnergy <= energy)
                                {
                                    Map[x, y].SetState(randomNeighbourState);
                                    previousIteration[x, y].SetState(randomNeighbourState);
                                    energyMap[x, y] = randomNeighbourEnergy;

                                }
                                else
                                {
                                    if (rand.NextDouble() <= Math.Exp(-(randomNeighbourEnergy - energy) / kt))
                                    {
                                        Map[x, y].SetState(randomNeighbourState);
                                        previousIteration[x, y].SetState(randomNeighbourState);
                                        energyMap[x, y] = randomNeighbourEnergy;
                                    }

                                }
                                break;
                            case 2:
                                neighbourhood = Absorbing_Pentagonal(x, y);
                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != Map[x, y].GetState() && neighbourhood[i].GetState() != 0)
                                    {
                                        energy++;
                                    }
                                }
                                energyMap[x, y] = energy;
                                randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();
                                while (randomNeighbourState == 0)
                                {
                                    randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();
                                }

                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != randomNeighbourState && neighbourhood[i].GetState() != 0)
                                    {
                                        randomNeighbourEnergy++;
                                    }
                                }

                                if (randomNeighbourEnergy <= energy)
                                {
                                    Map[x, y].SetState(randomNeighbourState);
                                    previousIteration[x, y].SetState(randomNeighbourState);
                                    energyMap[x, y] = randomNeighbourEnergy;

                                }
                                else
                                {
                                    if (rand.NextDouble() <= Math.Exp(-(randomNeighbourEnergy - energy) / kt))
                                    {
                                        Map[x, y].SetState(randomNeighbourState);
                                        previousIteration[x, y].SetState(randomNeighbourState);
                                        energyMap[x, y] = randomNeighbourEnergy;
                                    }

                                }
                                break;
                            case 3:
                                neighbourhood = Absorbing_Hexagonal_left(x, y);
                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != Map[x, y].GetState() && neighbourhood[i].GetState() != 0)
                                    {
                                        energy++;
                                    }
                                }
                                energyMap[x, y] = energy;
                                randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();
                                while (randomNeighbourState == 0)
                                {
                                    randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();
                                }

                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != randomNeighbourState && neighbourhood[i].GetState() != 0)
                                    {
                                        randomNeighbourEnergy++;
                                    }
                                }

                                if (randomNeighbourEnergy <= energy)
                                {
                                    Map[x, y].SetState(randomNeighbourState);
                                    previousIteration[x, y].SetState(randomNeighbourState);
                                    energyMap[x, y] = randomNeighbourEnergy;

                                }
                                else
                                {
                                    if (rand.NextDouble() <= Math.Exp(-(randomNeighbourEnergy - energy) / kt))
                                    {
                                        Map[x, y].SetState(randomNeighbourState);
                                        previousIteration[x, y].SetState(randomNeighbourState);
                                        energyMap[x, y] = randomNeighbourEnergy;
                                    }

                                }
                                break;
                            case 4:
                                neighbourhood = Absorbing_Hexagonal_right(x, y);
                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != Map[x, y].GetState() && neighbourhood[i].GetState() != 0)
                                    {
                                        energy++;
                                    }
                                }
                                energyMap[x, y] = energy;
                                randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();
                                while (randomNeighbourState == 0)
                                {
                                    randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();
                                }

                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != randomNeighbourState && neighbourhood[i].GetState() != 0)
                                    {
                                        randomNeighbourEnergy++;
                                    }
                                }

                                if (randomNeighbourEnergy <= energy)
                                {
                                    Map[x, y].SetState(randomNeighbourState);
                                    previousIteration[x, y].SetState(randomNeighbourState);
                                    energyMap[x, y] = randomNeighbourEnergy;

                                }
                                else
                                {
                                    if (rand.NextDouble() <= Math.Exp(-(randomNeighbourEnergy - energy) / kt))
                                    {
                                        Map[x, y].SetState(randomNeighbourState);
                                        previousIteration[x, y].SetState(randomNeighbourState);
                                        energyMap[x, y] = randomNeighbourEnergy;
                                    }

                                }
                                break;
                            case 5:
                                neighbourhood = Absorbing_Hexagonal_random(x, y);
                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != Map[x, y].GetState() && neighbourhood[i].GetState() != 0)
                                    {
                                        energy++;
                                    }
                                }
                                energyMap[x, y] = energy;
                                randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();
                                while (randomNeighbourState == 0)
                                {
                                    randomNeighbourState = neighbourhood[rand.Next(0, neighbourhood.Length)].GetState();
                                }

                                for (int i = 0; i < neighbourhood.Length; i++)
                                {
                                    if (neighbourhood[i].GetState() != randomNeighbourState && neighbourhood[i].GetState() != 0)
                                    {
                                        randomNeighbourEnergy++;
                                    }
                                }

                                if (randomNeighbourEnergy <= energy)
                                {
                                    Map[x, y].SetState(randomNeighbourState);
                                    previousIteration[x, y].SetState(randomNeighbourState);
                                    energyMap[x, y] = randomNeighbourEnergy;

                                }
                                else
                                {
                                    if (rand.NextDouble() <= Math.Exp(-(randomNeighbourEnergy - energy) / kt))
                                    {
                                        Map[x, y].SetState(randomNeighbourState);
                                        previousIteration[x, y].SetState(randomNeighbourState);
                                        energyMap[x, y] = randomNeighbourEnergy;
                                    }

                                }
                                break;
                            case 6:
                                neighbourhoodRadius = Absorbing_Radius(x, y);
                                for (int i = 0; i < neighbourhoodRadius.Count; i++)
                                {
                                    if (neighbourhoodRadius[i].GetState() != Map[x, y].GetState())
                                    {
                                        energy++;
                                    }
                                }
                                energyMap[x, y] = energy;
                                randomNeighbourState = neighbourhoodRadius[rand.Next(0, neighbourhoodRadius.Count)].GetState();

                                for (int i = 0; i < neighbourhoodRadius.Count; i++)
                                {
                                    if (neighbourhoodRadius[i].GetState() != randomNeighbourState)
                                    {
                                        randomNeighbourEnergy++;
                                    }
                                }

                                if (randomNeighbourEnergy <= energy)
                                {
                                    Map[x, y].SetState(randomNeighbourState);
                                    previousIteration[x, y].SetState(randomNeighbourState);
                                    energyMap[x, y] = randomNeighbourEnergy;

                                }
                                else
                                {
                                    if (rand.NextDouble() <= Math.Exp(-(randomNeighbourEnergy - energy) / kt))
                                    {
                                        Map[x, y].SetState(randomNeighbourState);
                                        previousIteration[x, y].SetState(randomNeighbourState);
                                        energyMap[x, y] = randomNeighbourEnergy;
                                    }

                                }
                                break;
                        }
                    }
                    energy = 0;
                    randomNeighbourEnergy = 0;
                }

            }

            //System.Windows.Forms.MessageBox.Show("MC done");
        }

        public void Recrystalization(double DeltaT, double Percent, double A, double B)
        {
        
            
            DeltaDislocationDensity = (A / B + (1 - A / B) * Math.Exp(-B * RecrystalizationTimeStep)) - DislocationDensity;


            DislocationDensity = ((A / B) + (1 - A / B) * Math.Exp(-1 * B * RecrystalizationTimeStep));
            DislocationDensityCritical = 46842668.25;
            writeToFile.SaveToFile(DislocationDensity);
            RecrystalizationTimeStep = RecrystalizationTimeStep + DeltaT;

            double avaragePackage = (DeltaDislocationDensity / (Map_height * Map_width)) * Percent;

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    Map[i, j].IncrementDislocationDensity(avaragePackage);
                    DensityMap[i, j] += avaragePackage;
                    DeltaDislocationDensity -= avaragePackage;
                }
            }
            //System.Windows.Forms.MessageBox.Show(DeltaDislocationDensity.ToString());
            double randomPackage;
            int Randx, Randy;
            double proability;
            /*
            double test2 = 69043324051.98999;
            int test;
            test = (int)test2;
            System.Windows.Forms.MessageBox.Show(test.ToString());
            bad toInt conversion, sign bit changes from '+' to '-'
            */
            while (DeltaDislocationDensity > 0)
            {
                Randx = rand.Next(0, Map_height);
                Randy = rand.Next(0, Map_width);

                if (CheckIfOnBorder(Randx, Randy))
                {
                    proability = rand.NextDouble();
                    randomPackage = DeltaDislocationDensity * rand.NextDouble();
                    if (randomPackage <= DeltaDislocationDensity && proability > 0.2)
                    {
                        Map[Randx, Randy].IncrementDislocationDensity(randomPackage);
                        DensityMap[Randx, Randy] += randomPackage;
                        DeltaDislocationDensity -= randomPackage;
                    }
                    if (DeltaDislocationDensity < 0.00001)
                    {
                        DeltaDislocationDensity = 0;
                    }
                }
                else
                {
                    proability = rand.NextDouble();
                    randomPackage = DeltaDislocationDensity * rand.NextDouble();
                    if (randomPackage <= DeltaDislocationDensity && proability <= 0.2)
                    {
                        Map[Randx, Randy].IncrementDislocationDensity(randomPackage);
                        DeltaDislocationDensity -= randomPackage;
                    }
                    if (DeltaDislocationDensity < 0.00001)
                    {
                        DeltaDislocationDensity = 0;
                    }
                }

            }

            List<Cell> LatelyRecrystalized = new List<Cell>();
            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (CheckIfOnBorder(i, j) && Map[i, j].GetDislocationDensity() > DislocationDensityCritical && Map[i,i].GetRecrystalizationState() == false)
                    {
                        NumberOfStates++;
                        Map[i, j].SetState(NumberOfStates);
                        Map[i, j].SetDislocationDensity(0);
                        DensityMap[i, j] = 0;
                        Map[i, j].SetRecrystalisationState(true);
                        LatelyRecrystalized.Add(Map[i, j]);
                    }
                }
            }

            updatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    int tmp = CheckIfNeighbourIsRecrystalized(i, j, LatelyRecrystalized);

                    if (tmp != 0 && previousIteration[i,j].GetRecrystalizationState() != true)
                    {
                        if (CheckIfNeighbourhoodDislocations(i, j))
                        {
                            Map[i, j].SetState(tmp);
                            Map[i, j].SetDislocationDensity(0);
                            DensityMap[i, j] = 0;
                            Map[i, j].SetRecrystalisationState(true);
                        }
                    }
                }
            }


        }
        int CheckIfNeighbourIsRecrystalized(int x, int y, List<Cell> LatelyRecrystalized)
        {
            {
                Cell[] neighbourhood;
                List<Cell> neighbourhoodRadius;

                if (BC == 0)
                {
                    switch (neighbourhoodType)
                    {
                        case 0:
                            neighbourhood = Periodical_Neumann(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (LatelyRecrystalized.Exists(Cell=>Cell.GetID() == neighbourhood[i].GetID()))
                                    return neighbourhood[i].GetState();
                            }
                            break;
                        case 1:
                            neighbourhood = Periodical_Moore(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (LatelyRecrystalized.Exists(Cell => Cell.GetID() == neighbourhood[i].GetID()))
                                    return neighbourhood[i].GetState();
                            }
                            break;
                        case 2:
                            neighbourhood = Periodical_Pentagonal(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (LatelyRecrystalized.Exists(Cell => Cell.GetID() == neighbourhood[i].GetID()))
                                    return neighbourhood[i].GetState();
                            }
                            break;
                        case 3:
                            neighbourhood = Periodical_Hexagonal_left(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (LatelyRecrystalized.Exists(Cell => Cell.GetID() == neighbourhood[i].GetID()))
                                    return neighbourhood[i].GetState();
                            }
                            break;
                        case 4:
                            neighbourhood = Periodical_Hexagonal_right(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (LatelyRecrystalized.Exists(Cell => Cell.GetID() == neighbourhood[i].GetID()))
                                    return neighbourhood[i].GetState();
                            }
                            break;
                        case 5:
                            neighbourhood = Periodical_Hexagonal_random(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (LatelyRecrystalized.Exists(Cell => Cell.GetID() == neighbourhood[i].GetID()))
                                    return neighbourhood[i].GetState();
                            }
                            break;
                        case 6:
                            neighbourhoodRadius = Periodical_Radius(x, y);
                            for (int i = 0; i < neighbourhoodRadius.Count; i++)
                            {
                                if (LatelyRecrystalized.Exists(Cell => Cell.GetID() == neighbourhoodRadius[i].GetID()))
                                    return neighbourhoodRadius[i].GetState();
                            }
                            break;

                    }
                }
                else if (BC == 1)
                {
                    switch (neighbourhoodType)
                    {
                        case 0:
                            neighbourhood = Absorbing_Neumann(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (LatelyRecrystalized.Exists(Cell => Cell.GetID() == neighbourhood[i].GetID()))
                                    return neighbourhood[i].GetState();
                            }
                            break;
                        case 1:
                            neighbourhood = Absorbing_Moore(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (LatelyRecrystalized.Exists(Cell => Cell.GetID() == neighbourhood[i].GetID()))
                                    return neighbourhood[i].GetState();
                            }
                            break;
                        case 2:
                            neighbourhood = Absorbing_Pentagonal(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (LatelyRecrystalized.Exists(Cell => Cell.GetID() == neighbourhood[i].GetID()))
                                    return neighbourhood[i].GetState();
                            }
                            break;
                        case 3:
                            neighbourhood = Absorbing_Hexagonal_left(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (LatelyRecrystalized.Exists(Cell => Cell.GetID() == neighbourhood[i].GetID()))
                                    return neighbourhood[i].GetState();
                            }
                            break;
                        case 4:
                            neighbourhood = Absorbing_Hexagonal_right(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (LatelyRecrystalized.Exists(Cell => Cell.GetID() == neighbourhood[i].GetID()))
                                    return neighbourhood[i].GetState();
                            }
                            break;
                        case 5:
                            neighbourhood = Absorbing_Hexagonal_random(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (LatelyRecrystalized.Exists(Cell => Cell.GetID() == neighbourhood[i].GetID()))
                                    return neighbourhood[i].GetState();
                            }
                            break;
                        case 6:
                            neighbourhoodRadius = Absorbing_Radius(x, y);
                            for (int i = 0; i < neighbourhoodRadius.Count; i++)
                            {
                                if (LatelyRecrystalized.Exists(Cell => Cell.GetID() == neighbourhoodRadius[i].GetID()))
                                    return neighbourhoodRadius[i].GetState();
                            }
                            break;

                    }
                }
                return 0;
            }
        }

        bool CheckIfNeighbourhoodDislocations(int x, int y)
        {
            {
                Cell[] neighbourhood;
                List<Cell> neighbourhoodRadius;

                if (BC == 0)
                {
                    switch (neighbourhoodType)
                    {
                        case 0:
                            neighbourhood = Periodical_Neumann(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].GetDislocationDensity() > previousIteration[x, y].GetDislocationDensity())
                                    return false;
                            }
                            break;
                        case 1:
                            neighbourhood = Periodical_Moore(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].GetDislocationDensity() > previousIteration[x, y].GetDislocationDensity())
                                    return false;
                            }
                            break;
                        case 2:
                            neighbourhood = Periodical_Pentagonal(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].GetDislocationDensity() > previousIteration[x, y].GetDislocationDensity())
                                    return false;
                            }
                            break;
                        case 3:
                            neighbourhood = Periodical_Hexagonal_left(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].GetDislocationDensity() > previousIteration[x, y].GetDislocationDensity())
                                    return false;
                            }
                            break;
                        case 4:
                            neighbourhood = Periodical_Hexagonal_right(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].GetDislocationDensity() > previousIteration[x, y].GetDislocationDensity())
                                    return false;
                            }
                            break;
                        case 5:
                            neighbourhood = Periodical_Hexagonal_random(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].GetDislocationDensity() > previousIteration[x, y].GetDislocationDensity())
                                    return false;
                            }
                            break;
                        case 6:
                            neighbourhoodRadius = Periodical_Radius(x, y);
                            for (int i = 0; i < neighbourhoodRadius.Count; i++)
                            {
                                if (neighbourhoodRadius[i].GetDislocationDensity() > previousIteration[x, y].GetDislocationDensity())
                                    return false;
                            }
                            break;

                    }
                }
                else if (BC == 1)
                {
                    switch (neighbourhoodType)
                    {
                        case 0:
                            neighbourhood = Absorbing_Neumann(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].GetDislocationDensity() > previousIteration[x, y].GetDislocationDensity())
                                    return false;
                            }
                            break;
                        case 1:
                            neighbourhood = Absorbing_Moore(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].GetDislocationDensity() > previousIteration[x, y].GetDislocationDensity())
                                    return false;
                            }
                            break;
                        case 2:
                            neighbourhood = Absorbing_Pentagonal(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].GetDislocationDensity() > previousIteration[x, y].GetDislocationDensity())
                                    return false;
                            }
                            break;
                        case 3:
                            neighbourhood = Absorbing_Hexagonal_left(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].GetDislocationDensity() > previousIteration[x, y].GetDislocationDensity())
                                    return false;
                            }
                            break;
                        case 4:
                            neighbourhood = Absorbing_Hexagonal_right(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].GetDislocationDensity() > previousIteration[x, y].GetDislocationDensity())
                                    return false;
                            }
                            break;
                        case 5:
                            neighbourhood = Absorbing_Hexagonal_random(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].GetDislocationDensity() > previousIteration[x, y].GetDislocationDensity())
                                    return false;
                            }
                            break;
                        case 6:
                            neighbourhoodRadius = Absorbing_Radius(x, y);
                            for (int i = 0; i < neighbourhoodRadius.Count; i++)
                            {
                                if (neighbourhoodRadius[i].GetDislocationDensity() > previousIteration[x, y].GetDislocationDensity())
                                    return false;
                            }
                            break;

                    }
                }
                return true;
            }
        }

        public int GetNumberOfStatesAfterRecrystalization()
        {
            return NumberOfStates;
        }
        public double GetMaxDislocation()
        {
            double max = 0;
            for(int i=0; i<Map_height;i++)
            {
                for(int j=0;j<Map_width;j++)
                {
                    if(max<DensityMap[i,j])
                    {
                        max = DensityMap[i, j];
                    }
                }
            }
            return max;
        }
    }
}
