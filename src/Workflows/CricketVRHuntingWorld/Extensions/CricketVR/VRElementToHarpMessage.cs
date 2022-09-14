using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using CricketVR;
using Bonsai.Harp;

namespace CricketVR
{
[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class VRElementToHarpMessage
{
    private int address = 201;
    public int Address
    {
        get { return address; }
        set { address = value; }
    }

    public IObservable<HarpMessage> Process(IObservable<Tuple<VrElement, double>> source)
    {
        return source.Select(value => {
            var vre = value.Item1;
            var ts = value.Item2;
            return HarpMessage.FromSingle(
                Address,
                ts,
                MessageType.Event,
                (float) vre.Position.X,
                (float) vre.Position.Y,
                (float) vre.Position.Z,
                (float) vre.Orientation.X,
                (float) vre.Orientation.Y,
                (float) vre.Orientation.Z
                );
        });
    }
}
}