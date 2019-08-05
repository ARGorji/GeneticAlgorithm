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

namespace EightMinisters
{
    public class Data
    {
        protected int intVal;
        protected string strVal;

        public Data(string strValue, int intValue)
        {
            intVal = intValue;
            strVal = strValue;
        }

        public int IntegerData {
            get
            {
                return intVal;
            }
            set
            {
                intVal = value;
            }
        }
        public string StringData {

            get
            {
                return strVal;
            }
            set
            {
                strVal = value;
            }
        }
    }

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            List<Data> list = new List<Data>();
            //list.Add(new Data("67203420", -1));
            list.Add(new Data("34521760", 10000));
            list.Add(new Data("21726035", 10000));
            list.Add(new Data("40657123", 10000));
            list.Add(new Data("72615034", 10000));


            GeneticAlgorithm(list);
            

//            int CrossCount = GetCrossCount(list[0]);

            int hhh = 1;
        }

        private void GeneticAlgorithm(List<Data> ChromosomeList)
        {
            for (int i = 0; i < ChromosomeList.Count; i++)
            {
                ChromosomeList[i].IntegerData = GetCrossCount(ChromosomeList[i].StringData); ;
            }

            int LowestCross = GetLowestCross(ChromosomeList, "");

            int FirstCandidIndex = GetLowestCross(ChromosomeList, ChromosomeList[LowestCross].StringData);
            int SecondCandidIndex = GetLowestCross(ChromosomeList, ChromosomeList[LowestCross].StringData + "," + ChromosomeList[FirstCandidIndex].StringData);

            Random rnd1 = new Random();

            int RandIndex1 = rnd1.Next(0, 8);
            Thread.Sleep(100);
            int RandIndex2 = rnd1.Next(0, 8);

            string strNewChromosomes1 = MergeChromosomes(ChromosomeList[LowestCross].StringData, ChromosomeList[FirstCandidIndex].StringData, RandIndex1, RandIndex2);
            string[] strNewChromosomesArray1 = strNewChromosomes1.Split(',');
            ChromosomeList.Add(new Data(strNewChromosomesArray1[0], 1000));
            ChromosomeList.Add(new Data(strNewChromosomesArray1[1], 1000));

            string strNewChromosomes2 = MergeChromosomes(ChromosomeList[LowestCross].StringData, ChromosomeList[SecondCandidIndex].StringData, RandIndex1, RandIndex2);
            string[] strNewChromosomesArray2 = strNewChromosomes2.Split(',');
            ChromosomeList.Add(new Data(strNewChromosomesArray2[0], 1000));
            ChromosomeList.Add(new Data(strNewChromosomesArray2[1], 1000));

            int JumpGeneIndex = rnd1.Next(0, 8);
            Thread.Sleep(100);
            int JumpChromosomeIndex = rnd1.Next(4, 8);
            ChromosomeList[JumpChromosomeIndex].StringData = JumpChromosome(ChromosomeList[JumpChromosomeIndex].StringData, JumpGeneIndex);

            for (int i = 0; i < ChromosomeList.Count; i++)
            {
                int CrossCnt = GetCrossCount(ChromosomeList[i].StringData);
                if(CrossCnt == 0)
                {
                    string Result = ChromosomeList[i].StringData;
                }
                ChromosomeList[i].IntegerData = CrossCnt;
            }

            int BestIndex1 = GetLowestCross(ChromosomeList, ChromosomeList[LowestCross].StringData);
            int BestIndex2 = GetLowestCross(ChromosomeList, ChromosomeList[LowestCross].StringData + "," + ChromosomeList[BestIndex1].StringData);
            int BestIndex3 = GetLowestCross(ChromosomeList, ChromosomeList[LowestCross].StringData + "," + ChromosomeList[BestIndex1].StringData + "," + ChromosomeList[BestIndex2].StringData);
            int BestIndex4 = GetLowestCross(ChromosomeList, ChromosomeList[LowestCross].StringData + "," + ChromosomeList[BestIndex1].StringData + "," + ChromosomeList[BestIndex2].StringData + "," + ChromosomeList[BestIndex3].StringData);

            List<Data> Newlist = new List<Data>();
            Newlist.Add(new Data(ChromosomeList[BestIndex1].StringData, 10000));
            Newlist.Add(new Data(ChromosomeList[BestIndex2].StringData, 10000));
            Newlist.Add(new Data(ChromosomeList[BestIndex3].StringData, 10000));
            Newlist.Add(new Data(ChromosomeList[BestIndex4].StringData, 10000));

            GeneticAlgorithm(Newlist);

            int hh = 1;
        }

        private string JumpChromosome(string strChoromosome, int JumpIndex)
        {
            string NewCr = "";
            for (int i = 0; i < 8; i++)
            {
                if (i != JumpIndex)
                    NewCr += strChoromosome.Substring(i, 1);
                else
                {
                    int CurGene = Convert.ToInt32( strChoromosome.Substring(i,1));
                    CurGene++;
                        if (CurGene == 8)
                        CurGene = 0;
                    NewCr += CurGene;
                }
            }

            return NewCr;
        }

        private string MergeChromosomes(string strChromosome1, string strChromosome2, int randIndex1, int randIndex2)
        {
            string NewChromosome1 = "";
            string NewChromosome2 = "";
            for (int i = 0; i < 8; i++)
            {
                if (i != randIndex1 && i != randIndex2)
                    NewChromosome1 += strChromosome1[i];
                else
                    NewChromosome1 += strChromosome2[i];
            }

            for (int i = 0; i < 8; i++)
            {
                if (i != randIndex1 && i != randIndex2)
                    NewChromosome2 += strChromosome2[i];
                else
                    NewChromosome2 += strChromosome1[i];
            }

            return NewChromosome1 + "," + NewChromosome2;

        }

        protected int GetLowestCross(List<Data> CrList, string strExcludeList)
        {
            int MinIndex = 0;
            int MinCross = 1000;

            string[] ExArray = strExcludeList.Split(',');
            for (int i = 0; i < CrList.Count; i++)
            {
                if (CrList[i].IntegerData <= MinCross && !ExArray.Contains(CrList[i].StringData))
                {
                    MinIndex = i;
                    MinCross = CrList[i].IntegerData;
                }
            }

            return MinIndex;
        }

        public int GetCrossCount(string Positions)
        {
            string CrossHistory = "";

            int CrossCount = 0;
            int[] Matrix = new int[8];

            //string Positions = data.StringData;
            for (int i = 0; i < 8; i++)
            {
                Matrix[i] = Convert.ToInt32(Positions.Substring(i, 1));
            }

            for (int j = 0; j < 8; j++)
            {
                for (int k = 0; k < 8; k++)
                {
                    if (j != k)
                    {
                        if (Matrix[j] == Matrix[k])
                        {
                            CrossCount++;
                            CrossHistory += Matrix[j] + "," + Matrix[k] + "|";
                        }
                    }
                }
            }

            //for (int i = 0; i < 8; i++)
            //{
            //    int CurCol = Matrix[i];
            //    for (int j = 0; j < 8; j++)
            //    {
            //        if(i != j)
            //        {
            //            if (CurCol > i)
            //            {
            //                if ((CurCol + 1) + (i + 1) - 1 == CurCol)
            //                    CrossCount++;
            //            }
            //            else
            //            {
            //                if ((CurCol + 1) + (i + 1) - 1 == CurCol + 89)
            //                    CrossCount++;

            //            }
            //        }
            //    }
            //}


            for (int i = 0; i < 8; i++)
            {
                if (Matrix[i] + i + 1 < 9)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (i != j)
                        {
                            if (
                                Matrix[i] + 1 + i + 1 - 1 == 
                                Matrix[j] + 1 + j + 1 - 1 
                                )
                            {
                                CrossCount++;
                                CrossHistory += Matrix[i] + "," + Matrix[j] + "|";

                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (i != j)
                        {
                            if (
                                    Matrix[i] + 1 + i + 1 - 1 ==
                                    Matrix[j] + 1 + j + 1 - 1 
                                    )
                            {
                                CrossCount++;
                                CrossHistory += Matrix[i] + "," + Matrix[j] + "|";

                            }
                        }
                    }
                }
            }

            for (int i = 0; i < 8; i++)
            {
                if (Matrix[i] > i)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (i != j)
                        {
                            if (
                                Matrix[i] + 1 - (i + 1) + 8 ==
                                Matrix[j] + 1 - (j + 1) + 8
                                )
                            {
                                CrossCount++;
                                CrossHistory += Matrix[i] + "," + Matrix[j] + "|";
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (i != j)
                        {
                            if (
                                Matrix[i] + 1 - (i + 1) + 8 ==
                                Matrix[j] + 1 - (j + 1) + 8
                                )
                            {
                                CrossCount++;
                                CrossHistory += Matrix[i] + "," + Matrix[j] + "|";
                            }
                        }
                    }
                }
            }

            return CrossCount / 2;
        }
    }
}
