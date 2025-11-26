# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**G2rism Beta API** is a .NET 9.0 Web API for a comprehensive tourism management system (Sistema de Turismo) by CodeLabG2. The system manages roles, permissions, users, authentication, clients (CRM), employees, providers, contracts, and travel services (airlines, flights).

**Current Status**: Production-ready API with 11 controllers, comprehensive business logic validation, and robust security features.

## Technology Stack

- **Framework**: .NET 9.0 (net9.0)
- **Database**: MySQL 9.0 via Pomelo.EntityFrameworkCore.MySql 9.0.0
- **ORM**: Entity Framework Core 9.0.9
- **Validation**: FluentValidation.AspNetCore 11.3.0
- **Mapping**: AutoMapper.Extensions.Microsoft.DependencyInjection 12.0.1
- **Authentication**: BCrypt.Net-Next 4.0.3 (password hashing with workFactor 11)
- **Documentation**: Swashbuckle.AspNetCore 9.0.6 (Swagger/OpenAPI)

## Common Commands

### Build and Run
```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application (dev mode with Swagger at http://localhost:5000/)
dotnet run

# Run with watch (auto-reload on changes)
dotnet watch run
```

### Database Migrations
```bash
# Create a new migration
dotnet ef migrations add MigrationName

# Apply migrations to database
dotnet ef database update

# Rollback to a specific migration
dotnet ef database update MigrationName

# Remove last migration (if not applied)
dotnet ef migrations remove

# List all migrations
dotnet ef migrations list
```

### Testing
```bash
# Run all tests (if test project exists)
dotnet test

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"
```

## Architecture

### Layered Architecture Pattern

The project follows a clean layered architecture with clear separation of concerns:

1. **Models** (`Models/`) - Domain entities with EF Core configuration (14 entities)
2. **DTOs** (`DTOs/`) - Data Transfer Objects organized by module subdirectories (56 DTOs)
3. **Interfaces** (`Interfaces/`) - Abstraction contracts for repositories and services (27 interfaces)
4. **Repositories** (`Repositories/`) - Data access layer implementing repository pattern (14 repositories)
5. **Services** (`Services/`) - Business logic layer with validation (11 services)
6. **Controllers** (`Controllers/`) - API endpoints following REST conventions (11 controllers)
7. **Validators** (`Validators/`) - FluentValidation rules for DTOs (24 validators)
8. **Middleware** (`Middleware/`) - Global exception handler with formatted stack traces
9. **Helpers** (`Helpers/`) - Utilities (EmailHelper, TokenGenerator, PasswordHasher)
10. **Mappings** (`Mappings/`) - AutoMapper profile for Model ↔ DTO conversions (MappingProfile.cs)
11. **Data** (`Data/`) - DbContext, DbInitializer (seeding), DbContextFactory
12. **Constants** (`Constants/`) - RoleConstants with predefined roles and helper methods

### Dependency Injection Flow

**Program.cs** registers services in this order:
1. DbContext with MySQL connection (auto-detect server version)
2. AutoMapper (scans all assemblies)
3. Generic Repository (`IGenericRepository<T>` → `GenericRepository<T>`)
4. Module-specific Repositories (13 repositories)
5. Module-specific Services (11 services)
6. FluentValidation (auto-registration via `AddValidatorsFromAssembly()`)
7. Controllers and Swagger
8. CORS policy "AllowAll" (for development)

### Modules and Features

The system is organized into 6 distinct modules with comprehensive functionality:

#### 1. Configuration Module (Roles & Permissions)
- **Models**: `Rol`, `Permiso`, `RolPermiso` (many-to-many)
- **Controllers**: `RolesController`, `PermisosController`
- **Endpoints**: 15 endpoints total
  - Roles: CRUD + get roles with permissions + assign/remove permissions
  - Permissions: CRUD + get permissions by module
- **Features**:
  - Hierarchical access levels (NivelAcceso: 1=SuperAdmin, 2=Admin, 10=Employee, 50=Client)
  - Permission assignment with flexible many-to-many relationships
  - Role management with status control
  - Computed property: `CantidadPermisos` in RolResponseDto

#### 2. User Authentication Module
- **Models**: `Usuario`, `UsuarioRol` (many-to-many), `TokenRecuperacion`
- **Controllers**: `AuthController`, `UsuariosController`
- **Endpoints**: 18 endpoints total
  - Auth: Register, Login, Logout, Password recovery (request + reset), Change password
  - Users: CRUD + get with roles + block/unblock + activate/deactivate + assign/remove roles
- **Features**:
  - BCrypt password hashing (workFactor: 11)
  - Password strength validation (uppercase, lowercase, number, special char)
  - Login attempt tracking and automatic account locking
  - Token-based password reset (with expiration)
  - User type segregation (cliente vs empleado)
  - **CRITICAL BUSINESS RULE**: Only ONE Super Administrator allowed in the system
  - **CRITICAL BUSINESS RULE**: Role assignment validated against user type (employees cannot have client roles and vice versa)
  - Soft delete with Estado field
  - Computed properties: None (fields stored in DB)

#### 3. CRM Module - Clients
- **Models**: `CategoriaCliente`, `Cliente`, `PreferenciaCliente`
- **Controllers**: `CategoriasClienteController`, `ClientesController`, `PreferenciasClienteController`
- **Endpoints**: 15 endpoints total
  - Categories: CRUD + get with client count
  - Clients: CRUD + get with category details + filter by category/city
  - Preferences: CRUD + get by client
