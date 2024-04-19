using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
namespace GnssLibCALC
{
    public class DOPCalulator
    {

        /// <summary>
        /// Calculates VDOP, HDOP, PDOP based on sateliites in satellitePositions and receivers location of XYZ. 
        /// Formulas can be found on page X in the thesis report. 
        /// </summary>
        /// <param name="receiverXYZ">Reciver's position in (X, Y, Z).</param>
        /// <param name="satellitePositions">A list of visible satelliteposition coordinates in (X, Y, Z).</param>
        /// <param name="PDOP">[out] Position Dilution of precision.</param>
        /// <param name="HDOP">[out] Horizontal Dilution of precision.</param>
        /// <param name="VDOP">[out] Vertical Dilution of precision.</param>
        public static void CalculateDOP(double[] receiverXYZ, List<double[]> satellitePositions,
            out double PDOP, out double HDOP, out double VDOP)
        {
            int numberOfSatellites = satellitePositions.Count;
            double[,] matrix = new double[numberOfSatellites, 4];
            for (int i = 0; i < numberOfSatellites; i++)
            {
                double[] unitVector = CalculateUnitVector(receiverXYZ, satellitePositions[i]);
                for (int j = 0; j < 3; j++)
                {
                    matrix[i, j] = unitVector[j];
                }
                matrix[i, 3] = 1; // Last element of each row is always 1
            }
            
            Matrix<double> A = DenseMatrix.OfArray(matrix);
            Matrix<double> Q = (A.Transpose() * A).Inverse(); 

            //retrive values from matrix to calculate DOP-Values
            double diagonalSum3 = Q.At(0, 0) + Q.At(1, 1) + Q.At(2, 2);
            double diagonalSum2 = Q.At(1, 1) + Q.At(0, 0);
            
            HDOP = Math.Round(Math.Sqrt(diagonalSum2), 1);
            PDOP = Math.Round(Math.Sqrt(diagonalSum3), 1);
            VDOP = Math.Round(Math.Sqrt(Q.At(2, 2)), 1);
        }

        /// <summary>
        /// Calculates distance between two points in ECEF coordinates.
        /// </summary>
        public static double CalculateDistance(double[] fromPosition, double[] toPosition)
        {
            double dx = toPosition[0] - fromPosition[0];
            double dy = toPosition[1] - fromPosition[1];
            double dz = toPosition[2] - fromPosition[2];
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// calculates the unitvector between the recivers position and the satellites position.
        /// </summary>
        private static double[] CalculateUnitVector(double[] receiverPosition, double[] satellitePosition)
        {
            double distance = CalculateDistance(receiverPosition, satellitePosition);
            double[] unitVector = new double[3];
            for (int i = 0; i < 3; i++)
            {
                unitVector[i] = (satellitePosition[i] - receiverPosition[i]) / distance;
            }
            return unitVector;
        }
    }
}

