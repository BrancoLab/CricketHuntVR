using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using OpenCV.Net;

namespace CricketVR{

    [Combinator]
    [Description("Projects the first Point2f onto the second Point2f in a tuple with paired values.")]
    [WorkflowElementCategory(ElementCategory.Transform)]

    public class DotProduct
    {
        public IObservable<double> Process(IObservable<Tuple<Point2f, Point2f>> source)
        {
            return source.Select(value => {
                return ( ((value.Item1.X * value.Item2.X) + (value.Item1.Y * value.Item2.Y)) /
                Math.Sqrt((value.Item2.X * value.Item2.X) + (value.Item2.Y * value.Item2.Y)));
            });
        }
    }
}