- **Features**:
  - Client segmentation with discount categories (percentage-based)
  - 1:1 relationship between Cliente and PreferenciaCliente (cascade delete)
  - N:1 relationship between Cliente and CategoriaCliente (restrict delete)
  - Cliente linked to Usuario (1:1, restrict delete)
  - Computed properties in Cliente: `Edad` (calculated from FechaNacimiento), `NombreCompleto`
  - Unique constraint on DocumentoIdentidad
  - Preferences tracking: accommodation type, destination type, activities, budget, special requirements

#### 4. CRM Module - Employees
- **Models**: `Empleado`
- **Controllers**: `EmpleadosController`
- **Endpoints**: 8 endpoints total
  - CRUD + get with boss info + get subordinates + get by department
- **Features**:
  - Employee hierarchy (self-referencing with `IdJefe`)
  - Navegación: `Empleado.Jefe` (boss) and `Empleado.Subordinados` (subordinates list)
  - DeleteBehavior.Restrict on self-reference (prevents cascading deletes)
  - Empleado linked to Usuario (N:1, restrict delete)
  - Computed properties: `NombreCompleto`, `Edad`, `AntiguedadAnios`, `AntiguedadMeses`, `EsJefe`, `CantidadSubordinados`
  - EmpleadoResponseDto includes nested `JefeBasicInfoDto` with boss details
  - Unique constraint on DocumentoIdentidad
  - Salary field (decimal 10,2) - visibility controlled by service layer permissions

#### 5. Providers Module
- **Models**: `Proveedor`, `ContratoProveedor`
- **Controllers**: `ProveedoresController`, `ContratosProveedorController`
- **Endpoints**: 14 endpoints total
  - Providers: CRUD + get by type + get active + get by rating
  - Contracts: CRUD + get by provider + get expiring soon + get active
- **Features**:
  - Provider types: 'hotel', 'aerolinea', 'transporte', 'servicio'
  - 1:N relationship (Proveedor → ContratoProveedor, restrict delete)
  - Contract management with expiration tracking
  - Provider rating system (1-5 scale, nullable)
  - Computed properties in ContratoProveedor: `EstaVigente`, `DiasRestantes`, `ProximoAVencer`, `DuracionDias`
  - Unique constraints: NitRut (provider), NumeroContrato (contract)
  - Status tracking for both providers and contracts

#### 6. Services Module (Airlines & Flights)
- **Models**: `Aerolinea`, `Vuelo`
- **Controllers**: `AerolineasController`
- **Endpoints**: 7 endpoints total
  - CRUD + get by country + get active + search by code
- **Features**:
  - IATA code validation (2 chars uppercase)
  - ICAO code validation (3 chars uppercase)
  - Unique constraints on both IATA and ICAO codes
  - Baggage policy management (equipaje_permitido, peso_maximo_equipaje)
  - 1:N relationship (Aerolinea → Vuelo, restrict delete)
  - **Vuelo model is temporary** (will be completed in Phase 3)
  - Computed properties in Aerolinea: `EstaActiva`, `NombreCompleto`, `TienePoliticasEquipaje`

### Database Design Patterns

1. **Many-to-Many Relationships**: Explicit join tables with composite keys
   - `RolPermiso` (IdRol + IdPermiso)
   - `UsuarioRol` (IdUsuario + IdRol)
   - Both include FechaAsignacion and optional AsignadoPor for audit

2. **One-to-One Relationships**:
   - `Cliente` ↔ `PreferenciaCliente` (cascade delete)
   - Foreign key in PreferenciaCliente

3. **One-to-Many Relationships**:
   - `CategoriaCliente` → `Cliente` (restrict delete)
   - `Proveedor` → `ContratoProveedor` (restrict delete)
   - `Aerolinea` → `Vuelo` (restrict delete)
   - `Usuario` → `Cliente` (restrict delete)
   - `Usuario` → `Empleado` (restrict delete)

4. **Self-Referencing Relationships**:
   - `Empleado.IdJefe` → `Empleado` (hierarchical structure, restrict delete)
   - Nullable IdJefe allows top-level employees (CEO, directors)

5. **Soft Delete**: Most entities use `Estado` boolean field instead of hard deletes
   - Usuario, Rol, Permiso, CategoriaCliente, Cliente, Empleado, Proveedor, ContratoProveedor, Aerolinea

6. **Audit Fields**:
   - Standard: `FechaCreacion`, `FechaModificacion`
   - Special: `FechaAsignacion` (join tables), `FechaRegistro` (Cliente, Proveedor), `FechaActualizacion` (PreferenciaCliente)

7. **Unique Constraints** (enforced via unique indexes):
   - Usuario: Username, Email
   - Cliente: DocumentoIdentidad
   - Empleado: DocumentoIdentidad
   - Proveedor: NitRut
   - ContratoProveedor: NumeroContrato
   - Aerolinea: CodigoIata, CodigoIcao
   - Rol: Nombre
   - Permiso: NombrePermiso

8. **Cascade Behavior**:
   - `Restrict`: Used for critical relationships (prevents accidental deletions)
   - `Cascade`: Used for dependent data (RolPermiso, UsuarioRol, PreferenciaCliente, TokenRecuperacion)

9. **Indexes** (all have custom names):
   - Unique indexes on all unique constraints
   - Performance indexes on foreign keys
   - Composite indexes on common query patterns (Permiso: Modulo+Accion, Cliente: Apellido+Nombre, etc.)
   - Status indexes for filtering (Estado, Bloqueado)

