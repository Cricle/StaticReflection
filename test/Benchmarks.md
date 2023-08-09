
## Method
``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2006/22H2/2022Update)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=8.0.100-preview.6.23330.14
  [Host]   : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2
  AOT      : .NET 7.0.8-servicing.23318.7, X64 NativeAOT AVX2
  ShortRun : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2

Platform=X64  Server=True  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|           Method |      Job |       Runtime | LoopCount |       Mean |       Error |     StdDev |  Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|----------------- |--------- |-------------- |---------- |-----------:|------------:|-----------:|-------:|--------:|-------:|----------:|------------:|
|              Raw |      AOT | NativeAOT 7.0 |      5012 |   6.016 μs |   0.1154 μs |  0.0063 μs |   1.00 |    0.00 |      - |         - |          NA |
|   ReflectionCall |      AOT | NativeAOT 7.0 |      5012 |  96.739 μs |  26.3280 μs |  1.4431 μs |  16.08 |    0.23 |      - |      32 B |          NA |
|   ExpressionCall |      AOT | NativeAOT 7.0 |      5012 | 680.023 μs | 217.8110 μs | 11.9390 μs | 113.04 |    1.89 | 6.8359 | 1042496 B |          NA |
| StaticReflection |      AOT | NativeAOT 7.0 |      5012 |   7.444 μs |   0.5588 μs |  0.0306 μs |   1.24 |    0.01 |      - |      32 B |          NA |
|                  |          |               |           |            |             |            |        |         |        |           |             |
|              Raw | ShortRun |      .NET 7.0 |      5012 |   7.374 μs |   1.0696 μs |  0.0586 μs |   1.00 |    0.00 |      - |         - |          NA |
|   ReflectionCall | ShortRun |      .NET 7.0 |      5012 | 110.094 μs |  36.3773 μs |  1.9940 μs |  14.93 |    0.23 |      - |      32 B |          NA |
|   ExpressionCall | ShortRun |      .NET 7.0 |      5012 | 466.819 μs |  31.4891 μs |  1.7260 μs |  63.31 |    0.74 | 5.8594 |  882112 B |          NA |
| StaticReflection | ShortRun |      .NET 7.0 |      5012 |  12.195 μs |   3.1662 μs |  0.1735 μs |   1.65 |    0.01 |      - |      32 B |          NA |


## ObjectCreate

``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2006/22H2/2022Update)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=8.0.100-preview.6.23330.14
  [Host]   : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2
  AOT      : .NET 7.0.8-servicing.23318.7, X64 NativeAOT AVX2
  ShortRun : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2

Platform=X64  Server=True  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|           Method |      Job |       Runtime | LoopCount |       Mean |      Error |    StdDev |  Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|----------------- |--------- |-------------- |---------- |-----------:|-----------:|----------:|-------:|--------:|-------:|----------:|------------:|
|              Raw |      AOT | NativeAOT 7.0 |      5012 |   1.171 μs |  0.0694 μs | 0.0038 μs |   1.00 |    0.00 |      - |         - |          NA |
|   ReflectionCall |      AOT | NativeAOT 7.0 |      5012 | 178.302 μs | 10.5394 μs | 0.5777 μs | 152.25 |    0.95 | 0.9766 |  160384 B |          NA |
|   ExpressionCall |      AOT | NativeAOT 7.0 |      5012 | 425.804 μs | 75.3138 μs | 4.1282 μs | 363.58 |    2.35 | 4.8828 |  721728 B |          NA |
| StaticReflection |      AOT | NativeAOT 7.0 |      5012 |   1.170 μs |  0.0987 μs | 0.0054 μs |   1.00 |    0.01 |      - |         - |          NA |
|                  |          |               |           |            |            |           |        |         |        |           |             |
|              Raw | ShortRun |      .NET 7.0 |      5012 |   1.161 μs |  0.1222 μs | 0.0067 μs |   1.00 |    0.00 |      - |         - |          NA |
|   ReflectionCall | ShortRun |      .NET 7.0 |      5012 |  64.604 μs |  3.7983 μs | 0.2082 μs |  55.66 |    0.46 | 1.0986 |  160384 B |          NA |
|   ExpressionCall | ShortRun |      .NET 7.0 |      5012 |  27.307 μs |  3.5370 μs | 0.1939 μs |  23.53 |    0.30 | 1.1292 |  160384 B |          NA |
| StaticReflection | ShortRun |      .NET 7.0 |      5012 |   1.191 μs |  0.2042 μs | 0.0112 μs |   1.03 |    0.01 |      - |         - |          NA |


## PropertyRead

