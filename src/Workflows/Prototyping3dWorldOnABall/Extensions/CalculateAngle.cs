using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using CricketVR;
using OpenTK;
namespace CricketVR
{
[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class CalculateAngle
{
    public IObservable<Tuple<double,double>> Process(IObservable<VrElement> source)
    {
        return source.Select(value => {

            var mag = value.Position.Length;

            var theta = Math.Acos(value.Position.Z / mag);
            var phi = Math.Asin(value.Position.Y / mag);

            return Tuple.Create(theta, phi);
            });
    }

    public IObservable<Tuple<double,double>> Process(IObservable<Tuple<VrElement, VrElement>> source)
    {
        return source.Select(value => {
            var delta_position = (value.Item1 - value.Item2).Position;
            var mag = delta_position.Length;

            var theta = Math.Acos(delta_position.Z / mag);
            var phi = Math.Asin(delta_position.Y / mag);

            return Tuple.Create(theta, phi);
        });
    }

}
}