### Key Patterns and Conventions

#### 1. Repository Pattern with Generic Base
- **Generic Repository** (`IGenericRepository<T>`, `GenericRepository<T>`):
  - Provides standard CRUD: GetAllAsync, GetByIdAsync, AddAsync, UpdateAsync, DeleteAsync, SaveChangesAsync
  - Uses EF Core DbContext
  - Async/await pattern throughout

- **Entity-specific Repositories** extend with custom queries:
  - Example: `IRolRepository : IGenericRepository<Rol>`
  - Custom methods: GetRolConPermisosAsync, ExistsByNombreAsync, etc.
  - Use Include() for eager loading related entities

#### 2. Service Layer Pattern
- **Services contain business logic** and orchestrate repositories
- **Services use AutoMapper** to convert Models ↔ DTOs
- **Services validate business rules** before repository calls
- **Services throw exceptions** for validation failures:
  - `ArgumentException` for invalid input (400)
  - `KeyNotFoundException` for not found (404)
  - `InvalidOperationException` for business rule violations (400)
  - `UnauthorizedAccessException` for permission issues (401)

- **Example**: UsuarioService validates:
  - Password strength
  - Username/email uniqueness
  - Role compatibility with user type
  - Super Administrator uniqueness constraint

#### 3. DTO Pattern
Organized in subdirectories by module:

- **CreateDto**: For creating new entities
  - Excludes: ID, FechaCreacion, FechaModificacion, navigation properties
  - Includes: All required fields + Password (for Usuario)
  - Example: `UsuarioCreateDto` includes Password and ConfirmPassword

- **UpdateDto**: For updates (partial updates supported)
  - Nullable fields for optional updates
  - AutoMapper configured with `.Condition()` to ignore null values
  - Excludes: ID, audit fields, navigation properties
  - Example: `UsuarioUpdateDto` has all nullable properties

- **ResponseDto**: For API responses
  - Excludes: Sensitive data (PasswordHash), navigation collections
  - Includes: Computed fields from models
  - Example: `ClienteResponseDto` includes NombreCompleto, Edad, NombreCategoria

- **Special DTOs**:
  - `XxxConYyyDto`: For responses with nested related data
    - Example: `RolConPermisosDto` includes full Permiso list
    - Example: `UsuarioConRolesDto` includes full Rol list
  - `AsignarXxxDto`: For assignment operations
    - Example: `AsignarPermisoDto`, `AsignarRolesMultiplesDto`

#### 4. Validation Strategy

**Two-layer validation**:

1. **FluentValidation** (structural/format validation):
   - Validators in `Validators/` directory
   - Named `{DtoName}Validator.cs`
   - Auto-registered via `AddValidatorsFromAssembly()`
   - Run before controller method execution
   - Example validations:
     - String length, format (regex)
     - Email format
     - Required fields
     - Password strength (using PasswordHasher helper)
     - List constraints (no duplicates, min/max count)
     - Cross-field validation (Password == ConfirmPassword)

