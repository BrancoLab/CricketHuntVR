using Bonsai;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using OpenTK.Input;
using System.Reflection;
using System.Collections.Generic;
using OpenTK;
using System.Linq;

[Combinator]
[Description("Generates a sequence of mouse state objects from the specified OS-specific ID.")]
[WorkflowElementCategory(ElementCategory.Combinator)]
public class WinRawMouse
{
    [TypeConverter(typeof(DeviceIdConverter))]
    [Description("The OS-specific ID of the mouse device. If it is not specified, the combined state of all devices is retrieved.")]
    public int? DeviceId { get; set; }

    static MouseState GetMouseState(int? id)
    {
        var index = id.HasValue ? RawMouseLookup.GetDeviceIndex(id.Value) : null;
        if (index.HasValue) return OpenTK.Input.Mouse.GetState(index.Value);
        else return OpenTK.Input.Mouse.GetState();
    }

    public IObservable<MouseState> Process<TSource>(IObservable<TSource> source)
    {
        return source.Select(input => GetMouseState(DeviceId));
    }

    class DeviceIdConverter : Int32Converter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var deviceList = RawMouseLookup.GetDeviceList().Select(pair => pair.Key).ToArray();
            return new StandardValuesCollection(deviceList);
        }
    }

    static class RawMouseLookup
    {
        static readonly Dictionary<ContextHandle, int> lookup = InitLookup();

        static Dictionary<ContextHandle, int> InitLookup()
        {
            var driverField = typeof(OpenTK.Input.Mouse).GetField("driver", BindingFlags.NonPublic | BindingFlags.Static);
            var driver = driverField != null ? driverField.GetValue(null) : null;
            if (driver != null)
            {
                return (Dictionary<ContextHandle, int>)driver
                    .GetType()
                    .GetField("rawids", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(driver);
            }

            return new Dictionary<ContextHandle, int>();
        }

        public static int? GetDeviceIndex(int deviceId)
        {
            int index;
            if (lookup.TryGetValue(new ContextHandle((IntPtr)deviceId), out index))
            {
                return index;
            }

            return null;
        }

        public static IEnumerable<KeyValuePair<ContextHandle, int>> GetDeviceList()
        {
            return lookup.OrderBy(pair => pair.Value);
        }
    }
}
