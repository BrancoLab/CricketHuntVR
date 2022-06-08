using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Bonsai.Harp;
using OpenCV.Net;

namespace CricketVR
{
    [Combinator]
    [Description("")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    public class CreateArmMessage
    {
        private int address = 32;

        // Tuple<Magnitude, Angle>
        public IObservable<HarpMessage> Process(IObservable<Tuple<float, float>> source)
        {


            return source.Select(value =>
            {
                var Magnitude = value.Item1;
                var Angle = value.Item2;

                return HarpMessage.FromSingle(address,
                MessageType.Write, new float[2] { (float)Magnitude, (float)Angle }
                );
            });
        }

        // Tuple<Magnitude, Angle>
        public IObservable<HarpMessage> Process(IObservable<Tuple<double, double>> source)
        {
            return source.Select(value =>
            {
                var Magnitude = (float)value.Item1;
                var Angle = (float)value.Item2;

                return HarpMessage.FromSingle(address,
                MessageType.Write, new float[2] { Magnitude, Angle }
                );
            });
        }

        // Point2f(X, Y)
        public IObservable<HarpMessage> Process(IObservable<Point2f> source)
        {
            return source.Select(value =>
            {
                var Magnitude = value.X;
                var Angle = value.Y;

                return HarpMessage.FromSingle(address,
                MessageType.Write, new float[2] { (float)Magnitude, (float)Angle }
                );
            });
        }

    }
}