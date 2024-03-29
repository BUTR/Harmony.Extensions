﻿// <auto-generated>
//   This code file has automatically been added by the "Harmony.Extensions" NuGet package (https://www.nuget.org/packages/Harmony.Extensions).
//   Please see https://github.com/BUTR/Harmony.Extensions for more information.
//
//   IMPORTANT:
//   DO NOT DELETE THIS FILE if you are using a "packages.config" file to manage your NuGet references.
//   Consider migrating to PackageReferences instead:
//   https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference
//   Migrating brings the following benefits:
//   * The "Harmony.Extensions" folder and the "AccessTools2.FieldRef.cs" file don't appear in your project.
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
        public static AccessTools.FieldRef<object, F>? FieldRefAccess<F>(string typeColonFieldname, bool logErrorInTrace = true)
        {
            if (!TryGetComponents(typeColonFieldname, out var type, out var name, logErrorInTrace: logErrorInTrace))
            {
                Trace.TraceError($"AccessTools2.FieldRefAccess: Could not find type or field for '{typeColonFieldname}'");
                return null;
            }

            return FieldRefAccess<F>(type, name, logErrorInTrace: logErrorInTrace);
        }
    
        /// <summary>Creates a field reference delegate for an instance field of a class</summary>
        /// <typeparam name="T">The class that defines the instance field, or derived class of this type</typeparam>
        /// <typeparam name="F">
        /// The type of the field; or if the field's type is a reference type (a class or interface, NOT a struct or other value type),
        /// a type that <see cref="Type.IsAssignableFrom(Type)">is assignable from</see> that type; or if the field's type is an enum type,
        /// either that type or the underlying integral type of that enum type
        /// </typeparam>
        /// <param name="fieldName">The name of the field</param>
        /// <returns>A readable/assignable <see cref="AccessTools.FieldRef{T,F}"/> delegate</returns>
        public static AccessTools.FieldRef<T, F>? FieldRefAccess<T, F>(string fieldName, bool logErrorInTrace = true) where T : class
        {
            if (fieldName is null)
                return null;

            var field = GetInstanceField(typeof(T), fieldName, logErrorInTrace: logErrorInTrace);
            if (field is null)
                return null;

            return FieldRefAccessInternal<T, F>(field, needCastclass: false, logErrorInTrace: logErrorInTrace);
        }

        /// <summary>Creates a field reference delegate for an instance field of a class or static field (NOT an instance field of a struct)</summary>
        /// <typeparam name="F">
        /// The type of the field; or if the field's type is a reference type (a class or interface, NOT a struct or other value type),
        /// a type that <see cref="Type.IsAssignableFrom(Type)">is assignable from</see> that type; or if the field's type is an enum type,
        /// either that type or the underlying integral type of that enum type
        /// </typeparam>
        /// <param name="type">
        /// The type that defines the field, or derived class of this type; must not be a struct type unless the field is static
        /// </param>
        /// <param name="fieldName">The name of the field</param>
        /// <returns>
        /// A readable/assignable <see cref="AccessTools.FieldRef{T,F}"/> delegate with <c>T=object</c>
        /// (for static fields, the <c>instance</c> delegate parameter is ignored)
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method is meant for cases where the given type is only known at runtime and thus can't be used as a type parameter <c>T</c>
        /// in e.g. <see cref="FieldRefAccess{T, F}(string)"/>.
        /// </para>
        /// </remarks>
        public static AccessTools.FieldRef<object, F>? FieldRefAccess<F>(Type type, string fieldName, bool logErrorInTrace = true)
        {
            if (type is null)
                return null;

            if (fieldName is null)
                return null;

            var fieldInfo = Field(type, fieldName, logErrorInTrace: logErrorInTrace);
            if (fieldInfo is null)
                return null;

            if (!fieldInfo.IsStatic && fieldInfo.DeclaringType is Type declaringType)
            {
                // When fieldInfo is passed to FieldRefAccess methods, the T generic class constraint is insufficient to ensure that
                // the field is not a struct instance field, since T could be object, ValueType, or an interface that the struct implements.
                if (declaringType.IsValueType)
                {
                    if (logErrorInTrace)
                        Trace.TraceError($"AccessTools2.FieldRefAccess<object, {typeof(F).FullName}>: FieldDeclaringType must be a class");
                    return null;
                }

                // Field's declaring type cannot be object, since object has no fields, so always need a castclass for T=object.
                return FieldRefAccessInternal<object, F>(fieldInfo, needCastclass: true, logErrorInTrace: logErrorInTrace);
            }

            return null;
        }

        /// <summary>Creates a field reference delegate for an instance field of a class or static field (NOT an instance field of a struct)</summary>
        /// <typeparam name="T">
        /// An arbitrary type if the field is static; otherwise the class that defines the field, or a parent class (including <see cref="object"/>),
        /// implemented interface, or derived class of this type ("<c>instanceOfT is FieldDeclaringType</c>" must be possible)
        /// </typeparam>
        /// <typeparam name="F">
        /// The type of the field; or if the field's type is a reference type (a class or interface, NOT a struct or other value type),
        /// a type that <see cref="Type.IsAssignableFrom(Type)">is assignable from</see> that type; or if the field's type is an enum type,
        /// either that type or the underlying integral type of that enum type
        /// </typeparam>
        /// <param name="fieldInfo">The field</param>
        /// <returns>A readable/assignable <see cref="AccessTools.FieldRef{T,F}"/> delegate</returns>
        /// <remarks>
        /// <para>
        /// This method is meant for cases where the field has already been obtained, avoiding the field searching cost in
        /// e.g. <see cref="FieldRefAccess{T, F}(string)"/>.
        /// </para>
        /// </remarks>
        public static AccessTools.FieldRef<T, F>? FieldRefAccess<T, F>(FieldInfo fieldInfo, bool logErrorInTrace = true) where T : class
        {
            if (fieldInfo is null)
                return null;

            if (!fieldInfo.IsStatic && fieldInfo.DeclaringType is Type declaringType)
            {
                // When fieldInfo is passed to FieldRefAccess methods, the T generic class constraint is insufficient to ensure that
                // the field is not a struct instance field, since T could be object, ValueType, or an interface that the struct implements.
                if (declaringType.IsValueType)
                {
                    if (logErrorInTrace)
                        Trace.TraceError($"AccessTools2.FieldRefAccess<{typeof(T).FullName}, {typeof(F).FullName}>: FieldDeclaringType must be a class");
                    return null;
                }

                if (FieldRefNeedsClasscast(typeof(T), declaringType, logErrorInTrace: logErrorInTrace) is not { } needCastclass)
                {
                    return null;
                }

                return FieldRefAccessInternal<T, F>(fieldInfo, needCastclass, logErrorInTrace: logErrorInTrace);
            }

            return null;
        }

        private static AccessTools.FieldRef<T, F>? FieldRefAccessInternal<T, F>(FieldInfo fieldInfo, bool needCastclass, bool logErrorInTrace = true) where T : class
        {
            if (!Helper.IsValid(logErrorInTrace))
            {
                return null;
            }

            if (fieldInfo.IsStatic)
            {
                if (logErrorInTrace)
                    Trace.TraceError($"AccessTools2.FieldRefAccessInternal<{typeof(T).FullName}, {typeof(F).FullName}>: Field must not be static");
                return null;
            }

            if (!ValidateFieldType<F>(fieldInfo, logErrorInTrace: logErrorInTrace))
            {
                return null;
            }
            
            var delegateInstanceType = typeof(T);
            var declaringType = fieldInfo.DeclaringType;

            var dm = DynamicMethodDefinitionHandle.Create(
                $"__refget_{delegateInstanceType.Name}_fi_{fieldInfo.Name}", typeof(F).MakeByRefType(), new[] { delegateInstanceType });

            if (dm?.GetILGenerator() is not { } il)
                return null;

            il.Emit(OpCodes.Ldarg_0);
            // The castclass is needed when T is a parent class or interface of declaring type (e.g. if T is object),
            // since there's no guarantee the instance passed to the delegate is actually of the declaring type.
            // In such a situation, the castclass will throw an InvalidCastException and thus prevent undefined behavior.
            if (needCastclass)
                il.Emit(OpCodes.Castclass, declaringType);
            il.Emit(OpCodes.Ldflda, fieldInfo);
            il.Emit(OpCodes.Ret);
            
            //return dm?.Generate() is { } methodInfo ? GetDelegate<AccessTools.FieldRef<T, F>>(methodInfo) : null;
            return dm?.Generate()?.CreateDelegate(typeof(AccessTools.FieldRef<T, F>)) as AccessTools.FieldRef<T, F>;
        }

        private static bool? FieldRefNeedsClasscast(Type delegateInstanceType, Type declaringType, bool logErrorInTrace = true)
        {
            var needCastclass = false;
            if (delegateInstanceType != declaringType)
            {
                needCastclass = delegateInstanceType.IsAssignableFrom(declaringType);
                if (needCastclass is false && declaringType.IsAssignableFrom(delegateInstanceType) is false)
                {
                    if (logErrorInTrace)
                        Trace.TraceError($"AccessTools2.FieldRefNeedsClasscast: FieldDeclaringType must be assignable from or to T (FieldRefAccess instance type) - 'instanceOfT is FieldDeclaringType' must be possible, delegateInstanceType '{delegateInstanceType}', declaringType '{declaringType}'");
                    return null;
                }
            }
            return needCastclass;
        }


        /// <summary>Creates an instance field reference delegate for a private type</summary>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="fieldInfo">The field</param>
        /// <returns>A read and writable <see cref="T:HarmonyLib.AccessTools.FieldRef`2" /> delegate</returns>
        public static AccessTools.FieldRef<object, TField>? FieldRefAccess<TField>(FieldInfo fieldInfo) => fieldInfo is null ? null : AccessTools.FieldRefAccess<object, TField>(fieldInfo);
    }
}

#pragma warning restore
#nullable restore
#endif // HARMONYEXTENSIONS_DISABLE