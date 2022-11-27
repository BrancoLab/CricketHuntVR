using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using OpenCV.Net;
using OpenTK;


namespace CricketVR
{
    [Combinator]
    [Description("Computes the (X,Y)[Cartesian] coordinates from a (Magnitude, Angle)[Polar] point.")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    public class PolarToCart
    {

        public IObservable<Point2f> Process(IObservable<Tuple<float, float>> source)
        {
            return source.Select(value =>
            {
                return new Point2f(
                    (float) (value.Item1 * Math.Cos(value.Item2)),
                    (float) (value.Item1 * Math.Sin(value.Item2)));
            });

        }

        public IObservable<Point2f> Process(IObservable<Tuple<double, double>> source)
        {
            return source.Select(value =>
            {
                return new Point2f(
                    (float) (value.Item1 * Math.Cos(value.Item2)),
                    (float) (value.Item1 * Math.Sin(value.Item2)));
            });

        }

        public IObservable<Point2f> Process(IObservable<Point2f> source)
        {
            return source.Select(value =>
            {
                return new Point2f(
                    (float) (value.X * Math.Cos(value.Y)),
                    (float) (value.X * Math.Sin(value.Y)));
            });
        }

        public IObservable<Point2f> Process(IObservable<Vector2> source)
        {
            return source.Select(value =>
            {
                return new Point2f(
                    (float) (value.X * Math.Cos(value.Y)),
                    (float) (value.X * Math.Sin(value.Y)));
            });
        }


    }
}
