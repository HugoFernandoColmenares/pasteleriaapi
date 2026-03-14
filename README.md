# PasteleriaApp API 🧁 (Backend Refactored)

¡Bienvenido/a a la API de **PasteleriaApp**! Este es un backend robusto desarrollado por **Feith Noir** y optimizado para una gestión eficiente de una pastelería moderna.

## ✨ Propósito del Proyecto

Este backend proporciona una base sólida, escalable y profesional para gestionar:

*   **Ingredientes:** Base de datos centralizada de materias primas con costos y proveedores.
*   **Inventario:** Control de stock en tiempo real con alertas de niveles bajos.
*   **Recetas:** Definición detallada de recetas con cálculo automático de costos.
*   **Documentación:** Registro y gestión de documentos importantes del negocio.
*   **Seguridad:** Sistema de autenticación JWT y roles (`Admin`, `User`, `Visitor`).

## 🛠️ Stack Tecnológico (Actualizado)

*   **Backend:**
    *   **Framework:** .NET 8.0 (C#) - ASP.NET Core API.
    *   **Base de Datos:** SQLite (para desarrollo ágil) - `pasteleriapp.db`.
    *   **Acceso a Datos:** Entity Framework Core con Patrón Repositorio.
    *   **Mapeo de Objetos:** AutoMapper para una comunicación limpia entre capas.
    *   **Autenticación:** ASP.NET Core Identity con JWT Bearer Tokens.
    *   **Documentación:** Swagger (OpenAPI) con soporte para XML comments.

## 🏗️ Arquitectura y Mejores Prácticas

*   **Clean Architecture:** Separación clara entre `Data`, `Business`, `Shared` y `API`.
*   **Patrón Repositorio:** Abstracción total del acceso a datos, facilitando pruebas unitarias.
*   **DTOs Optimizados:** Uso de objetos de transferencia de datos para evitar redundancias y fugas de entidades al cliente.
*   **Inyección de Dependencias:** Gestión nativa de servicios y repositorios.
*   **Respuestas Estandarizadas:** Todas las respuestas de la API siguen el formato `ApiResponse<T>`, garantizando coherencia para el frontend.

## 🔮 Futuras Mejoras y Buenas Prácticas

Para llevar este proyecto al siguiente nivel de robustez y profesionalismo, se consideran las siguientes implementaciones:

*   **Refresh Tokens:** Implementación de un sistema de rotación de tokens para mejorar la seguridad y la experiencia de usuario sin requerir logins constantes.
*   **Seguridad Avanzada con Bearer Tokens:** Restricción granular de endpoints sensibles mediante políticas de autorización basadas en roles y claims.
*   **Validación con FluentValidation:** Desacoplar la lógica de validación de los DTOs para un código más limpio y mantenible.
*   **Logging Centralizado:** Integración con **Serilog** para persistir logs en archivos, bases de datos o servicios en la nube (Seq, Azure Application Insights).
*   **Pruebas Automatizadas:** Cobertura de tests unitarios con **xUnit** y tests de integración para asegurar la estabilidad del sistema.
*   **Rate Limiting:** Protección contra ataques de fuerza bruta y abuso de la API limitando el número de peticiones por cliente.
*   **Caché de Datos:** Implementación de **Redis** o memoria caché para endpoints de alta demanda como la lista de ingredientes o recetas.
*   **CI/CD Pipelines:** Automatización de despliegues mediante GitHub Actions para garantizar que cada cambio pase por pruebas y se despliegue de forma segura.

## 💾 Modelo de Datos (Principales Entidades)

*   `User` (Gestionado por ASP.NET Identity)
*   `Ingredient` (Materias primas)
*   `InventoryItem` (Stock de ingredientes)
*   `Recipe` (Ficha técnica de productos)
*   `RecipeIngredient` (Relación ingredientes-receta)
*   `Document` (Registro documental)

## 🚀 Empezando con el Backend

1.  **SDK de .NET 8.0:** Asegúrate de tener instalada la última versión de .NET.
2.  **Configuración:** Revisa `appsettings.json` para las claves JWT y la ruta de la base de datos SQLite.
3.  **Base de Datos:** El proyecto ya incluye una base de datos SQLite preconfigurada. Si deseas reiniciarla, usa: `dotnet ef database update`.
60: 4.  **Ejecutar:** `dotnet run` dentro de la carpeta `Pasteleria.Api`.
5.  **Swagger:** Accede a `http://localhost:5000/swagger` para ver la documentación interactiva.

---

## 🧑‍💻 Desarrollado por: **Feith Noir**
