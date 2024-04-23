**Payment Service Platform Overview**
This project is an assignment to create a simple Payment Service Platform that handles payment requests and simulates interactions with payment acquirer. 

We've two payment acquirer(s) being used
 1. PayPal
 2. Stripe
 
**Tech Stack & Design Considerations**
This is a plain .net 8.0 web api project - that revolves around Command Query Request Segregation (CQRS) design pattern. 

**Data Storage**
We do not use any persistence technique at the moment, e.g. Database/Files. Information about payment requests is kept in-memory into a dictionary.

# Project Hierarchy and Architecture
This section outlines the structure and interrelationships of the projects within the solution. Our solution consists of several key projects, each serving a specific role within the overall application architecture.

## Api
**Type:** ASP.NET Core Web Application
Purpose : Exposes two endpoints 

For validation, we use FluentValidation
For Mediator - we use HumbleMediator
Logging - SeriLog
Dependency injection - SimpleInjector

 - Endpoint - POST /api/Payments  
 - Endpoint - GET /api/Payments

Defined in the PaymentsController - the endpoints accept the command and relays it to mediator to execute the command. 

## Application
**Type:** .NET Core Class Library
Purpose : Defines the Command Handlers and include validation using FluentValidation. CommandValidators are used to verify that input is correct, e.g. if CreatePaymentCommand.CardNumber is valid or not (for # verification). 

For Logging and validation - we inject decorators. 

## Domain
**Type:** .NET Core Class Library
Purpose : Defines the core DTOs for the project, including PaymentRequest, TransactionResult and some interfaces. 

## Infrastructure
**Type:** .NET Core Class Library
Purpose : Maintains InMemoryCacheService that stores the transaction information into memory in the form of a dictionary. 

## PaymentAcquirer
**Type:** .NET Core Class Library
Purpose : Has the core impelmentation for PayPal and Stripe Payment gateways. Both are glued together into the picture using PaymentProcessor. 

If the CardNumber ends with Even number - transaction is approved, otherwise rejected. 

## tests/UnitTests
**Type:** .net core console applcication
Purpose : Implements tests starting with Request validation, then checking the payment 

# Deployment to docker
Run the following commands at the root directory

    docker build -t paymentserviceprovider .
    docker run -d -p 5011:5001 --name myapp paymentserviceprovider
    
Afterwards, please open PaymentService.postman_collection.json file and import and then hit the endpoints

### Swagger is available on this URL - http://localhost:5011/swagger
### Project runs on http://localhost:5001 when running locally





