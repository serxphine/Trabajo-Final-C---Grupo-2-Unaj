using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

public class Program
{
    public static void Main(string[] args)
    {
        Banco banco = new Banco("Banco");
        int opcion;

        do
        {
            Console.WriteLine();
            Console.WriteLine("--- MENÚ BANCO ---");
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
                case 1:
                    banco.AgregarCuenta();
                    break;

                case 2:
                    banco.EliminarCuenta();
                    break;

                case 3:
                    banco.ListarClientesConMasDeUnaCuenta();
                    break;

                case 4:
                    banco.RealizarExtraccion();
                    break;

                case 5:
                    banco.DepositarDinero();
                    break;

                case 6:
                    banco.Transferir();
                    break;

                case 7:
                    banco.ListarCuentas();
                    break;

                case 8:
                    banco.ListarClientes();
                    break;
            }

        } while (opcion != 0);
    }
}

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

    // --- Agregar cuenta ---
    public void AgregarCuenta()
    {
        Console.WriteLine("------------------------------");
        Console.WriteLine();

        Console.Write("Ingrese el DNI del cliente: ");
        int dni = int.Parse(Console.ReadLine());

        Cliente cliente = clientes.Find(c => c.dni == dni);

        if (cliente == null)
        {
            Console.WriteLine();
            Console.WriteLine("No hay ninguna cuenta con ese DNI, debe ingresar los datos del nuevo cliente.");
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

            cliente = new Cliente(nombre, apellido, dni, direccion, telefono, mail);
            clientes.Add(cliente);
        }

        int numero = 1;

        if (cuentas.Count > 0)
        {
            numero = cuentas.Max(c => c.numero) + 1;
        }

        Console.WriteLine();
        Console.Write("Saldo inicial: ");
        double saldo = double.Parse(Console.ReadLine());
        Console.WriteLine();

        Cuenta cuenta = new Cuenta(numero, cliente.apellido, cliente.dni, saldo);
        cuentas.Add(cuenta);

        Console.WriteLine($"Cuenta creada con exito, su numero de cliente es: {numero}");

        Console.WriteLine();
        Console.WriteLine("------------------------------");
    }

    // --- Eliminar cuenta ---
    public void EliminarCuenta()
    {
        Console.WriteLine("------------------------------");
        Console.WriteLine();

        Console.Write("Ingrese el numero de cuenta: ");
        int num = int.Parse(Console.ReadLine());
        Console.WriteLine();

        Cuenta c = cuentas.Find(x => x.numero == num);

        if (c != null)
        {
            cuentas.Remove(c);
            Console.WriteLine("Su cuenta ha sido eliminada con exito");
        }
        else
            Console.WriteLine("No se encontró ninguna cuenta");

        Console.WriteLine();
        Console.WriteLine("------------------------------");
    }

    // --- Listar clientes con más de una cuenta ---
    public void ListarClientesConMasDeUnaCuenta()
    {
        Console.WriteLine("------------------------------");
        Console.WriteLine();

        Console.WriteLine("Clientes con más de una cuenta:");
        Console.WriteLine();

        foreach (Cliente cliente in clientes)
        {
            int contador = 0;

            foreach (Cuenta c in cuentas)
            {
                if (c.dni == cliente.dni)
                    contador++;
            }

            if (contador > 1)
            {
                Console.WriteLine($"Cliente: {cliente.apellido} DNI: {cliente.dni}");
            }
        }

        Console.WriteLine();
        Console.WriteLine("------------------------------");
    }

    // --- Extracción ---
    public void RealizarExtraccion()
    {
        Console.WriteLine("------------------------------");
        Console.WriteLine();

        Console.Write("Ingrese el numero de cuenta: ");
        int num = int.Parse(Console.ReadLine());
        Console.WriteLine();

        Cuenta c = cuentas.Find(x => x.numero == num);

        if (c == null)
        {
            Console.WriteLine();
            Console.WriteLine("Lo sentimos, no encontramos nada con ese numero de cuenta");
            return;
        }

        Console.Write("Ingrese el monto a extraer: ");
        double monto = double.Parse(Console.ReadLine());
        Console.WriteLine();

        try
        {
            c.Extraer(monto);
            Console.WriteLine($"Nuevo saldo: {c.saldo}");
        }
        catch (MontoinsuficienteExcepcion)
        {
            Console.WriteLine();
            Console.WriteLine("Lo sentimos, el saldo actual es insuficiente");
        }

        Console.WriteLine();
        Console.WriteLine("------------------------------");
    }

    // --- Depositar ---
    public void DepositarDinero()
    {
        Console.WriteLine("------------------------------");
        Console.WriteLine();

        Console.Write("Ingrese su numero de cuenta: ");
        int num = int.Parse(Console.ReadLine());
        Console.WriteLine();

        Cuenta c = cuentas.Find(x => x.numero == num);

        if (c == null)
        {
            Console.WriteLine("No se ha podido encontrar ninguna cuenta con ese numero");
            return;
        }

        Console.Write("Ingrese el dinero que desea depositar: ");
        double monto = double.Parse(Console.ReadLine());
        Console.WriteLine();

        c.Depositar(monto);
        Console.WriteLine($"Nuevo saldo: {c.saldo}");

        Console.WriteLine();
        Console.WriteLine("------------------------------");
    }

    // --- Transferencia ---
    public void Transferir()
    {
        Console.WriteLine("------------------------------");
        Console.WriteLine();

        Console.Write("Ingrese el numero de cuenta del titular: ");
        int origen = int.Parse(Console.ReadLine());
        Console.WriteLine();

        Console.Write("Ingrese el numero de la cuenta a transferir: ");
        int destino = int.Parse(Console.ReadLine());
        Console.WriteLine();

        Console.Write("Ingrese el dinero a transferir: ");
        double monto = double.Parse(Console.ReadLine());
        Console.WriteLine();

        Cuenta c1 = cuentas.Find(x => x.numero == origen);
        Cuenta c2 = cuentas.Find(x => x.numero == destino);

        if (c1 == null || c2 == null)
        {
            Console.WriteLine("Una de las cuentas no existe");
            return;
        }

        try
        {
            c1.Extraer(monto);
            c2.Depositar(monto);
            Console.WriteLine("Transferencia realizada con éxito.");
        }
        catch (MontoinsuficienteExcepcion)
        {
            Console.WriteLine("Saldo insuficiente en cuenta origen.");
        }

        Console.WriteLine();
        Console.WriteLine("------------------------------");
    }

    // --- Listar cuentas ---
    public void ListarCuentas()
    {
        Console.WriteLine("------------------------------");
        Console.WriteLine();

        if (cuentas.Count == 0)
        {
            Console.WriteLine("No hay cuentas registradas.");
            return;
        }

        foreach (var c in cuentas)
            Console.WriteLine($"Cuenta Nº {c.numero} | {c.apellido} | DNI: {c.dni} | Saldo: {c.saldo}");

        Console.WriteLine();
        Console.WriteLine("------------------------------");
    }

    // --- Listar clientes ---
    public void ListarClientes()
    {
        Console.WriteLine("------------------------------");
        Console.WriteLine();

        if (clientes.Count == 0)
        {
            Console.WriteLine("No hay clientes registrados.");
            return;
        }

        foreach (var c in clientes)
            c.DatosCliente();

        Console.WriteLine();
        Console.WriteLine("------------------------------");
    }
}

public class Cuenta
{
    public int numero { get; set; }
    public string apellido { get; set; }
    public int dni { get; set; }
    public double saldo { get; set; }

    public Cuenta(int numero, string apellido, int dni, double saldo)
    {
        this.numero = numero;
        this.apellido = apellido;
        this.dni = dni;
        this.saldo = saldo;
    }

    public void Depositar(double monto)
    {

        if (monto <= 0)
            Console.WriteLine("El monto debe ser mayor a 0.");
        else
            saldo += monto;
    }

    public void Extraer(double monto)
    {

        if (monto > saldo)
            throw new MontoinsuficienteExcepcion();
        saldo -= monto;
    }
}

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

    public void DatosCliente()
    {
        Console.WriteLine($"Cliente: {nombre} {apellido}, DNI: {dni}, Dirección: {direccion}, Teléfono: {telefono}, Mail: {mail}");
    }
}

public class MontoinsuficienteExcepcion : Exception
{

}
