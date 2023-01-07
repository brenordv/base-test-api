[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=brenordv_base-test-api&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=brenordv_base-test-api)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=brenordv_base-test-api&metric=bugs)](https://sonarcloud.io/summary/new_code?id=brenordv_base-test-api)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=brenordv_base-test-api&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=brenordv_base-test-api)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=brenordv_base-test-api&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=brenordv_base-test-api)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=brenordv_base-test-api&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=brenordv_base-test-api)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=brenordv_base-test-api&metric=coverage)](https://sonarcloud.io/summary/new_code?id=brenordv_base-test-api)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=brenordv_base-test-api&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=brenordv_base-test-api)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=brenordv_base-test-api&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=brenordv_base-test-api)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=brenordv_base-test-api&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=brenordv_base-test-api)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=brenordv_base-test-api&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=brenordv_base-test-api)

# Base Test API
I created this API as base for any other tests I want to do.
So this project have some basic functionality. As my tests require, I'll add more complex features.

## What's implemented so far?

1. Products service and repository with some basic CRUD (actually just CRU) funcionatlities;
2. MemoryCache decorator on the repository;
3. (A variant of the) Observer pattern to allow bust the cache whenever a product is modified;
4. Decorator on top of the controller to measure the elapsed time of each endpoint;
5. Swagger.

## What's next?

1. Secure the endpoints
2. Endpoint validation attributes (to reject invalid requests as soon as possible).
3. Exception interceptor (to prevent exceptions frrom leaking into the response).

## Storage
For storage, I'm using LiteDB because I wanted to try a new (to me) embedded NoSql.
If specified in the appsettings, it will save in a different folder, otherwise, the database file will be creted in the same folder as the API executable.

To change the folder, in the appsettings.json file, change the value of `testDbFolder` to something more appropriate.

Example: 
```json
  "AppConfig": {
    "testDbFolder": "c:\\full\\path\\to\\test\\db\\folder"
  },
```

> Note: You can also create an `appsettings.local.json` file and add this settings there.

## To populate a local Database
Run the API in DEBUG and you'll have a special endpoint that does that: `/dev/populate-db`


# Updates
## 2023-01-07 - Benchmarks regarding AutoMapper
So I read in a couple of articles that AutoMapper is bad for you... kind of. 
In a nutshell, the two main against AutoMapper were:
1. Somewhat messy setup that may place business rules in the wrong place;
2. Performance issues due to reflection.

I personally really like AutoMapper, but I wanted to see if it was really that bad. So I created a simple test to see how it would perform.

The benchmark scenarios are as follows:
1. AutoMapper_SingleEntityConverted: Using AutoMapper to convert 1 entity;
2. AutoMapper_ListOfEntitiesConverted: Using AutoMapper to convert a list of N entities;
3. Explicit_SingleEntityConverted: Using explicit mapping to convert 1 entity;
4. Explicit_ListOfEntitiesLINQConverted: Using explicit mapping to convert a list of N entities using LINQ;
5. DirectAssignment_SingleEntityConverted: Using direct assignment to convert 1 entity;
6. DirectAssignment_ListOfEntitiesConverted: Using direct assignment to convert a list of N entities.

The workloads were lists of Entities of the following sizes: 1, 5, 10, 100, 1000 and 5000.

### Results for workload: 1

| Method                                   | QuantityEntities |       Mean |     Error |    StdDev |     Median |        Min |       Max |   Gen0 | Gen1 | Allocated |
|------------------------------------------|------------------|-----------:|----------:|----------:|-----------:|-----------:|----------:|-------:|-----:|----------:|
| DirectAssignment_SingleEntityConverted   | 1                |   8.925 ns | 0.2411 ns | 0.7071 ns |   8.732 ns |   7.688 ns |  10.68 ns | 0.0089 |    - |      56 B |
| Explicit_SingleEntityConverted           | 1                |  11.130 ns | 0.2568 ns | 0.6203 ns |  11.175 ns |   9.985 ns |  12.65 ns | 0.0089 |    - |      56 B |
| DirectAssignment_ListOfEntitiesConverted | 1                |  51.700 ns | 1.0517 ns | 2.6959 ns |  51.696 ns |  45.026 ns |  58.74 ns | 0.0216 |    - |     136 B |
| Explicit_ListOfEntitiesLINQConverted     | 1                |  55.430 ns |  1.365 ns |  3.917 ns |   48.53 ns |   65.18 ns |  54.95 ns | 0.0216 |    - |     136 B |
| AutoMapper_SingleEntityConverted         | 1                | 112.558 ns | 2.9574 ns | 8.4853 ns | 110.865 ns |  97.234 ns | 134.19 ns | 0.0088 |    - |      56 B |
| AutoMapper_ListOfEntitiesConverted       | 1                | 123.850 ns | 3.0895 ns | 8.8645 ns | 122.714 ns | 108.206 ns | 146.93 ns | 0.0138 |    - |      88 B |


### Results for workload: 5

| Method                                   | QuantityEntities |       Mean |      Error |     StdDev |     Median |        Min |       Max |   Gen0 | Gen1 | Allocated |
|------------------------------------------|------------------|-----------:|-----------:|-----------:|-----------:|-----------:|----------:|-------:|-----:|----------:|
| DirectAssignment_SingleEntityConverted   | 5                |  43.214 ns |  1.0158 ns |  2.9792 ns |  43.148 ns |  37.802 ns |  51.16 ns | 0.0446 |    - |     280 B |
| Explicit_SingleEntityConverted           | 5                |  52.289 ns |  1.0783 ns |  2.6041 ns |  52.203 ns |  46.179 ns |  58.38 ns | 0.0446 |    - |     280 B |
| Explicit_ListOfEntitiesLINQConverted     | 5                |  111.85 ns |   2.273 ns |   6.372 ns |   98.77 ns |  126.97 ns | 111.20 ns | 0.0625 |    - |     392 B |
| DirectAssignment_ListOfEntitiesConverted | 5                | 114.376 ns |  2.7168 ns |  8.0105 ns | 113.188 ns |  97.998 ns | 135.93 ns | 0.0625 |    - |     392 B |
| AutoMapper_ListOfEntitiesConverted       | 5                | 185.697 ns |  3.7056 ns |  9.0899 ns | 186.415 ns | 167.897 ns | 207.19 ns | 0.0548 |    - |     344 B |
| AutoMapper_SingleEntityConverted         | 5                | 558.106 ns | 10.7913 ns | 30.0819 ns | 557.115 ns | 478.325 ns | 633.09 ns | 0.0439 |    - |     280 B |


### Results for workload: 10

| Method                                   | QuantityEntities |         Mean |      Error |     StdDev |       Median |        Min |         Max |   Gen0 |   Gen1 | Allocated |
|------------------------------------------|------------------|-------------:|-----------:|-----------:|-------------:|-----------:|------------:|-------:|-------:|----------:|
| DirectAssignment_SingleEntityConverted   | 10               |    96.726 ns |  3.3539 ns |  9.6767 ns |    96.377 ns |  76.876 ns |   121.48 ns | 0.0892 |      - |     560 B |
| Explicit_SingleEntityConverted           | 10               |   125.667 ns |  4.3233 ns | 12.4736 ns |   123.537 ns |  99.916 ns |   157.12 ns | 0.0892 |      - |     560 B |
| Explicit_ListOfEntitiesLINQConverted     | 10               |   192.290 ns |   4.585 ns |  13.448 ns |    166.99 ns |  223.30 ns |   191.48 ns | 0.1135 | 0.0002 |     712 B |
| DirectAssignment_ListOfEntitiesConverted | 10               |   215.799 ns |  6.6739 ns | 19.3621 ns |   215.034 ns | 173.931 ns |   266.60 ns | 0.1135 | 0.0002 |     712 B |
| AutoMapper_ListOfEntitiesConverted       | 10               |   283.181 ns |  8.8430 ns | 25.6553 ns |   279.812 ns | 233.257 ns |   350.96 ns | 0.1054 |      - |     664 B |
| AutoMapper_SingleEntityConverted         | 10               | 1,114.482 ns | 22.2255 ns | 61.5867 ns | 1,110.854 ns | 967.931 ns | 1,261.33 ns | 0.0877 |      - |     560 B |


### Results for workload: 100

| Method                                   | QuantityEntities |          Mean |       Error |      StdDev |        Median |          Min |          Max |   Gen0 |   Gen1 | Allocated |
|------------------------------------------|------------------|--------------:|------------:|------------:|--------------:|-------------:|-------------:|-------:|-------:|----------:|
| DirectAssignment_SingleEntityConverted   | 100              |    908.786 ns |  22.9625 ns |  67.7056 ns |    908.281 ns |   768.268 ns |  1,071.79 ns | 0.8926 |      - |    5600 B |
| Explicit_SingleEntityConverted           | 100              |  1,132.984 ns |  27.3023 ns |  78.3353 ns |  1,127.649 ns |   939.584 ns |  1,337.08 ns | 0.8926 |      - |    5600 B |
| Explicit_ListOfEntitiesLINQConverted     | 100              |  1,512.410 ns |   35.568 ns |  103.753 ns |   1,306.45 ns |  1,758.08 ns |  1,492.67 ns | 1.0300 | 0.0229 |    6472 B |
| DirectAssignment_ListOfEntitiesConverted | 100              |  1,689.281 ns |  41.6290 ns | 117.4153 ns |  1,678.258 ns | 1,434.566 ns |  2,013.04 ns | 1.0300 | 0.0229 |    6472 B |
| AutoMapper_ListOfEntitiesConverted       | 100              |  1,851.261 ns |  39.4718 ns | 114.5147 ns |  1,847.097 ns | 1,552.329 ns |  2,123.83 ns | 1.0223 | 0.0229 |    6424 B |
| AutoMapper_SingleEntityConverted         | 100              | 11,657.142 ns | 275.0807 ns | 811.0816 ns | 11,578.722 ns | 9,941.530 ns | 13,601.42 ns | 0.8850 |      - |    5600 B |


### Results for workload: 1000

| Method                                   | QuantityEntities |           Mean |         Error |        StdDev |         Median |           Min |           Max |    Gen0 |   Gen1 | Allocated |
|------------------------------------------|------------------|---------------:|--------------:|--------------:|---------------:|--------------:|--------------:|--------:|-------:|----------:|
| DirectAssignment_SingleEntityConverted   | 1000             |  11,216.206 ns |   375.1645 ns | 1,082.4355 ns |  11,040.719 ns |  9,041.888 ns |  14,046.28 ns |  8.9264 |      - |   56000 B |
| Explicit_SingleEntityConverted           | 1000             |  13,450.713 ns |   428.8266 ns | 1,230.3840 ns |  13,287.642 ns | 11,151.910 ns |  16,407.81 ns |  8.9264 |      - |   56000 B |
| Explicit_ListOfEntitiesLINQConverted     | 1000             |  17,302.590 ns |    474.721 ns |  1,354.405 ns |   14,430.51 ns |  20,782.40 ns |  17,247.46 ns | 10.1929 | 1.6785 |   64072 B |
| DirectAssignment_ListOfEntitiesConverted | 1000             |  17,319.449 ns |   521.3261 ns | 1,537.1419 ns |  17,190.840 ns | 14,384.125 ns |  21,001.33 ns | 10.1929 | 1.6785 |   64072 B |
| AutoMapper_ListOfEntitiesConverted       | 1000             |  18,776.653 ns |   438.6627 ns | 1,244.4132 ns |  18,642.285 ns | 15,514.127 ns |  21,474.93 ns | 10.1929 | 1.6785 |   64024 B |
| AutoMapper_SingleEntityConverted         | 1000             | 113,737.126 ns | 2,753.0284 ns | 7,854.5450 ns | 114,732.452 ns | 93,966.229 ns | 129,702.41 ns |  8.9111 |      - |   56000 B |


### Results for workload: 5000

| Method                                   | QuantityEntities |           Mean |          Error |         StdDev |         Median |            Min |           Max |    Gen0 |    Gen1 | Allocated |
|------------------------------------------|------------------|---------------:|---------------:|---------------:|---------------:|---------------:|--------------:|--------:|--------:|----------:|
| DirectAssignment_SingleEntityConverted   | 5000             |  58,105.340 ns |  1,489.5492 ns |  4,368.5900 ns |  57,166.626 ns |  50,531.189 ns |  69,923.85 ns | 44.6167 |       - |  280000 B |
| Explicit_SingleEntityConverted           | 5000             |  67,751.603 ns |  1,431.8089 ns |  4,176.6536 ns |  68,140.076 ns |  59,140.381 ns |  79,240.72 ns | 44.5557 |       - |  280000 B |
| Explicit_ListOfEntitiesLINQConverted     | 5000             |  97,476.330 ns |   2,787.658 ns |   8,219.472 ns |   82,194.13 ns |  116,012.71 ns |  97,009.33 ns | 50.9033 | 19.6533 |  320072 B |
| DirectAssignment_ListOfEntitiesConverted | 5000             | 100,007.469 ns |  2,064.1876 ns |  5,988.5826 ns | 100,247.125 ns |  87,072.430 ns | 113,061.63 ns | 50.9033 | 19.6533 |  320072 B |
| AutoMapper_ListOfEntitiesConverted       | 5000             | 106,053.679 ns |  2,270.5378 ns |  6,659.0944 ns | 106,045.776 ns |  91,202.710 ns | 124,143.97 ns | 50.9033 | 19.2871 |  320024 B |
| AutoMapper_SingleEntityConverted         | 5000             | 639,815.704 ns | 26,557.4647 ns | 74,469.9782 ns | 627,574.219 ns | 500,215.137 ns | 851,983.30 ns | 43.9453 |       - |  280001 B |


### Analysis
- In all types of workload, AutoMapper lost but it has it's honours: It didn't allocate more memory than the other methods and the batch (list) mapping is pretty optimized (comparing to converting 1 by 1).
- The direct assigment is the one made me feel like a neanderthal converting entities in a cave somewhere, but it's hands down the fastest method. I was a bit disappointed with the overhead generated by the list conversion for this method.
- Explicit conversion is a bit slower than direct assignment, but it's the most readable and maintainable method. The use of LINQ added a bit of overhead, but it's still the fastest method for batch conversion.

If I was building a new application or if performance was a critical factor, I would go with the explicit conversion method. It's the most readable and maintainable method and it's not that slow.
However, we're talking about nanoseconds here, so I honestly don't see much reason to abandon AutoMapper altogether because of this performance problem. One thing that I would definitely do is to make sure all the rules for mapping properties that 
are not directly related (I.E: requires some kind of calculation) are defined closer to the entities and properly tested.

#### Timing reference
Just as a refresher:
There are 1,000 nanoseconds (ns) in 1 microsecond (μs). 
There are 1,000 microseconds in 1 millisecond (ms). 
Therefore, there are 1,000 x 1,000 = 1,000,000 nanoseconds in 1 millisecond. 
To convert from nanoseconds to milliseconds, divide the number of nanoseconds by 1,000,000.

So, 10,000 nanoseconds is equal to 10,000 / 1,000,000 = 0.01 milliseconds.