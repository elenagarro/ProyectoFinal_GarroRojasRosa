# Sistema de Matrícula Universitaria

Proyecto final desarrollado para el curso de Programación Avanzada.

## Estudiante

Rosa Elena Garro Rojas

Universidad Fidélitas  
Cartago, Costa Rica

## Descripción

El Sistema de Matrícula Universitaria es una aplicación web desarrollada con ASP.NET Core MVC para administrar información académica y facilitar el proceso de matrícula de estudiantes.

El sistema cuenta con dos tipos de usuario: Administrador y Estudiante. Las funcionalidades disponibles se muestran de acuerdo con el rol del usuario autenticado.

## Tecnologías utilizadas

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server LocalDB
- ASP.NET Core Identity
- Razor
- Bootstrap
- JavaScript
- AJAX mediante Fetch API
- Cloud Firestore

## Funcionalidades

### Administrador

- Gestión de carreras.
- Gestión de cursos.
- Gestión de docentes.
- Consulta y gestión de estudiantes.
- Consulta de matrículas.
- Asociación de cursos con carreras.
- Asignación de docentes a cursos.
- Filtros y paginación de cursos.
- Sincronización de información con Cloud Firestore.

### Estudiante

- Registro e inicio de sesión.
- Selección de carrera.
- Consulta de carreras y cursos.
- Consulta de cursos disponibles según su carrera.
- Matrícula de cursos.
- Consulta de sus matrículas.
- Cancelación de matrícula.

## Seguridad

La aplicación utiliza ASP.NET Core Identity para la autenticación de usuarios y autorización basada en roles.

Los roles utilizados son:

- Administrador
- Estudiante

## Base de datos

La información principal del sistema se almacena en SQL Server mediante Entity Framework Core y migraciones Code First.

## Cloud Firestore

Como tecnología complementaria, el sistema permite sincronizar información con Cloud Firestore.

Se utilizan ocho colecciones:

- auditoria
- carreras
- cursos
- docentes
- estudiantes
- matriculas
- roles
- usuarios

## Credenciales de prueba

### Administrador

Correo: admin@universidad.com  
Contraseña: Admin123!

### Estudiante

Correo: carlos.estudiante@correo.com  
Contraseña: Prueba123!

## Ejecución

1. Abrir la solución en Visual Studio.
2. Restaurar los paquetes NuGet.
3. Verificar la cadena de conexión `DefaultConnection`.
4. Ejecutar las migraciones con:

   `Update-Database`

5. Compilar y ejecutar el proyecto.

Para utilizar la sincronización con Cloud Firestore se requiere configurar localmente las credenciales correspondientes mediante `GOOGLE_APPLICATION_CREDENTIALS`.

El archivo de credenciales de Firebase no debe incluirse en el repositorio.