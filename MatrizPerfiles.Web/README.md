# Gestión de Matriz de Perfiles

Aplicación web empresarial ASP.NET Core 8 MVC para gestionar la **Matriz de Perfiles (Profile Matrix)** de Banco Popular.

## Prerrequisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- SQL Server LocalDB (`(localdb)\mssqllocaldb`) o una instancia local de SQL Server `Server=.;`

## Configuración y Ejecución

La base de datos requerida y su estructura ya contarán con una migración inicial generada en el directorio `/Migrations`. Para aplicar los cambios y correr la aplicación:

### 1. Actualizar la cadena de conexión
Abre el archivo `appsettings.json` y asegúrate de que el `Server` en la Connection String conecte con tu instancia local.
Por defecto cuenta con:
`Server=.;Database=MatrizPerfilesDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True`

### 2. Actualizar la Base de Datos
Abre tu intérprete de comandos o terminal en la ruta de `MatrizPerfiles.Web` y ejecuta el Update de Entity Framework Core para aplicar la migración `InitialCreate` y popular los seeders.
```bash
dotnet ef database update
```
*(Si no tienes la CLI de EF Tools instalada, ejecuta: `dotnet tool install --global dotnet-ef` primero).*

### 3. Ejecutar la Aplicación Web
Una vez configurada la Base de Datos, puedes levantar el servidor embebido de .NET Core:
```bash
dotnet run
```

La aplicación estará corriendo normalmente en `https://localhost:7146` o `http://localhost:5032`.

---
## Funcionalidades
- **CRUD Completo** de Registros de Matrices y Catálogos (Sistemas, Puestos).
- UI construida basada en el **Manual de Marca de Banco Popular** predominando los tonos Azul Marino.
- Filtros Avanzados y Carga vía AJAX a través de DataTables Server-Side Processing.
- Validaciones en Cliente y Servidor usando Data Annotations y ViewModels.
