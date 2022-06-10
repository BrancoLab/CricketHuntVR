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
    [Description("")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    public class PolarToCart
    {

        public IObservable<Point2f> Process(IObservable<Tuple<float, float>> source)
        {
            return source.Select(value =>
            {
                var Magnitude = (float)(Math.Sqrt(value.Item1 * value.Item1 + value.Item2 * value.Item2));
                var Angle = (float)(Math.Atan2(value.Item1, value.Item2));
                return new Point2f(Magnitude, Angle);
            });

        }

        public IObservable<Point2f> Process(IObservable<Tuple<double, double>> source)
        {
            return source.Select(value =>
            {
                var Magnitude = (float) (Math.Sqrt(value.Item1 * value.Item1 + value.Item2 * value.Item2));
                var Angle = (float) Math.Atan2(value.Item1, value.Item2);
                return new Point2f(Magnitude, Angle);
            });

        }

        public IObservable<Point2f> Process(IObservable<Point2f> source)
        {
            return source.Select(value =>
            {
                var Magnitude = (float)(Math.Sqrt(value.X * value.X + value.Y * value.Y));
                var Angle = (float)Math.Atan2(value.X, value.Y);
                return new Point2f(Magnitude, Angle);
            });
        }
        public IObservable<Point2f> Process(IObservable<Vector2> source)
        {
            return source.Select(value =>
            {
                var Magnitude = (float)(Math.Sqrt(value.X * value.X + value.Y * value.Y));
                var Angle = (float)Math.Atan2(value.X, value.Y);
                return new Point2f(Magnitude, Angle);
            });
        }


    }
}
