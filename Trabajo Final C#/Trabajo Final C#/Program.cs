using System;
using System.Collections.Generic;
using System.Linq;


// PROGRAMA PRINCIPAL

public class Program
{
    public static void Main(string[] args)
    {
        Banco banco = new Banco("Banco Nacional");
        int opcion;

        do
        {
            Console.WriteLine();
            Console.WriteLine("------ MENÚ BANCO ------");
            Console.WriteLine();
            Console.WriteLine("1. Agregar cuenta");
            Console.WriteLine("2. Eliminar cuenta");
            Console.WriteLine("3. Listar clientes con más de una cuenta");
            Console.WriteLine("4. Realizar extracción");
            Console.WriteLine("5. Depositar dinero");
            Console.WriteLine("6. Transferir dinero");
            Console.WriteLine("7. Listar cuentas");
            Console.WriteLine("8. Listar clientes");
            Console.WriteLine("0. Salir");
            Console.WriteLine();
            Console.Write("Opción: ");

            opcion = int.Parse(Console.ReadLine());
            Console.WriteLine();

            switch (opcion)
            {
                // a) Agregar cuenta

                case 1:
                    Console.Write("DNI del cliente: ");
                    int dni = int.Parse(Console.ReadLine());

                    // Buscar cliente por DNI

                    Cliente cliente = banco.BuscarCliente(dni);

                    // Si no existe, se crea nuevo cliente

                    if (cliente == null)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Usted es un cliente nuevo, debe ingresar los datos:");
                        Console.WriteLine();
                        Console.Write("Nombre: ");
                        string nombre = Console.ReadLine();
                        Console.WriteLine();

                        Console.Write("Apellido: ");
                        string apellido = Console.ReadLine();
                        Console.WriteLine();

                        Console.Write("Dirección: ");
                        string direccion = Console.ReadLine();
                        Console.WriteLine();

                        Console.Write("Teléfono: ");
                        string telefono = Console.ReadLine();
                        Console.WriteLine();

                        Console.Write("Mail: ");
                        string mail = Console.ReadLine();
                        Console.WriteLine();

                        cliente = new Cliente(nombre, apellido, dni, direccion, telefono, mail);
                        banco.AgregarCliente(cliente);
                    }

                    Console.Write("Saldo inicial: ");
                    double saldo = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    // Crear nueva cuenta

                    int nroCuenta = banco.GenerarNumeroCuenta();
                    Cuenta nueva = new Cuenta(nroCuenta, cliente, saldo);
                    banco.AgregarCuenta(nueva);

                    Console.WriteLine($"Cuenta creada exitosamente, Nº de cuenta: {nroCuenta}");
                    Console.WriteLine();
                    break;

                // b) Eliminar cuenta

                case 2:
                    Console.Write("Número de cuenta: ");
                    int numero = int.Parse(Console.ReadLine());
                    Console.WriteLine();

                    Cuenta cuenta = banco.BuscarCuenta(numero);
                    if (cuenta != null)
                    {
                        banco.EliminarCuenta(cuenta);

                        // Si el cliente no tiene mas cuentas, se elimina tambien

                        if (!banco.TieneMasCuentas(cuenta.titular.dni))
                        {
                            banco.EliminarCliente(cuenta.titular);
                            Console.WriteLine();
                            Console.WriteLine("Cliente eliminado (ya no tenía cuentas).");
                        }

                        Console.WriteLine();
                        Console.WriteLine("Cuenta eliminada correctamente.");
                    }
                    else
                        Console.WriteLine("Cuenta no encontrada.");
                    break;

                // c) Listar clientes con más de una cuenta

                case 3:
                    var clientesRepetidos = banco.ObtenerClientesConMasDeUnaCuenta();
                    if (clientesRepetidos.Count == 0)
                        Console.WriteLine("No hay clientes con más de una cuenta.");
                    else
                    {
                        foreach (var cli in clientesRepetidos)
                        {
                            Console.WriteLine($"Cliente: {cli.apellido}, {cli.nombre} - DNI: {cli.dni}");
                            foreach (var cta in banco.cuentas.Where(c => c.titular.dni == cli.dni))
                                Console.WriteLine($"  Cuenta Nº {cta.numero} - Saldo: {cta.saldo}");
                        }
                    }
                    break;

                // d) Realizar extracción

                case 4:
                    Console.Write("Numero de cuenta: ");
                    int nCuenta = int.Parse(Console.ReadLine());
                    Cuenta extraerDe = banco.BuscarCuenta(nCuenta);

                    if (extraerDe == null)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Cuenta no encontrada.");
                        break;
                    }

                    Console.WriteLine();
                    Console.Write("Monto a extraer: ");
                    double montoExt = double.Parse(Console.ReadLine());

                    try
                    {
                        Console.WriteLine();
                        extraerDe.Extraer(montoExt);
                        Console.WriteLine($"Nuevo saldo: {extraerDe.saldo}");
                    }
                    catch (MontoinsuficienteExcepcion)
                    {
                        Console.WriteLine("Saldo insuficiente.");
                    }
                    break;

                // e) Depositar dinero

                case 5:
                    Console.Write("Numero de cuenta: ");
                    int nDep = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Cuenta depositarEn = banco.BuscarCuenta(nDep);

                    if (depositarEn == null)
                    {
                        Console.WriteLine("Cuenta no encontrada.");
                        break;
                    }

                    Console.Write("Monto a depositar: ");
                    double montoDep = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    depositarEn.Depositar(montoDep);
                    Console.WriteLine($"Nuevo saldo: {depositarEn.saldo}");
                    break;

                // f) Transferir dinero

                case 6:
                    Console.Write("Numero de cuenta origen: ");
                    int nroOrigen = int.Parse(Console.ReadLine());
                    Console.WriteLine();

                    Console.Write("Numero de cuenta destino: ");
                    int nroDestino = int.Parse(Console.ReadLine());
                    Console.WriteLine();

                    Console.Write("Monto: ");
                    double monto = double.Parse(Console.ReadLine());
                    Console.WriteLine();

                    try
                    {
                        banco.Transferir(nroOrigen, nroDestino, monto);
                        Console.WriteLine("Transferencia exitosa.");
                    }
                    catch (MontoinsuficienteExcepcion)
                    {
                        Console.WriteLine("Saldo insuficiente en la cuenta origen.");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error: una de las cuentas no existe.");
                    }
                    break;

                // g) Listar cuentas

                case 7:
                    foreach (var cta in banco.cuentas)
                        Console.WriteLine($"Cuenta Nº {cta.numero} | {cta.titular.apellido}, {cta.titular.nombre} | DNI: {cta.titular.dni} | Saldo: {cta.saldo}");
                    break;

                // h) Listar clientes

                case 8:
                    foreach (var cli in banco.clientes)
                        Console.WriteLine($"{cli.nombre} {cli.apellido} - DNI: {cli.dni} - Tel: {cli.telefono} - Mail: {cli.mail}");
                    break;
            }

        } while (opcion != 0);
    }
}