2. **Service Layer** (business logic validation):
   - Database-dependent checks (uniqueness, existence)
   - Complex business rules (Super Admin uniqueness, role compatibility)
   - State validation (can't delete category with active clients)
   - Relationship integrity

**Example**: AsignarRolesMultiplesDtoValidator
- FluentValidation: No duplicates, IDs > 0, no mixing employee/client roles
- Service: Roles exist in DB, compatible with user type, Super Admin uniqueness

#### 5. API Response Structure

**Consistent response format**:

- **Success Response** (`ApiResponse<T>`):
  ```csharp
  {
    "success": true,
    "message": "Operation completed successfully",
    "data": { /* T */ },
    "timestamp": "2024-01-15T10:30:00"
  }
  ```

- **Error Response** (`ApiErrorResponse`):
  ```csharp
  {
    "success": false,
    "message": "Error description",
    "statusCode": 400,
    "errorCode": "InvalidOperationException",
    "errors": null,  // Optional validation errors
    "stackTrace": "...",  // Only in Development
    "timestamp": "2024-01-15T10:30:00"
  }
  ```

- **Global Exception Handling** (`GlobalExceptionHandlerMiddleware`):
  - Catches all unhandled exceptions
  - Maps exception types to HTTP status codes
  - Formats stack traces (highlights user code vs framework)
  - Logs errors with structured logging
  - Returns consistent ApiErrorResponse

#### 6. Computed Properties in Models

Using `[NotMapped]` for calculated fields:

- **Cliente**:
  - `Edad`: Calculated from FechaNacimiento
  - `NombreCompleto`: $"{Nombre} {Apellido}"

- **Empleado**:
  - `NombreCompleto`: $"{Nombre} {Apellido}"
  - `Edad`: Calculated from FechaNacimiento
  - `AntiguedadAnios`: Calculated from FechaContratacion
  - `AntiguedadMeses`: Remaining months
  - `EsJefe`: Subordinados.Any()
  - `CantidadSubordinados`: Subordinados.Count

- **ContratoProveedor**:
  - `EstaVigente`: DateTime.Now between FechaInicio and FechaFin
  - `DiasRestantes`: Days until FechaFin
  - `ProximoAVencer`: DiasRestantes <= 30 && EstaVigente
  - `DuracionDias`: Total contract duration

- **Aerolinea**:
  - `EstaActiva`: Estado == true
  - `NombreCompleto`: $"{Nombre} ({CodigoIata})"
  - `TienePoliticasEquipaje`: !string.IsNullOrEmpty(EquipajePermitido)

**Benefits**:
- Prevents EF Core from mapping to database columns
- Allows rich domain models with business logic
- Computed once when entity is loaded
- Automatically included in ResponseDtos via AutoMapper

### AutoMapper Configuration

**MappingProfile.cs** contains all mappings:

- **CreateDto → Model**:
  - Ignores: ID, FechaCreacion, FechaModificacion, navigation properties
  - Maps: All required fields
  - Special handling for Usuario: Password excluded (hashed in service)

- **UpdateDto → Model**:
  - Ignores: ID, audit fields, navigation properties
  - Conditional mapping: `.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null))`
  - Enables partial updates

- **Model → ResponseDto**:
  - Includes: Computed properties from model
  - Maps: Related entity names (not full objects)
  - Example: ClienteResponseDto.NombreCategoria from Cliente.Categoria.Nombre

- **Special mappings**:
  - `Usuario → UsuarioLoginDto`: Flattens roles and permissions
  - `Rol → RolConPermisosDto`: Includes full Permiso list via RolesPermisos
  - `Empleado → EmpleadoResponseDto`: Includes nested JefeBasicInfoDto

### Constants and Helpers

#### Constants/RoleConstants.cs
Centralized role management:

```csharp
// Role IDs (match seeding)
SUPER_ADMINISTRADOR_ID = 1
ADMINISTRADOR_ID = 2
EMPLEADO_ID = 3
CLIENTE_ID = 4

// Role Names
SUPER_ADMINISTRADOR = "Super Administrador"
ADMINISTRADOR = "Administrador"
EMPLEADO = "Empleado"
CLIENTE = "Cliente"

// User Types
TIPO_EMPLEADO = "empleado"
TIPO_CLIENTE = "cliente"

// Access Levels
NIVEL_SUPER_ADMIN = 1
NIVEL_ADMIN = 2
NIVEL_EMPLEADO = 10
NIVEL_CLIENTE = 50

// Helper Methods
EsRolAdministrativo(int idRol)
EsSuperAdministrador(int idRol)
GetRolesPermitidos(string tipoUsuario)
EsRolValidoParaTipoUsuario(int idRol, string tipoUsuario)
```

#### Helpers/PasswordHasher.cs
Password hashing and validation:
- `HashPassword(string password)`: BCrypt with workFactor 11
- `VerifyPassword(string password, string hash)`: BCrypt verify
- `ValidatePasswordStrength(string password)`: Returns (bool, string)
  - Min 8 chars
  - At least 1 uppercase, 1 lowercase, 1 number, 1 special char

#### Helpers/TokenGenerator.cs
Token generation for password recovery:
- `GenerateToken()`: Secure random token (URL-safe)
- Used by AuthService for password reset

#### Helpers/EmailHelper.cs
Email sending (placeholder):
- `SendPasswordResetEmailAsync()`: Simulates email sending
- **TODO**: Implement actual SMTP integration

### Database Connection

**Connection String Location**: `appsettings.json` → `ConnectionStrings:DefaultConnection`

**Current Configuration**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1;Port=3306;Database=g2rism_beta_db;User=root;Password=mysqlPOPESVD6505.;"
  }
}
```

**IMPORTANT**:
- Never commit real credentials to version control
- Use environment variables or user secrets in production
- Connection string uses `ServerVersion.AutoDetect()` for MySQL compatibility

### Seeding Strategy

**DbInitializer.Initialize()** is called on application startup (Program.cs):

1. **Applies pending migrations** (if any)
2. **Checks for existing data** (idempotent)
3. **Seeds initial data** if database is empty:

   **Roles** (4):
   - Super Administrador (level 1)
   - Administrador (level 2)
   - Empleado (level 10)
   - Cliente (level 50)

   **Permissions** (8):
   - roles.crear, roles.leer, roles.actualizar, roles.eliminar
   - permisos.crear, permisos.leer, permisos.actualizar, permisos.eliminar

   **Role-Permission Assignments**:
   - Super Admin: ALL permissions
   - Admin: ALL except roles.eliminar
   - Empleado: Only .leer permissions
   - Cliente: No configuration permissions

   **Test Users** (3):
   - admin / Admin123! (Super Administrador)
   - empleado1 / Empleado123! (Empleado)
   - cliente1 / Cliente123! (Cliente)

**Seeding Output**: Detailed console logs with emojis and statistics

### Migrations

**Total Migrations**: 8

1. `20251028133800_InitialCreate_RolesPermisos`: Roles, Permisos, RolPermiso tables
2. `20251031133411_SecondCreateUsuarios`: Usuarios, UsuarioRol, TokenRecuperacion tables
3. `20251107002209_ModuloCategoriasCliente`: CategoriaCliente table
4. `20251107123658_ModuloCliente`: Cliente table
5. `20251109175531_ModuloPreferenciasCliente`: PreferenciaCliente table
6. `20251110042441_ModuloEmpleados`: Empleados table
7. `20251110205734_ModuloProveedores`: Proveedores, ContratosProveedor tables
8. `20251114173235_ModuloServiciosAerolineas`: Aerolineas, Vuelos tables
9. `20251120201304_EliminarIdReferenciaDeUsuarios`: Removed IdReferencia field from Usuario

**ApplicationDbContextModelSnapshot.cs**: Current database schema snapshot

## Development Workflow

### Adding a New Module

When adding a new entity/module, follow this order:

1. **Model** (`Models/YourEntity.cs`):
   - Define entity with proper annotations
   - Include computed properties with `[NotMapped]`
   - Add XML documentation comments

2. **DbContext** (`Data/ApplicationDbContext.cs`):
   - Add `DbSet<YourEntity>` property
   - Configure relationships in `OnModelCreating`:
     - Define indexes (unique, performance)
     - Configure foreign keys
     - Set cascade behaviors
     - Add constraints

3. **Migration**:
   ```bash
   dotnet ef migrations add ModuloYourEntity
   ```
   - Review generated migration code
   - Verify indexes and constraints

4. **DTOs** (`DTOs/YourEntity/`):
   - Create `YourEntityCreateDto.cs`
   - Create `YourEntityUpdateDto.cs` (nullable fields)
   - Create `YourEntityResponseDto.cs`
   - Create special DTOs if needed (ConXxx, AsignarXxx)

5. **AutoMapper** (`Mappings/MappingProfile.cs`):
   - Add CreateDto → Model mapping
   - Add UpdateDto → Model mapping (with conditional)
   - Add Model → ResponseDto mapping
   - Handle computed properties and nested objects

6. **Repository Interface** (`Interfaces/IYourEntityRepository.cs`):
   ```csharp
   public interface IYourEntityRepository : IGenericRepository<YourEntity>
   {
       Task<YourEntity?> GetByXxxAsync(int id);
       Task<bool> ExistsByXxxAsync(string xxx);
   }
   ```

7. **Repository Implementation** (`Repositories/YourEntityRepository.cs`):
   - Inherit from `GenericRepository<YourEntity>`
   - Implement custom queries
   - Use Include() for eager loading

8. **Service Interface** (`Interfaces/IYourEntityService.cs`):
   ```csharp
   public interface IYourEntityService
   {
       Task<YourEntity> CreateAsync(YourEntity entity);
       Task<IEnumerable<YourEntity>> GetAllAsync();
       // Custom business methods
   }
   ```

9. **Service Implementation** (`Services/YourEntityService.cs`):
   - Implement business logic
   - Add validation (throw appropriate exceptions)
   - Use repository for data access
   - Use AutoMapper for DTO conversions

10. **Validators** (`Validators/`):
    - Create `YourEntityCreateDtoValidator.cs`
    - Create `YourEntityUpdateDtoValidator.cs`
    - Inherit from `AbstractValidator<T>`
    - Add structural validations

11. **Controller** (`Controllers/YourEntitiesController.cs`):
    - Use `[ApiController]` and `[Route("api/[controller]")]`
    - Inject service, mapper, logger
    - Return `ApiResponse<T>` or `ApiErrorResponse`
    - Add XML comments for Swagger
    - Use proper HTTP status codes

12. **Register in Program.cs**:
    ```csharp
    builder.Services.AddScoped<IYourEntityRepository, YourEntityRepository>();
    builder.Services.AddScoped<IYourEntityService, YourEntityService>();
    ```

13. **Apply Migration**:
    ```bash
    dotnet ef database update
    ```

14. **Test**:
    - Use Swagger UI to test endpoints
    - Verify validation rules
    - Check business logic

### Making Changes to Existing Entities

1. **Modify the Model** class
2. **Update AutoMapper** mappings if DTOs changed
3. **Update Validators** if validation rules changed
4. **Create Migration**:
   ```bash
   dotnet ef migrations add DescriptiveChangeName
   ```
5. **Review Migration** code carefully
6. **Apply Migration**:
   ```bash
   dotnet ef database update
   ```

### Controller Conventions

**Standard Structure**:
```csharp
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class YourController : ControllerBase
{
    private readonly IYourService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<YourController> _logger;

    // Constructor injection

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<YourDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<YourDto>>>> GetAll()
    {
        _logger.LogInformation("📋 Getting all...");
        // Implementation
    }
}
```

**Key Points**:
- Use `[ApiController]` for automatic model validation
- Return `ApiResponse<T>` for success
- Return `ApiErrorResponse` for errors
- Use standard HTTP status codes
- Add XML comments for Swagger
- Log important operations with emojis for readability

**HTTP Status Codes**:
- 200 OK: Successful GET/PUT
- 201 Created: Successful POST
- 204 No Content: Successful DELETE
- 400 Bad Request: Validation errors, business rule violations
- 404 Not Found: Resource not found
- 500 Internal Server Error: Unhandled exceptions

### Naming Conventions

**Controllers**:
- Plural names: `RolesController`, `ClientesController`, `ProveedoresController`

**Services/Repositories**:
- Singular entity name + Service/Repository
- Examples: `RolService`, `ClienteRepository`, `ProveedorService`

**Interfaces**:
- Prefixed with `I`
- Examples: `IRolService`, `IClienteRepository`, `IGenericRepository<T>`

**DTOs**:
- Entity name + purpose
- Examples: `RolCreateDto`, `ClienteUpdateDto`, `UsuarioResponseDto`
- Special: `RolConPermisosDto`, `UsuarioConRolesDto`, `AsignarRolesMultiplesDto`

**Validators**:
- DTO name + Validator
- Examples: `RolCreateDtoValidator`, `ClienteUpdateDtoValidator`

**Database Tables**:
- Plural Spanish names
- Examples: `roles`, `clientes`, `empleados`, `proveedores`, `aerolineas`

**Columns**:
- Snake case: `id_usuario`, `fecha_creacion`, `nombre_completo`

**Foreign Keys**:
- `id_` + entity name
- Examples: `id_rol`, `id_cliente`, `id_proveedor`

**Navigation Properties**:
- Singular for 1:1 and N:1: `Usuario`, `Categoria`, `Jefe`
- Plural for 1:N: `Clientes`, `Subordinados`, `Contratos`, `Vuelos`
- Join tables for N:M: `RolesPermisos`, `UsuariosRoles`

## Important Notes

### 1. CORS Policy
Currently set to `AllowAll` for development:
```csharp
policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
```
**IMPORTANT**: Restrict to specific origins in production.

### 2. Password Security
- **NEVER** store plain text passwords
- Use `PasswordHasher.HashPassword()` before saving
- Verify with `PasswordHasher.VerifyPassword(plainText, hash)`
- BCrypt workFactor: 11 (balance between security and performance)
- Password requirements: 8+ chars, uppercase, lowercase, number, special char

### 3. Swagger/OpenAPI
- Available at root: `http://localhost:5000/`
- **Only in Development mode**
- Configured in Program.cs
- Uses XML comments for documentation
- SwaggerDoc info: "G2rism Beta API - Módulo de Configuración"

