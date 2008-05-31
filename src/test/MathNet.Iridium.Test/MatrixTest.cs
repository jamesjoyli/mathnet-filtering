#region Math.NET Iridium (LGPL) by Ruegg, Whitefoot
// Math.NET Iridium, part of the Math.NET Project
// http://mathnet.opensourcedotnet.info
//
// Copyright (c) 2002-2008, Christoph R�egg, http://christoph.ruegg.name
//                          Kevin Whitefoot, kwhitefoot@hotmail.com
//						
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published 
// by the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public 
// License along with this program; if not, write to the Free Software
// Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

using NUnit.Framework;

using MathNet.Numerics.LinearAlgebra;

namespace Iridium.Test
{
    [TestFixture]
    public class MatrixTest
    {
        Matrix ma3x2, mb3x2, mc2x2, md2x4;

        [SetUp]
        public void TestMatrix_Setup()
        {
            /*
            MATLAB:
            ma3x2 = [1 -2;-1 4;5 7]
            mb3x2 = [10 2.5;-3 -1.5;19 -6]
            mc2x2 = [1 2;3 4]
            md2x4 = [1 2 -3 12;3 3.1 4 2]
            */

            ma3x2 = new Matrix(new double[][] {
                new double[] { 1, -2 },
                new double[] { -1, 4 },
                new double[] { 5, 7 }});
            mb3x2 = new Matrix(new double[][] {
                new double[] { 10, 2.5 },
                new double[] { -3, -1.5 },
                new double[] { 19, -6 }});
            mc2x2 = new Matrix(new double[][] {
                new double[] { 1, 2 },
                new double[] { 3, 4 }});
            md2x4 = new Matrix(new double[][] {
                new double[] { 1, 2, -3, 12 },
                new double[] { 3, 3.1, 4, 2 }});
        }

        [Test]
        public void TestMatrix_Create()
        {
            double[][] a = { new double[] { 1, 2 }, new double[] { 2, 3 } };
            Matrix ma = Matrix.Create(a);
            double[][] b = { new double[] { 1.0, 2.0 }, new double[] { 2.0, 3.0 } };
            Matrix mb = Matrix.Create(a);
            Assert.IsTrue(ma.Equals(ma), "Matrices should be equal");
        }

        [Test]
        public void TestMatrix_AdditiveTranspose()
        {
            /*
            MATLAB:
            sum = ma3x2 + mb3x2
            diff = ma3x2 - mb3x2
            neg_m = -ma3x2
            trans_m = ma3x2'
            */

            Matrix sum = new Matrix(new double[][] {
                new double[] { 11, 0.5 },
                new double[] { -4, 2.5 },
                new double[] { 24, 1 }});
            NumericAssert.AreAlmostEqual(sum, ma3x2 + mb3x2, "sum 1");
            Matrix sum_inplace = ma3x2.Clone();
            sum_inplace.Add(mb3x2);
            NumericAssert.AreAlmostEqual(sum, sum_inplace, "sum 2");

            Matrix diff = new Matrix(new double[][] {
                new double[] { -9, -4.5 },
                new double[] { 2, 5.5 },
                new double[] { -14, 13 }});
            NumericAssert.AreAlmostEqual(diff, ma3x2 - mb3x2, "diff 1");
            Matrix diff_inplace = ma3x2.Clone();
            diff_inplace.Subtract(mb3x2);
            NumericAssert.AreAlmostEqual(diff, diff_inplace, "diff 2");

            Matrix neg_m = new Matrix(new double[][] {
                new double[] { -1, 2 },
                new double[] { 1, -4 },
                new double[] { -5, -7 }});
            NumericAssert.AreAlmostEqual(neg_m, -ma3x2, "neg 1");

            Matrix trans_m = new Matrix(new double[][] {
                new double[] { 1, -1, 5 },
                new double[] { -2, 4, 7 }});
            NumericAssert.AreAlmostEqual(trans_m, Matrix.Transpose(ma3x2), "trans 1");
            Matrix trans_inplace = ma3x2.Clone();
            trans_inplace.Transpose();
            NumericAssert.AreAlmostEqual(trans_m, trans_inplace, "trans 2");
        }

        //[Test]
        //public void TestMatrix_Multiplicative()
        //{
        //    // TODO
        //}


        [Test]
        public void TestMatrix_Multiply()
        {
            double[][] a = { new double[] { 1, 2 }, new double[] { 3, 5 } };
            Matrix ma = Matrix.Create(a);
            double[][] b = { new double[] { 7, 11 }, new double[] { 13, 17 } };
            Matrix mb = Matrix.Create(b);
            double[][] r = { new double[] { 33, 45 }, new double[] { 86, 118 } };
            Matrix mr = Matrix.Create(r);
            //Console.WriteLine("a");
            //Console.WriteLine(ma.ToString());
            //Console.WriteLine("b");
            //Console.WriteLine(mb.ToString());
            Matrix mp = ma * mb;
            //Console.WriteLine("product");
            //Console.WriteLine(mp.ToString());
            //Console.WriteLine("expected product");
            //Console.WriteLine(mr.ToString());

            Assert.AreEqual(mp.ToString(), mr.ToString(), "Matrices should be equal");
        }


