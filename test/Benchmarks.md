
## Method

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2006/22H2/2022Update)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.203
  [Host]   : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  AOT      : .NET 7.0.5-servicing.23174.5, X64 NativeAOT AVX2
  ShortRun : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2

Platform=X64  Server=True  IterationCount=3  
LaunchCount=1  WarmupCount=3  

           Method |      Job |       Runtime | LoopCount |       Mean |       Error |     StdDev |  Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
----------------- |--------- |-------------- |---------- |-----------:|------------:|-----------:|-------:|--------:|-------:|----------:|------------:|
              Raw |      AOT | NativeAOT 7.0 |      5012 |   6.063 μs |   0.6098 μs |  0.0334 μs |   1.00 |    0.00 |      - |         - |          NA |
   ReflectionCall |      AOT | NativeAOT 7.0 |      5012 | 107.638 μs |  36.3611 μs |  1.9931 μs |  17.75 |    0.39 |      - |      56 B |          NA |
   ExpressionCall |      AOT | NativeAOT 7.0 |      5012 | 733.846 μs | 220.5812 μs | 12.0908 μs | 121.05 |    2.37 | 8.7891 | 1283072 B |          NA |
 StaticReflection |      AOT | NativeAOT 7.0 |      5012 |   6.060 μs |   0.0509 μs |  0.0028 μs |   1.00 |    0.01 |      - |      24 B |          NA |
                  |          |               |           |            |             |            |        |         |        |           |             |
              Raw | ShortRun |      .NET 7.0 |      5012 |   6.108 μs |   0.1786 μs |  0.0098 μs |   1.00 |    0.00 |      - |         - |          NA |
   ReflectionCall | ShortRun |      .NET 7.0 |      5012 | 105.801 μs |   0.4038 μs |  0.0221 μs |  17.32 |    0.03 |      - |      56 B |          NA |
   ExpressionCall | ShortRun |      .NET 7.0 |      5012 | 543.634 μs | 115.6132 μs |  6.3371 μs |  89.01 |    1.18 | 7.8125 | 1122688 B |          NA |
 StaticReflection | ShortRun |      .NET 7.0 |      5012 |  12.284 μs |   0.2728 μs |  0.0150 μs |   2.01 |    0.00 |      - |      24 B |          NA |


## ObjectCreate


BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2006/22H2/2022Update)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.203
  [Host]   : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  AOT      : .NET 7.0.5-servicing.23174.5, X64 NativeAOT AVX2
  ShortRun : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2

Platform=X64  Server=True  IterationCount=3  
LaunchCount=1  WarmupCount=3  

           Method |      Job |       Runtime | LoopCount |       Mean |      Error |    StdDev |  Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
----------------- |--------- |-------------- |---------- |-----------:|-----------:|----------:|-------:|--------:|-------:|----------:|------------:|
              Raw |      AOT | NativeAOT 7.0 |      5012 |   1.176 μs |  0.2458 μs | 0.0135 μs |   1.00 |    0.00 |      - |         - |          NA |
   ReflectionCall |      AOT | NativeAOT 7.0 |      5012 | 168.770 μs | 13.0202 μs | 0.7137 μs | 143.59 |    2.24 | 0.9766 |  160384 B |          NA |
   ExpressionCall |      AOT | NativeAOT 7.0 |      5012 | 390.033 μs |  4.3779 μs | 0.2400 μs | 331.83 |    3.97 | 4.8828 |  721728 B |          NA |
 StaticReflection |      AOT | NativeAOT 7.0 |      5012 |   1.183 μs |  0.1288 μs | 0.0071 μs |   1.01 |    0.01 |      - |         - |          NA |
                  |          |               |           |            |            |           |        |         |        |           |             |
              Raw | ShortRun |      .NET 7.0 |      5012 |   1.199 μs |  0.2253 μs | 0.0123 μs |   1.00 |    0.00 |      - |         - |          NA |
   ReflectionCall | ShortRun |      .NET 7.0 |      5012 |  64.813 μs |  7.0774 μs | 0.3879 μs |  54.05 |    0.61 | 1.0986 |  160384 B |          NA |
   ExpressionCall | ShortRun |      .NET 7.0 |      5012 |  27.498 μs |  3.5019 μs | 0.1920 μs |  22.93 |    0.36 | 1.1292 |  160384 B |          NA |
 StaticReflection | ShortRun |      .NET 7.0 |      5012 |   1.169 μs |  0.1276 μs | 0.0070 μs |   0.97 |    0.01 |      - |         - |          NA |

## PropertyRead


BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2006/22H2/2022Update)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.203
  [Host]   : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  AOT      : .NET 7.0.5-servicing.23174.5, X64 NativeAOT AVX2
  ShortRun : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2

Platform=X64  Server=True  IterationCount=3  
LaunchCount=1  WarmupCount=3  

           Method |      Job |       Runtime | LoopCount |       Mean |      Error |    StdDev |  Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
----------------- |--------- |-------------- |---------- |-----------:|-----------:|----------:|-------:|--------:|-------:|----------:|------------:|
              Raw |      AOT | NativeAOT 7.0 |      5012 |   1.206 μs |  0.6962 μs | 0.0382 μs |   1.00 |    0.00 |      - |         - |          NA |
   ReflectionCall |      AOT | NativeAOT 7.0 |      5012 | 101.171 μs | 14.2436 μs | 0.7807 μs |  83.97 |    2.27 | 0.8545 |  120288 B |          NA |
   ExpressionCall |      AOT | NativeAOT 7.0 |      5012 | 596.508 μs | 63.7647 μs | 3.4952 μs | 495.13 |   16.01 | 5.8594 |  882112 B |          NA |
 StaticReflection |      AOT | NativeAOT 7.0 |      5012 |  20.610 μs |  0.4713 μs | 0.0258 μs |  17.11 |    0.54 | 0.8545 |  120288 B |          NA |
                  |          |               |           |            |            |           |        |         |        |           |             |
              Raw | ShortRun |      .NET 7.0 |      5012 |   1.173 μs |  0.1730 μs | 0.0095 μs |   1.00 |    0.00 |      - |         - |          NA |
   ReflectionCall | ShortRun |      .NET 7.0 |      5012 |  74.681 μs |  7.8228 μs | 0.4288 μs |  63.69 |    0.85 | 0.7324 |  120288 B |          NA |
   ExpressionCall | ShortRun |      .NET 7.0 |      5012 |  32.538 μs |  3.2685 μs | 0.1792 μs |  27.75 |    0.33 | 0.8545 |  120288 B |          NA |
 StaticReflection | ShortRun |      .NET 7.0 |      5012 |  26.590 μs |  8.4435 μs | 0.4628 μs |  22.68 |    0.35 | 0.8545 |  120288 B |          NA |

## PropertyWrite


BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2006/22H2/2022Update)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.203
  [Host]   : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  AOT      : .NET 7.0.5-servicing.23174.5, X64 NativeAOT AVX2
  ShortRun : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2

Platform=X64  Server=True  IterationCount=3  
LaunchCount=1  WarmupCount=3  

           Method |      Job |       Runtime | LoopCount |       Mean |      Error |    StdDev |  Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
----------------- |--------- |-------------- |---------- |-----------:|-----------:|----------:|-------:|--------:|-------:|----------:|------------:|
              Raw |      AOT | NativeAOT 7.0 |      5012 |   1.224 μs |  0.3970 μs | 0.0218 μs |   1.00 |    0.00 |      - |         - |          NA |
   ReflectionCall |      AOT | NativeAOT 7.0 |      5012 | 150.078 μs | 36.0379 μs | 1.9754 μs | 122.65 |    3.70 | 1.9531 |  280672 B |          NA |
   ExpressionCall |      AOT | NativeAOT 7.0 |      5012 | 770.908 μs | 74.8610 μs | 4.1034 μs | 629.97 |   14.60 | 8.7891 | 1283073 B |          NA |
 StaticReflection |      AOT | NativeAOT 7.0 |      5012 |  20.231 μs |  1.5305 μs | 0.0839 μs |  16.53 |    0.35 | 0.8545 |  120288 B |          NA |
                  |          |               |           |            |            |           |        |         |        |           |             |
              Raw | ShortRun |      .NET 7.0 |      5012 |   1.189 μs |  0.1809 μs | 0.0099 μs |   1.00 |    0.00 |      - |         - |          NA |
   ReflectionCall | ShortRun |      .NET 7.0 |      5012 | 146.563 μs | 12.8178 μs | 0.7026 μs | 123.28 |    1.61 | 0.7324 |  120288 B |          NA |
   ExpressionCall | ShortRun |      .NET 7.0 |      5012 |  27.239 μs |  2.5584 μs | 0.1402 μs |  22.91 |    0.08 | 0.8545 |  120288 B |          NA |
 StaticReflection | ShortRun |      .NET 7.0 |      5012 |  21.558 μs |  1.3979 μs | 0.0766 μs |  18.13 |    0.22 | 0.8545 |  120288 B |          NA |
