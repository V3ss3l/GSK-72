using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSK_72
{
    public class FigureClass
    {
        private List<CustomPoint> pointsOfFigure = new List<CustomPoint>();
        Pen DrawPen;
        Graphics g;

        public FigureClass(List<CustomPoint> Points, Pen drawPen, Graphics g)
        {
            pointsOfFigure = Points;
            DrawPen = drawPen;
            this.g = g;
        }

        public List<CustomPoint> GetPoints() => pointsOfFigure;
        public void SetPoints(List<CustomPoint> buff) => pointsOfFigure = buff;

        public void FillFigure(int Yemax)
        {
            int[] arrY = SearchYMinAndMax(Yemax);
            List<int> Xb = new List<int>();
            for (int Y = arrY[0]; Y < arrY[1]; Y++)
            {
                Xb.Clear();
                int k;
                for (int j = 0; j < pointsOfFigure.Count - 1; j++)
                {
                    k = j < pointsOfFigure.Count ? j + 1 : 1;
                    Xb = AddXPoints(j, k, Y, Xb);
                }

                Xb = AddXPoints(pointsOfFigure.Count - 1, 0, Y, Xb);
                Xb.Sort();
                for (int X = 0; X < Xb.Count; X += 2)
                {
                    g.DrawLine(DrawPen, new Point(Xb[X], Y), new Point(Xb[X + 1], Y));
                }
            }
        }

        // Поиск мин/макс Y
        public int[] SearchYMinAndMax(int Yemax)
        {
            int Ymax = pointsOfFigure[0].Y;
            int Ymin = pointsOfFigure[0].Y;
            int buff = 0;
            for (int i = 0; i < pointsOfFigure.Count; i++)
            {
                Ymin = pointsOfFigure[i].Y < Ymin ? pointsOfFigure[i].Y : Ymin;
                if (pointsOfFigure[i].Y > Ymax)
                {
                    Ymax = pointsOfFigure[i].Y;
                    buff = i;
                }
            }

            Ymin = Math.Max(Ymin, 0);
            Ymax = Math.Min(Ymax, Yemax);
            return new[] { Ymin, Ymax, buff };
        }

        // метод отвечающий за подсчет X (для метода FillInside)
        private List<int> AddXPoints(int fIndex, int sIndex, int Y, List<int> Xbuff)
        {
            if (Check(fIndex, sIndex, Y))
            {
                var x = -((Y * (pointsOfFigure[fIndex].X - pointsOfFigure[sIndex].X)) - pointsOfFigure[fIndex].X * pointsOfFigure[sIndex].Y +
                          pointsOfFigure[sIndex].X * pointsOfFigure[fIndex].Y)
                        / (pointsOfFigure[sIndex].Y - pointsOfFigure[fIndex].Y);
                Xbuff.Add(x);
            }

            return Xbuff;
        }

        //  Условие пересечения
        private bool Check(int i, int k, int Y) => (pointsOfFigure[i].Y < Y && pointsOfFigure[k].Y >= Y)
            || (pointsOfFigure[i].Y >= Y && pointsOfFigure[k].Y < Y);

        // подсчет левого и правого сегмента X
        public List<List<int>> CalculateXlAndXr(int Y, int k)
        {
            List<List<int>> list = new List<List<int>>();
            List<int> Xl = new List<int>();
            List<int> Xr = new List<int>();
            for (var i = 0; i < pointsOfFigure.Count - 1; i++)
            {
                k = i < pointsOfFigure.Count ? i + 1 : 1;
                if (Check(i, k, Y))
                {
                    var x = -((Y * (pointsOfFigure[i].X - pointsOfFigure[k].X))
                                - pointsOfFigure[i].X * pointsOfFigure[k].Y + pointsOfFigure[k].X * pointsOfFigure[i].Y)
                            / (pointsOfFigure[k].Y - pointsOfFigure[i].Y);
                    if (pointsOfFigure[k].Y - pointsOfFigure[i].Y > 0)
                        Xr.Add(x);
                    else
                        Xl.Add(x);
                }
            }

            if (Check(k, 0, Y))
            {
                var x = -((Y * (pointsOfFigure[k].X - pointsOfFigure[0].X))
                            - pointsOfFigure[k].X * pointsOfFigure[0].Y + pointsOfFigure[0].X * pointsOfFigure[k].Y)
                        / (pointsOfFigure[0].Y - pointsOfFigure[k].Y);
                if (pointsOfFigure[0].Y - pointsOfFigure[k].Y > 0)
                    Xr.Add(x);
                else
                    Xl.Add(x);
            }

            list.Add(Xl);
            list.Add(Xr);
            return list;
        }

        /// <summary>
        /// Отрисовка кубического сплайна
        /// </summary>
        public void CreateCubeSpline()
        {
            PointF[] L = new PointF[4]; // Матрица вещественных коэффициентов
            Point Pv1 = pointsOfFigure[0].ToPoint();
            Point Pv2 = pointsOfFigure[0].ToPoint();
            const double dt = 0.04;
            double t = 0;
            double xt, yt;
            Point Ppred = pointsOfFigure[0].ToPoint(), Pt = pointsOfFigure[0].ToPoint();

            // Касательные векторы
            Pv1.X = 4 * (pointsOfFigure[1].X - pointsOfFigure[0].X);
            Pv1.Y = 4 * (pointsOfFigure[1].Y - pointsOfFigure[0].Y);
            Pv2.X = 4 * (pointsOfFigure[3].X - pointsOfFigure[2].X);
            Pv2.Y = 4 * (pointsOfFigure[3].Y - pointsOfFigure[2].Y);

            // Коэффициенты полинома
            L[0].X = 2 * pointsOfFigure[0].X - 2 * pointsOfFigure[2].X + Pv1.X + Pv2.X;  // Ax
            L[0].Y = 2 * pointsOfFigure[0].Y - 2 * pointsOfFigure[2].Y + Pv1.Y + Pv2.Y;  // Ay
            L[1].X = -3 * pointsOfFigure[0].X + 3 * pointsOfFigure[2].X - 2 * Pv1.X - Pv2.X; // Bx
            L[1].Y = -3 * pointsOfFigure[0].Y + 3 * pointsOfFigure[2].Y - 2 * Pv1.Y - Pv2.Y; // By
            L[2].X = Pv1.X; // Cx
            L[2].Y = Pv1.Y; // Cy
            L[3].X = pointsOfFigure[0].X; // Dx
            L[3].Y = pointsOfFigure[0].Y; // Dy
                
            while (t < 1 + dt / 2)
            {
                xt = ((L[0].X * t + L[1].X) * t + L[2].X) * t + L[3].X; yt = ((L[0].Y * t + L[1].Y) * t + L[2].Y) * t + L[3].Y;
                Pt.X = (int)Math.Round(xt);
                Pt.Y = (int)Math.Round(yt);
                g.DrawLine(DrawPen, Ppred, Pt);
                Ppred = Pt;
                t = t + dt;
            }
        }

        public void Zoom(float[] zoom, MouseEventArgs eventMouse)
        {
            if (zoom[0] <= 0) zoom[0] = -0.1f;
            if (zoom[1] <= 0) zoom[1] = -0.1f;
            if (zoom[0] >= 0) zoom[0] = 0.1f;
            if (zoom[1] >= 0) zoom[1] = 0.1f;

            var sx = 1 + zoom[0];
            var sy = 1 + zoom[1];
            float[,] matrix =
            {
                {sx, 0, 0},
                {0, sy, 0},
                {0, 0, 1}
            };

            var e = new Point(eventMouse.X, eventMouse.Y);

            float[,] toCenter =
            {
                {1, 0, 0},
                {0, 1, 0},
                {-e.X, -e.Y, 1}
            };
            for (var i = 0; i < pointsOfFigure.Count; i++)
                pointsOfFigure[i] = Matrix_1x3_3x3(pointsOfFigure[i], toCenter);

            for (var i = 0; i < pointsOfFigure.Count; i++)
                pointsOfFigure[i] = Matrix_1x3_3x3(pointsOfFigure[i], matrix);

            float[,] fromCenter =
            {
                {1, 0, 0},
                {0, 1, 0},
                {e.X, e.Y, 1}
            };
            for (var i = 0; i < pointsOfFigure.Count; i++)
                pointsOfFigure[i] = Matrix_1x3_3x3(pointsOfFigure[i], fromCenter);
        }

        public void Mirror(char ch, MouseEventArgs eventMouse, List<CustomPoint> line)
        {
            switch (ch)
            {
                case 'c':
                    {
                        var matrix = new float[,]
                        {
                            {-1, 0, 0},
                            {0, -1, 0},
                            {0, 0, 1}
                        };

                        var e = new CustomPoint(eventMouse.X, eventMouse.Y);

                        float[,] toCenter =
                        {
                            {1, 0, 0},
                            {0, 1, 0},
                            {-e.X, -e.Y, 1}
                        };

                        for (var i = 0; i < pointsOfFigure.Count; i++)
                            pointsOfFigure[i] = Matrix_1x3_3x3(pointsOfFigure[i], toCenter);

                        for (var i = 0; i < pointsOfFigure.Count; i++)
                            pointsOfFigure[i] = Matrix_1x3_3x3(pointsOfFigure[i], matrix);

                        float[,] fromCenter =
                                    {
                            {1, 0, 0},
                            {0, 1, 0},
                            {e.X, e.Y, 1}
                        };

                        for (var i = 0; i < pointsOfFigure.Count; i++)
                            pointsOfFigure[i] = Matrix_1x3_3x3(pointsOfFigure[i], fromCenter);

                        break;
                    }
                case 'p':
                    {
                        float Dx = line[1].X - line[0].X;
                        float Dy = line[1].Y - line[0].Y;
                        float length = (float)Math.Sqrt(Math.Pow(Dx, 2) + Math.Pow(Dy, 2));
                        float Sn = Dy / length;
                        float Cs = Dx / length;

                        var r = new float[,]
                        {
                        {Cs, -Sn, 0},
                        {Sn, Cs, 0},
                        {0, 0, 1}
                        };

                        float[,] m =
                        {
                            {1, 0, 0},
                            {0, 1, 0},
                            {-line[0].X, -line[0].Y, 1}
                        };

                        for (var i = 0; i < pointsOfFigure.Count; i++)
                            pointsOfFigure[i] = Matrix_1x3_3x3(pointsOfFigure[i], m);

                        for (var i = 0; i < pointsOfFigure.Count; i++)
                            pointsOfFigure[i] = Matrix_1x3_3x3(pointsOfFigure[i], r);

                        float[,] s =
                        {
                            {1, 0, 0},
                            {0, -1, 0},
                            {0, 0, 1}
                        };

                        for (var i = 0; i < pointsOfFigure.Count; i++)
                            pointsOfFigure[i] = Matrix_1x3_3x3(pointsOfFigure[i], s);

                        float[,] r_1 =
                        {
                            {Cs, Sn, 0 },
                            {-Sn, Cs, 0 },
                            {0, 0, 1 }
                        };

                        for (var i = 0; i < pointsOfFigure.Count; i++)
                            pointsOfFigure[i] = Matrix_1x3_3x3(pointsOfFigure[i], r_1);

                        float[,] m_1 =
                        {
                            {1, 0, 0 },
                            {0, 1, 0 },
                            { line[0].X, line[0].Y, 1 }
                        };

                        for (var i = 0; i < pointsOfFigure.Count; i++)
                            pointsOfFigure[i] = Matrix_1x3_3x3(pointsOfFigure[i], m_1);
                        break;
                    }
            }
        }
        private static CustomPoint Matrix_1x3_3x3(CustomPoint point, float[,] matrix3X3) => new CustomPoint
        {
            X = (int)(point.X * matrix3X3[0, 0] + point.Y * matrix3X3[1, 0] + point.C * matrix3X3[2, 0]),
            Y = (int)(point.X * matrix3X3[0, 1] + point.Y * matrix3X3[1, 1] + point.C * matrix3X3[2, 1]),
            C = (int)(point.X * matrix3X3[0, 2] + point.Y * matrix3X3[1, 2] + point.C * matrix3X3[2, 2])
        };
    }


}
