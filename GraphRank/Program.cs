using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GraphRank
{

    // https://github.com/gkatsev/pagerank/blob/master/pagerank.py
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool bShowForm = false;
            if (bShowForm)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }


            float[,] AdjacencyMatrix = null;


            // http://www.peterbe.com/plog/blogitem-040321-1
            float[,] web = {
                 {0, 1, 0, 0}
                ,{0, 0, 1, 0}
                ,{0, 0, 0, 1}
                ,{1, 0, 0, 0}
            };


            // http://www.peterbe.com/plog/blogitem-040321-1
            float[,] web2 = {
                 {0, 1, 0, 0}
                ,{0, 0, 1, 0}
                ,{0, 1, 0, 1}
                ,{1, 1, 0, 0}
            };



            // http://www.math.cornell.edu/~mec/Winter2009/RalucaRemus/Lecture3/lecture3.html
            float[,] web3= {
                 {0, 0, 0}
                ,{0, 0, 0}
                ,{1, 1, 0}
            };



            float[,] web1 = {
                 {0, 1, 0, 0}
                ,{0, 0, 0, 1}
                ,{1, 1, 0, 0}
                ,{0, 0, 1, 0}
            };


            float[] web4= {3,3,3};


            //RankingAlgorithms.PageRank.Normalize(web4);


            RankingAlgorithms.PageRank.GetPageRank(web, 0.82f);


            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(" --- Press any key to continue --- ");
            Console.ReadKey();
        }
    }
}
