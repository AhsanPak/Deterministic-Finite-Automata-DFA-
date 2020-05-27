using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            

            try
            {

                string[] Token1 = System.IO.File.ReadAllLines("Token.txt");
                string[,] TransitionTbl = Transition_Table("test.txt");
                string[] Token = new string[Token1[0].Length];
                for (int m = 0; m < Token1[0].Length; m++)
                {
                    Token[m] = (Token1[0])[m].ToString();
                }

                string InitailState = GetInitialState();
                string[] FinalStates = GetFinalState();
                string[] Apt_OR_Rejt = new string[Token.Length];
                for (int i = 0; i < Apt_OR_Rejt.Length; i++)
                {
                    Apt_OR_Rejt[i] = "R";
                }


                for (int t = 0; t < Token.Length; t++)
                {
                    for (int i = 0; i < TransitionTbl.GetLength(0); i++)
                    {
                        if (InitailState == TransitionTbl[i, 1] && Token[t] == TransitionTbl[i, 2])
                        {
                            InitailState = TransitionTbl[i, 3];
                            Apt_OR_Rejt[t] = "A";
                            Console.WriteLine(Token[t] + "----> " + TransitionTbl[i, 1] + " " + TransitionTbl[i, 2] + " " + TransitionTbl[i, 3]);
                            break;
                        }

                    }
                }

                //initial stat == any fianl state
                //All A
                bool All_A = false;
                bool initialIsEqualToFinal = false;
                for (int i = 0; i < Apt_OR_Rejt.Length; i++)
                {
                    if (Apt_OR_Rejt[i] == "A")
                    {
                        All_A = true;
                    }
                    else
                    {
                        All_A = false;
                        break;
                    }
                }
                for (int j = 0; j < FinalStates.Length; j++)
                {
                    if (FinalStates[j] == InitailState)
                    {
                        initialIsEqualToFinal = true;
                        break;
                    }
                }

                if (initialIsEqualToFinal == true && All_A == true)
                {
                    Console.WriteLine("Accepted");
                }
                else
                {
                    Console.WriteLine("Rejected");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Incorrect Format");
            }

           
        }

        //Variables
        public static string[] variables(string FileName)
        {
            string[] lines = System.IO.File.ReadAllLines(FileName);
            string[] Variables = { };
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "Variables")
                {

                    Variables = new string[lines[i + 1].Length];
                    for (int j = 0; j < lines[i + 1].Length; j++)
                    {
                        Variables[j] = lines[i + 1][j].ToString();
                    }
                    break;
                }
            }
            return Variables;
        }
        //states
        public static string[,] states(string FileName)
        {
            string[] lines = System.IO.File.ReadAllLines(FileName);
            string[] states = {};
            int flag =0;
            int temp=0;
            int NoOfStates = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "States" || flag == 1)
                {
                    flag = 1;

                    NoOfStates++;
                    if (lines[i + 1] == "Transition Table")
                    {
                        NoOfStates--;
                    flag = 0;
                    break;
                    }
                }
            }

            states = new string[NoOfStates];
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "States" || flag == 1)
                {
                    if (lines[i + 1] == "Transition Table")
                    {
                        flag = 0;
                        break;
                    }
                    flag = 1;

                    states[temp] = lines[i + 1];
                    temp++;
                    
                }
            }

            string[,] Fstates = new string[states.Length, 2];
            string[] T = new string[2];
            for (int i = 0; i < states.Length; i++)
            {
                
                T = states[i].Split(' ');
                Fstates[i, 0] = T[0];
                Fstates[i, 1] = T[1];

            }

            return Fstates;
        }
        //Transition Table
        public static string[,] Transition_Table(string FileName)
        {
            string[] lines = System.IO.File.ReadAllLines(FileName);
            string[,] TranTbl = {};
            int rows = -1; // Because of 1 row addition
            int flag = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "Transition Table" || flag == 1)
                {
                    flag = 1;
                    rows++;
                }
            }

            flag = 0;
            string temp;
            string[] temp1 = new string[3];
            TranTbl = new string[rows,4];
            string[,] state = states("test.txt");
            int j = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "Transition Table" || flag == 1)
                {
                    flag = 1;
                    
                    if (lines[i] != "Transition Table")
                    {
                        temp = lines[i];
                        temp1 = temp.Split(' ');
                        for (int k = 0; k < state.GetLength(0); k++)
                        {
                            if (temp1[0] == state[k,0])
                            {
                                TranTbl[j, 0] = state[k, 1];
                            }
                        }
                        TranTbl[j,1]=temp1[0];
                        TranTbl[j,2]=temp1[1];
                        TranTbl[j,3]=temp1[2];
                        j++;
                    }

                }
            }

          

            //Console.WriteLine(rows);
            return TranTbl;
        }
        //GetInitialState
        public static string GetInitialState()
        {
            string Stat="NS";
            string[,] AllStates = states("test.txt");
            for (int i = 0; i < AllStates.GetLength(0); i++)
            {
                if (AllStates[i, 1] == "-+" || AllStates[i, 1] == "-")
                {
                    Stat = AllStates[i, 0];
                    break;
                }
            }
            return Stat;
        }
        //GetFinalStates
        public static string[] GetFinalState()
        {
           
            string[,] AllStates = states("test.txt");
            int j = 0;
            int NoOfFinalStates = 0;
            for (int i = 0; i < AllStates.GetLength(0); i++)
            {
                if (AllStates[i, 1] == "+" || AllStates[i, 1] == "-+")
                {
                    NoOfFinalStates++;
                }
            }
            string[] Stat =new string[NoOfFinalStates];

            for (int i = 0; i < AllStates.GetLength(0); i++)
            {
                if (AllStates[i, 1] == "+" || AllStates[i, 1] == "-+")
                {
                    Stat[j] = AllStates[i, 0];
                    j++;
                }
            }
            return Stat;
            
        }

    }
}
