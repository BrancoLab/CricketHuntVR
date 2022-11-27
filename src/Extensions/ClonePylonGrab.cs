using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using Bonsai.Harp;
using Bonsai.Pylon;

[Combinator]
[Description("Clones the pylon image reference.")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class ClonePylonGrab
{
    public IObservable<Timestamped<PylonDataFrame>> Process(IObservable<Timestamped<PylonDataFrame>> source)
    {
        return source.Select(value => {
            var pylondf = new PylonDataFrame(value.Value.Image, value.Value.GrabResult.Clone());
            return new Timestamped<PylonDataFrame>(pylondf, value.Seconds);
        });
    }
}
