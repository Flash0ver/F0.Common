# F0.Common
CHANGELOG

## vNext

## v0.11.0 (2022-08-15)
- Added `Mathematics.Comparable.Min<T>`, returning the smaller of two `System.IComparable<T>` instances.
- Added `Mathematics.Comparable.Max<T>`, returning the larger of two `System.IComparable<T>` instances.

## v0.10.0 (2021-11-11)
- Added target framework: `.NET 5`.
- Added support for platform `browser`.
- Added annotations for _nullable reference types_.
- Added `Mathematics.Parity` checks for _native-sized integers_ (`nint` and `nuint`).
- Changed `Extensions.TypeExtensions.GetFriendlyName` and `Extensions.TypeExtensions.GetFriendlyFullName`, pretty-printing both _native-sized integers_ (`System.IntPtr` and `System.UIntPtr`) as _C# 9.0_ keywords (`nint` and `nuint`).
- Updated `.NET Standard 2.0` dependency on `Microsoft.Bcl.AsyncInterfaces` from `1.1.1` to `5.0.0`.
- Fixed `TypeExtensions.GetFriendlyName`, throwing a `System.NullReferenceException` when passing a `System.Type` without a _namespace_.
- Package: Embed icon (fixed _NuGet Warning NU5048_), keep fallback icon URL.

## v0.9.0 (2020-10-20)
- Added `Extensions.TypeExtensions.GetFriendlyName`, pretty-printing a `System.Type` (_built-in C#_, _array_, _nested_, _generic_, _nullable value_, _anonymous_, _C# tuple_), excluding namespaces.
- Added `Extensions.TypeExtensions.GetFriendlyFullName`, pretty-printing a `System.Type` (_built-in C#_, _array_, _nested_, _generic_, _nullable value_, _anonymous_, _C# tuple_), including namespaces.
- Updated `.NET Standard 2.0` dependency on `Microsoft.Bcl.AsyncInterfaces` from `1.1.0` to `1.1.1`.

## v0.8.0 (2020-03-31)
- Added `Mathematics.Comparable.Clamp<T>`, clamping a `System.IComparable<T>` to an inclusive range.

## v0.7.0 (2019-12-31)
- Added target framework: `.NET Standard 2.1`.

## v0.6.0 (2019-10-30)
- Added `Primitives.NullAsyncDisposable`, a Null Object implementation of `System.IAsyncDisposable` as Singleton.

## v0.5.0 (2019-10-10)
- Added `Primitives.NullDisposable`, a Null Object implementation of `System.IDisposable` as Singleton.

## v0.4.0 (2019-07-31)
- Added `Mathematics.Parity` checks for `uint`, `long` and `ulong` (even/odd).

## v0.3.0 (2019-04-30)
- Changed target framework from `.NET Standard 1.0` to `.NET Standard 2.0`.
- Package: Use license expression instead of deprecated license URL (fixed _NuGet Warning NU5125_).

## v0.2.0 (2018-12-21)
- Added `Primitives.ImmediateProgress<T>`, a synchronous `System.IProgress<T>`, unlike `System.Progress<T>`.
- Added `Primitives.NullProgress<T>`, a Null Object implementation of `System.IProgress<T>` as Singleton.

## v0.1.0 (2018-09-18)
- Added `Mathematics.Parity`, checking whether an `int` is even or odd.
