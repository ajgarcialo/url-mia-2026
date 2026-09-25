Laboratorio No. 2 - Cloud Storage

1.Objetivo de la aplicación

Desarrollar una aplicación de consola en C# que permita conectarse a Azure Blob Storage mediante una Connection String y realizar operaciones básicas con archivos almacenados en la nube: subir, listar, descargar y eliminar archivos.

2.Tecnologías utilizadas

- C#
- .NET
- Visual Studio Code
- Microsoft Azure
- Azure Blob Storage
- Azure.Storage.Blobs
- GitHub

3.Configuración de Azure

Para realizar el laboratorio se utilizó un Storage Account en Microsoft Azure.

**Storage Account:** "miastorageag2026"

**Región:** France Central

**Rendimiento:** Standard

**Redundancia:** LRS

Dentro del Storage Account se creó un Blob Container llamado:
"mia-archivos"

El nivel de acceso del contenedor se configuró como **Private**.

La aplicación utiliza la Connection String del Storage Account para establecer la conexión con Azure Blob Storage.

4.Arquitectura de la solución

La aplicación está desarrollada como una aplicación de consola en C#.

La comunicación se realiza de la siguiente manera:

```text
Usuario
   |
   v
Aplicación de consola C#
   |
   v
BlobServiceClient
   |
   v
BlobContainerClient
   |
   v
Contenedor "mia-archivos"
   |
   v
BlobClient
   |
   v
Azure Blob Storage



5.Descripción de las cuatro operaciones

5.1 Subir archivo
Primero verifica que el archivo exista. Luego obtiene su nombre y crea un BlobClient para almacenarlo en el contenedor de Azure.
UploadAsync()

5.2 Listar archivos
La aplicación muestra los archivos que se encuentran almacenados en el contenedor.
Para obtener los archivos se utiliza:
GetBlobsAsync()
Se muestra el nombre y el tamaño de cada archivo en bytes.

5.3 Descargar archivo
La aplicación solicita el nombre del archivo que se desea descargar.
Primero verifica que el archivo exista en Azure y posteriormente solicita la carpeta de destino en la computadora.
Para realizar la descarga se utiliza:
DownloadToAsync()

5.4 Eliminar archivo
La aplicación solicita el nombre del archivo que se desea eliminar y verifica que exista.
Antes de eliminarlo solicita una confirmación al usuario.
Para eliminar el archivo de Azure se utiliza:
DeleteIfExistsAsync()
Esta operación elimina el archivo almacenado en Azure Blob Storage, pero no elimina el archivo original que se encuentra en la computadora.

6. Manejo de errores

La aplicación incluye diferentes validaciones para evitar errores durante su ejecución.

Se verifica:

Que la ruta del archivo no esté vacía.
Que el archivo local exista antes de subirlo.
Que el nombre del archivo no esté vacío.
Que el archivo exista en Azure antes de descargarlo.
Que el archivo exista antes de eliminarlo.
Que el usuario confirme la eliminación.

También se utiliza un bloque try-catch para manejar errores que puedan ocurrir durante la conexión o las operaciones realizadas con Azure Blob Storage.

Cuando ocurre un error, la aplicación muestra un mensaje indicando el problema.

7.Mecanismo utilizado para proteger la Connection String

La Connection String contiene información sensible del Storage Account, por lo que no se incluye directamente dentro del código fuente ni se publica en GitHub.

Para protegerla se utilizó una variable de entorno llamada:
AZURE_STORAGE_CONNECTION_STRING

La aplicación obtiene la Connection String mediante:
Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING")
De esta manera, la Connection String no queda escrita directamente en Program.cs ni dentro del repositorio de GitHub.

8. Instrucciones para ejecutar el proyecto
Paso 1. Abrir el proyecto

Abrir la carpeta del proyecto MIA_AzureBlob_AG en Visual Studio Code.

Paso 2. Restaurar las dependencias

Desde la terminal ejecutar:
dotnet restore

Paso 3. Instalar el paquete de Azure

Si todavía no está instalado, ejecutar:
dotnet add package Azure.Storage.Blobs

Paso 4. Configurar la Connection String

En PowerShell establecer la variable de entorno:
$env:AZURE_STORAGE_CONNECTION_STRING="TU_CONNECTION_STRING"

Se debe reemplazar TU_CONNECTION_STRING por la Connection String obtenida desde Azure Portal.

Paso 5. Ejecutar la aplicación

Ejecutar: dotnet run
El usuario puede seleccionar cualquiera de las cuatro operaciones o salir de la aplicación.
