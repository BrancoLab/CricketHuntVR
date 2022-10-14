using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using OpenCV.Net;

namespace CricketVR{
    [Combinator]
    [Description("Clamps an Arm position (X,Y) between the maximum and minimum radial parameter (theta)")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    public class ClampArmPosition
    {
        private float minRadius;
        public float MinRadius
        {
            get { return minRadius; }
            set { minRadius = value; }
        }

        private float maxRadius;
        public float MaxRadius
        {
            get { return maxRadius; }
            set { maxRadius = value; }
        }

        public IObservable<Point2f> Process(IObservable<Point2f> source)
        {
            return source.Select(value => {
                var magnitude = (float)(Math.Sqrt(value.X * value.X + value.Y * value.Y));
                if ((magnitude  <= maxRadius) & (magnitude >= minRadius)){
                    return value;
                }
                else{
                    var Angle = (float)(Math.Atan2(value.Y, value.X));
                    magnitude = magnitude > maxRadius ? maxRadius : (magnitude < minRadius ? minRadius : magnitude);
                    return new Point2f(
                        (float) Math.Cos(Angle) * magnitude,
                        (float) Math.Sin(Angle) * magnitude
                    );
                }
            });
        }
    }
}