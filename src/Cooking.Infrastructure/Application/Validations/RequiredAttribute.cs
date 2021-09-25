using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Reflection.Emit;

namespace Cooking.Infrastructure.Application.Validations
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class RequiredAttribute : ValidationAttribute
    {
        public bool AllowEmptyStrings { get; set; } = false;
        public bool AllowDefaultValues { get; set; } = true;

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            var type = value.GetType();

            if (type.IsEnum)
                return Enum.IsDefined(type, value);

            if (value is string str && AllowEmptyStrings == false)
                return string.IsNullOrWhiteSpace(str) == false;

            if (AllowDefaultValues == false)
                return value.Equals(type.DefaultValue()) == false;

            return true;
        }
    }

    public static class TypeUtils
    {
        private static readonly Lazy<ModuleBuilder> _dynamicModuleBuilder;
        private static readonly MethodInfo _getDefaultValueMethod;

        static TypeUtils()
        {
            _dynamicModuleBuilder = new Lazy<ModuleBuilder>(() =>
            {
                var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                    new AssemblyName($"Assembly_{Guid.NewGuid():N}"),
                    AssemblyBuilderAccess.Run);

                var moduleBuilder = assemblyBuilder.DefineDynamicModule(
                    $"DynamicModule_{Guid.NewGuid():N}");

                return moduleBuilder;
            });

            _getDefaultValueMethod = typeof(TypeUtils).GetMethod(nameof(GetDefaultValue),
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod);
        }

        private static T GetDefaultValue<T>()
        {
            return default;
        }

        public static object DefaultValue(this Type type)
        {
            return _getDefaultValueMethod.MakeGenericMethod(type).Invoke(null, new object[0]);
        }
    }
}