        [Test]
        public void TestMatrix_Solve()
        {
            double[][] a = { new double[] { 1, 2 }, new double[] { 3, 5 } };
            Matrix ma = Matrix.Create(a);
            double[][] b = { new double[] { 29.0 }, new double[] { 76.0 } };
            Matrix mb = Matrix.Create(b);
            double[][] r = { new double[] { 7 }, new double[] { 11.0 } };
            Matrix mr = Matrix.Create(r);
            //Console.WriteLine("a");
            //Console.WriteLine(ma.ToString());
            //Console.WriteLine("b");
            //Console.WriteLine(mb.ToString());
            Matrix mx = null;
            MyStopwatch.MethodToTime m = delegate
            {
                mx = ma.Solve(mb);
            };
            Console.Write("Solve Time (ms): ");
            MyStopwatch.Time(m);

            //Console.WriteLine("solution");
            //Console.WriteLine(mx.ToString());
            //Console.WriteLine("expected solution");
            //Console.WriteLine(mr.ToString());

            Assert.AreEqual(mx.ToString(), mr.ToString(), "Matrices should be equal");

            //Check by multiplying a by x
            Matrix mc = ma * mx;
            Assert.AreEqual(mc.ToString(), mb.ToString(), "Matrices should be equal");
        }

        [Test]
        public void TestMatrix_SolveA()
        {
            TestMatrix(
              new double[][] { new double[]{ 1, 2 }, 
                        new double[]{ 3, 5 } },
              new double[][] { new double[]{ 7 }, 
                        new double[]{ 11.0 } },
              1e-13, false);
        }

        [Test]
        public void TestMatrix_SolveB()
        {
            TestMatrix(
              new double[][] { new double[] { 1,  2,  3}, 
                        new double[]{ 5,  7, 11 },
                        new double[]{13, 17, 19}},
              new double[][] { new double[]{ 23 }, 
                        new double[]{ 29 }, 
                        new double[]{ 31 }},
              1e-13, false);
        }


        [Test]
        public void TestMatrix_Solve010()
        {
            TestMatrix_NxN(10, 1e-12, false);
        }

        [Test]
        public void TestMatrix_Solve020()
        {
            TestMatrix_NxN(20, 1e-12, false);
        }
        [Test]
        public void TestMatrix_Solve040()
        {
            TestMatrix_NxN(40, 1e-9, false);
        }

        [Test]
        public void TestMatrix_Solve060()
        {
            TestMatrix_NxN(60, 1e-9, false);
        }

        [Test]
        public void TestMatrix_Solve080()
        {
            TestMatrix_NxN(80, 1e-9, false);
        }

        [Test]
        public void TestMatrix_Solve100()
        {
            TestMatrix_NxN(100, 1e-9, false);
        }

        [Test]
        public void TestMatrix_Solve110()
        {
            TestMatrix_NxN(110, 1e-9, false);
        }

        [Test]
        public void TestMatrix_Solve120()
        {
            TestMatrix_NxN(120, 1e-9, false);
        }

        [Test]
        public void TestMatrix_Solve150()
        {
            TestMatrix_NxN(150, 1e-9, false);
        }

        [Test]
        public void TestMatrix_Solve200()
        {
            TestMatrix_NxN(200, 1e-9, false);
        }

        [Test]
        public void TestMatrix_Solve330()
        {
            TestMatrix_NxN(330, 1e-9, false);
        }

        //[Test]
        //public void TestMatrix_Solve1000()
        //{
        //    TestMatrix_NxN(1000, 1e-9, false);
        //}

        //[Test]
        //public void TestMatrix_Solve4000()
        //{
        //    TestMatrix_NxN(4000, 1e-9, false);
        //}

        private void TestMatrix_NxN(int n, double epsilon, bool show)
        {
            Random r = new Random();
            double[][] a = Matrix.CreateMatrixData(n, n);
            double[][] x = Matrix.CreateMatrixData(n, 1);
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    if(j == 0)
                    {
                        x[i][ 0] = r.NextDouble();
                    }
                    a[i][ j] = r.NextDouble();
                }
            }
            TestMatrix(a, x, epsilon, show);
        }


        //calculate right hand side and then solve for x, show all matrices on console out.
        private void TestMatrix(double[][] a, double[][] x, double epsilon, bool show)
        {
            Matrix ma = Matrix.Create(a);
            Matrix mx = Matrix.Create(x);
            if(show)
            {
                Console.WriteLine("a");
                Console.WriteLine(ma.ToString());
                Console.WriteLine("x");
                Console.WriteLine(mx.ToString());
            }
            Matrix ms = TestMatrix_Solutions(ma, mx, epsilon, show);
            if(show)
            {
                Console.WriteLine("solution");
                Console.WriteLine(ms.ToString());
                Console.WriteLine("expected solution");
                Console.WriteLine(mx.ToString());
            }
        }

        /*Test a given solution by calculating b and then solving for x.
        Shows only the elapsed time on console out so that we can use 
        matrices too large to print.*/
        private Matrix TestMatrix_Solutions(Matrix ma, Matrix mx, double epsilon, bool showB)
        {
            Matrix mb = ma * mx;
            if(showB)
            {
                Console.WriteLine("b");
                Console.WriteLine(mb.ToString());
            }
            Matrix ms = null;
            MyStopwatch.MethodToTime m = delegate
            {
                ms = ma.Solve(mb);
            };
            Console.Write("Solve Time (ms) for " + ma.ColumnCount + ": ");
            MyStopwatch.Time(m);

            Assert.IsTrue(CompareMatrices(ms, mx, epsilon), "Matrices should be equal");
            //Assert.AreEqual(ms.ToString(), mx.ToString(), "Matrices should be equal");

            return ms;
        }

        private bool CompareMatrices(Matrix a, Matrix b, double epsilon)
        {
            Matrix c = a - b;
            for(int i = 0; i < c.RowCount; i++)
            {
                for(int j = 0; j < c.ColumnCount; j++)
                {
                    if(epsilon < Math.Abs(c[i, j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }



    }
}
