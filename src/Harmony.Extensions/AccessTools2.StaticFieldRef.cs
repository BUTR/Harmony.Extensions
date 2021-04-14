﻿// <auto-generated>
//   This code file has automatically been added by the "Harmony.Extensions" NuGet package (https://www.nuget.org/packages/Harmony.Extensions).
//   Please see https://github.com/BUTR/Harmony.Extensions for more information.
//
//   IMPORTANT:
//   DO NOT DELETE THIS FILE if you are using a "packages.config" file to manage your NuGet references.
//   Consider migrating to PackageReferences instead:
//   https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference
//   Migrating brings the following benefits:
//   * The "Harmony.Extensions" folder and the "AccessTools2.cs" file don't appear in your project.
//   * The added file is immutable and can therefore not be modified by coincidence.
//   * Updating/Uninstalling the package will work flawlessly.
// </auto-generated>

#region License
// MIT License
//
// Copyright (c) Bannerlord's Unofficial Tools & Resources, Andreas Pardeike
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

#if !HARMONYEXTENSIONS_DISABLE
#nullable enable
#if !HARMONYEXTENSIONS_ENABLEWARNINGS
#pragma warning disable
#endif

namespace HarmonyLib.BUTR.Extensions
{
    using global::System;
    using global::System.Diagnostics;
    using global::System.Reflection;
    using global::System.Reflection.Emit;

    /// <summary>An extension of Harmony's helper class for reflection related functions</summary>
#if !HARMONYEXTENSIONS_PUBLIC
    internal
#else
    public
#endif
        static partial class AccessTools2
    {
        /// <summary>Creates a static field reference delegate</summary>
		/// <typeparam name="F">
		/// The type of the field; or if the field's type is a reference type (a class or interface, NOT a struct or other value type),
		/// a type that <see cref="Type.IsAssignableFrom(Type)">is assignable from</see> that type; or if the field's type is an enum type,
		/// either that type or the underlying integral type of that enum type
		/// </typeparam>
		/// <param name="fieldInfo">The field</param>
		/// <returns>A readable/assignable <see cref="AccessTools.FieldRef{F}"/> delegate</returns>
		///
		public static AccessTools.FieldRef<F>? StaticFieldRefAccess<F>(FieldInfo fieldInfo)
        {
            if (fieldInfo is null)
                return null;

            return StaticFieldRefAccessInternal<F>(fieldInfo);
        }

        /// <summary>Creates a static field reference delegate</summary>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="type">The type holding the field field</param>
        /// <param name="fieldName">The field name</param>
        /// <returns>A read and writable <see cref="T:HarmonyLib.AccessTools.FieldRef`1" /> delegate</returns>
        public static AccessTools.FieldRef<TField>? StaticFieldRefAccess<TField>(Type type, string fieldName)
        {
            var fieldInfo = Field(type, fieldName);
            if (fieldInfo is null)
                return null;

            return StaticFieldRefAccess<TField>(fieldInfo);
        }


        private static AccessTools.FieldRef<F>? StaticFieldRefAccessInternal<F>(FieldInfo fieldInfo)
        {
            if (!Helper.IsValid())
            {
                return null;
            }

            if (!fieldInfo.IsStatic)
            {
                Trace.TraceError("AccessTools2.StaticFieldRefAccessInternal: Field must be static");
                return null;
            }

            if (!ValidateFieldType<F>(fieldInfo))
            {
                return null;
            }

            var dm = DynamicMethodDefinitionHandle.Create(
                $"__refget_{fieldInfo.DeclaringType?.Name ?? "null"}_static_fi_{fieldInfo.Name}", typeof(F).MakeByRefType(), new Type[0]);

            if (dm?.GetILGenerator() is not { } il)
                return null;

            il.Emit(OpCodes.Ldsflda, fieldInfo);
            il.Emit(OpCodes.Ret);

            //return dm?.Generate() is { } methodInfo ? GetDelegate<AccessTools.FieldRef<F>>(methodInfo) : null;
            return dm?.Generate()?.CreateDelegate(typeof(AccessTools.FieldRef<F>)) as AccessTools.FieldRef<F>;
        }
    }
}

#pragma warning restore
#nullable restore
#endif // HARMONYEXTENSIONS_DISABLE