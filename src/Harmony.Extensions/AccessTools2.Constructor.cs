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

    /// <summary>An extension of Harmony's helper class for reflection related functions</summary>
    internal static partial class AccessTools2
    {
        /// <summary>Gets the reflection information for a directly declared constructor</summary>
		/// <param name="type">The class/type where the constructor is declared</param>
		/// <param name="parameters">Optional parameters to target a specific overload of the constructor</param>
		/// <param name="searchForStatic">Optional parameters to only consider static constructors</param>
		/// <returns>A constructor info or null when type is null or when the constructor cannot be found</returns>
        public static ConstructorInfo? DeclaredConstructor(Type type, Type[]? parameters = null, bool searchForStatic = false)
		{
			if (type is null)
			{
                Trace.TraceError("AccessTools2.DeclaredConstructor: 'type' is null");
				return null;
			}

			if (parameters is null) parameters = Array.Empty<Type>();
			var flags = searchForStatic ? AccessTools.allDeclared & ~BindingFlags.Instance : AccessTools.allDeclared & ~BindingFlags.Static;
			return type.GetConstructor(flags, null, parameters, Array.Empty<ParameterModifier>());
		}

		/// <summary>Gets the reflection information for a constructor by searching the type and all its super types</summary>
		/// <param name="type">The class/type where the constructor is declared</param>
		/// <param name="parameters">Optional parameters to target a specific overload of the method</param>
		/// <param name="searchForStatic">Optional parameters to only consider static constructors</param>
		/// <returns>A constructor info or null when type is null or when the method cannot be found</returns>
		///
		public static ConstructorInfo? Constructor(Type type, Type[]? parameters = null, bool searchForStatic = false)
		{
			if (type is null)
			{
                Trace.TraceError("AccessTools2.ConstructorInfo: 'type' is null");
				return null;
			}

			if (parameters is null) parameters = Array.Empty<Type>();
			var flags = searchForStatic ? AccessTools.all & ~BindingFlags.Instance : AccessTools.all & ~BindingFlags.Static;
			return FindIncludingBaseTypes(type, t => t.GetConstructor(flags, null, parameters, Array.Empty<ParameterModifier>()));
		}


        /// <summary>Gets the reflection information for a constructor by searching the type and all its super types</summary>
        /// <param name="typeString">The class/type full name where the constructor is declared</param>
        /// <param name="parameters">Optional parameters to target a specific overload of the method</param>
        /// <param name="searchForStatic">Optional parameters to only consider static constructors</param>
        /// <returns>A constructor info or null when type is null or when the method cannot be found</returns>
        public static ConstructorInfo? Constructor(string typeString, Type[]? parameters = null, bool searchForStatic = false)
        {
            if (string.IsNullOrWhiteSpace(typeString))
            {
                Trace.TraceError("AccessTools2.Constructor: 'typeString' is null or whitespace/empty");
                return null;
            }

            var type = TypeByName(typeString);
            if (type is null)
                return null;

            return DeclaredConstructor(type, parameters, searchForStatic);
        }
    }
}

#pragma warning restore
#nullable restore
#endif // HARMONYEXTENSIONS_DISABLE