### 4. Middleware Order
**CRITICAL**: `GlobalExceptionHandlerMiddleware` MUST be first:
```csharp
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();  // FIRST!
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
```

### 5. Entity Relationships
- **Eager Loading**: Use `Include()` and `ThenInclude()` in repositories
- **Circular References**: AutoMapper handles this automatically
- **Delete Behavior**:
  - `Restrict`: For critical relationships (prevents accidental deletes)
    - CategoriaCliente → Cliente
    - Proveedor → ContratoProveedor
    - Usuario → Cliente/Empleado
    - Empleado → Empleado (self-reference)
  - `Cascade`: For dependent data (will be deleted automatically)
    - Rol → RolPermiso
    - Usuario → UsuarioRol
    - Cliente → PreferenciaCliente
    - Usuario → TokenRecuperacion

### 6. Migration Safety
- **ALWAYS** review migrations before applying
- **NEVER** delete migrations applied to production
- Use descriptive names: `ModuloNombre`, `AgregarCampoX`, etc.
- Check indexes and constraints in generated code
- Test rollback strategy before production deployment

### 7. Generic Repository
- Use for standard CRUD operations
- Extend with custom methods for complex queries
- All methods are async
- SaveChangesAsync() must be called explicitly

### 8. Service Layer
- **Business rules belong here**, not in controllers or repositories
- Validate before calling repositories
- Throw specific exceptions for different error types
- Use constants (like RoleConstants) for business logic
- Keep controllers thin (orchestration only)

