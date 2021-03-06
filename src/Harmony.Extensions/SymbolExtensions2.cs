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

namespace Harmony.Extensions
{
    using global::HarmonyLib;

    using global::System;
    using global::System.Linq.Expressions;
    using global::System.Reflection;

    public static class SymbolExtensions2
    {
        public static AccessTools.FieldRef<TObject, TField>? FieldRefAccess<TObject, TField>(
            Expression<Func<TObject, TField>> expression)
        {
            return FieldRefAccess<TObject, TField>((LambdaExpression) expression);
        }

        public static AccessTools.FieldRef<TObject, TField>? FieldRefAccess<TObject, TField>(
            LambdaExpression expression)
        {
            if (expression.Body is MemberExpression { Member: FieldInfo fieldInfo })
                return fieldInfo == null ? null : AccessTools.FieldRefAccess<TObject, TField>(fieldInfo);

            throw new ArgumentException("Invalid Expression. Expression should consist of a Field return only.");
        }


        public static ConstructorInfo GetConstructorInfo<T>(Expression<Func<T>> expression)
        {
            return GetConstructorInfo((LambdaExpression) expression);
        }

        public static ConstructorInfo GetConstructorInfo<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            return GetConstructorInfo((LambdaExpression) expression);
        }

        public static ConstructorInfo GetConstructorInfo(LambdaExpression expression)
        {
            if (expression.Body is NewExpression { Constructor: { } } body)
                return body.Constructor;

            throw new ArgumentException("Invalid Expression. Expression should consist of a Field return only.");
        }

        public static FieldInfo GetFieldInfo<T>(Expression<Func<T>> expression)
        {
            return GetFieldInfo((LambdaExpression) expression);
        }

        public static FieldInfo GetFieldInfo<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            return GetFieldInfo((LambdaExpression) expression);
        }

        public static FieldInfo GetFieldInfo(LambdaExpression expression)
        {
            if (expression.Body is MemberExpression { Member: FieldInfo fieldInfo })
                return fieldInfo;
            throw new ArgumentException("Invalid Expression. Expression should consist of a Field return only.");
        }

        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T>> expression)
        {
            return GetPropertyInfo((LambdaExpression) expression);
        }

        public static PropertyInfo GetPropertyInfo<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            return GetPropertyInfo((LambdaExpression) expression);
        }

        public static PropertyInfo GetPropertyInfo(LambdaExpression expression)
        {
            if (expression.Body is MemberExpression { Member: PropertyInfo propertyInfo })
                return propertyInfo;

            throw new ArgumentException("Invalid Expression. Expression should consist of a Property return only.");
        }
    }
}

#pragma warning restore
#nullable restore
#endif // HARMONYEXTENSIONS_DISABLE