using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using Bonsai.Harp;

[Combinator]
[Description("Modifies targetted parts of an Harp Message.")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class ModifyMessage
{
    /// <summary>
    /// Gets or sets the address of the register to which the Harp message refers to.
    /// </summary>
    [Description("The address of the register to which the Harp message refers to.")]
    public int? Address { get; set; }

    /// <summary>
    /// Gets or sets the type of the Harp message.
    /// </summary>
    [Description("The type of the Harp message.")]
    public MessageType?  MessageTypeModified { get; set; }

    HarpMessage BuildMessage(ArraySegment<byte> payload, int address, MessageType messageType, PayloadType payloadtype)
    {
        var payloadType = payloadtype & ~PayloadType.Timestamp;
        if (messageType == MessageType.Read) return HarpMessage.FromPayload(address, messageType, payloadType);
        var data = GetPayload(payload, payloadType);
        switch (payloadType)
        {
            case PayloadType.U8: return HarpMessage.FromByte(address, messageType, (byte[])data);
            case PayloadType.S8: return HarpMessage.FromSByte(address, messageType, (sbyte[])data);
            case PayloadType.U16: return HarpMessage.FromUInt16(address, messageType, (ushort[])data);
            case PayloadType.S16: return HarpMessage.FromInt16(address, messageType, (short[])data);
            case PayloadType.U32: return HarpMessage.FromUInt32(address, messageType, (uint[])data);
            case PayloadType.S32: return HarpMessage.FromInt32(address, messageType, (int[])data);
            case PayloadType.U64: return HarpMessage.FromUInt64(address, messageType, (ulong[])data);
            case PayloadType.S64: return HarpMessage.FromInt64(address, messageType, (long[])data);
            case PayloadType.Float: return HarpMessage.FromSingle(address, messageType, (float[])data);
            default: throw new InvalidOperationException("Invalid payload type.");
        }
    }

    HarpMessage BuildTimestampedMessage(ArraySegment<byte> payload, int address, MessageType messageType, PayloadType payloadtype, double timestamp)
    {
        var payloadType = payloadtype;
        if (messageType == MessageType.Read) return HarpMessage.FromPayload(address, messageType, payloadType);
        var data = GetPayload(payload, payloadType);
        switch (payloadType)
        {
            case PayloadType.TimestampedU8: return HarpMessage.FromByte(address, timestamp, messageType, (byte[])data);
            case PayloadType.TimestampedS8: return HarpMessage.FromSByte(address, timestamp, messageType, (sbyte[])data);
            case PayloadType.TimestampedU16: return HarpMessage.FromUInt16(address, timestamp, messageType, (ushort[])data);
            case PayloadType.TimestampedS16: return HarpMessage.FromInt16(address, timestamp, messageType, (short[])data);
            case PayloadType.TimestampedU32: return HarpMessage.FromUInt32(address, timestamp, messageType, (uint[])data);
            case PayloadType.TimestampedS32: return HarpMessage.FromInt32(address, timestamp, messageType, (int[])data);
            case PayloadType.TimestampedU64: return HarpMessage.FromUInt64(address, timestamp, messageType, (ulong[])data);
            case PayloadType.TimestampedS64: return HarpMessage.FromInt64(address, timestamp, messageType, (long[])data);
            case PayloadType.TimestampedFloat: return HarpMessage.FromSingle(address, timestamp, messageType, (float[])data);
            default: throw new InvalidOperationException("Invalid payload type.");
        }
    }

    Array GetPayload(ArraySegment<byte> payload, PayloadType payloadType)
    {
        Array data;
        switch (payloadType)
        {
            case PayloadType.U8:
            case PayloadType.TimestampedU8:
                data = new byte[payload.Count];
                break;
            case PayloadType.S8:
            case PayloadType.TimestampedS8:
                data = new sbyte[payload.Count];
                break;
            case PayloadType.U16:
            case PayloadType.TimestampedU16:
                data = new ushort[payload.Count / 2];
                break;
            case PayloadType.S16:
            case PayloadType.TimestampedS16:
                data =  new short[payload.Count / 2];
                break;
            case PayloadType.U32:
            case PayloadType.TimestampedU32:
                data =  new uint[payload.Count / 4];
                break;
            case PayloadType.S32:
            case PayloadType.TimestampedS32:
                data =  new int[payload.Count / 4];
                break;
            case PayloadType.U64:
            case PayloadType.TimestampedU64:
                data = new ulong[payload.Count / 8];
                break;
            case PayloadType.S64:
            case PayloadType.TimestampedS64:
                data = new long[payload.Count / 8];
                break;
            case PayloadType.Float:
            case PayloadType.TimestampedFloat:
                data = new float[payload.Count / 4];
                break;
            default: throw new InvalidOperationException("Invalid payload type.");
        }

        Buffer.BlockCopy(payload.Array, payload.Offset, data, 0, payload.Count);
        return data;
    }

    ArraySegment<byte> ParsePayload(HarpMessage harpMessage){
        return harpMessage.GetPayload();
    }

    PayloadType GetTimestampedPayloadType(PayloadType inPayloadType){
        switch (inPayloadType){
        case PayloadType.U8: return PayloadType.TimestampedU8;
        case PayloadType.S8: return PayloadType.TimestampedS8;
        case PayloadType.U16: return  PayloadType.TimestampedU16;
        case PayloadType.S16: return  PayloadType.TimestampedS16;
        case PayloadType.U32: return  PayloadType.TimestampedU32;
        case PayloadType.S32: return  PayloadType.TimestampedS32;
        case PayloadType.U64: return PayloadType.TimestampedU64;
        case PayloadType.S64: return PayloadType.TimestampedS64;
        case PayloadType.Float: return PayloadType.TimestampedFloat;
        default:
            throw new InvalidOperationException("Invalid Harp payload type.");
        }
    }

    public IObservable<HarpMessage> Process(IObservable<HarpMessage> source)
    {
        return source.Select(value => {
            var address = Address;
            if (address == null){
                address = value.Address;
            }
            if (MessageTypeModified == null){
                MessageTypeModified = value.MessageType;
            }
            var Payload = ParsePayload(value);
            if (value.IsTimestamped){
                var timestamp = value.GetTimestamp();
                return BuildTimestampedMessage(Payload, (int) address, (MessageType) MessageTypeModified, value.PayloadType, timestamp);
            }
            else{
                return BuildMessage(Payload, (int) address, (MessageType) MessageTypeModified, value.PayloadType);
            }
        });
    }

    public IObservable<HarpMessage> Process(IObservable<Tuple<HarpMessage, double>> source)
    {
        return source.Select(value => {
            var address = Address;
            if (address == null){
                address = value.Item1.Address;
            }
            if (MessageTypeModified == null){
                MessageTypeModified = value.Item1.MessageType;
            }
            var Payload = ParsePayload(value.Item1);
            return BuildTimestampedMessage(Payload, (int) address, (MessageType) MessageTypeModified, GetTimestampedPayloadType(value.Item1.PayloadType), value.Item2);
        });
    }
}
