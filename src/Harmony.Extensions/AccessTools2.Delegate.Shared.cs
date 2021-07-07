﻿// <auto-generated>
//   This code file has automatically been added by the "Harmony.Extensions" NuGet package (https://www.nuget.org/packages/Harmony.Extensions).
//   Please see https://github.com/BUTR/Harmony.Extensions for more information.
//
//   IMPORTANT:
//   DO NOT DELETE THIS FILE if you are using a "packages.config" file to manage your NuGet references.
//   Consider migrating to PackageReferences instead:
//   https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference
//   Migrating brings the following benefits:
//   * The "Harmony.Extensions" folder and the "AccessTools2.Delegate.Shared.cs" file don't appear in your project.
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
#if !HARMONYEXTENSIONS_ENABLEWARNINGS
#pragma warning disable
#endif

namespace HarmonyLib.BUTR.Extensions
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics;
    using global::System.Linq;
    using global::System.Linq.Expressions;
    using global::System.Reflection;

    /// <summary>An extension of Harmony's helper class for reflection related functions</summary>
#if !HARMONYEXTENSIONS_PUBLIC
    internal
#else
    public
#endif
        static partial class AccessTools2
    {
        // Duplicate from BUTR.Shared

        public static TDelegate? GetDelegate<TDelegate>(ConstructorInfo constructorInfo) where TDelegate : Delegate
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
                .Select((pe, i) =>
                {
                    if (pe.IsByRef || pe.Type.Equals(constructorParameters[i].ParameterType)) // TODO: Convert?
                        return (Expression) pe;
                    else
                        return (Expression) Expression.Convert(pe, constructorParameters[i].ParameterType);
                })
                .ToList();

            Expression @new = Expression.New(constructorInfo, inputParameters);
            var body = @new.Type.Equals(delegateInvoke.ReturnType) 
                ? @new 
                : Expression.Convert(@new, delegateInvoke.ReturnType);

            try
            {
                return Expression.Lambda<TDelegate>(body, returnParameters).Compile();
            }
            catch (Exception ex)
            {
                Trace.TraceError($"AccessTools2.GetDelegate<{typeof(TDelegate).FullName}>: Error while compiling lambds expression '{ex}'");
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
        public static TDelegate? GetDelegate<TDelegate>(object? instance, MethodInfo methodInfo) where TDelegate : Delegate
        {
            if (methodInfo is null) return null;

            if (typeof(TDelegate).GetMethod("Invoke") is not { } delegateInvoke) return null;

            if (!delegateInvoke.ReturnType.IsAssignableFrom(methodInfo.ReturnType)) return null;
            
            var delegateParameters = delegateInvoke.GetParameters();
            var methodParameters = methodInfo.GetParameters();

            var hasSameParameters = delegateParameters.Length - methodParameters.Length == 0 && ParametersAreEqual(delegateParameters, methodParameters);
            var hasInstance = instance is not null;
            var hasInstanceType = delegateParameters.Length - methodParameters.Length == 1 &&
                                  (delegateParameters[0].ParameterType.IsAssignableFrom(methodInfo.DeclaringType) || methodInfo.DeclaringType.IsAssignableFrom(delegateParameters[0].ParameterType));

            if (!hasInstance && !hasInstanceType && !methodInfo.IsStatic) return null;
            if (hasInstance && methodInfo.IsStatic) return null;
            if (hasInstance && !methodInfo.IsStatic && !methodInfo.DeclaringType.IsAssignableFrom(instance!.GetType())) return null;
            //if (hasInstanceType && !delegateParameters[0].ParameterType.IsAssignableFrom(methodInfo.DeclaringType)) return null;
            
            if (hasSameParameters && hasInstanceType) return null;
            if (hasInstance && (hasInstanceType || !hasSameParameters)) return null;
            if (hasInstanceType && (hasInstance || hasSameParameters)) return null;
            if (!hasInstanceType && !hasInstance && !hasSameParameters) return null;

            var instanceParameter = hasInstanceType
                ? Expression.Parameter(delegateParameters[0].ParameterType, "instance")
                  //delegateParameters[0].ParameterType.Equals(methodInfo.DeclaringType)
                  //  ? Expression.Parameter(delegateParameters[0].ParameterType, "instance")
                  //  : Expression.Parameter(methodInfo.DeclaringType, "instance")
                : null;
            var returnParameters = delegateParameters
                .Skip(hasInstanceType ? 1 : 0)
                .Select((pi, i) => Expression.Parameter(pi.ParameterType, $"p{i}"))
                .ToList();
            var inputParameters = returnParameters
                .Select((pe, i) =>
                {
                    if (pe.IsByRef || pe.Type.Equals(methodParameters[i].ParameterType)) // TODO: Convert?
                        return (Expression) pe;
                    else
                        return (Expression) Expression.Convert(pe, methodParameters[i].ParameterType);
                })
                .ToList();

            var call = hasInstance
                ? instance!.GetType().Equals(methodInfo.DeclaringType)
                    ? Expression.Call(Expression.Constant(instance), methodInfo, inputParameters)
                    : Expression.Call(Expression.Convert(Expression.Constant(instance), instance.GetType()), methodInfo, inputParameters)
                : hasSameParameters
                    ? Expression.Call(methodInfo, inputParameters)
                    : hasInstanceType
                        ? instanceParameter!.Type.Equals(methodInfo.DeclaringType)
                            ? Expression.Call(instanceParameter, methodInfo, inputParameters)
                            : Expression.Call(Expression.Convert(instanceParameter, methodInfo.DeclaringType), methodInfo, inputParameters)
                        : null;

            if (call is null) return null;

            var body = call.Type.Equals(methodInfo.ReturnType) 
                ? (Expression) call
                : (Expression) Expression.Convert(call, methodInfo.ReturnType);

            try
            {
                return Expression.Lambda<TDelegate>(body, hasInstanceType
                    ? new List<ParameterExpression> { instanceParameter! }.Concat(returnParameters)
                    : returnParameters).Compile();
            }
            catch (Exception ex)
            {
                Trace.TraceError($"AccessTools2.GetDelegate<{typeof(TDelegate).FullName}>: Error while compiling lambds expression '{ex}'");
                return null;
            }
        }

        /// <summary>Get a delegate for a method described by <paramref name="methodInfo"/>.</summary>
        /// <param name="methodInfo">The method's <see cref="MethodInfo"/>.</param>
        /// <returns>A delegate or <see langword="null"/> when <paramref name="methodInfo"/> is <see langword="null"/>.</returns>
        public static TDelegate? GetDelegate<TDelegate>(MethodInfo methodInfo) where TDelegate : Delegate => GetDelegate<TDelegate>(null, methodInfo);

        public static TDelegate? GetDelegateObjectInstance<TDelegate>(MethodInfo methodInfo) where TDelegate : Delegate => GetDelegate<TDelegate>(methodInfo);


        public static TDelegate? GetDelegate<TDelegate>(object? instance, string typeColonMethod, Type[]? parameters = null, Type[]? generics = null) where TDelegate : Delegate =>
            Method(typeColonMethod, parameters, generics) is { } methodInfo ? GetDelegate<TDelegate>(instance, methodInfo) : null;

        public static TDelegate? GetDelegate<TDelegate>(string typeColonMethod, Type[]? parameters = null, Type[]? generics = null) where TDelegate : Delegate =>
            GetDelegate<TDelegate>((object?) null, typeColonMethod, parameters, generics);

        public static TDelegate? GetDelegateObjectInstance<TDelegate>(string typeColonMethod, Type[]? parameters = null, Type[]? generics = null) where TDelegate : Delegate =>
            GetDelegate<TDelegate>(typeColonMethod, parameters, generics);


        private static bool ParametersAreEqual(ParameterInfo[] delegateParameters, ParameterInfo[] methodParameters)
        {
            if (delegateParameters.Length - methodParameters.Length == 0)
            {
                for (var i = 0; i < methodParameters.Length; i++)
                {
                    if (delegateParameters[i].ParameterType.IsByRef != methodParameters[i].ParameterType.IsByRef)
                        return false;

                    if (!delegateParameters[i].ParameterType.IsAssignableFrom(methodParameters[i].ParameterType))
                        return false;
                }
                return true;
            }
            else if (delegateParameters.Length - methodParameters.Length == 1)
            {
                for (var i = 0; i < methodParameters.Length; i++)
                {
                    if (delegateParameters[i + 1].ParameterType.IsByRef != methodParameters[i].ParameterType.IsByRef)
                        return false;

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
    }
}

#pragma warning restore
#nullable restore
#endif // HARMONYEXTENSIONS_DISABLE