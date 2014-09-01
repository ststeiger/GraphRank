using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace RankingAlgorithms
{


    class HITS
    {
        /// <summary>
        /// Nhân ma trân với ma trận chuyển vị của chính nó. Kết quả là một ma trận đối xứng qua đường chéo chính
        /// Do đó, chỉ tính nửa trên ma trận và đồng thời phép nhân chính là phép nhân trực tiếp giữa các dòng trong ma trận đó
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="levelMatrix"></param>
        /// <returns></returns>
        private static float[,] NhanMaTranChuyenVi(float[,] matrix, int levelMatrix)
        {
            float[,] result = new float[levelMatrix, levelMatrix];
            float temp;
            for (int i = 0; i < levelMatrix; i++)
                for (int j = i; j < levelMatrix; j++)
                {
                    temp = 0f;
                    for (int k = 0; k < levelMatrix; k++)
                        temp += matrix[i, k] * matrix[j, k];
                    result[i, j] = temp;
                    result[j, i] = temp;
                }

            return result;
        }
        
        /// <summary>
        /// Chuyển vị ma trận
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="levelMatrix"></param>
        /// <returns></returns>
        private static float[,] ChuyenViMatran(float[,] matrix, int levelMatrix)
        {
            float[,] matran_chuyenvi = new float[levelMatrix, levelMatrix];
            for (int i = 0; i < levelMatrix; i++)
                for (int j = 0; j < levelMatrix; j++)
                    matran_chuyenvi[i, j] = matrix[j, i];

            return matran_chuyenvi;
        }

        /// <summary>
        /// Nhân ma trận - vector. Dòng ma trận nhân với vector
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="levelMatrix"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        private static float[] NhanMatranVector(float[,] matrix, int levelMatrix, float[] vector)
        {
            float[] result = new float[levelMatrix];
            float temp;

            for (int i = 0; i < levelMatrix; i++)
            {
                temp = 0f;
                for (int k = 0; k < levelMatrix; k++)
                    temp += matrix[i, k] * vector[k];
                result[i] = temp;
            }

            return result;
        }

        /// <summary>
        /// Kiểm tra sự giống nhau giữa 2 vector dựa trên sự sai số (sau dấu chấm phẩy)
        /// </summary>
        /// <param name="vectorA"></param>
        /// <param name="vectorB"></param>
        /// <param name="levelVector"></param>
        /// <param name="SaiSo"></param>
        /// <returns></returns>
        private static bool IsSameVector(float[] vectorA, float[] vectorB, int levelVector, int SaiSo)
        {
            for (int i = 0; i < levelVector; i++)
                if ((float)Math.Round(vectorA[i], SaiSo) != (float)Math.Round(vectorB[i], SaiSo))
                    return false;
            return true;
        }

        private static float[] ChuanHoaVector(float[] vectorA, int levelVector, int SaiSo)
        {
            float[] result = new float[levelVector];
            float temp = 0f;

            foreach (float value in vectorA)
                temp += value;

            for (int i = 0; i < levelVector; i++)
                result[i] = (float)Math.Round(vectorA[i] / temp, SaiSo);

            return result;
        }

        /// <summary>
        ///  Tính hub score
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="levelMatrix"></param>
        /// <param name="Saiso"></param>
        /// <param name="listHub"></param>
        public static void TinhHubScore(float[,] matrix, int levelMatrix, int Saiso, out List<float> listHub)
        {
            listHub = new List<float>();

            matrix = NhanMaTranChuyenVi(matrix, levelMatrix);

            int countLoop = 20;
            bool flag = false;
            float temp = (float)Math.Round(1.0f / levelMatrix, Saiso);

            for (int i = 0; i < levelMatrix; i++)
                listHub.Add(temp);

            while (countLoop > 0 && flag == false)
            {
                float[] vector = NhanMatranVector(matrix, levelMatrix, listHub.ToArray());
                vector = ChuanHoaVector(vector, levelMatrix, Saiso);
                flag = IsSameVector(vector, listHub.ToArray(), levelMatrix, Saiso);

                for (int i = 0; i < levelMatrix; i++)
                    listHub[i] = vector[i];

                countLoop--;
            }
        }

        public static void TinhHubAuthorityScore(float[,] matrix,float[,] matrix_save, int levelMatrix, int Saiso, out List<float> listHub,out List<float> listAuthority)
        {
            listHub = new List<float>();

            matrix = NhanMaTranChuyenVi(matrix, levelMatrix);

            int countLoop = 20;
            bool flag = false;
            float temp = (float)Math.Round(1.0f / levelMatrix, Saiso);

            for (int i = 0; i < levelMatrix; i++)
                listHub.Add(temp);

            while (countLoop > 0 && flag == false)
            {
                float[] vector = NhanMatranVector(matrix, levelMatrix, listHub.ToArray());
                vector = ChuanHoaVector(vector, levelMatrix, Saiso);
                flag = IsSameVector(vector, listHub.ToArray(), levelMatrix, Saiso);

                for (int i = 0; i < levelMatrix; i++)
                    listHub[i] = vector[i];

                countLoop--;
            }

            listAuthority = new List<float>();            
            float[] vectorTemp = NhanMatranVector(ChuyenViMatran(matrix_save, levelMatrix), levelMatrix, listHub.ToArray());
            listAuthority.AddRange(ChuanHoaVector(vectorTemp, levelMatrix, Saiso));
        }


    }


}