### 9. DTO Validation
- **FluentValidation** for structural/format validation
- **Service Layer** for business logic validation
- Auto-registered validators (AddValidatorsFromAssembly)
- Custom validators for database-dependent rules
- Two-layer approach prevents invalid data from reaching the service

### 10. Partial Updates
- UpdateDto classes have nullable properties
- AutoMapper configured with `.Condition()` to ignore nulls:
  ```csharp
  .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null))
  ```
- Only provided fields are updated
- Audit fields (FechaModificacion) updated automatically

### 11. Logging
- Structured logging with ILogger
- Emojis for visual categorization:
  - 📝 Creating
  - ✅ Success
  - ⚠️ Warning
  - ❌ Error
  - 🔍 Searching
  - 🔗 Assigning
  - 🗑️ Deleting
- Log important operations with contextual data

### 12. Business Rules Enforcement

**Critical Business Rules**:

1. **Super Administrator Uniqueness**:
   - Only ONE user can have the Super Administrador role
   - Validated in UsuarioService.CrearUsuarioAsync and AsignarRolesAsync
   - Uses UsuarioRolRepository.ExisteSuperAdministradorAsync()

2. **Role-User Type Compatibility**:
   - Employee users can only have: Super Admin, Admin, or Empleado roles
   - Client users can only have: Cliente role
   - Validated using RoleConstants.EsRolValidoParaTipoUsuario()
   - Prevents mixing employee and client roles

3. **Prevent Deletion with Dependencies**:
   - Can't delete CategoriaCliente if it has active Clientes
   - Can't delete Proveedor if it has active Contratos
   - Enforced by DeleteBehavior.Restrict in database

4. **Password Strength**:
   - Validated in PasswordHasher.ValidatePasswordStrength()
   - FluentValidation in CreateDto validators
   - Service layer validation before hashing

5. **Unique Constraints**:
   - Enforced at database level (unique indexes)
   - Service layer checks before insert/update
   - Custom repository methods (ExistsByXxxAsync)

### 13. Security Considerations

**Current Implementation**:
- Password hashing with BCrypt (workFactor 11)
- Login attempt tracking (IntentosFallidos)
- Account locking (Bloqueado field)
- Token-based password reset (TokenRecuperacion with expiration)
- Soft delete (Estado field)

**TODO for Production**:
- Implement JWT authentication
- Add authorization attributes to controllers
- Rate limiting on login endpoint
- HTTPS enforcement
- CORS restriction to specific origins
- Environment-based configuration
- Implement actual email sending (EmailHelper)
- Add audit logging for sensitive operations
- Consider adding refresh tokens

### 14. Development vs Production

