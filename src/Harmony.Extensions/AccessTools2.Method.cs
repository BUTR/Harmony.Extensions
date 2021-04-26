﻿// <auto-generated>
//   This code file has automatically been added by the "Harmony.Extensions" NuGet package (https://www.nuget.org/packages/Harmony.Extensions).
//   Please see https://github.com/BUTR/Harmony.Extensions for more information.
//
//   IMPORTANT:
//   DO NOT DELETE THIS FILE if you are using a "packages.config" file to manage your NuGet references.
//   Consider migrating to PackageReferences instead:
//   https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference
//   Migrating brings the following benefits:
//   * The "Harmony.Extensions" folder and the "AccessTools2.Method.cs" file don't appear in your project.
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

    /// <summary>An extension of Harmony's helper class for reflection related functions</summary>
#if !HARMONYEXTENSIONS_PUBLIC
    internal
#else
    public
#endif
        static partial class AccessTools2
    {
        /// <summary>Gets the reflection information for a directly declared method</summary>
        /// <param name="type">The class/type where the method is declared</param>
        /// <param name="name">The name of the method (case sensitive)</param>
        /// <param name="parameters">Optional parameters to target a specific overload of the method</param>
        /// <param name="generics">Optional list of types that define the generic version of the method</param>
        /// <returns>A method or null when type/name is null or when the method cannot be found</returns>
        public static MethodInfo? DeclaredMethod(Type type, string name, Type[]? parameters = null, Type[]? generics = null)
        {
            if (type is null)
            {
                Trace.TraceError("AccessTools2.DeclaredMethod: 'type' is null");
                return null;
            }
            if (name is null)
            {
                Trace.TraceError("AccessTools2.DeclaredMethod: 'name' is null");
                return null;
            }

            MethodInfo? result;
            if (parameters is null)
            {
                try
                {
                    result = type.GetMethod(name, AccessTools.allDeclared);
                }
                catch (AmbiguousMatchException ex)
                {
                    result = type.GetMethod(name, AccessTools.allDeclared, null, Type.EmptyTypes, new ParameterModifier[0]);
                    if (result is null)
                    {
                        Trace.TraceError($"AccessTools2.DeclaredMethod: Ambiguous match for type '{type}' and name '{name}' and parameters '{parameters?.Description()}', '{ex}'");
                        return null;
                    }
                }
            }
            else
            {
                result = type.GetMethod(name, AccessTools.allDeclared, null, parameters, new ParameterModifier[0]);
            }

            if (result is null)
            {
                Trace.TraceError($"AccessTools2.DeclaredMethod: Could not find method for type '{type}' and name '{name}' and parameters '{parameters?.Description()}'");
                return null;
            }

            if (generics is object) result = result.MakeGenericMethod(generics);
            return result;
        }

        /// <summary>Gets the reflection information for a method by searching the type and all its super types</summary>
        /// <param name="type">The class/type where the method is declared</param>
        /// <param name="name">The name of the method (case sensitive)</param>
        /// <param name="parameters">Optional parameters to target a specific overload of the method</param>
        /// <param name="generics">Optional list of types that define the generic version of the method</param>
        /// <returns>A method or null when type/name is null or when the method cannot be found</returns>
        public static MethodInfo? Method(Type type, string name, Type[]? parameters = null, Type[]? generics = null)
        {
            if (type is null)
            {
                Trace.TraceError("AccessTools2.Method: 'type' is null");
                return null;
            }
            if (name is null)
            {
                Trace.TraceError("AccessTools2.Method: 'name' is null");
                return null;
            }

            MethodInfo? result;
            if (parameters is null)
            {
                try
                {
                    result = FindIncludingBaseTypes(type, t => t.GetMethod(name, AccessTools.all));
                }
                catch (AmbiguousMatchException ex)
                {
                    result = FindIncludingBaseTypes(type, t => t.GetMethod(name, AccessTools.all, null, Type.EmptyTypes, new ParameterModifier[0]));
                    if (result is null)
                    {
                        Trace.TraceError($"AccessTools2.Method: Ambiguous match for type '{type}' and name '{name}' and parameters '{parameters?.Description()}', '{ex}'");
                        return null;
                    }
                }
            }
            else
            {
                result = FindIncludingBaseTypes(type, t => t.GetMethod(name, AccessTools.all, null, parameters, new ParameterModifier[0]));
            }

            if (result is null)
            {
                Trace.TraceError($"AccessTools2.Method: Could not find method for type '{type}' and name '{name}' and parameters '{parameters?.Description()}'");
                return null;
            }

            if (generics is object) result = result.MakeGenericMethod(generics);
            return result;
        }


        /// <summary>Gets the reflection information for a method by searching the type and all its super types</summary>
        /// <param name="typeColonMethodname">The target method in the form <c>TypeFullName:MethodName</c>, where the type name matches a form recognized by <a href="https://docs.microsoft.com/en-us/dotnet/api/system.type.gettype">Type.GetType</a> like <c>Some.Namespace.Type</c>.</param>
        /// <param name="parameters">Optional parameters to target a specific overload of the method</param>
        /// <param name="generics">Optional list of types that define the generic version of the method</param>
        /// <returns>A method or null when type/name is null or when the method cannot be found</returns>
        public static MethodInfo? Method(string typeColonMethodname, Type[]? parameters = null, Type[]? generics = null)
        {
            if (!TryGetComponents(typeColonMethodname, out var type, out var name))
            {
                Trace.TraceError($"AccessTools2.Method: Could not find type or property for '{typeColonMethodname}'");
                return null;
            }
            
            return DeclaredMethod(type, name, parameters, generics);
        }
    }
}

#pragma warning restore
#nullable restore
#endif // HARMONYEXTENSIONS_DISABLE