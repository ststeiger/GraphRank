
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



            // https://www.eff.org/https-everywhere
            // https://tools.ietf.org/html/rfc6797
            // https://www.eff.org/deeplinks/2014/04/why-web-needs-perfect-forward-secrecy
            // http://stackoverflow.com/questions/20268954/page-rank-matlab
            // http://www.math.iit.edu/~fass/matlab/pagerank.m
            // http://www.math.cornell.edu/~mec/Winter2009/RalucaRemus/Lecture3/lecture3.html
            // http://michaelnielsen.org/blog/using-your-laptop-to-compute-pagerank-for-millions-of-webpages/
            // http://langvillea.people.cofc.edu/PRDataCode/index.html


            //RankingAlgorithms.PageRank.Normalize(web4);


            RankingAlgorithms.PageRank.GetPageRank(web, 0.82f);


            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(" --- Press any key to continue --- ");
            Console.ReadKey();
        }


    }


}
