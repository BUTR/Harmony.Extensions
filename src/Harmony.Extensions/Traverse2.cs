﻿// <auto-generated>
//   This code file has automatically been added by the "Harmony.Extensions" NuGet package (https://www.nuget.org/packages/Harmony.Extensions).
//   Please see https://github.com/BUTR/Harmony.Extensions for more information.
//
//   IMPORTANT:
//   DO NOT DELETE THIS FILE if you are using a "packages.config" file to manage your NuGet references.
//   Consider migrating to PackageReferences instead:
//   https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference
//   Migrating brings the following benefits:
//   * The "Harmony.Extensions" folder and the "Traverse2.cs" file don't appear in your project.
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
    using global::HarmonyLib;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Globalization;
    using global::System.Reflection;
    using global::System.Runtime.CompilerServices;

    /// <summary>A reflection helper to read and write private elements</summary>
    /// <typeparam name="T">The result type defined by GetValue()</typeparam>
#if !HARMONYEXTENSIONS_PUBLIC
    internal
#else
    public
#endif
        class Traverse2<T>
	{
        /// <summary>Gets/Sets the current value</summary>
        /// <value>The value to read or write</value>
        public T? Value
        {
            get => _traverse.GetValue<T>();
            set => _traverse.SetValue(value!);
        }

		private readonly Traverse2 _traverse;

        private Traverse2()
        {
            _traverse = new Traverse2(null);
        }

		/// <summary>Creates a traverse instance from an existing instance</summary>
		/// <param name="traverse">The existing <see cref="Traverse"/> instance</param>
		///
		public Traverse2(Traverse2 traverse)
		{
            _traverse = traverse;
		}
    }

	/// <summary>A reflection helper to read and write private elements</summary>
#if !HARMONYEXTENSIONS_PUBLIC
    internal
#else
    public
