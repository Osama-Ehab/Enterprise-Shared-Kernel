# Enterprise Shared Kernel

This repository serves as the foundational architecture (Shared Kernel) for enterprise .NET applications. It centralizes cross-cutting concerns, architectural patterns, and base implementations to enforce consistency, maximize code reusability, and maintain a strict Clean Architecture across multiple business domains.

## 🏗️ Architecture Layers

The solution is highly modular and strictly divided into four primary projects to enforce the Separation of Concerns (SoC):

* **`Enterprise.SharedKernel` (Core Layer):** Acts as the center of the architecture. It contains the foundational domain interfaces, context definitions (like `AppLayoutContext.cs`), and core models including DTOs (`BaseFilterDTO.cs`, `BaseIdentifiableDTO.cs`). It defines the standard response wrappers (`Result.cs`, `GenericResult.cs`) and the base repository interfaces (`IReadRepository.cs`, `IWriteRepository.cs`).
* **`Enterprise.SharedKernel.Application` (Application Layer):** Manages application-level abstractions and validation logic. It includes base application services (`BaseGenericService.cs`), validation configurations (`BaseValidator.cs`, `FluentValidationExtensions.cs`), authentication DTOs (`LoginRequestDto.cs`), and core service interfaces such as `IAuthService.cs` and `IGenericService.cs`.
* **`Enterprise.SharedKernel.Infrastructure` (Infrastructure Layer):** Implements the core abstractions and handles external data access. It houses the base domain entities (`BaseEntity.cs`, `AuditableEntity.cs`), concrete repository implementations (`BaseReadRepository.cs`, `BaseWriteRepository.cs`), SQL error mapping utilities, and EF Core T4 templates (`DbContext.t4`, `EntityType.t4`) used for custom code generation.
* **`Enterprise.SharedKernel.UI` (Presentation/UI Abstraction):** Contains UI-agnostic event abstractions and payload carriers, specifically `DtoEventArgs.cs` and `SelectorEventArgs.cs`, to decouple UI interactions from the underlying business logic.

## 🧠 Core Architectural Patterns

### 1. Command Query Responsibility Segregation (CQS) & Generic Repositories
To optimize data access and enforce strict behavioral boundaries, the architecture splits database interactions into read and write operations.
* **Interfaces:** Defined in the Core layer as `IReadRepository.cs` and `IWriteRepository.cs`.
* **Implementations:** Concrete implementations are handled in the Infrastructure layer via `BaseReadRepository.cs` and `BaseWriteRepository.cs`. This allows any business domain module to inherit these base repositories without rewriting standard CRUD operations.

### 2. The Result Pattern
The system avoids using exceptions for business logic control flow by standardizing all service responses.
* Implemented via `Result.cs` and `GenericResult.cs` in the Core Models directory.
* This ensures that every layer communicates success, failure, and error configurations (`ErrorConfig.cs`) in a predictable, strongly-typed manner.

### 3. Domain Model & Auditing Abstraction
Instead of repeating ID properties and tracking fields (e.g., CreatedAt, CreatedBy) across every database table, the domain model relies on centralized base classes.
* The Infrastructure layer provides `IAuditableEntity.cs` to automatically track entity lifecycles and enforce standard primary keys across the entire database schema.

### 4. Interface Abstraction & Adapter Pattern
The architecture heavily utilizes Dependency Injection (DI) and interface abstraction to ensure components remain loosely coupled and environment-agnostic.
* Core business operations interact through interfaces like `IGenericService.cs` and `IAuthService.cs`.
* **The Adapter Pattern (Environment Context):** The architecture defines abstractions like an `ICurrentUser` interface to handle environmental logic. This allows the Shared Kernel to remain completely isolated from the host environment, while concrete applications (e.g., the WinForms-based School ERP) provide the adapter implementation to map a local Desktop User Session to the core interfaces.

### 5. Automated T4 Scaffolding
To eliminate manual repetitive coding while strictly enforcing the architecture, the Infrastructure layer includes custom code templates.
* The `CodeTemplates/EFCore` directory contains `DbContext.t4` and `EntityType.t4`.
* These templates intercept the default Entity Framework Core scaffolding process to dynamically generate C# classes that automatically inherit from `BaseEntity` or implement required architecture interfaces, seamlessly bridging database-first design with Domain-Driven Design (DDD) principles.