**Current Configuration** (Development):
- Swagger enabled
- AllowAll CORS policy
- Detailed error messages with stack traces
- Database connection string in appsettings.json

**Production Checklist**:
- [ ] Disable Swagger
- [ ] Restrict CORS to specific origins
- [ ] Hide stack traces in error responses
- [ ] Move connection string to environment variables or Azure Key Vault
- [ ] Implement actual email sending
- [ ] Add JWT authentication
- [ ] Enable HTTPS redirect
- [ ] Add health check endpoints
- [ ] Configure production logging (Application Insights, Serilog)
- [ ] Review and optimize indexes
- [ ] Add caching strategy
- [ ] Implement rate limiting

### 15. Project Structure Summary

```
G2rismBeta.API/
├── Constants/
│   └── RoleConstants.cs (Role IDs, names, helper methods)
├── Controllers/ (11 controllers)
│   ├── AerolineasController.cs
│   ├── AuthController.cs
│   ├── CategoriasClienteController.cs
│   ├── ClientesController.cs
│   ├── ContratosProveedorController.cs
│   ├── EmpleadosController.cs
│   ├── PermisosController.cs
│   ├── PreferenciasClienteController.cs
│   ├── ProveedoresController.cs
│   ├── RolesController.cs
│   └── UsuariosController.cs
├── Data/
│   ├── ApplicationDbContext.cs (DbContext with OnModelCreating)
│   ├── ApplicationDbContextFactory.cs (For migrations)
│   └── DbInitializer.cs (Seeding logic)
├── DTOs/ (56 DTOs organized by module)
│   ├── Aerolinea/ (3)
│   ├── Auth/ (8)
│   ├── CategoriaCliente/ (3)
│   ├── Cliente/ (4)
│   ├── ContratoProveedor/ (3)
│   ├── Empleado/ (4)
│   ├── Permiso/ (3)
│   ├── PreferenciaCliente/ (3)
│   ├── Proveedor/ (3)
│   ├── Rol/ (3)
│   ├── RolPermiso/ (3)
│   ├── Usuario/ (4)
│   └── UsuarioRol/ (2)
├── Helpers/
│   ├── EmailHelper.cs (TODO: SMTP implementation)
│   ├── PasswordHasher.cs (BCrypt hashing + validation)
│   └── TokenGenerator.cs (Secure token generation)
├── Interfaces/ (27 interfaces)
│   ├── IGenericRepository.cs
│   ├── 13 entity repositories
│   └── 11 services + IAuthService
├── Mappings/
│   └── MappingProfile.cs (All AutoMapper mappings)
├── Middleware/
│   └── GlobalExceptionHandlerMiddleware.cs (Exception handling + formatted stack traces)
├── Migrations/ (9 migrations + snapshot)
├── Models/ (14 entities)
│   ├── Aerolinea.cs
│   ├── ApiErrorResponse.cs
│   ├── ApiResponse.cs
│   ├── CategoriaCliente.cs
│   ├── Cliente.cs
│   ├── ContratoProveedor.cs
│   ├── Empleado.cs
│   ├── Permiso.cs
│   ├── PreferenciaCliente.cs
│   ├── Proveedor.cs
│   ├── Rol.cs
│   ├── RolPermiso.cs
│   ├── TokenRecuperacion.cs
│   ├── Usuario.cs
│   ├── UsuarioRol.cs
│   └── Vuelo.cs (temporary)
├── Repositories/ (14 repositories)
│   ├── GenericRepository.cs
│   └── 13 entity repositories
├── Services/ (11 services)
│   ├── AerolineaService.cs
│   ├── AuthService.cs
│   ├── CategoriaClienteService.cs
│   ├── ClienteService.cs
│   ├── ContratoProveedorService.cs
│   ├── EmpleadoService.cs
│   ├── PermisoService.cs
│   ├── PreferenciaClienteService.cs
│   ├── ProveedorService.cs
│   ├── RolService.cs
│   └── UsuarioService.cs
├── Validators/ (24 validators)
│   └── [DTO name]Validator.cs
├── appsettings.json (Connection string + logging config)
├── CLAUDE.md (This file)
├── G2rismBeta.API.csproj (Project file with dependencies)
└── Program.cs (Application entry point + DI configuration)
```

### 16. API Endpoints Summary

Total: **95+ endpoints** across 11 controllers

**RolesController** (8 endpoints):
- GET /api/roles
- GET /api/roles/{id}
- GET /api/roles/{id}/con-permisos
- POST /api/roles
- PUT /api/roles/{id}
- DELETE /api/roles/{id}
- POST /api/roles/{id}/asignar-permiso
- DELETE /api/roles/{id}/remover-permiso/{idPermiso}

**PermisosController** (6 endpoints):
- GET /api/permisos
- GET /api/permisos/{id}
- GET /api/permisos/modulo/{modulo}
- POST /api/permisos
- PUT /api/permisos/{id}
- DELETE /api/permisos/{id}

**AuthController** (6 endpoints):
- POST /api/auth/register
- POST /api/auth/login
- POST /api/auth/logout
- POST /api/auth/recuperar-password
- POST /api/auth/reset-password
- POST /api/auth/cambiar-password

