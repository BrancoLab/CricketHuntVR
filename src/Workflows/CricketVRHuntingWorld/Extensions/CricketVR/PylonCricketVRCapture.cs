using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using Bonsai.Pylon;
using Bonsai.Harp;

    [Description("Configures and initializes a Pylon camera for triggered acquisition.")]
    public class PylonCricketVRCapture : PylonCapture
    {
        public PylonCricketVRCapture()
        {
        }

        [Description("Harp event address that will be used to pair camera data with")]
        private int address;
        public int HarpAddress
        {
            get { return address; }
            set { address = value; }
        }
        public IObservable<Timestamped<PylonDataFrame>> Generate(IObservable<HarpMessage> source)
        {
            var frames = Generate();
            var triggers = source.Where(address, MessageType.Event);
            return frames.Zip(triggers, (frame, trigger) =>
            {
                var payload = trigger.GetTimestampedPayloadByte();
                return Timestamped.Create(frame, payload.Seconds);
            });
        }
    }