
# Product API

This API manages permissions for employees. It was built using .NET Core 3.1 and several patterns and tools, which are explained in this readme.


## Tools
- [FluentAssertions](https://fluentassertions.com/)
- [Dependency Injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-6.0)
- [AutoMapper](https://automapper.org/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Swagger](https://swagger.io/)
- [Moq](https://github.com/devlooped/moq)
- [FakeItEasy](https://fakeiteasy.github.io/)


## Patterns
- [Repository](https://docs.microsoft.com/en-us/azure/architecture/patterns/repository)
- [CQRS](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)
- [MediatR](https://github.com/jbogard/MediatR)



## Getting Started
el proyecto utiliza entity framework con migrations y seeds de los objetos para una carga inicial
solo se debe correr el comando
***dotnet ef database update***
si se desea correrlo en una db local.

los log del proyecto se guardan en la carpeta Logs dentro de la misma se generan archivos de con nombre ex o inf dependiendo el tipo de log generado

la base de datos esta hosteada en freesqldatabase.com

el servicio de Descuentos se encuentra en https://mockapi.io/ en la url https://65d265bc987977636bfc4c1c.mockapi.io/api/v1/Discount



## Unit Testing
se testean por separado los handlers y los command asi como el repository de producto

### Running the Tests

para correr los test solo se deben ejecutar


## Authors

- [Alan Ezequiel Diaz](https://github.com/AlanEDiaz) - Unique Contributor.
