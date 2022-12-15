namespace GSK_72
{
    public partial class Form1 : Form
    {
        private enum FigureEnum
        {
            Triangle,
            Box,
            Star,
            Ugl3
        }
        private readonly Graphics g;
        private readonly Bitmap bitmap;
        FigureEnum figureSelected;
        Pen DrawPen = new Pen(Color.Black, 1);
        List<CustomPoint> Points = new List<CustomPoint>();
        List<FigureClass> Figures = new List<FigureClass>();
        int[] setQ = new int[2];
        int AngleCount = 0;
        bool isSplineChecked = false;
        bool isFigureFormed = false;
        bool isTMODone = false;
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
            MouseWheel += GTonTMOAndFigures;
        }

        /// <summary>
        /// Метод, закрашивающий фигуру, а также добавляет фигуру в список фигур
        /// </summary>
        private void MakeFill(FigureClass figure)
        {
            if(isSplineChecked) figure.CreateCubeSpline();
            else figure.FillFigure(Yemax);
            pictureBox1.Image = bitmap;
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

                if (Xal.Count == 0 && Xbl.Count == 0) continue;

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

        private void GTonTMOAndFigures(object sender, MouseEventArgs e) 
        {
            if (isTMODone)
            {
                GeomTrans(Figures[Figures.Count - 1], e);
                GeomTrans(Figures[Figures.Count - 2], e);
                g.Clear(Color.White);
                TMO();
                pictureBox1.Image = bitmap;
            }
            else GeomTrans(Figures[Figures.Count - 1], e);
        }

        private void GeomTrans(FigureClass buff, MouseEventArgs e)
        {
            if (GeomAction == 0)
            {
                g.Clear(Color.White);
                buff.Zoom(new float[] { e.Delta, e.Delta }, e);
                MakeFill(buff);
            }
            else if (GeomAction == 1)
            {
                g.Clear(Color.White);
                buff.Mirror('c', e, null);
                MakeFill(buff);
            }
            else if (GeomAction == 2)
            {
                List<CustomPoint> line = new List<CustomPoint>(Points.ToList());
                g.Clear(Color.White);
                buff.Mirror('p', e, line);
                MakeFill(buff);
            }
        }

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
                new CustomPoint(e.X, e.Y - 150),
                new CustomPoint(e.X + 150, e.Y + 75),
                new CustomPoint(e.X - 150, e.Y + 75)
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
        public void CreateUgl3(MouseEventArgs e)
        {
            var ug = new List<CustomPoint>
            {
                new CustomPoint(e.X - 100, e.Y - 100),
                new CustomPoint(e.X - 40, e.Y + 40),
                new CustomPoint(e.X + 100, e.Y + 100),
                new CustomPoint(e.X - 100, e.Y + 100),
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
                        FigureClass spline = new FigureClass(Points.ToList(), DrawPen, g);
                        spline.CreateCubeSpline();
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
                case FigureEnum.Ugl3:
                    CreateUgl3(e);
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
                    figureSelected = FigureEnum.Ugl3;
                    break;
            }

            isFigureFormed = true;
        }

        private void comboBoxAngle_SelectedIndexChanged(object sender, EventArgs e) =>
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
                isTMODone = true;
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
                    GeomAction = 2;
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
            if (isFigureFormed)
            {
                CreateFigure(e);
                FigureClass figure = new FigureClass(Points.ToList(), DrawPen, g);
                MakeFill(figure);
                Figures.Add(figure);
                Points.Clear();
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

            else if (e.Button == MouseButtons.Right)
            {
                FigureClass figure = new FigureClass(Points.ToList(), DrawPen, g);
                MakeFill(figure);
                Figures.Add(figure);
                Points.Clear();
            }
        }
    }
}