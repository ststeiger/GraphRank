
using System;
using System.Collections.Generic;
using System.Text;


// http://wenku.baidu.com/view/a1981d93daef5ef7ba0d3c6e.html
// http://imagetransformationtool.googlecode.com/svn/trunk/graph/social_network_tool/PageRank.cs
// https://code.google.com/p/imagetransformationtool/
// http://imagetransformationtool.googlecode.com/svn/trunk/graph/social_network_tool/HITS.cs
// http://digital.library.unt.edu/ark:/67531/metadc30962/m1/1/
// http://wenku.baidu.com/view/f37277232f60ddccda38a026.html


namespace RankingAlgorithms
{


    // http://www.peterbe.com/plog/blogitem-040321-1
    // http://www.codeproject.com/Articles/17425/A-Vector-Type-for-C
    class PageRank
    {


        private static void GetBinaryMatrix(ref float[,] matrix, int level)
        {
            for (int i = 0; i < level; i++)
                for (int j = 0; j < level; j++)
                    if (matrix[i, j] >= 1f)
                        matrix[i, j] = 1f;
                    else
                        matrix[i, j] = 0f;
        }


        private static void GetStochasticMatrix_old(ref float[,] matrix, int level, float percentJumping)
        {
            int countRow = 0;
            float tam = percentJumping / level;
            for (int i = 0; i < level; i++)
            {
                countRow = 0;
                for (int j = 0; j < level; j++)
                    if (matrix[i, j] == 1f)
                        countRow++;
                for (int j = 0; j < level; j++)
                    matrix[i, j] = matrix[i, j] / countRow * (1 - percentJumping) + tam;
            }
        }

        private static void GetStochasticMatrix(ref float[,] matrix, int level, float percentJumping)
        {
            int countRow = 0;
            float tam = percentJumping / level;

            for (int i = 0; i < level; i++)
            {
                countRow = 0;
                for (int j = 0; j < level; j++)
                    if (matrix[i, j] == 1f)
                        countRow++;
                for (int j = 0; j < level; j++)
                    if (countRow > 0)
                        matrix[i, j] = matrix[i, j] / countRow * (1 - percentJumping) + tam;
                    else
                        matrix[i, j] = 0;
                        //matrix[i, j] = (1.0f / (float)level) * (1 - percentJumping) + tam;

            }
        }



        private static void GetStochasticMatrix_wrong(ref float[,] matrix, int level, float percentJumping)
        {
            int row_length = level;
            float damping_factor = percentJumping;

            float d = (1 - damping_factor) / row_length;


            int row_total = 0;
            float tam = percentJumping / level;
            for (int i = 0; i < level; i++)
            {
                row_total = 0;
                for (int j = 0; j < level; j++)
                    if (matrix[i, j] == 1f)
                        row_total++;

                for (int j = 0; j < level; j++)
                {
                    if (row_total > 0)
                        matrix[i, j] = matrix[i, j] / (float) row_total * (1 - damping_factor) + d;
                    else
                        matrix[i, j] = (1.0f / (float)row_length) * (1 - damping_factor) + d;
                } // Next j
                    
            } // Next i
        }





        private static float[] MultiplyVectorMatrix(float[,] matrix, float[] vector, int level)
        {
            float[] result = new float[level];
            float temp;

            for (int i = 0; i < level; i++)
            {
                temp = 0f;
                for (int k = 0; k < level; k++)
                    temp += matrix[k, i] * vector[k];
                result[i] = temp;
            }
            return result;
        }


        private static float[] CreateVector(int level)
        {
            float[] result = new float[level];
            result[0] = 1;
            return result;
        }


        private static float[] CreateVector(int level, int id)
        {
            if (id < level)
            {
                float[] result = new float[level];
                result[id] = 1;
                return result;
            }
            else
                return null;
        }


        private static bool IsSameVector(float[] vectorA, float[] vectorB, int levelVector, int SaiSo)
        {
            for (int i = 0; i < levelVector; i++)
                if ((float)Math.Round(vectorA[i], SaiSo) != (float)Math.Round(vectorB[i], SaiSo))
                    return false;
            return true;
        }

        /*
        public static float[] GetPageRank(float[,] matrix, int level, float percentJumping)
        {
            GetBinaryMatrix(ref matrix, level);
            GetStochasticMatrix(ref matrix, level, percentJumping);

            float[] vector = CreateVector(level);
            float[] pagerank = new float[level];
            int count = 0;

            do
            {
                pagerank = MultiplyVectorMatrix(matrix, vector, level);
                if (IsSameVector(pagerank, vector, level, 5) == true)
                    break;
                else
                    for (int i = 0; i < level; i++)
                        vector[i] = pagerank[i];
                count++;
            } while (count < 1000);

            for (int i = 0; i < pagerank.Length; i++)
                pagerank[i] = (float)Math.Round(pagerank[i], 3);
            return pagerank;
        }
        

        public static float[] GetPageRank(float[,] matrix, int level, float percentJumping, int idCreateVector)
        {
            GetBinaryMatrix(ref matrix, level);
            GetStochasticMatrix(ref matrix, level, percentJumping);

            float[] vector = CreateVector(level, idCreateVector);
            float[] pagerank = new float[level];
            int count = 0;

            do
            {
                pagerank = MultiplyVectorMatrix(matrix, vector, level);
                if (IsSameVector(pagerank, vector, level, 5) == true)
                    break;
                else
                    for (int i = 0; i < level; i++)
                        vector[i] = pagerank[i];
                count++;
            } while (count < 20);

            for (int i = 0; i < pagerank.Length; i++)
                pagerank[i] = (float)Math.Round(pagerank[i], 3);
            return pagerank;
        }
        */

        public static float[] Normalize(float[] A)
        {
            double sum = 0.0;

            for(int i = 0; i < A.Length; ++i)
            {
                sum += (A[i] * A[i]);
            }

            //float distance = (float) Math.Sqrt(sum);
            float distance = (float) sum;

            float[] vec = new float[A.Length];
            for(int i = 0; i < A.Length; ++i)
            {
                vec[i] += A[i]/distance;
            }

            return vec;
        }


        public static float[] PowerIterate(float[,] matrix, int digitsPrecision)
        {
            // , , float percentJumping,  
            int level = matrix.GetLength(0);
            //int idCreateVector = level;
            int idCreateVector = 0;

            float[] vector = CreateVector(level, idCreateVector);
            float[] pagerank = new float[level];
            int count = 0;

            

            do
            {
                // vector = Normalize(vector);
                pagerank = MultiplyVectorMatrix(matrix, vector, level);

                if (IsSameVector(pagerank, vector, level, digitsPrecision) == true)
                    break;
                else
                    for (int i = 0; i < level; i++)
                        vector[i] = pagerank[i];

                count++;
            } while (count < 20);

            // vector = RankingAlgorithms.PageRank.Normalize(vector);

            return pagerank;
        }



        public static float[] GetPageRank(float[,] matrix, float percentJumping)
        {
            int level = matrix.GetLength(0);
            //, int idCreateVector, int SaiSo

            GetBinaryMatrix(ref matrix, level);
            GetStochasticMatrix(ref matrix, level, percentJumping);
            float[] pagerank = PowerIterate(matrix, 3);

            for (int i = 0; i < pagerank.Length; i++)
                pagerank[i] = (float)Math.Round(pagerank[i], 3);

            //pagerank = Normalize(pagerank);

            return pagerank;
        }


    }


}
