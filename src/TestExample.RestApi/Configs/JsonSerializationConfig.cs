using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace TestExample.RestApi.Configs
{
    internal class JsonSerializationConfig : Configuration
    {
        public override void ConfigureServiceContainer(IServiceCollection container)
        {
            container.AddMvcCore().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.IgnoreNullValues = true;

                options.JsonSerializerOptions.Converters.Add(new EnumConverterFactory());
                options.JsonSerializerOptions.Converters.Add(new UtcDateTimeConverter());
            });
        }
    }

    internal class UtcDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetDateTime().ToUniversalTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            var zonedDateValue = GetZonedDate(value);
            writer.WriteStringValue(zonedDateValue.ToUniversalTime());
        }

        private DateTime GetZonedDate(DateTime value)
        {
            return value.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(value, DateTimeKind.Utc) : value;
        }
    }

    internal class EnumConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
    {
        private readonly Type _enumType;
        private readonly TypeCode _enumTypeCode;

        public EnumConverter()
        {
            _enumType = typeof(TEnum);
            _enumTypeCode = Type.GetTypeCode(typeof(TEnum));
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return _enumTypeCode switch
            {
                TypeCode.Byte => Read(reader.GetByte()),
                TypeCode.SByte => Read(reader.GetSByte()),
                TypeCode.Int16 => Read(reader.GetInt16()),
                TypeCode.UInt16 => Read(reader.GetUInt16()),
                TypeCode.Int32 => Read(reader.GetInt32()),
                TypeCode.UInt32 => Read(reader.GetUInt32()),
                TypeCode.Int64 => Read(reader.GetInt64()),
                TypeCode.UInt64 => Read(reader.GetUInt64()),
                _ => throw new InvalidOperationException()
            };
        }

        private void CheckValueIsValid<T>(T value)
        {
            if (!Enum.IsDefined(_enumType, value))
                throw new InvalidOperationException($"Invalid enumeration '{_enumType.Name}' value '{value}'");
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            switch (_enumTypeCode)
            {
                case TypeCode.Byte:
                    writer.WriteNumberValue(As<byte>(value));
                    break;
                case TypeCode.SByte:
                    writer.WriteNumberValue(As<sbyte>(value));
                    break;
                case TypeCode.Int16:
                    writer.WriteNumberValue(As<short>(value));
                    break;
                case TypeCode.UInt16:
                    writer.WriteNumberValue(As<ushort>(value));
                    break;
                case TypeCode.Int32:
                    writer.WriteNumberValue(As<int>(value));
                    break;
                case TypeCode.UInt32:
                    writer.WriteNumberValue(As<uint>(value));
                    break;
                case TypeCode.Int64:
                    writer.WriteNumberValue(As<long>(value));
                    break;
                case TypeCode.UInt64:
                    writer.WriteNumberValue(As<ulong>(value));
                    break;
                default: throw new InvalidOperationException();
            }
        }

        private TEnum Read<T>(T value)
        {
            CheckValueIsValid(value);
            return Enum.Parse<TEnum>(value.ToString());
        }

        private TTo As<TTo>(object value)
        {
            CheckValueIsValid(value);
            return (TTo) value;
        }
    }

    internal class EnumConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converterType = typeof(EnumConverter<>).MakeGenericType(typeToConvert);
            return Activator.CreateInstance(converterType) as JsonConverter;
        }
    }
}