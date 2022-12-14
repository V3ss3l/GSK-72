namespace GSK_72
{
    public partial class Form1 : Form
    {
        private enum FigureEnum
        {
            Triangle,
            Box,
            Star,
            Ugl13
        }
        private readonly Graphics g;
        private readonly Bitmap bitmap;
        FigureEnum figureSelected;
        Pen DrawPen = new Pen(Color.Black, 1);
        private List<CustomPoint> Points = new List<CustomPoint>();
        List<FigureClass> Figures = new List<FigureClass>();
        int[] setQ = new int[2];
        int AngleCount = 0;
        bool isSplineChecked = false;
        bool isFigureFormed = false;
        int GeomAction = 0;
        int Yemax = 0;
        int Xemin = 0;
        int Xemax = 0;
        int CountPoints = 0;

        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bitmap);
            Yemax = pictureBox1.Height;
            Xemax = pictureBox1.Width;
            MouseWheel += GeomTrans;
        }

        /// <summary>
        /// Метод, закрашивающий фигуру, а также добавляет фигуру в список фигур
        /// </summary>
        private void MakeFill()
        {
            FigureClass figure = new FigureClass(Points.ToList(), DrawPen, g);
            figure.FillFigure(Yemax);
            Figures.Add(figure);
            pictureBox1.Image = bitmap;
            Points.Clear();
        }

        /// <summary>
        /// Метод, выполняющий Теоретико Множественный операции
        /// </summary>
        private void TMO()
        {
            int[] arrY_1 = Figures[0].SearchYMinAndMax(Yemax);
            int[] arrY_2 = Figures[1].SearchYMinAndMax(Yemax);
            int yMin = arrY_1[0] < arrY_2[0] ? arrY_1[0] : arrY_2[0];
            int yMax = arrY_1[1] > arrY_2[1] ? arrY_1[1] : arrY_2[1];
            List<int> Xal = new List<int>();
            List<int> Xar = new List<int>();
            List<int> Xbl = new List<int>();
            List<int> Xbr = new List<int>();
            for (int Y = yMin; Y < yMax; Y++)
            {
                var A = Figures[0].CalculateXlAndXr(Y, 0);
                Xal = A[0];
                Xar = A[1];
                var B = Figures[1].CalculateXlAndXr(Y, 0);
                Xbl = B[0];
                Xbr = B[1];

                var arrM = new M[Xal.Count * 2 + Xbl.Count * 2];
                var n = Xal.Count;
                for (int i = 0; i < n; i++)
                    arrM[i] = new M { x = Xal[i], dQ = 2 };

                var nM = n;
                n = Xar.Count;
                for (int i = 0; i < n; i++)
                    arrM[nM + i] = new M { x = Xar[i], dQ = -2 };

                nM += n;
                n = Xbl.Count;
                for (int i = 0; i < n; i++)
                    arrM[nM + i] = new M { x = Xbl[i], dQ = 1 };

                nM += n;
                n = Xbr.Count;
                for (int i = 0; i < n; i++)
                    arrM[nM + i] = new M { x = Xbr[i], dQ = -1 };

                nM += n;
                arrM = SortArrayM(arrM);

                int Q = 0;
                List<int> Xrr = new List<int>();
                List<int> Xrl = new List<int>();
                if (arrM[0].x >= Xemin && arrM[0].dQ < 0)
                {
                    Xrl.Add(Xemin);
                    Q = -arrM[0].dQ;
                }

                for (int i = 0; i < nM; i++)
                {
                    var x = arrM[i].x;
                    var Qnew = Q + arrM[i].dQ;
                    if (!(setQ[0] <= Q && Q <= setQ[1]) && (setQ[0] <= Qnew && Qnew <= setQ[1]))
                    {
                        Xrl.Add(x);
                    }

                    if ((setQ[0] <= Q && Q <= setQ[1]) && !(setQ[0] <= Qnew && Qnew <= setQ[1]))
                    {
                        Xrr.Add(x);
                    }

                    Q = Qnew;
                }

                if (setQ[0] < Q && Q < setQ[1])
                    Xrr.Add(Xemax);

                for (var i = 0; i < Xrr.Count; i++)
                {
                    g.DrawLine(DrawPen, new Point { X = Xrr[i], Y = Y }, new Point { X = Xrl[i], Y = Y });
                }
            }
        }

        private void GeomTrans(object sender, MouseEventArgs e)
        {
            if (GeomAction == 0)
            {
                g.Clear(Color.White);
                Zoom(new float[] { e.Delta, e.Delta }, e);
                if (isSplineChecked) PreparationsForSpline();
                else MakeFill();
                pictureBox1.Image = bitmap;
            }
            else if (GeomAction == 1)
            {
                g.Clear(Color.White);
                Mirror('c', e, null);
                MakeFill();
                pictureBox1.Image = bitmap;
            }
            else if (GeomAction == 2)
            {
                List<CustomPoint> line = Figures[1].GetPoints().ToList();
                g.Clear(Color.White);
                Mirror('p', e, line);
                MakeFill();
                pictureBox1.Image = bitmap;
            }
        }

        private void Zoom(float[] zoom, MouseEventArgs eventMouse)
        {
            Points = Figures[Figures.Count - 1].GetPoints();
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
            for (var i = 0; i < Points.Count; i++)
                Points[i] = Matrix_1x3_3x3(Points[i], toCenter);

            for (var i = 0; i < Points.Count; i++)
                Points[i] = Matrix_1x3_3x3(Points[i], matrix);

            float[,] fromCenter =
            {
                {1, 0, 0},
                {0, 1, 0},
                {e.X, e.Y, 1}
            };
            for (var i = 0; i < Points.Count; i++)
                Points[i] = Matrix_1x3_3x3(Points[i], fromCenter);
        }

        public void Mirror(char ch, MouseEventArgs eventMouse, List<CustomPoint> line)
        {
            if (Figures.Count == 2) Points = Figures[0].GetPoints();
            else Points = Figures[Figures.Count - 1].GetPoints();
            //var matrix = new float[3, 3];
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

                        for (var i = 0; i < Points.Count; i++)
                            Points[i] = Matrix_1x3_3x3(Points[i], toCenter);

                        for (var i = 0; i < Points.Count; i++)
                            Points[i] = Matrix_1x3_3x3(Points[i], matrix);

                        float[,] fromCenter =
                                    {
                            {1, 0, 0},
                            {0, 1, 0},
                            {e.X, e.Y, 1}
                        };

                        for (var i = 0; i < Points.Count; i++)
                            Points[i] = Matrix_1x3_3x3(Points[i], fromCenter);

                        break;
                    }
                case 'p':
                    {
                        float Dx = line[1].ToPoint().X - line[0].ToPoint().X;
                        float Dy = line[1].ToPoint().Y - line[0].ToPoint().Y;
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

                        for (var i = 0; i < Points.Count; i++)
                            Points[i] = Matrix_1x3_3x3(Points[i], m);

                        for (var i = 0; i < Points.Count; i++)
                            Points[i] = Matrix_1x3_3x3(Points[i], r);

                        float[,] s =
                        {
                            {1, 0, 0},
                            {0, -1, 0},
                            {0, 0, 1}
                        };

                        for (var i = 0; i < Points.Count; i++)
                            Points[i] = Matrix_1x3_3x3(Points[i], s);

                        float[,] r_1 =
                        {
                            {Cs, Sn, 0 },
                            {-Sn, Cs, 0 },
                            {0, 0, 1 }
                        };

                        for (var i = 0; i < Points.Count; i++)
                            Points[i] = Matrix_1x3_3x3(Points[i], r_1);

                        float[,] m_1 =
                        {
                            {1, 0, 0 },
                            {0, 1, 0 },
                            { line[1].X, line[1].Y, 1 }
                        };

                        for (var i = 0; i < Points.Count; i++)
                            Points[i] = Matrix_1x3_3x3(Points[i], m_1);
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

        /// <summary>
        /// Метод, сортирующий массив М
        /// </summary>
        /// <param name="arrM"></param>
        /// <returns></returns>
        private M[] SortArrayM(M[] arrM)
        {
            for (var write = 0; write < arrM.Length; write++)
                for (var sort = 0; sort < arrM.Length - 1; sort++)
                    if (arrM[sort].x > arrM[sort + 1].x)
                    {
                        var buff = new M(arrM[sort + 1].x, arrM[sort + 1].dQ);
                        arrM[sort + 1] = arrM[sort];
                        arrM[sort] = buff;
                    }

            return arrM;
        }

        /// <summary>
        /// Создание фигуры "Треугольник"
        /// </summary>
        /// <param name="e"></param>
        private void CreateTriangle(MouseEventArgs e)
        {
            var triangle = new List<CustomPoint>
            {
                new CustomPoint(e.X, e.Y - 200),
                new CustomPoint(e.X + 200, e.Y + 100),
                new CustomPoint(e.X - 200, e.Y + 100)
            };
            Points = triangle;
        }
        /// <summary>
        /// Создание фигуры "Куб"
        /// </summary>
        /// <param name="e"></param>
        private void CreateBox(MouseEventArgs e)
        {
            var box = new List<CustomPoint>
            {
                new CustomPoint(e.X - 150, e.Y - 150),
                new CustomPoint(e.X + 150, e.Y - 150),
                new CustomPoint(e.X + 150, e.Y + 150),
                new CustomPoint(e.X - 150, e.Y + 150),
            };
            Points = box;
        }
        /// <summary>
        /// Создание фигуры "Угол 13"
        /// </summary>
        /// <param name="e"></param>
        public void CreateUgl13(MouseEventArgs e)
        {
            var ug = new List<CustomPoint>
            {
                new CustomPoint(e.X - 100, e.Y - 100),
                new CustomPoint(e.X - 100, e.Y + 100),
                new CustomPoint(e.X + 100, e.Y + 100),
                new CustomPoint(e.X + 30, e.Y + 70),
            };
            Points = ug;
        }
        /// <summary>
        /// Создание фигуры "Звезда"
        /// </summary>
        /// <param name="e"></param>
        private void CreateStar(MouseEventArgs e)
        {
            const double R = 25;
            const double r = 50;
            const double d = 0;
            double a = d, da = Math.PI / AngleCount, l;
            var star = new List<CustomPoint>();
            for (var k = 0; k < 2 * AngleCount + 1; k++)
            {
                l = k % 2 == 0 ? r : R;
                star.Add(new CustomPoint((int)(e.X + l * Math.Cos(a)), (int)(e.Y + l * Math.Sin(a))));
                a += da;
            }

            Points = star;
        }

        /// <summary>
        /// Отрисовка кубического сплайна
        /// </summary>
        public void CreateCubeSpline()
        {
            PointF[] L = new PointF[4]; // Матрица вещественных коэффициентов
            Point Pv1 = Points[0].ToPoint();
            Point Pv2 = Points[0].ToPoint();
            const double dt = 0.04;
            double t = 0;
            double xt, yt;
            Point Ppred = Points[0].ToPoint(), Pt = Points[0].ToPoint();

            // Касательные векторы
            Pv1.X = 4 * (Points[1].X - Points[0].X);
            Pv1.Y = 4 * (Points[1].Y - Points[0].Y);
            Pv2.X = 4 * (Points[3].X - Points[2].X);
            Pv2.Y = 4 * (Points[3].Y - Points[2].Y);

            // Коэффициенты полинома
            L[0].X = 2 * Points[0].X - 2 * Points[2].X + Pv1.X + Pv2.X;  // Ax
            L[0].Y = 2 * Points[0].Y - 2 * Points[2].Y + Pv1.Y + Pv2.Y;  // Ay
            L[1].X = -3 * Points[0].X + 3 * Points[2].X - 2 * Pv1.X - Pv2.X; // Bx
            L[1].Y = -3 * Points[0].Y + 3 * Points[2].Y - 2 * Pv1.Y - Pv2.Y; // By
            L[2].X = Pv1.X; // Cx
            L[2].Y = Pv1.Y; // Cy
            L[3].X = Points[0].X; // Dx
            L[3].Y = Points[0].Y; // Dy

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

        public void PreparationsForSpline()
        {
            switch (CountPoints)
            {
                case 1: // первый вектор
                    {
                        g.DrawLine(DrawPen, Points[0].ToPoint(), Points[1].ToPoint());
                        CountPoints++;
                    }
                    break;
                case 3: // второй вектор
                    {
                        g.DrawLine(DrawPen, Points[2].ToPoint(), Points[3].ToPoint());
                        CreateCubeSpline();
                        FigureClass spline = new FigureClass(Points.ToList(), DrawPen, g);
                        Figures.Add(spline);
                        Points.Clear();
                        CountPoints = 0;
                    }
                    break;
                default:
                    CountPoints++;
                    break;
            }
            pictureBox1.Image = bitmap;
        }

        /// <summary>
        /// Метод, отвечающий за создание выбранной фигуры
        /// </summary>
        /// <param name="e"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void CreateFigure(MouseEventArgs e)
        {
            switch (figureSelected)
            {
                case FigureEnum.Triangle:
                    CreateTriangle(e);
                    break;
                case FigureEnum.Box:
                    CreateBox(e);
                    break;
                case FigureEnum.Star:
                    CreateStar(e);
                    break;
                case FigureEnum.Ugl13:
                    CreateUgl13(e);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void comboBoxFigure_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxFigure.SelectedIndex)
            {
                case 0:
                    figureSelected = FigureEnum.Triangle;
                    break;
                case 1:
                    figureSelected = FigureEnum.Box;
                    break;
                case 2:
                    figureSelected = FigureEnum.Star;
                    break;
                case 3:
                    figureSelected = FigureEnum.Ugl13;
                    break;
            }

            isFigureFormed = true;
        }

        private void comboBoxAngle_SelectedIndexChanged(object sender, EventArgs e)=> 
            AngleCount = comboBoxAngle.SelectedIndex + 5;

        private void checkBoxSpline_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSpline.Checked) isSplineChecked = true;
            else isSplineChecked = false;
        }

        private void comboBoxTMO_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxTMO.SelectedIndex)
            {
                case 0:
                    setQ[0] = 3;
                    setQ[1] = 3;
                    break;
                case 1:
                    setQ[0] = 1;
                    setQ[1] = 3;
                    break;
            }
        }

        private void buttonTMO_Click(object sender, EventArgs e)
        {
            if (Figures.Count > 1)
            {
                g.Clear(Color.White);
                TMO();
                pictureBox1.Image = bitmap;
                Points.Clear();
            }

        }

        private void comboBoxGeomActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxGeomActions.SelectedIndex)
            {
                case 0:
                    GeomAction = 0;
                    break;
                case 1:
                    GeomAction = 1;
                    break;
                case 2:
                    GeomAction= 2;
                    break;
            }
        }

        private void comboBoxColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxColor.SelectedIndex)
            {
                case 0:
                    DrawPen.Color = Color.Black;
                    break;
                case 1:
                    DrawPen.Color = Color.Red;
                    break;
                case 2:
                    DrawPen.Color = Color.Blue;
                    break;
                case 3:
                    DrawPen.Color = Color.Green;
                    break;
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            Figures.Clear();
            Points.Clear();
            CountPoints = 0;
            checkBoxSpline.Checked = false;
            pictureBox1.Image = bitmap;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(isFigureFormed)
            {
                CreateFigure(e);
                MakeFill();
                isFigureFormed = false;
            }
            
            else if (e.Button == MouseButtons.Left && isSplineChecked)
            {
                Points.Add(new CustomPoint { X = e.X, Y = e.Y });
                g.DrawEllipse(DrawPen, e.X - 2, e.Y - 2, 5, 5);
                PreparationsForSpline();
            }
            
            else if (e.Button == MouseButtons.Left)
            {
                Points.Add(new CustomPoint(e.X, e.Y));
                if (Points.Count > 1)
                {
                    g.DrawLine(DrawPen, Points[Points.Count - 2].ToPoint(), Points[Points.Count - 1].ToPoint());
                    pictureBox1.Image = bitmap;
                }
                
            }
            
            else if (e.Button == MouseButtons.Right) MakeFill();
        }
    }
}