// CLASE BANCO

public class Banco
{
    public string nombre { get; set; }
    public List<Cliente> clientes { get; set; }
    public List<Cuenta> cuentas { get; set; }

    public Banco(string nombre)
    {
        this.nombre = nombre;
        clientes = new List<Cliente>();
        cuentas = new List<Cuenta>();
    }

    // Metodos simples
    public void AgregarCliente(Cliente c) => clientes.Add(c);
    public void EliminarCliente(Cliente c) => clientes.Remove(c);
    public void AgregarCuenta(Cuenta c) => cuentas.Add(c);
    public void EliminarCuenta(Cuenta c) => cuentas.Remove(c);

    public Cliente BuscarCliente(int dni) => clientes.Find(c => c.dni == dni);
    public Cuenta BuscarCuenta(int numero) => cuentas.Find(c => c.numero == numero);
    public int GenerarNumeroCuenta() => cuentas.Count > 0 ? cuentas.Max(c => c.numero) + 1 : 1;

    // Devuelve si un cliente tiene más de una cuenta
    public bool TieneMasCuentas(int dni) => cuentas.Count(c => c.titular.dni == dni) > 0;

    // Devuelve lista de clientes con más de una cuenta
    public List<Cliente> ObtenerClientesConMasDeUnaCuenta() =>
        clientes.Where(c => cuentas.Count(cta => cta.titular.dni == c.dni) > 1).ToList();

    // Transferencia entre cuentas
    public void Transferir(int nroOrigen, int nroDestino, double monto)
    {
        Cuenta origen = BuscarCuenta(nroOrigen);
        Cuenta destino = BuscarCuenta(nroDestino);

        if (origen == null || destino == null)
            throw new Exception("Cuenta inexistente");

        origen.Extraer(monto);
        destino.Depositar(monto);
    }
}

// CLASE CUENTA

public class Cuenta
{
    public int numero { get; set; }
    public Cliente titular { get; set; } // Guarda cliente completo
    public double saldo { get; set; }

    public Cuenta(int numero, Cliente titular, double saldo)
    {
        this.numero = numero;
        this.titular = titular;
        this.saldo = saldo;
    }

    public void Depositar(double monto)
    {
        if (monto <= 0)
            throw new Exception("El monto debe ser mayor que 0");
        saldo += monto;
    }

    public void Extraer(double monto)
    {
        if (monto > saldo)
            throw new MontoinsuficienteExcepcion();
        saldo -= monto;
    }
}

// CLASE CLIENTE

public class Cliente
{
    public string nombre { get; set; }
    public string apellido { get; set; }
    public int dni { get; set; }
    public string direccion { get; set; }
    public string telefono { get; set; }
    public string mail { get; set; }

    public Cliente(string nombre, string apellido, int dni, string direccion, string telefono, string mail)
    {
        this.nombre = nombre;
        this.apellido = apellido;
        this.dni = dni;
        this.direccion = direccion;
        this.telefono = telefono;
        this.mail = mail;
    }
}

// EXCEPCIÓN PERSONALIZADA
public class MontoinsuficienteExcepcion : Exception { }
