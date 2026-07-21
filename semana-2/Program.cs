using System.Text.Json;
namespace semana_2;
 class Program
{
    static void Main(string[]args)
    {
        string rutaCsv="estudiantes.csv";
        string rutaJson="estudiantes.json";

        string[]lineas=File.ReadAllLines(rutaCsv);
        List<Estudiante>estudiantes=new List<Estudiante>();
        for(int i = 1; i < lineas.Length; i++)
        {
            string linea=lineas[i];
            if (string.IsNullOrWhiteSpace(linea))
            {
                continue;
            }
            string[]datos=linea.Split(',');
            
            Estudiante estudiante=new Estudiante
            {
                Id=int.Parse(datos[0]),
                Nombre=datos[1],
                Carrera=datos[2]
            };
            estudiantes.Add(estudiante);

        }
        foreach(Estudiante e in estudiantes)
        {
            Console.WriteLine($"{e.Id}-{e.Nombre}-{e.Carrera}");
        }
        var opciones=new JsonSerializerOptions{WriteIndented=true};
        string json=JsonSerializer.Serialize(estudiantes,opciones);

        File.WriteAllText(rutaJson,json);
        Console.WriteLine("Archivo estudiantes.json creado correctamente");
    }
}
