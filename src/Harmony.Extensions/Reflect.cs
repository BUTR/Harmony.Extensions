﻿// <auto-generated>
//   This code file has automatically been added by the "Harmony.Extensions" NuGet package (https://www.nuget.org/packages/Harmony.Extensions).
//   Please see https://github.com/BUTR/Harmony.Extensions for more information.
//
//   IMPORTANT:
//   DO NOT DELETE THIS FILE if you are using a "packages.config" file to manage your NuGet references.
//   Consider migrating to PackageReferences instead:
//   https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference
//   Migrating brings the following benefits:
//   * The "Harmony.Extensions" folder and the "SymbolExtensions2.cs" file don't appear in your project.
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
    using global::System;
    using global::System.Linq;
    using global::System.Reflection;

    internal static class Reflect
    {
        public class Method
        {
            public Type OwnerType { get; }

            public string Name { get; }

            public Type[]? Parameters { get; }

            public Type[]? Generics { get; }

            public MethodInfo? MethodInfo { get; }

            public Type? Type => MethodInfo is { DeclaringType: { } dt } ? dt : null;

            protected virtual string MethodType => "method";

            protected virtual MethodInfo? ResolveMethodInfo() => AccessTools.Method(OwnerType, Name, Parameters, Generics);

            public Method(Type type, string name, Type[]? parameters = null, Type[]? generics = null)
            {
                Name = name;
                Parameters = parameters;
                Generics = generics;
                OwnerType = type;
                MethodInfo = ResolveMethodInfo();
            }

            public TDelegate? GetDelegate<TDelegate>(object? instance) where TDelegate : Delegate => AccessTools2.GetDelegate<TDelegate>(instance, MethodInfo);
            public TDelegate? GetOpenDelegate<TDelegate>() where TDelegate : Delegate => AccessTools2.GetDelegate<TDelegate>(MethodInfo);
            public TDelegate? GetDelegate<TDelegate>() where TDelegate : Delegate => AccessTools2.GetDelegate<TDelegate>(MethodInfo);
            public TDelegate? GetDelegateWithObjectAsInstance<TDelegate>() where TDelegate : Delegate => AccessTools2.GetDelegateObjectInstance<TDelegate>(MethodInfo);

            protected string ParametersString => Parameters is null || Parameters.Length == 0
                ? string.Empty
                : $"({string.Join(", ", Parameters.Select(t => t.Name))})";

            protected string GenericsString => Generics is null || Generics.Length == 0
                ? string.Empty
                : $"<{string.Join(",", Generics.Select(t => t.Name))}>";

            public override string ToString() => $"{MethodType} {Name}{GenericsString}{ParametersString}";
        }

        public class DeclaredMethod : Method
        {
            public DeclaredMethod(Type type, string name, Type[]? parameters = null, Type[]? generics = null)
                : base(type, name, parameters, generics) { }

            protected override MethodInfo? ResolveMethodInfo() => AccessTools.DeclaredMethod(Type, Name, Parameters, Generics);
        }

        public class Getter : Method
        {
            public Getter(Type type, string name) : base(type, name) { }
            protected override MethodInfo? ResolveMethodInfo() => AccessTools.PropertyGetter(Type, Name);
            protected override string MethodType => "property getter";
        }

        public class DeclaredGetter : Getter
        {
            public DeclaredGetter(Type type, string name) : base(type, name) { }
            protected override MethodInfo? ResolveMethodInfo() => AccessTools.DeclaredPropertyGetter(Type, Name);
        }

        public class Setter : Method
        {
            public Setter(Type type, string name) : base(type, name) { }
            protected override MethodInfo? ResolveMethodInfo() => AccessTools.PropertySetter(Type, Name);
            protected override string MethodType => "property setter";
        }

        public class DeclaredSetter : Setter
        {
            public DeclaredSetter(Type type, string name) : base(type, name) { }
            protected override MethodInfo? ResolveMethodInfo() => AccessTools.DeclaredPropertySetter(Type, Name);
        }

        public sealed class Method<T> : Method
        {
            public Method(string name, Type[]? parameters = null, Type[]? generics = null)
                : base(typeof(T), name, parameters, generics) { }
        }

        public sealed class DeclaredMethod<T> : DeclaredMethod
        {
            public DeclaredMethod(string name, Type[]? parameters = null, Type[]? generics = null)
                : base(typeof(T), name, parameters, generics) { }
        }

        public sealed class Getter<T> : Getter
        {
            public Getter(string name) : base(typeof(T), name) { }
        }

        public sealed class DeclaredGetter<T> : DeclaredGetter
        {
            public DeclaredGetter(string name) : base(typeof(T), name) { }
        }

        public sealed class Setter<T> : Setter
        {
            public Setter(string name) : base(typeof(T), name) { }
        }

        public sealed class DeclaredSetter<T> : DeclaredSetter
        {
            public DeclaredSetter(string name) : base(typeof(T), name) { }
        }
    }
}

#pragma warning restore
#nullable restore
#endif // HARMONYEXTENSIONS_DISABLE