using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
namespace GnssLibCALC
{
    public class DOPCalulator
    {
        public static void CalculateDOP(double[] receiverXYZ, List<double[]> satellitePositions,
            out double PDOP, out double HDOP, out double VDOP)
        {
            int numSatellites = satellitePositions.Count;
            double[,] matrixA = new double[numSatellites, 4];
            for (int i = 0; i < numSatellites; i++)
            {
                double[] unitVector = CalculateUnitVector(receiverXYZ, satellitePositions[i]);
                for (int j = 0; j < 3; j++)
                {
                    matrixA[i, j] = unitVector[j];
                }
                matrixA[i, 3] = 1; // Last element of each row is always 1
            }
            Matrix<double> A = DenseMatrix.OfArray(matrixA);
            Matrix<double> returnMatrix = (A.Transpose() * A).Inverse();


            double t = returnMatrix.At(0, 0) + returnMatrix.At(1, 1) + returnMatrix.At(2, 2);
            double t2 = returnMatrix.At(1, 1) + returnMatrix.At(0, 0);
            HDOP = Math.Round(Math.Sqrt(t2), 1);
            PDOP = Math.Round(Math.Sqrt(t), 1);
            VDOP = Math.Round(Math.Sqrt(returnMatrix.At(2, 2)), 1);
        }


        // Function to calculate distance between two points in ECEF coordinates
        private static double CalculateDistance(double[] pos1, double[] pos2)
        {
            double dx = pos2[0] - pos1[0];
            double dy = pos2[1] - pos1[1];
            double dz = pos2[2] - pos1[2];
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
        // Function to calculate unit vector from receiver to satellite
        private static double[] CalculateUnitVector(double[] receiverPos, double[] satellitePos)
        {
            double distance = CalculateDistance(receiverPos, satellitePos);
            double[] unitVector = new double[3];
            for (int i = 0; i < 3; i++)
            {
                unitVector[i] = (satellitePos[i] - receiverPos[i]) / distance;
            }
            return unitVector;
        }
    }


}

