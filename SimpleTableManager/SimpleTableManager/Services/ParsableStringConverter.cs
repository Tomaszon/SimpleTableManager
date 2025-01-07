using System.ComponentModel;
using System.Globalization;

namespace SimpleTableManager.Services
{
    public class ParsableStringConverter<T> : TypeConverter
    where T : IParsable<T>
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            return value is string s ?
                (object)T.Parse(s, culture) :
                throw new NotSupportedException();
        }
    }
}