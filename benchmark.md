[< Back to README](./readme.md)

# Minimal API vs Web API

So, every article I've read and Youtube video said that Minimal APIs are way faster than the "old" Web APIs. Since I
already had this
project setup, I decided to put that to the test.

The set up was easy, because I already had the adapters (AppServices), which will guarantee me that no API will have
unfair advantages.
Also in the name of fairness, each API was using a different database file.

## Test 1: Fetching 1 record from the database by id.

In this scenario, for both APIs the target of the test was a get by id route, returning the same record over and over
again.

> test info

test suite: `nbomber_default_test_suite_name`

test name: `nbomber_default_test_name`

session id: `2023-03-01_02.02.33_session_b237a78d`

> scenario stats

scenario: `Minimal API`

- ok count: `28800`

- fail count: `0`

- all data: `11,7` MB

- duration: `00:01:00`

load simulations:

- `inject`, rate: `24`, interval: `00:00:00.0500000`, during: `00:01:00`

| step               | ok stats                                                               |
|--------------------|------------------------------------------------------------------------|
| name               | `global information`                                                   |
| request count      | all = `28800`, ok = `28800`, RPS = `480`                               |
| latency            | min = `0,19`, mean = `0,88`, max = `31,46`, StdDev = `0,7`             |
| latency percentile | p50 = `0,79`, p75 = `1,03`, p95 = `1,39`, p99 = `1,75`                 |
| data transfer      | min = `0,417` KB, mean = `0,417` KB, max = `0,417` KB, all = `11,7` MB |

> status codes for scenario: `Minimal API`

| status code | count | message |
|-------------|-------|---------|
| OK          | 28800 ||

> scenario stats

scenario: `Web API`

- ok count: `28800`

- fail count: `0`

- all data: `12,3` MB

- duration: `00:01:00`

load simulations:

- `inject`, rate: `24`, interval: `00:00:00.0500000`, during: `00:01:00`

| step               | ok stats                                                               |
|--------------------|------------------------------------------------------------------------|
| name               | `global information`                                                   |
| request count      | all = `28800`, ok = `28800`, RPS = `480`                               |
| latency            | min = `0,39`, mean = `1`, max = `32,29`, StdDev = `0,46`               |
| latency percentile | p50 = `0,95`, p75 = `1,16`, p95 = `1,51`, p99 = `1,84`                 |
| data transfer      | min = `0,438` KB, mean = `0,438` KB, max = `0,438` KB, all = `12,3` MB |

> status codes for scenario: `Web API`

| status code | count | message |
|-------------|-------|---------|
| OK          | 28800 ||

## Results

Although Minimal APIs were faster, it wasn't by much. The difference was only 0.12 seconds,
which is not a lot. Both were able to handle 480 requests per second during 1 minute.

## Test 2: Hitting an almost empty endpoint

To test the performance of the API, this tests uses only and endpoint that returns the current datetime. No services, no
dependency injection, nothing.

> test info

test suite: `nbomber_default_test_suite_name`

test name: `nbomber_default_test_name`

session id: `2023-03-01_02.53.62_session_501e17e1`

> scenario stats

scenario: `Minimal API`

- ok count: `28800`

- fail count: `0`

- all data: `2,6` MB

- duration: `00:01:00`

load simulations:

- `inject`, rate: `24`, interval: `00:00:00.0500000`, during: `00:01:00`

| step               | ok stats                                                              |
|--------------------|-----------------------------------------------------------------------|
| name               | `global information`                                                  |
| request count      | all = `28800`, ok = `28800`, RPS = `480`                              |
| latency            | min = `0,19`, mean = `0,79`, max = `15,5`, StdDev = `0,46`            |
| latency percentile | p50 = `0,74`, p75 = `0,94`, p95 = `1,28`, p99 = `1,56`                |
| data transfer      | min = `0,088` KB, mean = `0,091` KB, max = `0,092` KB, all = `2,6` MB |

> status codes for scenario: `Minimal API`

| status code | count | message |
|-------------|-------|---------|
| OK          | 28800 ||

> scenario stats

scenario: `Web API`

- ok count: `28800`

- fail count: `0`

- all data: `2,6` MB

- duration: `00:01:00`

load simulations:

- `inject`, rate: `24`, interval: `00:00:00.0500000`, during: `00:01:00`

| step               | ok stats                                                              |
|--------------------|-----------------------------------------------------------------------|
| name               | `global information`                                                  |
| request count      | all = `28800`, ok = `28800`, RPS = `480`                              |
| latency            | min = `0,26`, mean = `1,07`, max = `32,4`, StdDev = `0,72`            |
| latency percentile | p50 = `1,03`, p75 = `1,22`, p95 = `1,53`, p99 = `1,8`                 |
| data transfer      | min = `0,088` KB, mean = `0,091` KB, max = `0,092` KB, all = `2,6` MB |

> status codes for scenario: `Web API`

| status code | count | message |
|-------------|-------|---------|
| OK          | 28800 ||

## Results

This time, the difference was bigger. Minimal APIs were 0.28 seconds faster than Web APIs. Maybe the
database was the bottleneck in the previous test.
Still, not a big difference.

# Future plans

I might try change the database, maybe use an in-memory database. It was my first time trying out NBomber,
so there's the distinct possibility of me doing something wrong. I might also try to add more endpoints to
the Web API, to see if it can handle more requests.
