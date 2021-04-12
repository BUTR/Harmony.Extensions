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
// Copyright (c) Bannerlord's Unofficial Tools & Resources
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
#pragma warning disable

namespace HarmonyLib.BUTR.Extensions
{
    using global::HarmonyLib;

    using global::System.Diagnostics.CodeAnalysis;
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Linq.Expressions;
    using global::System.Reflection;

    using static global::HarmonyLib.AccessTools;

    /// <summary>An extension of Harmony's helper class for reflection related functions</summary>
    internal static partial class AccessTools2
    {
        public static TDelegate? GetConstructorDelegate<TDelegate>(Type type, Type[]? parameters = null) where TDelegate : Delegate
            => GetDelegate<TDelegate>(Constructor(type, parameters));

        public static TDelegate? GetDeclaredConstructorDelegate<TDelegate>(Type type, Type[]? parameters = null) where TDelegate : Delegate
            => GetDelegate<TDelegate>(DeclaredConstructor(type, parameters));

        public static TDelegate? GetConstructorDelegate<TDelegate>(string typeString, Type[]? parameters = null) where TDelegate : Delegate
            => GetDelegate<TDelegate>(Constructor(typeString, parameters));


        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, declared by <paramref name="type"/> or any of its base types,
        /// and then bind it to an instance type of <see cref="object"/>.
        /// </summary>
        /// <param name="type">The type where the method is declared.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegateObjectInstance<TDelegate>(Type type, string method) where TDelegate : Delegate
            => GetDelegateObjectInstance<TDelegate>(Method(type, method));

        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, declared by <paramref name="type"/> or any of its base types,
        /// and then bind it to an instance type of <see cref="object"/>.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="type">The type from which to start searching for the method's definition.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegateObjectInstance<TDelegate>(Type type, string method, Type[]? parameters, Type[]? generics = null) where TDelegate : Delegate
            => GetDelegateObjectInstance<TDelegate>(Method(type, method, parameters, generics));

        /// <summary>
        /// Try to get a delegate for a method named <paramref name="method"/>, declared by <paramref name="type"/> or any of its base types,
        /// and then bind it to an instance type of <see cref="object"/>.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="outDelegate">
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </param>
        /// <param name="type">The type where the method is declared.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// <see langword="true"/> if the delegate was successfully resolved and created, else <see langword="false"/>.
        /// </returns>
        internal static bool TryGetDelegateObjectInstance<TDelegate>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                                     Type type,
                                                                     string method,
                                                                     Type[]? parameters = null,
                                                                     Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = GetDelegateObjectInstance<TDelegate>(Method(type, method, parameters, generics))) is not null;

        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, directly declared by <paramref name="type"/>,
        /// and then bind it to an instance type of <see cref="object"/>.
        /// </summary>
        /// <param name="type">The type where the method is declared.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDeclaredDelegateObjectInstance<TDelegate>(Type type, string method) where TDelegate : Delegate
            => GetDelegateObjectInstance<TDelegate>(DeclaredMethod(type, method));

        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, directly declared by <paramref name="type"/>,
        /// and then bind it to an instance type of <see cref="object"/>.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="type">The type from which to start searching for the method's definition.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDeclaredDelegateObjectInstance<TDelegate>(Type type, string method, Type[]? parameters, Type[]? generics = null) where TDelegate : Delegate
            => GetDelegateObjectInstance<TDelegate>(DeclaredMethod(type, method, parameters, generics));

