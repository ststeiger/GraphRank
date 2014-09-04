
using System;
using System.Collections.Generic;


namespace RankingAlgorithms
{


    class MarkovCluster
    {


        public static void AddingSelfLoop(ref float[,] matrix, int n)
        {
            for (int i = 0; i < n; i++)
                matrix[i, i] = 1f;
        }


        public static void NormalizeMatrix(ref float[,] matrix, int n)
        {
            float[] listfloat = new float[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    listfloat[i] += matrix[j, i];

                if (listfloat[i] == 0f)
                    listfloat[i] = 1f;
            }

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    matrix[j, i] = (float)Math.Round(matrix[j, i] / listfloat[i], 3);
        }


        public static float[,] PowerMatrix(float[,] matrix, int n)
        {
            float[,] result = new float[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    result[i, j] = 0.0f;
                    for (int k = 0; k < n; k++)
                        result[i, j] += matrix[i, k] * matrix[k, j];
                    result[i, j] = (float)Math.Round(result[i, j], 3);
                }
            return result;
        }


        public static void InflateMatrix(ref float[,] matrix, int n)
        {
            float sumSquare;
            for (int i = 0; i < n; i++)
            {
                sumSquare = 0f;
                for (int j = 0; j < n; j++)
                {
                    matrix[j, i] = matrix[j, i] * matrix[j, i];
                    sumSquare += matrix[j, i];
                }
                if (sumSquare == 0.0d)
                    sumSquare = 1.0f;
                for (int j = 0; j < n; j++)
                    matrix[j, i] = (float)Math.Round(matrix[j, i] / sumSquare, 3);
            }
        }


        public static bool IsSteadyMatrix(float[,] matrix, int n)
        {
            float value = 0f;
            bool flag;
            for (int i = 0; i < n; i++)
            {
                flag = false;
                for (int j = 0; j < n; j++)
                    if (matrix[j, i] != 0.000d)
                    {
                        if (flag == false)
                        {
                            value = matrix[j, i];
                            flag = true;
                        }
                        else
                            if (value != matrix[j, i])
                                return false;
                    }
            }
            return true;                    
        }


        public static List<List<int>> InterpretMatrix(float[,] matrix, int n)
        {
            List<List<int>> listCluster = new List<List<int>>();
            List<int> id_cluster = new List<int>();

            /// Interperting Matrix To Cluster
            for (int i = 0; i < n; i++)
            {
                List<int> cluster = new List<int>();
                for (int j = 0; j < n; j++)
                    if (id_cluster.Contains(j) == false && matrix[i, j] != 0)
                    {
                        cluster.Add(j);
                        id_cluster.Add(j);

                        if (id_cluster.Count == n && cluster.Count != 0)
                        {
                            listCluster.Add(cluster);
                            return listCluster;
                        }
                    }
                if (cluster.Count != 0)
                    listCluster.Add(cluster);
            }
            return listCluster;
        }


        public static List<List<int>> InterpretMatrixOverlap(float[,] matrix, int n)
        {
            List<List<int>> listCluster = new List<List<int>>();
            /// Interperting Matrix To Cluster
            for (int i = 0; i < n; i++)
            {
                List<int> cluster = new List<int>();
                for (int j = 0; j < n; j++)
                    if (matrix[i, j] != 0)
                        cluster.Add(j);

                if (cluster.Count != 0)
                    listCluster.Add(cluster);
            }
            return listCluster;
        }     


        /// <summary>
        /// Cluster grouping function performed. (Hàm thực hiện gom nhóm Cluster.) 
        /// </summary>
        /// <param name="matrix">[n,n] ma trận vuông đồng dạng | khoảng cách</param>
        /// <param name="n">cấp ma trận</param>
        /// <param name="numberLoop">Number of iterations to workers (Số lần lặp để nhân)</param>
        /// <param name="AddSelfLoop">True: The main diagonal elements (phần tử đường chéo chính) = 1. False: Conversely (Ngược lại)</param>
        /// <param name="Case">
        /// 1: using the function (sử dụng hàm) InterpretMatrix(...)
        /// 2: using the function (sử dụng hàm) InterpretMatrixOverlap(...)
        /// </param>
        /// <returns></returns>
        public static List<List<int>> MarkovClusterAlgorithm(ref float[,] matrix, int n, out int numberLoop, bool AddSelfLoop, int Case)
        {
            if (AddSelfLoop == true)
                AddingSelfLoop(ref matrix, n);

            NormalizeMatrix(ref matrix, n);

            numberLoop = 0;
            while (numberLoop < 30)
            {
                matrix = PowerMatrix(matrix, n);
                InflateMatrix(ref matrix, n);
                numberLoop++;

                if (IsSteadyMatrix(matrix, n) == true)
                    break;
            }

            List<List<int>> result = new List<List<int>>();
            switch (Case)
            {
                case 1: ///InterpretMatrix(..)
                    {
                        result = InterpretMatrix(matrix, n);
                        break;
                    }
                case 2: ///InterpretMatrix(..)
                    {
                        result = InterpretMatrixOverlap(matrix, n);
                        break;
                    }
            }
            return result;
        }


        public static void MarkovClusterAlgorithm(ref float[,] matrix, int n, out int numberLoop, bool AddSelfLoop)
        {
            if (AddSelfLoop == true)
                AddingSelfLoop(ref matrix, n);

            NormalizeMatrix(ref matrix, n);

            numberLoop = 0;
            while (numberLoop < 30)
            {
                matrix = PowerMatrix(matrix, n);
                InflateMatrix(ref matrix, n);
                numberLoop++;

                if (IsSteadyMatrix(matrix, n) == true)
                    break;
            }
        }


    }


}
