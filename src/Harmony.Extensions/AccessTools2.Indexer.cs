﻿// <auto-generated>
//   This code file has automatically been added by the "Harmony.Extensions" NuGet package (https://www.nuget.org/packages/Harmony.Extensions).
//   Please see https://github.com/BUTR/Harmony.Extensions for more information.
//
//   IMPORTANT:
//   DO NOT DELETE THIS FILE if you are using a "packages.config" file to manage your NuGet references.
//   Consider migrating to PackageReferences instead:
//   https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference
//   Migrating brings the following benefits:
//   * The "Harmony.Extensions" folder and the "AccessTools2.Indexer.cs" file don't appear in your project.
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
    using global::System.Linq;

    /// <summary>An extension of Harmony's helper class for reflection related functions</summary>
#if !HARMONYEXTENSIONS_PUBLIC
    internal
#else
    public
#endif
        static partial class AccessTools2
    {
        /// <summary>Gets the reflection information for an indexer property by searching the type and all its super types</summary>
        /// <param name="type">The class/type</param>
        /// <param name="parameters">Optional parameters to target a specific overload of multiple indexers</param>
        /// <returns>An indexer property or null when type is null or when it cannot be found</returns>
        public static PropertyInfo? Indexer(Type type, Type[]? parameters = null)
        {
            if (type is null)
            {
                Trace.TraceError("AccessTools2.Indexer: 'type' is null");
                return null;
            }

            // Can find multiple indexers without specified parameters, but only one with specified ones
            Func<Type, PropertyInfo> func = parameters is null ?
                t => t.GetProperties(AccessTools.all).SingleOrDefault(property => property.GetIndexParameters().Any())
                : t => t.GetProperties(AccessTools.all).FirstOrDefault(property => property.GetIndexParameters().Select(param => param.ParameterType).SequenceEqual(parameters));

            try
            {
                var indexer = FindIncludingBaseTypes(type, func);

                if (indexer is null) Trace.TraceError($"AccessTools2.Indexer: Could not find indexer for type '{type}' and parameters {parameters?.Description()}");

                return indexer;
            }
            catch (InvalidOperationException ex)
            {
                throw new AmbiguousMatchException("Multiple possible indexers were found.", ex);
            }
        }
        
        /// <summary>Gets the reflection information for the getter method of an indexer property by searching the type and all its super types</summary>
        /// <param name="type">The class/type</param>
        /// <param name="parameters">Optional parameters to target a specific overload of multiple indexers</param>
        /// <returns>A method or null when type is null or when the indexer property cannot be found</returns>
        public static MethodInfo? IndexerGetter(Type type, Type[]? parameters = null)
        {
            return Indexer(type, parameters)?.GetGetMethod(true);
        }
        
        /// <summary>Gets the reflection information for the setter method of an indexer property by searching the type and all its super types</summary>
        /// <param name="type">The class/type</param>
        /// <param name="parameters">Optional parameters to target a specific overload of multiple indexers</param>
        /// <returns>A method or null when type is null or when the indexer property cannot be found</returns>
        public static MethodInfo? IndexerSetter(Type type, Type[]? parameters = null)
        {
            return Indexer(type, parameters)?.GetSetMethod(true);
        }
        
        /// <summary>Gets the reflection information for a directly declared indexer property</summary>
        /// <param name="type">The class/type where the indexer property is declared</param>
        /// <param name="parameters">Optional parameters to target a specific overload of multiple indexers</param>
        /// <returns>An indexer property or null when type is null or when it cannot be found</returns>
        public static PropertyInfo? DeclaredIndexer(Type type, Type[]? parameters = null)
        {
            if (type is null)
            {
                Trace.TraceError("AccessTools2.DeclaredIndexer: 'type' is null");
                return null;
            }

            try
            {
                // Can find multiple indexers without specified parameters, but only one with specified ones
                var indexer = parameters is null ?
                    type.GetProperties(AccessTools.allDeclared).SingleOrDefault(property => property.GetIndexParameters().Any())
                    : type.GetProperties(AccessTools.allDeclared).FirstOrDefault(property => property.GetIndexParameters().Select(param => param.ParameterType).SequenceEqual(parameters));

                if (indexer is null) Trace.TraceError($"AccessTools2.DeclaredIndexer: Could not find indexer for type '{type}' and parameters {parameters?.Description()}");

                return indexer;
            }
            catch (InvalidOperationException ex)
            {
                throw new AmbiguousMatchException("Multiple possible indexers were found.", ex);
            }
        }
        
        /// <summary>Gets the reflection information for the getter method of a directly declared indexer property</summary>
        /// <param name="type">The class/type where the indexer property is declared</param>
        /// <param name="parameters">Optional parameters to target a specific overload of multiple indexers</param>
        /// <returns>A method or null when type is null or when indexer property cannot be found</returns>
        ///
        public static MethodInfo? DeclaredIndexerGetter(Type type, Type[]? parameters = null)
        {
            return DeclaredIndexer(type, parameters)?.GetGetMethod(true);
        }
        
        /// <summary>Gets the reflection information for the setter method of a directly declared indexer property</summary>
        /// <param name="type">The class/type where the indexer property is declared</param>
        /// <param name="parameters">Optional parameters to target a specific overload of multiple indexers</param>
        /// <returns>A method or null when type is null or when indexer property cannot be found</returns>
        ///
        public static MethodInfo? DeclaredIndexerSetter(Type type, Type[] parameters)
        {
            return DeclaredIndexer(type, parameters)?.GetSetMethod(true);
        }
    }
}

#pragma warning restore
#nullable restore
#endif // HARMONYEXTENSIONS_DISABLE