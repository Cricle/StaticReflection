
## Method
``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2006/22H2/2022Update)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.203
  [Host]   : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  AOT      : .NET 7.0.5-servicing.23174.5, X64 NativeAOT AVX2
  ShortRun : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2

Platform=X64  Server=True  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|           Method |      Job |       Runtime | LoopCount |       Mean |       Error |     StdDev |  Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|----------------- |--------- |-------------- |---------- |-----------:|------------:|-----------:|-------:|--------:|-------:|----------:|------------:|
|              Raw |      AOT | NativeAOT 7.0 |      5012 |   6.211 μs |   4.8017 μs |  0.2632 μs |   1.00 |    0.00 |      - |         - |          NA |
|   ReflectionCall |      AOT | NativeAOT 7.0 |      5012 | 102.560 μs |  39.6599 μs |  2.1739 μs |  16.52 |    0.41 |      - |      56 B |          NA |
|   ExpressionCall |      AOT | NativeAOT 7.0 |      5012 | 785.787 μs | 261.7251 μs | 14.3460 μs | 126.72 |    7.45 | 8.7891 | 1283072 B |          NA |
| StaticReflection |      AOT | NativeAOT 7.0 |      5012 |   7.456 μs |   1.9480 μs |  0.1068 μs |   1.20 |    0.06 |      - |      24 B |          NA |
|                  |          |               |           |            |             |            |        |         |        |           |             |
|              Raw | ShortRun |      .NET 7.0 |      5012 |   6.176 μs |   0.5750 μs |  0.0315 μs |   1.00 |    0.00 |      - |         - |          NA |
|   ReflectionCall | ShortRun |      .NET 7.0 |      5012 | 107.819 μs |  20.6378 μs |  1.1312 μs |  17.46 |    0.10 |      - |      56 B |          NA |
|   ExpressionCall | ShortRun |      .NET 7.0 |      5012 | 549.402 μs |  12.6833 μs |  0.6952 μs |  88.95 |    0.50 | 7.8125 | 1122688 B |          NA |
| StaticReflection | ShortRun |      .NET 7.0 |      5012 |  12.423 μs |   1.6104 μs |  0.0883 μs |   2.01 |    0.00 |      - |      24 B |          NA |


## ObjectCreate

``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2006/22H2/2022Update)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.203
  [Host]   : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  AOT      : .NET 7.0.5-servicing.23174.5, X64 NativeAOT AVX2
  ShortRun : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2

Platform=X64  Server=True  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|           Method |      Job |       Runtime | LoopCount |       Mean |      Error |    StdDev |  Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|----------------- |--------- |-------------- |---------- |-----------:|-----------:|----------:|-------:|--------:|-------:|----------:|------------:|
|              Raw |      AOT | NativeAOT 7.0 |      5012 |   1.194 μs |  0.0216 μs | 0.0012 μs |   1.00 |    0.00 |      - |         - |          NA |
|   ReflectionCall |      AOT | NativeAOT 7.0 |      5012 | 189.577 μs |  5.1819 μs | 0.2840 μs | 158.73 |    0.16 | 0.9766 |  160384 B |          NA |
|   ExpressionCall |      AOT | NativeAOT 7.0 |      5012 | 422.646 μs | 53.4768 μs | 2.9312 μs | 353.87 |    2.71 | 4.8828 |  721728 B |          NA |
| StaticReflection |      AOT | NativeAOT 7.0 |      5012 |   1.224 μs |  0.3699 μs | 0.0203 μs |   1.02 |    0.02 |      - |         - |          NA |
|                  |          |               |           |            |            |           |        |         |        |           |             |
|              Raw | ShortRun |      .NET 7.0 |      5012 |   1.202 μs |  0.2588 μs | 0.0142 μs |   1.00 |    0.00 |      - |         - |          NA |
|   ReflectionCall | ShortRun |      .NET 7.0 |      5012 |  66.662 μs | 30.9529 μs | 1.6966 μs |  55.46 |    1.96 | 1.0986 |  160384 B |          NA |
|   ExpressionCall | ShortRun |      .NET 7.0 |      5012 |  30.965 μs | 11.1004 μs | 0.6085 μs |  25.76 |    0.79 | 1.1292 |  160384 B |          NA |
| StaticReflection | ShortRun |      .NET 7.0 |      5012 |   1.214 μs |  0.0808 μs | 0.0044 μs |   1.01 |    0.02 |      - |         - |          NA |


## PropertyRead

