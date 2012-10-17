using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.IO.IsolatedStorage;

namespace Memoriam
{
    public struct Circle
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Radius { get; set; }

        static Random seed = new Random();

        public static Circle[] GeneratePoints(int Count, int w, int h, double rad = 0)
        {
            
            var points = new Point[Count];
            var radiuses = new double[Count];

            for (int i = 0; i < Count; i++)
                radiuses[i] = rad != 0 ? rad : seed.NextDouble(130.0 / Math.Pow(Count, 0.33), 130.0 / Math.Pow(Count, 0.25));

            for (int i = 0; i < Count; i++)
                points[i] = new Point(seed.NextDouble(radiuses[i], w - radiuses[i]), seed.NextDouble(radiuses[i], h - radiuses[i]));

            int iter = 0;
            while (points.Intersections(radiuses) != 0)
            {
                if (iter++ > 100000) return GeneratePoints(Count, w, h, 50);
                for (int i = 0; i < points.Length; i++)
                    for (int j = i + 1; j < points.Length; j++)
                    {
                        var d = points[i].DistanceTo(points[j]);
                        if (d > radiuses[i] + radiuses[j] + 10) continue;

                        points[i] = points[i].Move(points[i].To(points[j]), -5 / (d * d));
                        points[j] = points[j].Move(points[j].To(points[i]), -5 / (d * d));

                        if (points[i].X < radiuses[i]) points[i].X = radiuses[i];
                        if (points[i].Y < radiuses[i]) points[i].Y = radiuses[i];
                        if (points[i].X > w - radiuses[i]) points[i].X = w - radiuses[i];
                        if (points[i].Y > h - radiuses[i]) points[i].Y = h - radiuses[i];

                        if (points[j].X < radiuses[j]) points[j].X = radiuses[j];
                        if (points[j].Y < radiuses[j]) points[j].Y = radiuses[j];
                        if (points[j].X > w - radiuses[j]) points[j].X = w - radiuses[j];
                        if (points[j].Y > h - radiuses[j]) points[j].Y = h - radiuses[j];

                    }
            }

            var crc = new Circle[Count];
            for (int i = 0; i < Count; i++)
            {
                crc[i].X = points[i].X;
                crc[i].Y = points[i].Y;
                crc[i].Radius = radiuses[i];
            }
            return crc;
        }



    }

    public static class Tutorial
    {
        public static void ShowTutorial(bool force = false)
        {
            var ss = IsolatedStorageSettings.ApplicationSettings;

            if (!force && ss.Contains("tutorialshown")) return;

            MessageBox.Show(@"Memoriam® is a simple game that allows you to train your memory. Rules are very easy:  1. You memorize contents of all circles; 2. Tags are cleared: now you need to repeat them in correct order by “opening” circles.  There is two methods to play: general and timed. In general run, you memorize the sequence, click okay button and then repeat the sequence. In timed run, you memorize the sequence in the limited amount of time.", "Memoriam®", MessageBoxButton.OK);

            MessageBox.Show(@"In Memoriam®, seven difficulty levels are available.  First three (Novice, Beginner, and Intermediate) differ only by amount of circles on each level; each level contains ordered sequences of numbers or letters.  4th and 5th levels (Advanced and Expert) are pair of levels with random sequence, but only one type of symbols (letter or number) can occur. 
Last ones (Insane and Jedi) are practically impossible to beat: On this levels lie random sequences of letters and numbers.", "Memoriam®", MessageBoxButton.OK);



            ss["tutorialshown"] = 1;
            ss.Save();
        }        
    }

    public static class PointExtensions
    {
        public static int Intersections(this Point[] points, double[] radiuses)
        {
            int res = 0;
            for (int i = 0; i < radiuses.Length; i++)
                for (int j = i + 1; j < radiuses.Length; j++)
                {
                    if (points[i].DistanceTo(points[j]) < radiuses[i] + radiuses[j]) res++;
                }

            return res;
        }

        public static double DistanceTo(this Point curr, Point p)
        {
            return Math.Sqrt((curr.X - p.X) * (curr.X - p.X) + (curr.Y - p.Y) * (curr.Y - p.Y));
        }

        public static double NextDouble(this Random r, double from, double to)
        {
            return from + r.NextDouble() * (to - from);
        }

        public static double Radius(this IEnumerable<Point> center)
        {
            double md = double.MaxValue;
            double cd = 0;
            foreach (var c in center) foreach (var d in center) if (c != d && (cd = c.DistanceTo(d)) < md) md = cd;
            return md;
        }

        public static Point Center(Point p1, Point p2, double w1, double w2)
        {
            return new Point((p1.X * w1 + p2.X * w2) / (w1 + w2), (p1.Y * w1 + p2.Y * w2) / (w1 + w2));
        }

        public static Point Center(this Point[] center)
        {
            Point res = center[0];
            for (int i = 1; i < center.Length; i++) res = Center(res, center[i], i, 1);
            return res;
        }

        public static Point To(this Point from, Point to) { return new Point(to.X - from.X, to.Y - from.Y); }
        public static Point Move(this Point curr, Point vec, double w = 1) { return new Point(curr.X + w * vec.X, curr.Y + w * vec.Y); }

    }
    public enum PlayStyle { GeneralRun, TimedRun };
}
