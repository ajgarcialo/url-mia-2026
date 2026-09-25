using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

class Program
{
    private static readonly string connectionString =
        Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING")
        ?? throw new Exception("No se encontró la Connection String.");

    // Nombre del contenedor que creaste en Azure
    private const string containerName = "mia-archivos";

    static async Task Main()
    {
        try
        {
            BlobServiceClient blobServiceClient =
                new BlobServiceClient(connectionString);

            BlobContainerClient containerClient =
                blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync();

            int opcion;

            do
            {
                Console.Clear();

                Console.WriteLine("=================================");
                Console.WriteLine("      MIA - AZURE BLOB STORAGE");
                Console.WriteLine("=================================");
                Console.WriteLine("1. Subir archivo");
                Console.WriteLine("2. Listar archivos");
                Console.WriteLine("3. Descargar archivo");
                Console.WriteLine("4. Eliminar archivo");
                Console.WriteLine("5. Salir");
                Console.WriteLine("=================================");
                Console.Write("Seleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine("Opción no válida.");
                    Console.ReadKey();
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        await SubirArchivo(containerClient);
                        break;

                    case 2:
                        await ListarArchivos(containerClient);
                        break;

                    case 3:
                        await DescargarArchivo(containerClient);
                        break;

                    case 4:
                        await EliminarArchivo(containerClient);
                        break;

                    case 5:
                        Console.WriteLine("Saliendo...");
                        break;

                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }

                if (opcion != 5)
                {
                    Console.WriteLine("\nPresione una tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcion != 5);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocurrió un error:");
            Console.WriteLine(ex.Message);
        }
    }

    // 1. SUBIR ARCHIVO
    static async Task SubirArchivo(BlobContainerClient containerClient)
    {
        Console.Write("\nIngrese la ruta local del archivo: ");
        string? ruta = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(ruta))
        {
            Console.WriteLine("La ruta no puede estar vacía.");
            return;
        }

        if (!File.Exists(ruta))
        {
            Console.WriteLine("El archivo no existe.");
            return;
        }

        string nombreArchivo = Path.GetFileName(ruta);

        BlobClient blobClient =
            containerClient.GetBlobClient(nombreArchivo);

        await blobClient.UploadAsync(ruta, overwrite: true);

        Console.WriteLine(
            $"Archivo '{nombreArchivo}' subido correctamente."
        );
    }

    // 2. LISTAR ARCHIVOS
    static async Task ListarArchivos(BlobContainerClient containerClient)
    {
        Console.WriteLine("\nArchivos en el contenedor:");
        Console.WriteLine("--------------------------------");

        bool hayArchivos = false;

        await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
        {
            hayArchivos = true;

            Console.WriteLine($"Nombre: {blobItem.Name}");
            Console.WriteLine(
                $"Tamaño: {blobItem.Properties.ContentLength ?? 0} bytes"
            );

            Console.WriteLine("--------------------------------");
        }

        if (!hayArchivos)
        {
            Console.WriteLine("No hay archivos en el contenedor.");
        }
    }

    // 3. DESCARGAR ARCHIVO
    static async Task DescargarArchivo(
        BlobContainerClient containerClient)
    {
        Console.Write("\nIngrese el nombre del archivo: ");
        string? nombreArchivo = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nombreArchivo))
        {
            Console.WriteLine("El nombre no puede estar vacío.");
            return;
        }

        BlobClient blobClient =
            containerClient.GetBlobClient(nombreArchivo);

        if (!await blobClient.ExistsAsync())
        {
            Console.WriteLine("El archivo no existe en Azure.");
            return;
        }

        Console.Write("Ingrese la carpeta de destino: ");
        string? carpetaDestino = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(carpetaDestino))
        {
            Console.WriteLine("La carpeta no puede estar vacía.");
            return;
        }

        Directory.CreateDirectory(carpetaDestino);

        string rutaDestino =
            Path.Combine(carpetaDestino, nombreArchivo);

        await blobClient.DownloadToAsync(rutaDestino);

        Console.WriteLine(
            $"Archivo descargado correctamente en: {rutaDestino}"
        );
    }

    // 4. ELIMINAR ARCHIVO
    static async Task EliminarArchivo(
        BlobContainerClient containerClient)
    {
        Console.Write("\nIngrese el nombre del archivo: ");
        string? nombreArchivo = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nombreArchivo))
        {
            Console.WriteLine("El nombre no puede estar vacío.");
            return;
        }

        BlobClient blobClient =
            containerClient.GetBlobClient(nombreArchivo);

        if (!await blobClient.ExistsAsync())
        {
            Console.WriteLine("El archivo no existe.");
            return;
        }

        Console.Write(
            $"¿Está seguro de eliminar '{nombreArchivo}'? (S/N): "
        );

        string? confirmacion = Console.ReadLine();

        if (confirmacion?.ToUpper() != "S")
        {
            Console.WriteLine("Operación cancelada.");
            return;
        }

        await blobClient.DeleteIfExistsAsync();

        Console.WriteLine("Archivo eliminado correctamente.");
    }
}