``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2006/22H2/2022Update)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.203
  [Host]   : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  AOT      : .NET 7.0.5-servicing.23174.5, X64 NativeAOT AVX2
  ShortRun : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2

Platform=X64  Server=True  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|           Method |      Job |       Runtime | LoopCount |       Mean |       Error |    StdDev |  Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|----------------- |--------- |-------------- |---------- |-----------:|------------:|----------:|-------:|--------:|-------:|----------:|------------:|
|              Raw |      AOT | NativeAOT 7.0 |      5012 |   1.197 μs |   0.3370 μs | 0.0185 μs |   1.00 |    0.00 |      - |         - |          NA |
|   ReflectionCall |      AOT | NativeAOT 7.0 |      5012 | 101.288 μs |  12.0815 μs | 0.6622 μs |  84.65 |    1.63 | 0.8545 |  120288 B |          NA |
|   ExpressionCall |      AOT | NativeAOT 7.0 |      5012 | 605.515 μs | 111.2058 μs | 6.0956 μs | 506.02 |    7.21 | 5.8594 |  882112 B |          NA |
| StaticReflection |      AOT | NativeAOT 7.0 |      5012 |  21.233 μs |   1.3268 μs | 0.0727 μs |  17.74 |    0.23 | 0.8545 |  120288 B |          NA |
|                  |          |               |           |            |             |           |        |         |        |           |             |
|              Raw | ShortRun |      .NET 7.0 |      5012 |   1.179 μs |   0.1655 μs | 0.0091 μs |   1.00 |    0.00 |      - |         - |          NA |
|   ReflectionCall | ShortRun |      .NET 7.0 |      5012 |  75.988 μs |   8.7597 μs | 0.4802 μs |  64.48 |    0.90 | 0.8545 |  120288 B |          NA |
|   ExpressionCall | ShortRun |      .NET 7.0 |      5012 |  33.993 μs |   9.6227 μs | 0.5275 μs |  28.84 |    0.61 | 0.8545 |  120288 B |          NA |
| StaticReflection | ShortRun |      .NET 7.0 |      5012 |  27.295 μs |   4.5498 μs | 0.2494 μs |  23.16 |    0.06 | 0.8545 |  120288 B |          NA |

## PropertyWrite

``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2006/22H2/2022Update)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.203
  [Host]   : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  AOT      : .NET 7.0.5-servicing.23174.5, X64 NativeAOT AVX2
  ShortRun : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2

Platform=X64  Server=True  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|           Method |      Job |       Runtime | LoopCount |       Mean |       Error |    StdDev |  Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|----------------- |--------- |-------------- |---------- |-----------:|------------:|----------:|-------:|--------:|-------:|----------:|------------:|
|              Raw |      AOT | NativeAOT 7.0 |      5012 |   2.517 μs |   1.1000 μs | 0.0603 μs |   1.00 |    0.00 |      - |         - |          NA |
|   ReflectionCall |      AOT | NativeAOT 7.0 |      5012 | 160.702 μs |  12.2707 μs | 0.6726 μs |  63.88 |    1.80 | 1.9531 |  280672 B |          NA |
|   ExpressionCall |      AOT | NativeAOT 7.0 |      5012 | 766.822 μs | 182.1186 μs | 9.9825 μs | 304.83 |   10.86 | 8.7891 | 1283072 B |          NA |
| StaticReflection |      AOT | NativeAOT 7.0 |      5012 |  21.119 μs |   9.8296 μs | 0.5388 μs |   8.39 |    0.32 | 0.8545 |  120288 B |          NA |
|                  |          |               |           |            |             |           |        |         |        |           |             |
|              Raw | ShortRun |      .NET 7.0 |      5012 |   1.191 μs |   0.1313 μs | 0.0072 μs |   1.00 |    0.00 |      - |         - |          NA |
|   ReflectionCall | ShortRun |      .NET 7.0 |      5012 | 143.513 μs |  27.4212 μs | 1.5031 μs | 120.52 |    1.97 | 0.7324 |  120288 B |          NA |
|   ExpressionCall | ShortRun |      .NET 7.0 |      5012 |  27.813 μs |   1.0046 μs | 0.0551 μs |  23.36 |    0.10 | 0.8545 |  120288 B |          NA |
| StaticReflection | ShortRun |      .NET 7.0 |      5012 |  22.083 μs |   2.6402 μs | 0.1447 μs |  18.54 |    0.09 | 0.8545 |  120288 B |          NA |
