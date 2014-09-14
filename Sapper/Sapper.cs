using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sapper
{

    [Serializable()]	
    public class Sapper : ISapper
    {
        private ICell[][] field;
        private Statuses status;

        private HashSet<Point> watchedCells;

        public ICell[][] Field
        {
            get
            {
                return field;
            }
        }

        public Statuses Status
        {
            get { return status; }
        }

        public Sapper(int width, int height, int countOfBombs)
        {
            int commonCount = width * height;
            if (countOfBombs > commonCount) countOfBombs = commonCount;

            Random r = new Random();
            HashSet<int> bombs = new HashSet<int>();



            for (int i = 0; i < countOfBombs; i++)
            {
                int bomb = r.Next(1, commonCount);
                while (bombs.Contains(bomb))
                {
                    bomb = r.Next(1, commonCount);
                }
                bombs.Add(bomb);
            }
            

            field = new ICell[height][];
            for (int i = 0; i < field.Length; i++)
            {
                field[i] = new ICell[width];
                for (int j = 0; j < field[i].Length; j++)
                {
                    if (bombs.Contains(i * field[i].Length + (j + 1)))
                    {
                        field[i][j] = new Cell(true);
                    }
                    else
                    {
                        field[i][j] = new Cell(false);
                    }
                }
            }

            for (int i = 0; i < field.Length; i++)
            {
                for (int j = 0; j < field[i].Length; j++)
                {
                    Point[] neighbors = getNeighbors(new Point(i, j));
                    int count = 0;
                    foreach (Point p in neighbors)
                    {
                        if (field[p.X][p.Y].Mined == PlaceHolders.MINED) count++;
                    }
                    field[i][j].Count = count;
                }
            }

            watchedCells = new HashSet<Point>();
            status = Statuses.CONTINUE;
        }

        public ICell[][] recalculateField(Point p, bool mouseButton)
        {

            if (!mouseButton)
            {
                if (field[p.X][p.Y].Opened == Visibility.CLOSED)
                {
                    if (field[p.X][p.Y].Marked == Marks.MARKED) field[p.X][p.Y].Marked = Marks.NOT_MARKED;
                    else field[p.X][p.Y].Marked = Marks.MARKED;
                }
                return field;
            }

            if (field[p.X][p.Y].Active == Activity.ENABLED)
            {
                field[p.X][p.Y].Opened = Visibility.OPENED;

                if (field[p.X][p.Y].Mined == PlaceHolders.MINED)
                {
                    status = Statuses.FAIL;
                    foreach (ICell[] cells in field)
                        foreach (ICell cell in cells)
                        {
                            cell.Opened = Visibility.OPENED;
                        }

                }
                else
                {                  
                    watchedCells.Clear();
                    calculateCells(p);
                    if (isWin()) status = Statuses.WIN;
                }
                
            }

            

            return field;
        }

        private bool isWin()
        {
            foreach (ICell[] cells in field)
                foreach (ICell cell in cells)
                {
                    if (cell.Opened == Visibility.CLOSED && cell.Mined == PlaceHolders.NOT_MINED) return false;
                }

            return true;
        }

        private void calculateCells(Point p)
        {
            if (!watchedCells.Contains(p))
            {
                watchedCells.Add(p);
                field[p.X][p.Y].Opened = Visibility.OPENED;
                if (field[p.X][p.Y].Count == 0)
                {
                    Point[] neighbors = getNeighbors(p);
                    foreach (Point c in neighbors)
                    {
                        calculateCells(c);
                    }
                }
            }
        }

        private Point[] getNeighbors(Point p)
        {
            List<Point> points = new List<Point>();

            points.Add(new Point(p.X, p.Y - 1));
            points.Add(new Point(p.X, p.Y + 1));

            if (p.X % 2 == 0)
            {       
                points.Add(new Point(p.X-1, p.Y));
                points.Add(new Point(p.X-1, p.Y-1));
                points.Add(new Point(p.X + 1, p.Y));
                points.Add(new Point(p.X + 1, p.Y - 1));
            }
            else
            {
                points.Add(new Point(p.X - 1, p.Y));
                points.Add(new Point(p.X - 1, p.Y + 1));
                points.Add(new Point(p.X + 1, p.Y));
                points.Add(new Point(p.X + 1, p.Y + 1));
            }

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X < 0 || points[i].Y < 0 || points[i].X > field.Length - 1 || points[i].Y > field[points[i].X].Length - 1)
                {
                    points.RemoveAt(i);
                    i--;
                }
            }

            return points.ToArray();
        }
    }

    [Serializable()]	
    class Cell : ICell
    {
        public Cell(bool mined)
        {
            active = Activity.ENABLED;
            opened = Visibility.CLOSED;
            if (mined) this.mined = PlaceHolders.MINED;
            else this.mined = PlaceHolders.NOT_MINED;
            marked = Marks.NOT_MARKED;
            count = 0;
        }

        public Cell()
        {
        }

        public virtual bool Equals(ICell cell)
        {
            if (cell == null) return false;

            if (this.active != cell.Active) return false;
            if (this.opened != cell.Opened) return false;
            if (this.mined != cell.Mined) return false;
            if (this.marked != cell.Marked) return false;
            if (this.count != cell.Count) return false;

            return true;
        }

        public ICell Copy()
        {
            ICell copy = new Cell(this.mined==PlaceHolders.MINED?true:false);
            copy.Active = this.active;
            copy.Count = this.count;
            copy.Opened = this.opened;
            copy.Marked = this.marked;
            return copy;
        }

        private Activity active;
        private Visibility opened;
        private PlaceHolders mined;
        private Marks marked;
        private int count;

        public Activity Active
        {
            get { return active; }
            set { active = value; }
        }

        public Visibility Opened
        {
            get { return opened; }
            set { opened = value; }
        }

        public PlaceHolders Mined
        {
            get { return mined; }
        }

        public Marks Marked
        {
            get { return marked; }
            set { marked = value; }
        }

        public int Count
        {
            get { return count; }
            set { count = value; }
        }
    }
}