        /// <summary>
        /// Try to get a delegate for a method named <paramref name="method"/>, directly declared by <paramref name="type"/>,
        /// and then bind it to an instance type of <see cref="object"/>.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="outDelegate">
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </param>
        /// <param name="type">The type where the method is declared.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// <see langword="true"/> if the delegate was successfully resolved and created, else <see langword="false"/>.
        /// </returns>
        internal static bool TryGetDeclaredDelegateObjectInstance<TDelegate>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                                             Type type,
                                                                             string method,
                                                                             Type[]? parameters = null,
                                                                             Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = GetDelegateObjectInstance<TDelegate>(DeclaredMethod(type, method, parameters, generics))) is not null;

        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, declared by <paramref name="type"/> or any of its base types.
        /// </summary>
        /// <param name="type">The type from which to start searching for the method's definition.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegate<TDelegate>(Type type, string method) where TDelegate : Delegate
            => GetDelegate<TDelegate>(Method(type, method));

        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, declared by <paramref name="type"/>
        /// or any of its base types.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="type">The type from which to start searching for the method's definition.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegate<TDelegate>(Type type, string method, Type[]? parameters, Type[]? generics = null) where TDelegate : Delegate
            => GetDelegate<TDelegate>(Method(type, method, parameters, generics));

        /// <summary>
        /// Try to get a delegate for a method named <paramref name="method"/>, declared by <paramref name="type"/>
        /// or any of its base types.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="outDelegate">
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </param>
        /// <param name="type">The type where the method is declared.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// <see langword="true"/> if the delegate was successfully resolved and created, else <see langword="false"/>.
        /// </returns>
        internal static bool TryGetDelegate<TDelegate>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                       Type type,
                                                       string method,
                                                       Type[]? parameters = null,
                                                       Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = GetDelegate<TDelegate>(Method(type, method, parameters, generics))) is not null;

        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, directly declared by <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type where the method is declared.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDeclaredDelegate<TDelegate>(Type type, string method) where TDelegate : Delegate
            => GetDelegate<TDelegate>(DeclaredMethod(type, method));

        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, directly declared by <paramref name="type"/>.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="type">The type where the method is declared.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDeclaredDelegate<TDelegate>(Type type, string method, Type[]? parameters, Type[]? generics = null) where TDelegate : Delegate
            => GetDelegate<TDelegate>(DeclaredMethod(type, method, parameters, generics));

        /// <summary>
        /// Try to get a delegate for a method named <paramref name="method"/>, directly declared by <paramref name="type"/>.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="outDelegate">
        /// A delegate or <see langword="null"/> when <paramref name="type"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </param>
        /// <param name="type">The type where the method is declared.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// <see langword="true"/> if the delegate was successfully resolved and created, else <see langword="false"/>.
        /// </returns>
        internal static bool TryGetDeclaredDelegate<TDelegate>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                               Type type,
                                                               string method,
                                                               Type[]? parameters = null,
                                                               Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = GetDelegate<TDelegate>(DeclaredMethod(type, method, parameters, generics))) is not null;

        /// <summary>
        /// Get a delegate for an instance method declared by <paramref name="instance"/>'s type or any of its base types.
        /// </summary>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance instance, string method) where TDelegate : Delegate
            => instance is null ? null : GetDelegate<TDelegate, TInstance>(instance, Method(instance.GetType(), method));

        /// <summary>
        /// Get a delegate for a method named <paramref name="method"/>, declared by <paramref name="instance"/>'s type or any of its base types.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance instance, string method, Type[]? parameters, Type[]? generics = null) where TDelegate : Delegate
            => instance is null ? null : GetDelegate<TDelegate, TInstance>(instance, Method(instance.GetType(), method, parameters, generics));

        /// <summary>
        /// Try to get a delegate for an instance method named <paramref name="method"/>,
        /// declared by <paramref name="instance"/>'s type or any of its base types.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="outDelegate">
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </param>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// <see langword="true"/> if the delegate was successfully resolved and created, else <see langword="false"/>.
        /// </returns>
        internal static bool TryGetDelegate<TDelegate, TInstance>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                                  TInstance instance,
                                                                  string method,
                                                                  Type[]? parameters = null,
                                                                  Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = instance is null
                ? null : GetDelegate<TDelegate, TInstance>(instance, Method(instance.GetType(), method, parameters, generics))) is not null;

        /// <summary>
        /// Get a delegate for an instance method directly declared by <paramref name="instance"/>'s type.
        /// </summary>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDeclaredDelegate<TDelegate, TInstance>(TInstance instance, string method) where TDelegate : Delegate
            => instance is null ? null : GetDelegate<TDelegate, TInstance>(instance, DeclaredMethod(instance.GetType(), method));

        /// <summary>
        /// Get a delegate for an instance method named <paramref name="method"/>, directly declared by <paramref name="instance"/>'s type.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDeclaredDelegate<TDelegate, TInstance>(TInstance instance, string method, Type[]? parameters, Type[]? generics = null) where TDelegate : Delegate
            => instance is null ? null : GetDelegate<TDelegate, TInstance>(instance, DeclaredMethod(instance.GetType(), method, parameters, generics));

        /// <summary>
        /// Try to get a delegate for an instance method named <paramref name="method"/>,
        /// directly declared by <paramref name="instance"/>'s type.
        /// Choose the overload with the given <paramref name="parameters"/> if not <see langword="null"/>
        /// and/or the generic arguments <paramref name="generics"/> if not <see langword="null"/>.
        /// </summary>
        /// <param name="outDelegate">
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="method"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </param>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="method">The name of the method (case sensitive).</param>
        /// <param name="parameters">The method's parameter types (when not <see langword="null"/>).</param>
        /// <param name="generics">The generic arguments of the method (when not <see langword="null"/>).</param>
        /// <returns>
        /// <see langword="true"/> if the delegate was successfully resolved and created, else <see langword="false"/>.
        /// </returns>
        internal static bool TryGetDeclaredDelegate<TDelegate, TInstance>([NotNullWhen(true)] out TDelegate? outDelegate,
                                                                          TInstance instance,
                                                                          string method,
                                                                          Type[]? parameters = null,
                                                                          Type[]? generics = null) where TDelegate : Delegate
            => (outDelegate = instance is null
                ? null : GetDelegate<TDelegate, TInstance>(instance, DeclaredMethod(instance.GetType(), method, parameters, generics))) is not null;

        /// <summary>
        /// Get a delegate for an instance method described by <paramref name="methodInfo"/> and bound to <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="methodInfo">The method's <see cref="MethodInfo"/>.</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="methodInfo"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegate<TDelegate, TInstance>(TInstance instance, MethodInfo? methodInfo) where TDelegate : Delegate
            => GetDelegate<TDelegate>(instance, methodInfo);


        // Duplicate from BUTR.Shared

        private static bool ParametersAreEqual(ParameterInfo[] delegateParameters, ParameterInfo[] methodParameters)
        {
            if (delegateParameters.Length - methodParameters.Length == 0)
            {
                for (var i = 0; i < methodParameters.Length; i++)
                {
                    if (!delegateParameters[i].ParameterType.IsAssignableFrom(methodParameters[i].ParameterType))
                        return false;
                }
                return true;
            }
            else if (delegateParameters.Length - methodParameters.Length == 1)
            {
                for (var i = 0; i < methodParameters.Length; i++)
                {
                    if (!delegateParameters[i + 1].ParameterType.IsAssignableFrom(methodParameters[i].ParameterType))
                        return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static TDelegate? GetDelegate<TDelegate>(ConstructorInfo? constructorInfo) where TDelegate : Delegate
        {
            if (constructorInfo is null) return null;
            
            if (typeof(TDelegate).GetMethod("Invoke") is not { } delegateInvoke) return null;

            if (!delegateInvoke.ReturnType.IsAssignableFrom(constructorInfo.DeclaringType)) return null;

            var delegateParameters = delegateInvoke.GetParameters();
            var constructorParameters = constructorInfo.GetParameters();

            if (delegateParameters.Length - constructorParameters.Length != 0 && !ParametersAreEqual(delegateParameters, constructorParameters)) return null;

            var instance = Expression.Parameter(typeof(object), "instance");

            var returnParameters = delegateParameters
                .Select((pi, i) => Expression.Parameter(pi.ParameterType, $"p{i}"))
                .ToList();
            var inputParameters = returnParameters
                .Select((pe, i) => Expression.Convert(pe, constructorParameters[i].ParameterType))
                .ToList();

            Expression @new = Expression.New(constructorInfo, inputParameters);
            var body = Expression.Convert(@new, constructorInfo.DeclaringType);

            try
            {
                return Expression.Lambda<TDelegate>(body, returnParameters).Compile();
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// Get a delegate for an instance method described by <paramref name="methodInfo"/> and bound to <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The instance for which the method is defined.</param>
        /// <param name="methodInfo">The method's <see cref="MethodInfo"/>.</param>
        /// <returns>
        /// A delegate or <see langword="null"/> when <paramref name="instance"/> or <paramref name="methodInfo"/>
        /// is <see langword="null"/> or when the method cannot be found.
        /// </returns>
        public static TDelegate? GetDelegate<TDelegate>(object? instance, MethodInfo? methodInfo) where TDelegate : Delegate
        {
            if (methodInfo is null) return null;

            if (typeof(TDelegate).GetMethod("Invoke") is not { } delegateInvoke) return null;

            if (!delegateInvoke.ReturnType.IsAssignableFrom(methodInfo.ReturnType)) return null;

            var delegateParameters = delegateInvoke.GetParameters();
            var methodParameters = methodInfo.GetParameters();

            var hasSameParameters = delegateParameters.Length - methodParameters.Length == 0 && ParametersAreEqual(delegateParameters, methodParameters);
            var hasInstance = instance is not null;
            var hasInstanceType = delegateParameters.Length - methodParameters.Length == 1 && delegateParameters[0].ParameterType.IsAssignableFrom(methodInfo.DeclaringType);

            if (hasSameParameters && hasInstanceType) return null;
            if (hasInstance && (hasInstanceType || !hasSameParameters)) return null;
            if (hasInstanceType && (hasInstance || hasSameParameters)) return null;

            var instanceParameter = hasInstanceType
                ? Expression.Parameter(delegateParameters[0].ParameterType, "instance")
                : null;
            var returnParameters = delegateParameters
                .Skip(hasInstanceType ? 1 : 0)
                .Select((pi, i) => Expression.Parameter(pi.ParameterType, $"p{i}"))
                .ToList();
            var inputParameters = returnParameters
                .Select((pe, i) => Expression.Convert(pe, methodParameters[i].ParameterType))
                .ToList();

            var call = hasInstance
                ? Expression.Call(Expression.Constant(instance), methodInfo, inputParameters)
                : hasSameParameters
                    ? Expression.Call(methodInfo, inputParameters)
                    : hasInstanceType
                        ? Expression.Call(Expression.Convert(instanceParameter, methodInfo.DeclaringType!), methodInfo, inputParameters)
                        : null;

            if (call is null) return null;

            var body = Expression.Convert(call, methodInfo.ReturnType);

            try
            {
                return Expression.Lambda<TDelegate>(body, hasInstanceType
                    ? new List<ParameterExpression> { instanceParameter }.Concat(returnParameters)
                    : returnParameters).Compile();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>Get a delegate for a method described by <paramref name="methodInfo"/>.</summary>
        /// <param name="methodInfo">The method's <see cref="MethodInfo"/>.</param>
        /// <returns>A delegate or <see langword="null"/> when <paramref name="methodInfo"/> is <see langword="null"/>.</returns>
        public static TDelegate? GetDelegate<TDelegate>(MethodInfo? methodInfo) where TDelegate : Delegate => GetDelegate<TDelegate>(null, methodInfo);

        public static TDelegate? GetDelegateObjectInstance<TDelegate>(MethodInfo? methodInfo) where TDelegate : Delegate => GetDelegate<TDelegate>(methodInfo);


        public static TDelegate? GetDelegate<TDelegate>(object? instance, string typeColonMethod, Type[]? parameters = null, Type[]? generics = null) where TDelegate : Delegate =>
            GetDelegate<TDelegate>(null, Method(typeColonMethod, parameters, generics));

        public static TDelegate? GetDelegate<TDelegate>(string typeColonMethod, Type[]? parameters = null, Type[]? generics = null) where TDelegate : Delegate =>
            GetDelegate<TDelegate>(null, typeColonMethod, parameters, generics);

        public static TDelegate? GetDelegateObjectInstance<TDelegate>(string typeColonMethod, Type[]? parameters = null, Type[]? generics = null) where TDelegate : Delegate =>
            GetDelegate<TDelegate>(typeColonMethod, parameters, generics);
    }
}

#pragma warning restore
#nullable restore
#endif // HARMONYEXTENSIONS_DISABLE