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
