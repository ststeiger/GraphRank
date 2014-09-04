
using System;
using System.Collections.Generic;


// PageRank sculpting
// http://en.wikipedia.org/wiki/Nofollow
// https://support.google.com/webmasters/answer/96569?hl=en
// http://en.wikipedia.org/wiki/Matt_Cutts
// Matthew "Matt" Cutts leads the Webspam team at Google, and works with the search quality team on search engine optimization issues.[1][2]
// https://www.mattcutts.com/blog/pagerank-sculpting/




// http://en.wikipedia.org/wiki/Diagonal_matrix
// http://de.wikipedia.org/wiki/Inverse_Matrix
// http://en.wikipedia.org/wiki/Invertible_matrix





// http://wenku.baidu.com/view/a1981d93daef5ef7ba0d3c6e.html
// http://imagetransformationtool.googlecode.com/svn/trunk/graph/social_network_tool/PageRank.cs
// https://code.google.com/p/imagetransformationtool/
// http://imagetransformationtool.googlecode.com/svn/trunk/graph/social_network_tool/HITS.cs
// http://digital.library.unt.edu/ark:/67531/metadc30962/m1/1/
// http://wenku.baidu.com/view/f37277232f60ddccda38a026.html

// http://arxiv.org/abs/1206.4897

// http://books.google.de/books?id=KsHTl_2Pfl8C&pg=PA176&lpg=PA176&dq=pagerank.py+StochasticMatrix&source=bl&ots=rMd_0pc2XG&sig=576xI4ItzqDWgvyMoTREpqBGqVE&hl=en&sa=X&ei=9xIIVIuzLKXX7AbcrIGYDw&ved=0CEcQ6AEwBQ#v=onepage&q=pagerank.py%20StochasticMatrix&f=false


// http://www.codeproject.com/Articles/19032/C-Matrix-Library
// http://en.wikipedia.org/wiki/Scheduling_%28computing%29#Scheduling_disciplines
// http://msdn.microsoft.com/en-us/magazine/jj863137.aspx


namespace RankingAlgorithms
{


    // http://en.wikipedia.org/wiki/PageRank#Computation
    // http://www.math.cornell.edu/~mec/Winter2009/RalucaRemus/Lecture3/lecture3.html


    // http://www.peterbe.com/plog/blogitem-040321-1
    // https://github.com/peterbe/Peterbecom/blob/master/PageRank.py
    // http://www.codeproject.com/Articles/17425/A-Vector-Type-for-C
    class PageRank
    {


        /*
* Perform LUP decomposition on a matrix A.
* Return L and U as a single matrix(double[][]) and P as an array of ints.
* We implement the code to compute LU "in place" in the matrix A.
* In order to make some of the calculations more straight forward and to 
* match Cormen's et al. pseudocode the matrix A should have its first row and first columns
* to be all 0.
* */
        // http://www.rkinteractive.com/blogs/SoftwareDevelopment/post/2013/05/07/Algorithms-In-C-LUP-Decomposition.aspx
        public static Tuple<double[][], int[]> LUPDecomposition(double[][] A)
        {
            int n = A.Length - 1;
            /*
            * pi represents the permutation matrix.  We implement it as an array
            * whose value indicates which column the 1 would appear.  We use it to avoid 
            * dividing by zero or small numbers.
            * */
            int[] pi = new int[n + 1];
            double p = 0;
            int kp = 0;
            int pik = 0;
            int pikp = 0;
            double aki = 0;
            double akpi = 0;

            //Initialize the permutation matrix, will be the identity matrix
            for (int j = 0; j <= n; j++)
            {
                pi[j] = j;
            }

            for (int k = 0; k <= n; k++)
            {
                /*
                * In finding the permutation matrix p that avoids dividing by zero
                * we take a slightly different approach.  For numerical stability
                * We find the element with the largest 
                * absolute value of those in the current first column (column k).  If all elements in
                * the current first column are zero then the matrix is singluar and throw an
                * error.
                * */
                p = 0;
                for (int i = k; i <= n; i++)
                {
                    if (Math.Abs(A[i][k]) > p)
                    {
                        p = Math.Abs(A[i][k]);
                        kp = i;
                    }
                }
                if (p == 0)
                {
                    throw new Exception("singular matrix");
                }
                /*
                * These lines update the pivot array (which represents the pivot matrix)
                * by exchanging pi[k] and pi[kp].
                * */
                pik = pi[k];
                pikp = pi[kp];
                pi[k] = pikp;
                pi[kp] = pik;

                /*
                * Exchange rows k and kpi as determined by the pivot
                * */
                for (int i = 0; i <= n; i++)
                {
                    aki = A[k][i];
                    akpi = A[kp][i];
                    A[k][i] = akpi;
                    A[kp][i] = aki;
                }

                /*
                    * Compute the Schur complement
                    * */
                for (int i = k + 1; i <= n; i++)
                {
                    A[i][k] = A[i][k] / A[k][k];
                    for (int j = k + 1; j <= n; j++)
                    {
                        A[i][j] = A[i][j] - (A[i][k] * A[k][j]);
                    }
                }
            }
            return Tuple.Create(A, pi);
        }