**UsuariosController** (12 endpoints):
- GET /api/usuarios
- GET /api/usuarios/{id}
- GET /api/usuarios/{id}/roles
- POST /api/usuarios
- PUT /api/usuarios/{id}
- DELETE /api/usuarios/{id}
- POST /api/usuarios/{id}/bloquear
- POST /api/usuarios/{id}/desbloquear
- POST /api/usuarios/{id}/activar
- POST /api/usuarios/{id}/desactivar
- POST /api/usuarios/{id}/asignar-roles
- DELETE /api/usuarios/{id}/remover-rol/{idRol}

**CategoriasClienteController** (5 endpoints):
- GET /api/categoriascliente
- GET /api/categoriascliente/{id}
- POST /api/categoriascliente
- PUT /api/categoriascliente/{id}
- DELETE /api/categoriascliente/{id}

**ClientesController** (7 endpoints):
- GET /api/clientes
- GET /api/clientes/{id}
- GET /api/clientes/{id}/con-categoria
- GET /api/clientes/categoria/{idCategoria}
- POST /api/clientes
- PUT /api/clientes/{id}
- DELETE /api/clientes/{id}

**PreferenciasClienteController** (5 endpoints):
- GET /api/preferenciascliente
- GET /api/preferenciascliente/{id}
- GET /api/preferenciascliente/cliente/{idCliente}
- POST /api/preferenciascliente
- PUT /api/preferenciascliente/{id}

**EmpleadosController** (8 endpoints):
- GET /api/empleados
- GET /api/empleados/{id}
- GET /api/empleados/{id}/con-jefe
- GET /api/empleados/{id}/subordinados
- GET /api/empleados/departamento/{departamento}
- POST /api/empleados
- PUT /api/empleados/{id}
- DELETE /api/empleados/{id}

**ProveedoresController** (8 endpoints):
- GET /api/proveedores
- GET /api/proveedores/{id}
- GET /api/proveedores/tipo/{tipo}
- GET /api/proveedores/activos
- GET /api/proveedores/calificacion/{min}
- POST /api/proveedores
- PUT /api/proveedores/{id}
- DELETE /api/proveedores/{id}

**ContratosProveedorController** (8 endpoints):
- GET /api/contratosproveedor
- GET /api/contratosproveedor/{id}
- GET /api/contratosproveedor/proveedor/{idProveedor}
- GET /api/contratosproveedor/vigentes
- GET /api/contratosproveedor/proximos-vencer
- POST /api/contratosproveedor
- PUT /api/contratosproveedor/{id}
- DELETE /api/contratosproveedor/{id}

**AerolineasController** (7 endpoints):
- GET /api/aerolineas
- GET /api/aerolineas/{id}
- GET /api/aerolineas/pais/{pais}
- GET /api/aerolineas/activas
- GET /api/aerolineas/buscar/{codigo}
- POST /api/aerolineas
- PUT /api/aerolineas/{id}

### 17. Testing Strategy

**Current**: Manual testing via Swagger UI

**Recommended for Production**:

1. **Unit Tests**:
   - Service layer business logic
   - Validators
   - Helper methods (PasswordHasher, RoleConstants)

2. **Integration Tests**:
   - Repository layer with in-memory database
   - Controller endpoints
   - AutoMapper mappings

3. **End-to-End Tests**:
   - Complete user flows (register → login → operations)
   - Business rule enforcement
   - Error handling

**Testing Tools**:
- xUnit or NUnit
- FluentAssertions
- Moq (for mocking)
- Microsoft.EntityFrameworkCore.InMemory
- Microsoft.AspNetCore.Mvc.Testing (for integration tests)

## Quick Reference

### Common Tasks

**Create a new entity**:
1. Model → DbContext → Migration → DTOs → Mappings → Repository → Service → Validators → Controller → Register in Program.cs → Apply migration

**Add a new endpoint**:
1. Add method to service interface → Implement in service → Add controller action → Test in Swagger

**Modify database schema**:
1. Update model → Create migration → Review → Apply migration

**Debug validation error**:
1. Check FluentValidation validator → Check service layer validation → Check model annotations

**Fix relationship error**:
1. Check OnModelCreating configuration → Check navigation properties → Recreate migration

### Useful Queries

**Find all uses of a service**:
```bash
grep -r "IYourService" --include="*.cs"
```

**List all endpoints**:
Check each controller's HTTP methods

**View current database schema**:
Check Migrations/ApplicationDbContextModelSnapshot.cs

**See seeded data**:
Check Data/DbInitializer.cs

### Performance Tips

1. Use `.AsNoTracking()` for read-only queries
2. Implement pagination for large result sets
3. Use indexes strategically (already configured)
4. Consider caching for frequently accessed data
5. Use Select() to project only needed fields
6. Implement lazy loading carefully (prefer explicit Include)

## Future Enhancements

**Phase 2** (Planned):
- [ ] Implement JWT authentication
- [ ] Add authorization attributes based on permissions
- [ ] Complete email sending functionality
- [ ] Add file upload support (for documents, images)
- [ ] Implement audit trail logging

**Phase 3** (Planned):
- [ ] Complete Vuelo model and controller
- [ ] Add reservation/booking module
- [ ] Implement payment processing
- [ ] Add reporting and analytics
- [ ] Create admin dashboard

**Phase 4** (Future):
- [ ] Multi-language support
- [ ] Mobile app API
- [ ] Real-time notifications (SignalR)
- [ ] Advanced search and filtering
- [ ] Data export (PDF, Excel)

---

**Last Updated**: 2024-01-15
**Analyzed By**: Claude Code
**Project Version**: Beta 1.0
**Database Version**: Migration #9 (EliminarIdReferenciaDeUsuarios)
