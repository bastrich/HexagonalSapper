using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sapper
{
    public class Draw
    {
        
        public const int START_POSITION_X = 70;
        public const int START_POSITION_Y = 100;

        public const int SIZE = 20;
        private PointF[] path;

        private float STEP_RIGHT;
        private float STEP_DOWN;

        private PointF[][] cellsPoints;
        private Graphics g;

        private static Color BACKGROUND_COLOR = Color.Aqua;
        private static Color CLOSED_COLOR = Color.Gray;
        private static Color EMPTY_COLOR = Color.White;
        private static Color MINED_COLOR = Color.Red;
        private static Color TOUCHED_COLOR = Color.Green;


        private Color backgroundColor;
        private Color closedColor;
        private Color emptyColor;
        private Color minedColor;
        private Color touchedColor;

        private Statuses mode;

        public Statuses Mode
        {
            //get;
            set
            {
                mode = value;
            }
        }

        public int Width
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }


        public Draw(Graphics g, int width, int height)
        {
            this.g = g;
            backgroundColor = BACKGROUND_COLOR;
            closedColor = CLOSED_COLOR;
            emptyColor = EMPTY_COLOR;
            minedColor = MINED_COLOR;
            touchedColor = TOUCHED_COLOR;

            path = new PointF[5] {new PointF(SIZE * (float)Math.Cos(Math.PI / 6), SIZE * (float)Math.Sin(Math.PI / 6)),
                                  new PointF(2 * SIZE * (float)Math.Cos(Math.PI / 6), 0), 
                                  new PointF(2*SIZE * (float)Math.Cos(Math.PI / 6), -SIZE), 
                                  new PointF(SIZE * (float)Math.Cos(Math.PI / 6),-SIZE-SIZE * (float)Math.Sin(Math.PI / 6)), 
                                  new PointF(0, -SIZE)};

            STEP_DOWN = SIZE + SIZE * (float)Math.Sin(Math.PI / 6);
            STEP_RIGHT = 2 * SIZE * (float)Math.Cos(Math.PI / 6);

            cellsPoints = new PointF[height][];
            for (int i = 0; i < cellsPoints.Length; i++)
            {
                cellsPoints[i] = new PointF[width];
                float y = START_POSITION_Y + i * STEP_DOWN;

                if (i % 2 != 0)
                {
                    for (int j = 0; j < cellsPoints[i].Length; j++)
                    {
                        float x = START_POSITION_X + SIZE * (float)Math.Cos(Math.PI / 6)  + j * STEP_RIGHT;
                        cellsPoints[i][j] = new PointF(x, y);
                    }
                }
                else
                {
                    for (int j = 0; j < cellsPoints[i].Length; j++)
                    {
                        float x = START_POSITION_X + j * STEP_RIGHT;
                        cellsPoints[i][j] = new PointF(x, y);
                    }
                }
            }

            mode = Statuses.CONTINUE;

            Width = (int)((1+width * 2)* SIZE * (float)Math.Cos(Math.PI / 6) + 2 * START_POSITION_X);
            Height = (int)(height * (SIZE * (float)Math.Sin(Math.PI / 6) + SIZE) + 2 * START_POSITION_Y); 
        }

        public void drawCells(ICell[][] field)
        {
            for (int i = 0; i < field.Length; i++)
            {
                for (int j = 0; j < field[i].Length; j++)
                {
                    drawCell(field[i][j], cellsPoints[i][j]);
                }
            }
        }

        public void drawBackground(int w, int h)
        {
            g.FillRectangle(new SolidBrush(backgroundColor), 0, 0, w, h);
        }

        public void drawField(ICell[][] field)
        {
            drawBackground(Width, Height);

            drawCells(field);
        }

        public void drawChangedField(ICell[][] oldField, ICell[][] newField)
        {
            for (int i = 0; i < oldField.Length; i++)
            {
                for (int j = 0; j < oldField[i].Length; j++)
                {
                    if (!newField[i][j].Equals(oldField[i][j]) || (newField[i][j].Mined == PlaceHolders.MINED && mode == Statuses.WIN))
                    {
                        drawCell(newField[i][j], cellsPoints[i][j]);
                    }
                }
            }
        }

        public void drawTouched(Point p)
        {
            PointF[] points = new PointF[6];
            points[0] = cellsPoints[p.X][p.Y];
            for (int i = 1; i < points.Length; i++)
            {
                points[i].X = points[0].X + path[i - 1].X;
                points[i].Y = points[0].Y + path[i - 1].Y;
            }

            g.FillPolygon((new SolidBrush(Color.FromArgb(100,12,123,123))), points);
            g.DrawPolygon(new Pen(Color.Black), points);
        }

        public void drawUntouched(Point p,ICell[][] field)
        {
            drawCell(field[p.X][p.Y], cellsPoints[p.X][p.Y]);
        }

        public Point getCellPoint(PointF p)
        {
            List<Point> points = new List<Point>();

            for (int i = 0; i < cellsPoints.Length; i++)
            {
                for (int j = 0; j < cellsPoints[i].Length; j++)
                {
                    if (cellsPoints[i][j].X < p.X && cellsPoints[i][j].X + 2 * SIZE * (float)Math.Cos(Math.PI / 6) > p.X
                        && p.Y < cellsPoints[i][j].Y + SIZE * (float)Math.Sin(Math.PI / 6) && p.Y > cellsPoints[i][j].Y - SIZE - SIZE * (float)Math.Sin(Math.PI / 6))
                    {
                        points.Add(new Point(i,j));
                    }
                }
            }

            //if (points.Count == 1) return points[0];

            for (int i = 0; i < points.Count; i++)
            {
                PointF v1 = vector(cellsPoints[points[i].X][points[i].Y], p);
                PointF v2 = vector(cellsPoints[points[i].X][points[i].Y], new PointF(cellsPoints[points[i].X][points[i].Y].X + path[0].X, cellsPoints[points[i].X][points[i].Y].Y + path[0].Y));
                if (vectorMult(v1, v2) < 0) continue;

                v1 = vector(new PointF(cellsPoints[points[i].X][points[i].Y].X + path[0].X, cellsPoints[points[i].X][points[i].Y].Y + path[0].Y), p);
                v2 = vector(new PointF(cellsPoints[points[i].X][points[i].Y].X + path[0].X, cellsPoints[points[i].X][points[i].Y].Y + path[0].Y), new PointF(cellsPoints[points[i].X][points[i].Y].X + path[1].X, cellsPoints[points[i].X][points[i].Y].Y + path[1].Y));
                if (vectorMult(v1, v2) < 0) continue;

                v1 = vector(new PointF(cellsPoints[points[i].X][points[i].Y].X + path[2].X, cellsPoints[points[i].X][points[i].Y].Y + path[2].Y), p); 
                v2 = vector(new PointF(cellsPoints[points[i].X][points[i].Y].X + path[2].X, cellsPoints[points[i].X][points[i].Y].Y + path[2].Y), new PointF(cellsPoints[points[i].X][points[i].Y].X + path[3].X, cellsPoints[points[i].X][points[i].Y].Y + path[3].Y));
                if (vectorMult(v1, v2) < 0) continue;

                v1 = vector(new PointF(cellsPoints[points[i].X][points[i].Y].X + path[3].X, cellsPoints[points[i].X][points[i].Y].Y + path[3].Y), p);
                v2 = vector(new PointF(cellsPoints[points[i].X][points[i].Y].X + path[3].X, cellsPoints[points[i].X][points[i].Y].Y + path[3].Y), new PointF(cellsPoints[points[i].X][points[i].Y].X + path[4].X, cellsPoints[points[i].X][points[i].Y].Y + path[4].Y));
                if (vectorMult(v1, v2) < 0) continue;

                return points[i];
            }
        
            return new Point(-1,-1);
        }

        private float distance(PointF p1, PointF p2)
        {
            return (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        private float vectorMult(PointF v1, PointF v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        private PointF vector(PointF p1, PointF p2)
        {
            return new PointF(p2.X - p1.X, p2.Y - p1.Y);
        }

        private void drawCell(ICell cell, PointF p)
        {
            if (cell.Active == Activity.DISABLED)
            {
                drawNotActive(p);
            }
            else
            {
                if (cell.Opened == Visibility.CLOSED && cell.Marked == Marks.MARKED)
                {
                    drawMarkedCell(p);
                }
                else if (cell.Opened == Visibility.CLOSED && mode != Statuses.WIN)
                {
                    drawClosedCell(p);
                }
                else if (cell.Mined == PlaceHolders.MINED)
                {
                    if (mode != Statuses.WIN) drawMinedCell(p);
                    else drawWinMinedCell(p);
                }
                else
                {
                    drawEmptyCell(p,cell.Count);
                }
            }
            
        }

        private void drawClosedCell(PointF p)
        {
            PointF[] points = new PointF[6];
            points[0] = p;
            for (int i = 1; i < points.Length; i++)
			{
			    points[i].X = points[0].X+path[i-1].X;
                points[i].Y = points[0].Y + path[i - 1].Y;
			}

            g.FillPolygon(new SolidBrush(closedColor), points);
            g.DrawPolygon(new Pen(Color.Black), points);
        }

        private void drawEmptyCell(PointF p,int count)
        {
            PointF[] points = new PointF[6];
            points[0] = p;
            for (int i = 1; i < points.Length; i++)
            {
                points[i].X = points[0].X + path[i - 1].X;
                points[i].Y = points[0].Y + path[i - 1].Y;
            }

            g.FillPolygon(new SolidBrush(emptyColor), points);
            g.DrawPolygon(new Pen(Color.Black), points);

            if (count > 0) {
                g.DrawString(count.ToString(), new Font(FontFamily.GenericSansSerif,(int)(SIZE*0.5)), new SolidBrush(Color.Black),p.X+SIZE * (float)Math.Sin(Math.PI / 6),p.Y - SIZE);
            }
        }

        private void drawMinedCell(PointF p)
        {
            PointF[] points = new PointF[6];
            points[0] = p;
            for (int i = 1; i < points.Length; i++)
            {
                points[i].X = points[0].X + path[i - 1].X;
                points[i].Y = points[0].Y + path[i - 1].Y;
            }

            g.FillPolygon(new SolidBrush(minedColor), points);
            g.DrawPolygon(new Pen(Color.Black), points);
        }

        private void drawWinMinedCell(PointF p)
        {
            PointF[] points = new PointF[6];
            points[0] = p;
            for (int i = 1; i < points.Length; i++)
            {
                points[i].X = points[0].X + path[i - 1].X;
                points[i].Y = points[0].Y + path[i - 1].Y;
            }

            g.FillPolygon(new SolidBrush(Color.Coral), points);
            g.DrawPolygon(new Pen(Color.Black), points);
        }

        private void drawMarkedCell(PointF p)
        {
            PointF[] points = new PointF[6];
            points[0] = p;
            for (int i = 1; i < points.Length; i++)
            {
                points[i].X = points[0].X + path[i - 1].X;
                points[i].Y = points[0].Y + path[i - 1].Y;
            }

            g.FillPolygon(new SolidBrush(Color.Brown), points);
            g.DrawPolygon(new Pen(Color.Black), points);
        }

        private void drawNotActive(PointF p)
        {
            PointF[] points = new PointF[6];
            points[0] = p;
            for (int i = 1; i < points.Length; i++)
            {
                points[i].X = points[0].X + path[i - 1].X;
                points[i].Y = points[0].Y + path[i - 1].Y;
            }

            g.DrawPolygon(new Pen(Color.Black), points);
        }

    }
}