``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2006/22H2/2022Update)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=8.0.100-preview.6.23330.14
  [Host]   : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2
  AOT      : .NET 7.0.8-servicing.23318.7, X64 NativeAOT AVX2
  ShortRun : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2

Platform=X64  Server=True  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|           Method |      Job |       Runtime | LoopCount |       Mean |      Error |    StdDev |  Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|----------------- |--------- |-------------- |---------- |-----------:|-----------:|----------:|-------:|--------:|-------:|----------:|------------:|
|              Raw |      AOT | NativeAOT 7.0 |      5012 |   1.183 μs |  0.1807 μs | 0.0099 μs |   1.00 |    0.00 |      - |         - |          NA |
|   ReflectionCall |      AOT | NativeAOT 7.0 |      5012 |  82.294 μs |  3.3044 μs | 0.1811 μs |  69.54 |    0.43 |      - |         - |          NA |
|   ExpressionCall |      AOT | NativeAOT 7.0 |      5012 | 565.929 μs | 50.3752 μs | 2.7612 μs | 478.25 |    6.17 | 4.8828 |  761824 B |          NA |
| StaticReflection |      AOT | NativeAOT 7.0 |      5012 |  14.452 μs |  0.5682 μs | 0.0311 μs |  12.21 |    0.13 |      - |      32 B |          NA |
|                  |          |               |           |            |            |           |        |         |        |           |             |
|              Raw | ShortRun |      .NET 7.0 |      5012 |   1.203 μs |  1.0619 μs | 0.0582 μs |   1.00 |    0.00 |      - |         - |          NA |
|   ReflectionCall | ShortRun |      .NET 7.0 |      5012 |  54.136 μs |  2.1328 μs | 0.1169 μs |  45.06 |    2.23 |      - |         - |          NA |
|   ExpressionCall | ShortRun |      .NET 7.0 |      5012 |   7.538 μs |  0.1485 μs | 0.0081 μs |   6.27 |    0.30 |      - |         - |          NA |
| StaticReflection | ShortRun |      .NET 7.0 |      5012 |  10.810 μs |  0.9135 μs | 0.0501 μs |   9.00 |    0.44 |      - |      32 B |          NA |

## PropertyWrite

``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2006/22H2/2022Update)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=8.0.100-preview.6.23330.14
  [Host]   : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2
  AOT      : .NET 7.0.8-servicing.23318.7, X64 NativeAOT AVX2
  ShortRun : .NET 7.0.9 (7.0.923.32018), X64 RyuJIT AVX2

Platform=X64  Server=True  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|           Method |      Job |       Runtime | LoopCount |      Mean |     Error |   StdDev | Ratio | RatioSD |   Gen0 |  Allocated | Alloc Ratio |
|----------------- |--------- |-------------- |---------- |----------:|----------:|---------:|------:|--------:|-------:|-----------:|------------:|
|              Raw |      AOT | NativeAOT 7.0 |      5012 |  52.27 μs | 22.661 μs | 1.242 μs |  1.00 |    0.00 | 1.0986 |  156.31 KB |        1.00 |
|   ReflectionCall |      AOT | NativeAOT 7.0 |      5012 | 198.79 μs | 17.250 μs | 0.946 μs |  3.81 |    0.10 | 2.1973 |  312.94 KB |        2.00 |
|   ExpressionCall |      AOT | NativeAOT 7.0 |      5012 | 853.24 μs | 72.186 μs | 3.957 μs | 16.33 |    0.32 | 7.8125 | 1174.38 KB |        7.51 |
| StaticReflection |      AOT | NativeAOT 7.0 |      5012 |  64.65 μs |  2.422 μs | 0.133 μs |  1.24 |    0.03 | 1.0986 |  156.34 KB |        1.00 |
|                  |          |               |           |           |           |          |       |         |        |            |             |
|              Raw | ShortRun |      .NET 7.0 |      5012 |  62.83 μs | 24.774 μs | 1.358 μs |  1.00 |    0.00 | 1.0986 |  156.31 KB |        1.00 |
|   ReflectionCall | ShortRun |      .NET 7.0 |      5012 | 177.58 μs |  0.703 μs | 0.039 μs |  2.83 |    0.06 | 0.9766 |  156.31 KB |        1.00 |
|   ExpressionCall | ShortRun |      .NET 7.0 |      5012 |  66.29 μs |  9.170 μs | 0.503 μs |  1.06 |    0.03 | 1.0986 |  156.31 KB |        1.00 |
| StaticReflection | ShortRun |      .NET 7.0 |      5012 |  69.13 μs | 15.621 μs | 0.856 μs |  1.10 |    0.03 | 1.0986 |  156.34 KB |        1.00 |