#endif
        class Traverse2
	{
        private static readonly AccessCacheHandle? Cache;

        private readonly Type? _type;
        private readonly object? _root;
        private readonly MemberInfo? _info;
        private readonly MethodBase? _method;
        private readonly object[]? _params;

		[MethodImpl(MethodImplOptions.Synchronized)]
		static Traverse2()
        {
            Cache ??= AccessCacheHandle.Create();
        }

		/// <summary>Creates a new traverse instance from a class/type</summary>
		/// <param name="type">The class/type</param>
		/// <returns>A <see cref="Traverse"/> instance</returns>
        public static Traverse2 Create(Type? type) => new(type);

        /// <summary>Creates a new traverse instance from a class T</summary>
		/// <typeparam name="T">The class</typeparam>
		/// <returns>A <see cref="Traverse"/> instance</returns>
        public static Traverse2 Create<T>() => Create(typeof(T));

        /// <summary>Creates a new traverse instance from an instance</summary>
		/// <param name="root">The object</param>
		/// <returns>A <see cref="Traverse"/> instance</returns>
        public static Traverse2 Create(object? root) => new(root);

        /// <summary>Creates a new traverse instance from a named type</summary>
		/// <param name="name">The type name, for format see <see cref="AccessTools.TypeByName"/></param>
		/// <returns>A <see cref="Traverse"/> instance</returns>
        public static Traverse2 CreateWithType(string name) => new(AccessTools2.TypeByName(name));

        /// <summary>Creates a new and empty traverse instance</summary>
        private Traverse2() { }

		/// <summary>Creates a new traverse instance from a class/type</summary>
		/// <param name="type">The class/type</param>
        public Traverse2(Type? type)
		{
			_type = type;
		}

		/// <summary>Creates a new traverse instance from an instance</summary>
		/// <param name="root">The object</param>
        public Traverse2(object? root)
		{
			_root = root;
			_type = root?.GetType();
		}

		private Traverse2(object? root, MemberInfo info, object[]? index)
		{
			_root = root;
			_type = root?.GetType() ?? info.GetUnderlyingType();
			_info = info;
			_params = index;
		}

        private Traverse2(object? root, MethodInfo method, object[]? parameter)
		{
			_root = root;
			_type = method.ReturnType;
			_method = method;
			_params = parameter;
		}

		/// <summary>Gets the current value</summary>
		/// <value>The value</value>
        [SuppressMessage("ReSharper", "ExceptionNotDocumented", Justification = "We handle it in Field()")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper plugin Exceptional")]
        public object? GetValue()
		{
			if (_info is FieldInfo fieldInfo)
				return fieldInfo.GetValue(_root);

			if (_info is PropertyInfo propertyInfo)
				return propertyInfo.GetValue(_root, AccessTools.all, null, _params, CultureInfo.CurrentCulture);

			if (_method is { } methodBase)
				return methodBase.Invoke(_root, _params);

			if (_root is null && _type is not null)
                return _type;

			return _root;
		}

		/// <summary>Gets the current value</summary>
		/// <typeparam name="T">The type of the value</typeparam>
		/// <value>The value</value>
        public T? GetValue<T>()
		{
            if (GetValue() is T value)
                return value;

			return default;
		}

        /// <summary>Invokes the current method with arguments and returns the result</summary>
        /// <param name="arguments">The method arguments</param>
        /// <value>The value returned by the method</value>
        /// <exception cref="TargetParameterCountException">The <paramref name="arguments" /> array does not have the correct number of arguments.</exception>
        /// <exception cref="ArgumentException">The elements of the <paramref name="arguments" /> array do not match the signature of the method or constructor reflected by this instance.</exception>
        [SuppressMessage("ReSharper", "ExceptionNotDocumented", Justification = "We handle it in Field()")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper plugin Exceptional")]
        public object? GetValue(params object[] arguments) => _method?.Invoke(_root, arguments);

        /// <summary>Invokes the current method with arguments and returns the result</summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="arguments">The method arguments</param>
        /// <value>The value returned by the method</value>
        /// <exception cref="TargetParameterCountException">The <paramref name="arguments" /> array does not have the correct number of arguments.</exception>
        /// <exception cref="ArgumentException">The elements of the <paramref name="arguments" /> array do not match the signature of the method or constructor reflected by this instance.</exception>
        [SuppressMessage("ReSharper", "ExceptionNotDocumented", Justification = "We handle it in Field()")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper plugin Exceptional")]
        public T? GetValue<T>(params object[] arguments)
        {
            if (_method?.Invoke(_root, arguments) is T value)
                return value;

			return default;
		}

        /// <summary>Sets a value of the current field or property</summary>
        /// <param name="value">The value</param>
        /// <returns>The same traverse instance</returns>
        [SuppressMessage("ReSharper", "ExceptionNotDocumented", Justification = "We handle it in Field()")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper plugin Exceptional")]
        public Traverse2 SetValue(object value)
        {
            if (_root is null)
                return this;

			if (_info is FieldInfo fieldInfo)
				fieldInfo.SetValue(_root, value, AccessTools.all, null, CultureInfo.CurrentCulture);

			if (_info is PropertyInfo propertyInfo)
				propertyInfo.SetValue(_root, value, AccessTools.all, null, _params, CultureInfo.CurrentCulture);

			return this;
		}

		/// <summary>Gets the type of the current field or property</summary>
		/// <returns>The type</returns>
        public Type? GetValueType()
		{
			if (_info is FieldInfo fieldInfo)
				return fieldInfo.FieldType;

			if (_info is PropertyInfo propertyInfo)
				return propertyInfo.PropertyType;

			return null;
		}

        private Traverse2 Resolve()
		{
			if (_root is null)
			{
				if (_info is FieldInfo { IsStatic: true })
					return new Traverse2(GetValue());

				if (_info is PropertyInfo propertyInfo && propertyInfo.GetGetMethod().IsStatic)
					return new Traverse2(GetValue());

				if (_method is { IsStatic: true })
					return new Traverse2(GetValue());

				if (_type is not null)
					return this;
			}
			return new Traverse2(GetValue());
		}

		/// <summary>Moves the current traverse instance to a inner type</summary>
		/// <param name="name">The type name</param>
		/// <returns>A traverse instance</returns>
        public Traverse2 Type(string name)
        {
            if (string.IsNullOrEmpty(name))
                return new Traverse2();

            if (_type is null)
                return new Traverse2();

            var type = AccessTools.Inner(_type, name);
            if (type is null)
                return new Traverse2();

            return new Traverse2(type);
        }

		/// <summary>Moves the current traverse instance to a field</summary>
		/// <param name="name">The type name</param>
		/// <returns>A traverse instance</returns>
        public Traverse2 Field(string name)
		{
            if (string.IsNullOrEmpty(name))
                return new Traverse2();

			var resolved = Resolve();
			if (resolved._type is null)
                return new Traverse2();

			var fieldInfo = Cache?.GetFieldInfo(resolved._type, name);
			if (fieldInfo is null)
                return new Traverse2();

			if (fieldInfo.IsStatic is false && resolved._root is null)
                return new Traverse2();

			return new Traverse2(resolved._root, fieldInfo, null);
		}

		/// <summary>Moves the current traverse instance to a field</summary>
		/// <typeparam name="T">The type of the field</typeparam>
		/// <param name="name">The type name</param>
		/// <returns>A traverse instance</returns>
        public Traverse2<T> Field<T>(string name) => new(Field(name));

        /// <summary>Gets all fields of the current type</summary>
		/// <returns>A list of field names</returns>
        public List<string> Fields()
		{
			var resolved = Resolve();
			return AccessTools.GetFieldNames(resolved._type);
		}

		/// <summary>Moves the current traverse instance to a property</summary>
		/// <param name="name">The type name</param>
		/// <param name="index">Optional property index</param>
		/// <returns>A traverse instance</returns>
        public Traverse2 Property(string name, object[]? index = null)
		{
			if (string.IsNullOrEmpty(name))
                return new Traverse2();

			var resolved = Resolve();
			if (resolved._type is null)
                return new Traverse2();

			var info = Cache?.GetPropertyInfo(resolved._type, name);
			if (info is null)
                return new Traverse2();

			return new Traverse2(resolved._root, info, index);
		}

		/// <summary>Moves the current traverse instance to a field</summary>
		/// <typeparam name="T">The type of the property</typeparam>
		/// <param name="name">The type name</param>
		/// <param name="index">Optional property index</param>
		/// <returns>A traverse instance</returns>
        public Traverse2<T> Property<T>(string name, object[]? index = null) => new(Property(name, index));

        /// <summary>Gets all properties of the current type</summary>
		/// <returns>A list of property names</returns>
        public List<string> Properties()
		{
			var resolved = Resolve();
			return AccessTools.GetPropertyNames(resolved._type);
		}

		/// <summary>Moves the current traverse instance to a method</summary>
		/// <param name="name">The name of the method</param>
		/// <param name="arguments">The arguments defining the argument types of the method overload</param>
		/// <returns>A traverse instance</returns>
        public Traverse2 Method(string name, params object[] arguments)
		{
            if (string.IsNullOrEmpty(name))
                return new Traverse2();

			var resolved = Resolve();
			if (resolved._type is null)
                return new Traverse2();

			var types = AccessTools.GetTypes(arguments);
			var method = Cache?.GetMethodInfo(resolved._type, name, types);
			if (method is not MethodInfo methodInfo)
                return new Traverse2();

			return new Traverse2(resolved._root, methodInfo, arguments);
		}

		/// <summary>Moves the current traverse instance to a method</summary>
		/// <param name="name">The name of the method</param>
		/// <param name="paramTypes">The argument types of the method</param>
		/// <param name="arguments">The arguments for the method</param>
		/// <returns>A traverse instance</returns>
        public Traverse2 Method(string name, Type[] paramTypes, object[]? arguments = null)
		{
            if (string.IsNullOrEmpty(name))
                return new Traverse2();

			var resolved = Resolve();
			if (resolved._type is null)
                return new Traverse2();

			var method = Cache?.GetMethodInfo(resolved._type, name, paramTypes);
			if (method is not MethodInfo methodInfo)
                return new Traverse2();

			return new Traverse2(resolved._root, methodInfo, arguments);
		}

		/// <summary>Gets all methods of the current type</summary>
		/// <returns>A list of method names</returns>
        public List<string> Methods()
		{
			var resolved = Resolve();
			return AccessTools.GetMethodNames(resolved._type);
		}

		/// <summary>Checks if the current traverse instance is for a field</summary>
		/// <returns>True if its a field</returns>
        public bool FieldExists() => _info is FieldInfo;

        /// <summary>Checks if the current traverse instance is for a property</summary>
		/// <returns>True if its a property</returns>
        public bool PropertyExists() => _info is PropertyInfo;

        /// <summary>Checks if the current traverse instance is for a method</summary>
		/// <returns>True if its a method</returns>
        public bool MethodExists() => _method is not null;

        /// <summary>Checks if the current traverse instance is for a type</summary>
		/// <returns>True if its a type</returns>
        public bool TypeExists() => _type is not null;

        /// <summary>Iterates over all fields of the current type and executes a traverse action</summary>
		/// <param name="source">Original object</param>
		/// <param name="action">The action receiving a <see cref="Traverse"/> instance for each field</param>
        public static void IterateFields(object source, Action<Traverse2> action)
		{
			if (action is null)
				return;

			var sourceTrv = Create(source);
			AccessTools.GetFieldNames(source).ForEach(f => action(sourceTrv.Field(f)));
		}

		/// <summary>Iterates over all fields of the current type and executes a traverse action</summary>
		/// <param name="source">Original object</param>
		/// <param name="target">Target object</param>
		/// <param name="action">The action receiving a pair of <see cref="Traverse"/> instances for each field pair</param>
        public static void IterateFields(object source, object target, Action<Traverse2, Traverse2> action)
		{
            if (action is null)
                return;

			var sourceTrv = Create(source);
			var targetTrv = Create(target);
			AccessTools.GetFieldNames(source).ForEach(f => action(sourceTrv.Field(f), targetTrv.Field(f)));
		}

		/// <summary>Iterates over all fields of the current type and executes a traverse action</summary>
		/// <param name="source">Original object</param>
		/// <param name="target">Target object</param>
		/// <param name="action">The action receiving a dot path representing the field pair and the <see cref="Traverse"/> instances</param>
        public static void IterateFields(object source, object target, Action<string, Traverse2, Traverse2> action)
		{
            if (action is null)
                return;

			var sourceTrv = Create(source);
			var targetTrv = Create(target);
			AccessTools.GetFieldNames(source).ForEach(f => action(f, sourceTrv.Field(f), targetTrv.Field(f)));
		}

		/// <summary>Iterates over all properties of the current type and executes a traverse action</summary>
		/// <param name="source">Original object</param>
		/// <param name="action">The action receiving a <see cref="Traverse"/> instance for each property</param>
        public static void IterateProperties(object source, Action<Traverse2> action)
		{
            if (action is null)
                return;

			var sourceTrv = Create(source);
			AccessTools.GetPropertyNames(source).ForEach(f => action(sourceTrv.Property(f)));
		}

		/// <summary>Iterates over all properties of the current type and executes a traverse action</summary>
		/// <param name="source">Original object</param>
		/// <param name="target">Target object</param>
		/// <param name="action">The action receiving a pair of <see cref="Traverse"/> instances for each property pair</param>
        public static void IterateProperties(object source, object target, Action<Traverse2, Traverse2> action)
		{
            if (action is null)
                return;

			var sourceTrv = Create(source);
			var targetTrv = Create(target);
			AccessTools.GetPropertyNames(source).ForEach(f => action(sourceTrv.Property(f), targetTrv.Property(f)));
		}

		/// <summary>Iterates over all properties of the current type and executes a traverse action</summary>
		/// <param name="source">Original object</param>
		/// <param name="target">Target object</param>
		/// <param name="action">The action receiving a dot path representing the property pair and the <see cref="Traverse"/> instances</param>
        public static void IterateProperties(object source, object target, Action<string, Traverse2, Traverse2> action)
		{
            if (action is null)
                return;

			var sourceTrv = Create(source);
			var targetTrv = Create(target);
			AccessTools.GetPropertyNames(source).ForEach(f => action(f, sourceTrv.Property(f), targetTrv.Property(f)));
		}

		/// <summary>A default field action that copies fields to fields</summary>
        public static Action<Traverse2, Traverse2> CopyFields = (from, to) =>
        {
			if (from is null || to is null)
				return;

            _ = to.SetValue(from.GetValue()!);
        };

		/// <summary>Returns a string that represents the current traverse</summary>
		/// <returns>A string representation</returns>
        public override string? ToString() => (_method ?? GetValue())?.ToString();
    }
}

#pragma warning restore
#nullable restore
#endif // HARMONYEXTENSIONS_DISABLE