using MultiThreading.Task3.MatrixMultiplier.Matrices;
using System.Threading.Tasks;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public class MatricesMultiplierParallel : IMatricesMultiplier
    {
        public IMatrix Multiply(IMatrix m1, IMatrix m2)
        {
            var resultMatrix = new Matrix(m1.RowCount, m2.ColCount);

            Parallel.For(0, (int)m1.RowCount, delegate (int i) 
            {
                Parallel.For(0, (int)m2.ColCount, delegate (int j) 
                {
                    long sum = 0;

                    Parallel.For(0, (int)m1.ColCount, delegate (int k) {
                        sum += m1.GetElement(i, k) * m2.GetElement(k, j);
                    });

                    resultMatrix.SetElement(i, j, sum);
                });
            });

            return resultMatrix;
        }
    }
}