        /*
* Given L,U,P and b solve for x.
* Input the L and U matrices as a single matrix LU.
* Return the solution as a double[].
* LU will be a n+1xm+1 matrix where the first row and columns are zero.
* This is for ease of computation and consistency with Cormen et al.
* pseudocode.
* The pi array represents the permutation matrix.
* */
        // http://www.rkinteractive.com/blogs/SoftwareDevelopment/post/2013/05/14/Algorithms-In-C-Solving-A-System-Of-Linear-Equations.aspx
        public static double[] LUPSolve(double[][] LU, int[] pi, double[] b)
        {
            int n = LU.Length - 1;
            double[] x = new double[n + 1];
            double[] y = new double[n + 1];
            double suml = 0;
            double sumu = 0;
            double lij = 0;

            /*
            * Solve for y using formward substitution
            * */
            for (int i = 0; i <= n; i++)
            {
                suml = 0;
                for (int j = 0; j <= i - 1; j++)
                {
                    /*
                    * Since we've taken L and U as a singular matrix as an input
                    * the value for L at index i and j will be 1 when i equals j, not LU[i][j], since
                    * the diagonal values are all 1 for L.
                    * */
                    if (i == j)
                    {
                        lij = 1;
                    }
                    else
                    {
                        lij = LU[i][j];
                    }
                    suml = suml + (lij * y[j]);
                }
                y[i] = b[pi[i]] - suml;
            }
            //Solve for x by using back substitution
            for (int i = n; i >= 0; i--)
            {
                sumu = 0;
                for (int j = i + 1; j <= n; j++)
                {
                    sumu = sumu + (LU[i][j] * x[j]);
                }
                x[i] = (y[i] - sumu) / LU[i][i];
            }
            return x;
        }


        // http://www.rkinteractive.com/blogs/SoftwareDevelopment/post/2013/05/21/Algorithms-In-C-Finding-The-Inverse-Of-A-Matrix.aspx

        // Given an nXn matrix A, solve n linear equations to find the inverse of A.
        public static double[][] InvertMatrix(double[][] A)
        {
            int n = A.Length;
            //e will represent each column in the identity matrix
            double[] e;
            //x will hold the inverse matrix to be returned
            double[][] x = new double[n][];
            for (int i = 0; i < n; i++)
            {
                x[i] = new double[A[i].Length];
            }

            // solve will contain the vector solution for the LUP decomposition as we solve
            // for each vector of x.  We will combine the solutions into the double[][] array x.
            double[] solve;

            //Get the LU matrix and P matrix (as an array)
            Tuple<double[][], int[]> results = LUPDecomposition(A);

            double[][] LU = results.Item1;
            int[] P = results.Item2;

            // Solve AX = e for each column ei of the identity matrix using LUP decomposition
            for (int i = 0; i < n; i++)
            {
                e = new double[A[i].Length];
                e[i] = 1;
                solve = LUPSolve(LU, P, e);
                for (int j = 0; j < solve.Length; j++)
                {
                    x[j][i] = solve[j];
                }
            }
            return x;
        }


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

                // // http://www.math.cornell.edu/~mec/Winter2009/RalucaRemus/Lecture3/lecture3.html
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
                sum += Math.Pow(A[i], 2);
                //sum += (A[i] * A[i]);
            }

            float distance = (float) Math.Sqrt(sum);
            

            float[] vec = new float[A.Length];
            for(int i = 0; i < A.Length; ++i)
            {
                vec[i] += A[i]/distance;
            }

            return vec;
        }


        // http://en.wikipedia.org/wiki/Power_iteration
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
                pagerank = MultiplyVectorMatrix(matrix, vector, level);
                // pagerank = Normalize(pagerank);

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
