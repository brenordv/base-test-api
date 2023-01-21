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

1. Products service and repository with some basic CRUD (actually just CRU) functionalities;
2. MemoryCache decorator on the repository;
3. (A variant of the) Observer pattern to allow bust the cache whenever a product is modified;
4. Decorator on top of the controller to measure the elapsed time of each endpoint;
5. Swagger.

## Considerations
### Self-validated entities.
The entity [Product](https://github.com/brenordv/base-test-api/blob/master/Raccoon.Ninja.Domain/Entities/Product.cs) has validation on all pertinent setters.
According to my understanding of Domain-Driven Design (DDD), entities should be responsible for validating their own properties. 
This is because entities are considered to be the core of the domain model and should encapsulate the domain's validation rules. 
By having entities validate their own properties, the validation logic is kept within the domain model and is not spread out across multiple 
layers of the application. 

Additionally, it ensures that the validation rules are always enforced and that the state of the entity is always consistent with the domain's rules.
Which makes testing super easy and focused.

This entity also have an extra constructor, it was created to integrate the update part of the entity. I decided to do that mostly because of the database I'm using.
If I was using a relational database, I would not use that [this extra constructor](https://github.com/brenordv/base-test-api/blob/master/Raccoon.Ninja.Domain/Entities/Product.cs#L13) 
and rather used an simple update query to do the job. I relly didn't like that constructor, it's clumsy and not elegant at all.

### Adapter between Services and Controllers.
The [ProductAppService](https://github.com/brenordv/base-test-api/blob/master/Raccoon.Ninja.AppServices/AppServices/ProductsAppService.cs) serves as an adapter between the 
controller and the service. This allows for greater isolation between layers making super easy to exchange the Minimal API to something else.
It's not that the controller is incompatible with the service, it's just that I wanted to keep the controller as simple and as decoupled as possible.

This is a structural design pattern that allows objects (with usually with incompatible) interfaces to work together. 
It provides a way to adapt one interface to another so that classes can work together that could not otherwise because of incompatible interfaces.

There are several advantages to using the Adapter pattern:

1. Loose coupling: The Adapter pattern allows two classes with incompatible interfaces to work together without changing the source code of either class. This promotes loose coupling, making it easier to change the implementation of one class without affecting the other.
2. Reusability: By using the Adapter pattern, you can reuse existing classes that cannot be used directly in a new system.
3. Flexibility: The Adapter pattern makes it easy to add new functionality to existing classes without modifying their source code.
4. Extensibility: Adapter pattern allows for extending the functionality of existing classes by adding new adapters for new interfaces.
5. Single Responsibility Principle (SRP): The Adapter pattern helps to adhere to the Single Responsibility Principle (SRP) by separating the interface adaptation and implementation concerns.
6. Interoperability: It helps to integrate the different systems, frameworks, and libraries that have different interfaces.
7. Simplicity: The adapter pattern is simple to implement and understand.

In summary, the Adapter pattern is a powerful and flexible solution for dealing with incompatible interfaces, it promotes loose coupling and 
reusability, and it allows for the easy addition of new functionality to existing classes.

### Decorator pattern.
I've created two decorators, one to [measure the elapsed time](https://github.com/brenordv/base-test-api/blob/master/Raccoon.Ninja.Infra.DI/Decorators/StopWatchDecorator.cs) of each endpoint and [another to cache the repository](https://github.com/brenordv/base-test-api/blob/master/Raccoon.Ninja.Infra.DI/Decorators/CachingDecorator.cs).
Since they are explicitly used just during Dependency Injection, I've left in the Infra.Di project. If I were less lazy or if this were a real project, I would have created a dedicated project for the decorators.

The Decorator pattern is a structural design pattern that allows you to add new behavior to existing objects at runtime by wrapping them in a decorator object. 
It is an alternative to subclassing, which allows you to change the behavior of a class by creating a new subclass.

The decorator pattern uses a set of decorator classes that are used to wrap an existing object. 
Each decorator class has the same interface as the object it is decorating, and it adds new behavior by forwarding method calls to the wrapped object.

The decorator pattern has several advantages:

1. Flexibility: You can add or remove behavior from an object at runtime by wrapping or unwrapping it in different decorator objects.
2. Extensibility: The decorator pattern allows you to extend the functionality of a class without modifying its source code.
3. Open/Closed Principle (OCP): The decorator pattern adheres to the Open/Closed Principle (OCP), which states that classes should be open for extension but closed for modification.
4. Single Responsibility Principle (SRP): The decorator pattern helps to adhere to the Single Responsibility Principle (SRP) by separating the implementation and decoration concerns.
5. Reusability: Decorator classes can be reused across different objects, making it easier to add similar behavior to multiple objects.
6. Composition over Inheritance: The decorator pattern uses composition to add behavior to an object, rather than inheritance, which allows for more flexibility and reusability.

In summary, The decorator pattern is a way of adding new behavior to an existing object without modifying its class, 
it allows you to add or remove behavior at runtime, it promotes flexibility, extensibility, reusability and the Open/Closed Principle (OCP) 
and Single Responsibility Principle (SRP) and it is an alternative to subclassing.


## What's next?

1. Secure the endpoints
2. Endpoint validation attributes (to reject invalid requests as soon as possible).
3. Exception interceptor (to prevent exceptions from leaking into the response).

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
## 2023-01-21 - Moved Benchmarks
The benchmarks project was making too much noise in this readme file and the project didn't really belong here.
So I moved it to a new repository: [csharp-benchmarks](https://github.com/brenordv/csharp-benchmarks)
