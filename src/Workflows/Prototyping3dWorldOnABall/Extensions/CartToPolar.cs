using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using OpenCV.Net;


namespace CricketVR
{
[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class CartToPolar
{

    // Tuple<Magnitude, Angle>
    public IObservable<Tuple<float, float>> Process(IObservable<Tuple<float, float>> source)
    {
        return source.Select(value => {
            var Magnitude = (float) (Math.Sqrt(value.Item1 * value.Item1 + value.Item2 * value.Item2));
            var Angle =  (float) (Math.Atan2(value.Item1, value.Item2));
            return Tuple.Create(Magnitude, Angle);
        });

    }

    // Tuple<Magnitude, Angle>
    public IObservable<Tuple<double, double>> Process(IObservable<Tuple<double, double>> source)
    {
        return source.Select(value => {
            var Magnitude = (Math.Sqrt(value.Item1 * value.Item1 + value.Item2 * value.Item2));
            var Angle =  Math.Atan2(value.Item1, value.Item2);
            return Tuple.Create(Magnitude, Angle);
        });

    }

    // Point2f(X, Y)
    public IObservable<Point2f> Process(IObservable<Point2f> source)
    {
        return source.Select(value => {
            var Magnitude = (float) (Math.Sqrt(value.X * value.X + value.Y * value.Y));
            var Angle = (float) Math.Atan2(value.X, value.Y);
            return new Point2f(Magnitude, Angle);
        });
    }

}
}
