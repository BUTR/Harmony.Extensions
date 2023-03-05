﻿// <auto-generated>
//   This code file has automatically been added by the "Harmony.Extensions" NuGet package (https://www.nuget.org/packages/Harmony.Extensions).
//   Please see https://github.com/BUTR/Harmony.Extensions for more information.
//
//   IMPORTANT:
//   DO NOT DELETE THIS FILE if you are using a "packages.config" file to manage your NuGet references.
//   Consider migrating to PackageReferences instead:
//   https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference
//   Migrating brings the following benefits:
//   * The "Harmony.Extensions" folder and the "HarmonyExtensions.cs" file don't appear in your project.
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
    using global::System.Diagnostics;
    using global::System.Reflection;

    /// <summary>Extension class for working with Harmony.</summary>
#if !HARMONYEXTENSIONS_PUBLIC
    internal
#else
    public
#endif
        static class HarmonyExtensions
    {
        public static bool TryPatch(this Harmony harmony, MethodBase? original, MethodInfo? prefix = null, MethodInfo? postfix = null, MethodInfo? transpiler = null, MethodInfo? finalizer = null)
        {
            if (original is null || (prefix is null && postfix is null && transpiler is null && finalizer is null))
            {
                Trace.TraceError($"HarmonyExtensions.TryPatch: 'original' or all methods are null");
                return false;
            }

            var prefixMethod = prefix is null ? null : new HarmonyMethod(prefix);
            var postfixMethod = postfix is null ? null : new HarmonyMethod(postfix);
            var transpilerMethod = transpiler is null ? null : new HarmonyMethod(transpiler);
            var finalizerMethod = finalizer is null ? null : new HarmonyMethod(finalizer);

            try
            {
#if HARMONYEXTENSIONS_HARMONYX
                harmony.Patch(original, prefixMethod, postfixMethod, transpilerMethod, finalizerMethod, null);
#else
                harmony.Patch(original, prefixMethod, postfixMethod, transpilerMethod, finalizerMethod);
#endif
            }
            catch (Exception e)
            {
                Trace.TraceError($"HarmonyExtensions.TryPatch: Exception occurred: {e}, original '{original}'");
                return false;
            }

            return true;
        }
        
#if HARMONYEXTENSIONS_HARMONYX
        public static bool TryPatch(this Harmony harmony, MethodBase? original, MethodInfo? prefix = null, MethodInfo? postfix = null, MethodInfo? transpiler = null, MethodInfo? finalizer = null, MethodInfo? ilmanipulator = null)
        {
            if (original is null || (prefix is null && postfix is null && transpiler is null && finalizer is null && ilmanipulator is null))
            {
                Trace.TraceError($"HarmonyExtensions.TryPatch: 'original' or all methods are null");
                return false;
            }

            var prefixMethod = prefix is null ? null : new HarmonyMethod(prefix);
            var postfixMethod = postfix is null ? null : new HarmonyMethod(postfix);
            var transpilerMethod = transpiler is null ? null : new HarmonyMethod(transpiler);
            var finalizerMethod = finalizer is null ? null : new HarmonyMethod(finalizer);
            var ilmanipulatorMethod = ilmanipulator is null ? null : new HarmonyMethod(ilmanipulator);

            try
            {
                harmony.Patch(original, prefixMethod, postfixMethod, transpilerMethod, finalizerMethod, ilmanipulatorMethod);
            }
            catch (Exception e)
            {
                Trace.TraceError($"HarmonyExtensions.TryPatch: Exception occurred: {e}, original '{original}'");
                return false;
            }

            return true;
        }
#endif

        public static ReversePatcher? TryCreateReversePatcher(this Harmony harmony, MethodBase? original, MethodInfo? standin)
        {
            if (original is null || standin is null)
            {
                Trace.TraceError($"HarmonyExtensions.TryCreateReversePatcher: 'original' or 'standin' is null");
                return null;
            }

            try
            {
                return harmony.CreateReversePatcher(original, new HarmonyMethod(standin));
            }
            catch (Exception e)
            {
                Trace.TraceError($"HarmonyExtensions.TryCreateReversePatcher: Exception occurred: {e}, original '{original}'");
                return null;
            }
        }

        public static bool TryCreateReversePatcher(this Harmony harmony, MethodBase? original, MethodInfo? standin, out ReversePatcher? result)
        {
            if (original is null || standin is null)
            {
                Trace.TraceError($"HarmonyExtensions.TryCreateReversePatcher: 'original' or 'standin' is null");
                result = null;
                return false;
            }

            try
            {
                result = harmony.CreateReversePatcher(original, new HarmonyMethod(standin));
                return true;
            }
            catch (Exception e)
            {
                Trace.TraceError($"HarmonyExtensions.TryCreateReversePatcher: Exception occurred: {e}, original '{original}'");
                result = null;
                return false;
            }
        }
    }
}

#pragma warning restore
#nullable restore
#endif // HARMONYEXTENSIONS_DISABLE