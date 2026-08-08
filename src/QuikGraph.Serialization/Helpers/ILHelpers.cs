using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;


namespace QuikGraph.Serialization
{
    /// <summary>
    /// Helpers to manipulate <see cref="ILGenerator"/> and dynamic stuff.
    /// </summary>
    public static class ILHelpers
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <exception cref="NotSupportedException"></exception>
        public static void EmitValue(ILGenerator generator, PropertyInfo property, object value)
        {
            Debug.Assert(generator != null);
            Debug.Assert(property != null);

            switch (Type.GetTypeCode(property.PropertyType))
            {
                case TypeCode.Int32:
                    generator.Emit(OpCodes.Ldc_I4, (int)value);
                    break;
                case TypeCode.Int64:
                    generator.Emit(OpCodes.Ldc_I8, (long)value);
                    break;
                case TypeCode.Single:
                    generator.Emit(OpCodes.Ldc_R4, (float)value);
                    break;
                case TypeCode.Double:
                    generator.Emit(OpCodes.Ldc_R8, (double)value);
                    break;
                case TypeCode.String:
                    generator.Emit(OpCodes.Ldstr, (string)value);
                    break;
                case TypeCode.Boolean:
                    generator.Emit((bool)value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
                    break;
                default:
                    throw new NotSupportedException($"Unsupported type {property.PropertyType.FullName}.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="method"></param>
        public static void EmitCall(ILGenerator generator, MethodInfo method)
        {
            // Call the method passing the object on the stack (only virtual if needed)
            generator.Emit(
                method.IsFinal || !method.IsVirtual
                    ? OpCodes.Call
                    : OpCodes.Callvirt,
                method);
        }
    }
}