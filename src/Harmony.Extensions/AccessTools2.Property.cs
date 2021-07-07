﻿// <auto-generated>
//   This code file has automatically been added by the "Harmony.Extensions" NuGet package (https://www.nuget.org/packages/Harmony.Extensions).
//   Please see https://github.com/BUTR/Harmony.Extensions for more information.
//
//   IMPORTANT:
//   DO NOT DELETE THIS FILE if you are using a "packages.config" file to manage your NuGet references.
//   Consider migrating to PackageReferences instead:
//   https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference
//   Migrating brings the following benefits:
//   * The "Harmony.Extensions" folder and the "AccessTools2.Property.cs" file don't appear in your project.
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
        /// <summary>Gets the reflection information for a directly declared property</summary>
        /// <param name="type">The class/type where the property is declared</param>
        /// <param name="name">The name of the property (case sensitive)</param>
        /// <returns>A property or null when type/name is null or when the property cannot be found</returns>
        public static PropertyInfo? DeclaredProperty(Type type, string name)
        {
            if (type is null)
            {
                Trace.TraceError("AccessTools2.DeclaredProperty: 'type' is null");
                return null;
            }
            if (name is null)
            {
                Trace.TraceError($"AccessTools2.DeclaredProperty: type '{type}', 'name' is null");
                return null;
            }
            var property = type.GetProperty(name, AccessTools.allDeclared);
            if (property is null)
                Trace.TraceError($"AccessTools2.DeclaredProperty: Could not find property for type '{type}' and name '{name}'");
            return property;
        }

        /// <summary>Gets the reflection information for the getter method of a directly declared property</summary>
        /// <param name="type">The class/type where the property is declared</param>
        /// <param name="name">The name of the property (case sensitive)</param>
        /// <returns>A method or null when type/name is null or when the property cannot be found</returns>
        public static MethodInfo? DeclaredPropertyGetter(Type type, string name) => DeclaredProperty(type, name)?.GetGetMethod(true);

        /// <summary>Gets the reflection information for the setter method of a directly declared property</summary>
        /// <param name="type">The class/type where the property is declared</param>
        /// <param name="name">The name of the property (case sensitive)</param>
        /// <returns>A method or null when type/name is null or when the property cannot be found</returns>
        public static MethodInfo? DeclaredPropertySetter(Type type, string name) => DeclaredProperty(type, name)?.GetSetMethod(true);

        /// <summary>Gets the reflection information for a property by searching the type and all its super types</summary>
        /// <param name="type">The class/type</param>
        /// <param name="name">The name</param>
        /// <returns>A property or null when type/name is null or when the property cannot be found</returns>
        public static PropertyInfo? Property(Type type, string name)
        {
            if (type is null)
            {
                Trace.TraceError("AccessTools2.Property: 'type' is null");
                return null;
            }
            if (name is null)
            {
                Trace.TraceError($"AccessTools2.Property: type '{type}', 'name' is null");
                return null;
            }
            var property = FindIncludingBaseTypes(type, t => t.GetProperty(name, AccessTools.all));
            if (property is null)
                Trace.TraceError($"AccessTools2.Property: Could not find property for type '{type}' and name '{name}'");
            return property;
        }

        /// <summary>Gets the reflection information for the getter method of a property by searching the type and all its super types</summary>
        /// <param name="type">The class/type</param>
        /// <param name="name">The name</param>
        /// <returns>A method or null when type/name is null or when the property cannot be found</returns>
        public static MethodInfo? PropertyGetter(Type type, string name) => Property(type, name)?.GetGetMethod(true);

        /// <summary>Gets the reflection information for the setter method of a property by searching the type and all its super types</summary>
        /// <param name="type">The class/type</param>
        /// <param name="name">The name</param>
        /// <returns>A method or null when type/name is null or when the property cannot be found</returns>
        public static MethodInfo? PropertySetter(Type type, string name) => Property(type, name)?.GetSetMethod(true);


        public static PropertyInfo? Property(string typeColonPropertyName)
        {
            if (!TryGetComponents(typeColonPropertyName, out var type, out var name))
            {
                Trace.TraceError($"AccessTools2.Property: Could not find type or property for '{typeColonPropertyName}'");
                return null;
            }

            return DeclaredProperty(type, name);
        }

        public static MethodInfo? PropertyGetter(string typeColonPropertyName) => Property(typeColonPropertyName)?.GetGetMethod(true);

        public static MethodInfo? PropertySetter(string typeColonPropertyName) => Property(typeColonPropertyName)?.GetSetMethod(true);
    }
}

#pragma warning restore
#nullable restore
#endif // HARMONYEXTENSIONS_DISABLE