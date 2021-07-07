﻿// <auto-generated>
//   This code file has automatically been added by the "Harmony.Extensions" NuGet package (https://www.nuget.org/packages/Harmony.Extensions).
//   Please see https://github.com/BUTR/Harmony.Extensions for more information.
//
//   IMPORTANT:
//   DO NOT DELETE THIS FILE if you are using a "packages.config" file to manage your NuGet references.
//   Consider migrating to PackageReferences instead:
//   https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference
//   Migrating brings the following benefits:
//   * The "Harmony.Extensions" folder and the "AccessCacheHandle.cs" file don't appear in your project.
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
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Reflection;

    [ExcludeFromCodeCoverage]
    internal readonly struct AccessCacheHandle
    {
        internal enum MemberType { Any, Static, Instance }

        private delegate object AccessCacheCtorDelegate();
        private delegate FieldInfo GetFieldInfoDelegate(object instance, Type type, string name, MemberType memberType = MemberType.Any, bool declaredOnly = false);
        private delegate PropertyInfo GetPropertyInfoDelegate(object instance, Type type, string name, MemberType memberType = MemberType.Any, bool declaredOnly = false);
        private delegate MethodBase GetMethodInfoDelegate(object instance, Type type, string name, Type[] arguments, MemberType memberType = MemberType.Any, bool declaredOnly = false);

        private static readonly AccessCacheCtorDelegate? AccessCacheCtorMethod;
        private static readonly GetFieldInfoDelegate? GetFieldInfoMethod;
        private static readonly GetPropertyInfoDelegate? GetPropertyInfoMethod;
        private static readonly GetMethodInfoDelegate? GetMethodInfoMethod;

        static AccessCacheHandle()
        {
            AccessCacheCtorMethod = AccessTools2.GetConstructorDelegate<AccessCacheCtorDelegate>("HarmonyLib.AccessCache");
            GetFieldInfoMethod = AccessTools2.GetDelegateObjectInstance<GetFieldInfoDelegate>("HarmonyLib.AccessCache:GetFieldInfo");
            GetPropertyInfoMethod = AccessTools2.GetDelegateObjectInstance<GetPropertyInfoDelegate>("HarmonyLib.AccessCache:GetPropertyInfo");
            GetMethodInfoMethod = AccessTools2.GetDelegateObjectInstance<GetMethodInfoDelegate>("HarmonyLib.AccessCache:GetMethodInfo");
        }


        public static AccessCacheHandle? Create()
        {
            var accessCache = AccessCacheCtorMethod?.Invoke();
            return accessCache is not null ? new(accessCache) : null;
        }

        private readonly object _accessCache;

        private AccessCacheHandle(object accessCache) => _accessCache = accessCache;

        public FieldInfo? GetFieldInfo(Type type, string name, MemberType memberType = MemberType.Any, bool declaredOnly = false) =>
            GetFieldInfoMethod?.Invoke(_accessCache, type, name, memberType, declaredOnly);

        public PropertyInfo? GetPropertyInfo(Type type, string name, MemberType memberType = MemberType.Any, bool declaredOnly = false) =>
            GetPropertyInfoMethod?.Invoke(_accessCache, type, name, memberType, declaredOnly);

        public MethodBase? GetMethodInfo(Type type, string name, Type[] arguments, MemberType memberType = MemberType.Any, bool declaredOnly = false) =>
            GetMethodInfoMethod?.Invoke(_accessCache, type, name, arguments, memberType, declaredOnly);
    }
}

#pragma warning restore
#nullable restore
#endif // HARMONYEXTENSIONS_DISABLE