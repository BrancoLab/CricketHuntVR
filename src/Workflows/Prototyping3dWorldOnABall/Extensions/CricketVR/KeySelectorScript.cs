using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using OpenTK.Input;
namespace CricketVR{

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Condition)]

public class KeySelectorScript
{
    public Key KeyFilter { get; set; }
    public IObservable<KeyboardState> Process(IObservable<KeyboardState> source)
    {
        return source.Where(value =>
        {
            if (value[KeyFilter])
                return true;
            return false;
        });
    }
}
}