using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using CricketVR;
namespace CricketVR
{
[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class CalculateDistance
{

    public IObservable<float> Process(IObservable<VrElement> source)
    {
        return source.Select(value => {
            return value.Position.Length;
        });
    }

    public IObservable<float> Process(IObservable<Tuple<VrElement, VrElement>> source)
    {
        return source.Select(value => {
            VrElement delta = value.Item1 - value.Item2;
            return delta.Position.Length;
        });
    }

}
}