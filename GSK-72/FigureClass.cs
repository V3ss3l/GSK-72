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
    }
}
