using System.ComponentModel;
using System.Xml;
using System.Xml.Linq;

namespace HyperSoa.Contracts.Legacy {
    public class LegacyHyperNodeMessage {
        protected readonly string nil = "nil";

        protected static XNamespace xn_s = "http://schemas.xmlsoap.org/soap/envelope/";
        protected static XNamespace xn_t = "http://tempuri.org/";
        protected static XNamespace xn_a = "http://schemas.datacontract.org/2004/07/Hyper.NodeServices.Contracts";
        protected static XNamespace xn_i = "http://www.w3.org/2001/XMLSchema-instance";

        protected T? ParseElementValue<T>(XElement? xel) {
            if (xel != null) {
                XAttribute? xatt = xel.Attribute(xn_i + nil);
                if (xatt != null && xatt.Value != "true") {
                    return default;
                }
                if (typeof(T).IsEnum) {
                    return (T)Enum.Parse(typeof(T), xel.Value);
                }
                if (typeof(T) == typeof(TimeSpan)) {
                    if (string.IsNullOrWhiteSpace(xel.Value)) {
                        return default;
                    } else {
                        return (T)(object)XmlConvert.ToTimeSpan(xel.Value);
                    }
                }

                return (T?)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(xel.Value);
            }
            return default;
        }
    }
}
