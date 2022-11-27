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
    [Description("Computes the (Magnitude, Angle)[Polar] of a (X,Y)[Cartesian] point.")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    public class CartToPolar
    {

        public IObservable<Point2f> Process(IObservable<Tuple<float, float>> source)
        {
            return source.Select(value =>
            {
                var Magnitude = (float)(Math.Sqrt(value.Item1 * value.Item1 + value.Item2 * value.Item2));
                var Angle = (float)(Math.Atan2(value.Item2, value.Item1));
                return new Point2f(Magnitude, Angle);
            });

        }

        public IObservable<Point2f> Process(IObservable<Tuple<double, double>> source)
        {
            return source.Select(value =>
            {
                var Magnitude = (float) (Math.Sqrt(value.Item1 * value.Item1 + value.Item2 * value.Item2));
                var Angle = (float) Math.Atan2(value.Item2, value.Item1);
                return new Point2f(Magnitude, Angle);
            });

        }

        public IObservable<Point2f> Process(IObservable<Point2f> source)
        {
            return source.Select(value =>
            {
                var Magnitude = (float)(Math.Sqrt(value.X * value.X + value.Y * value.Y));
                var Angle = (float)Math.Atan2(value.Y, value.X);
                return new Point2f(Magnitude, Angle);
            });
        }
        public IObservable<Point2f> Process(IObservable<Vector2> source)
        {
            return source.Select(value =>
            {
                var Magnitude = (float)(Math.Sqrt(value.X * value.X + value.Y * value.Y));
                var Angle = (float)Math.Atan2(value.Y, value.X);
                return new Point2f(Magnitude, Angle);
            });
        }


    }
}
