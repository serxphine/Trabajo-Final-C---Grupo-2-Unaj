using System;
using System.Collections.Generic;
using System.Linq;

// PROGRAMA PRINCIPAL

// ¿Privatizar atributos? PREGUNTAR SI ES MEJOR

public class Program
{
    public static void Main(string[] args)
    {
        Banco banco = new Banco("Banco");
        int opcion;

        do
        {
            Console.WriteLine();
            Console.WriteLine("------ MENÚ BANCO ------");
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
                // AGREGAR CUENTA
                case 1:
                    Console.Write("DNI del cliente: ");
                    int dni = int.Parse(Console.ReadLine());

                    // Busca al cliente (cambiamos esto, antes lo teniamos como metodo de banco, preguntar)

                    Cliente cliente = banco.TodosLosClientes().Find(c => c.dni == dni);

                    if (cliente == null)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Usted es un cliente nuevo, debe ingresar los datos:");
                        Console.WriteLine();
                        Console.Write("Nombre: ");
                        string nombre = Console.ReadLine();
                        Console.Write("Apellido: ");
                        string apellido = Console.ReadLine();
                        Console.Write("Dirección: ");
                        string direccion = Console.ReadLine();
                        Console.Write("Teléfono: ");
                        int telefono = int.Parse(Console.ReadLine());
                        Console.Write("Mail: ");
                        string mail = Console.ReadLine();

                        cliente = new Cliente(nombre, apellido, dni, direccion, telefono, mail);
                        banco.AgregarCliente(cliente);
                    }

                    Console.Write("Saldo inicial: ");
                    double saldo = double.Parse(Console.ReadLine());

                    // Genera numero de cuenta (cambiamos esto, antes lo teniamos como metodo de banco, preguntar)

                    int nroCuenta = banco.CantidadCuentas() > 0 ?
                        banco.TodasLasCuentas().Max(c => c.numero) + 1 : 1;

                    Cuenta nueva = new Cuenta(nroCuenta, cliente, saldo);
                    banco.AgregarCuenta(nueva);

                    Console.WriteLine();
                    Console.WriteLine($"Cuenta creada exitosamente. Nº de cuenta: {nroCuenta}");
                    break;

                // ELIMINAR CUENTA

                case 2:
                    Console.Write("Número de cuenta: ");
                    int numero = int.Parse(Console.ReadLine());

                    // Busca la cuenta (cambiamos esto, antes lo teniamos en metodo como banco, preguntar)

                    Cuenta cuenta = banco.TodasLasCuentas().Find(c => c.numero == numero);

                    if (cuenta != null)
                    {
                        banco.EliminarCuenta(cuenta);

                        // Tiene mas de una cuenta (cambiamos esto, antes lo teniamos como metodo de banco, preguntar)

                        bool tieneMasCuentas = banco.TodasLasCuentas()
                            .Count(c => c.titular.dni == cuenta.titular.dni) > 0;

                        if (!tieneMasCuentas)
                        {
                            banco.EliminarCliente(cuenta.titular);
                            Console.WriteLine("Cliente eliminado (ya no tenía más cuentas).");
                        }

                        Console.WriteLine("Cuenta eliminada correctamente.");
                    }
                    else
                        Console.WriteLine("Cuenta no encontrada.");
                    break;

                // CLIENTES CON MÁS DE UNA CUENTA

                case 3:

                    // Obtiene clientes con mas de una cuenta (cambiamos esto, antes lo teniamos como metodo de banco, preguntar)

                    var clientesRepetidos = banco.TodosLosClientes()
                        .Where(c => banco.TodasLasCuentas()
                            .Count(cta => cta.titular.dni == c.dni) > 1)
                        .ToList();

                    if (clientesRepetidos.Count == 0)
                        Console.WriteLine("No hay clientes con más de una cuenta.");
                    else
                    {
                        foreach (var cli in clientesRepetidos)
                        {
                            Console.WriteLine($"Cliente: {cli.apellido}, {cli.nombre} - DNI: {cli.dni}");
                            foreach (var cta in banco.TodasLasCuentas()
                                .Where(c => c.titular.dni == cli.dni))
                            {
                                Console.WriteLine($"  Cuenta Nº {cta.numero} - Saldo: {cta.saldo}");
                            }
                        }
                    }
                    break;

                // EXTRAER DINERO

                case 4:
                    Console.Write("Número de cuenta: ");
                    int nroExtraccion = int.Parse(Console.ReadLine());

                    // Busca Cuenta (cambiamos esto, antes lo teniamos como metodo de banco, preguntar)

                    Cuenta extraerDe = banco.TodasLasCuentas().Find(c => c.numero == nroExtraccion);

                    if (extraerDe == null)
                    {
                        Console.WriteLine("Cuenta no encontrada.");
                        break;
                    }

                    Console.Write("Monto a extraer: ");
                    double montoExt = double.Parse(Console.ReadLine());

                    try
                    {
                        extraerDe.Extraer(montoExt);
                        Console.WriteLine($"Nuevo saldo: {extraerDe.saldo}");
                    }
                    catch (MontoinsuficienteExcepcion)
                    {
                        Console.WriteLine("Saldo insuficiente.");
                    }
                    break;

                // DEPOSITAR DINERO

                case 5:
                    Console.Write("Número de cuenta: ");
                    int nroDep = int.Parse(Console.ReadLine());

                    // Busca Cuenta (cambiamos esto, antes lo teniamos como metodo de banco, preguntar)

                    Cuenta depositarEn = banco.TodasLasCuentas().Find(c => c.numero == nroDep);

                    if (depositarEn == null)
                    {
                        Console.WriteLine("Cuenta no encontrada.");
                        break;
                    }

                    Console.Write("Monto a depositar: ");
                    double montoDep = double.Parse(Console.ReadLine());
                    depositarEn.Depositar(montoDep);

                    Console.WriteLine($"Nuevo saldo: {depositarEn.saldo}");
                    break;

                // TRANSFERIR DINERO

                case 6:
                    Console.Write("Número de cuenta origen: ");
                    int nroOrigen = int.Parse(Console.ReadLine());
                    Console.Write("Número de cuenta destino: ");
                    int nroDestino = int.Parse(Console.ReadLine());
                    Console.Write("Monto: ");
                    double monto = double.Parse(Console.ReadLine());

                    // Transferir ahora se hace en el main (cambiamos esto, antes lo teniamos como metodo de banco, preguntar)

                    Cuenta origen = banco.TodasLasCuentas().Find(c => c.numero == nroOrigen);
                    Cuenta destino = banco.TodasLasCuentas().Find(c => c.numero == nroDestino);

                    if (origen == null || destino == null)
                    {
                        Console.WriteLine("Error: una de las cuentas no existe.");
                        break;
                    }

                    try
                    {
                        origen.Extraer(monto);
                        destino.Depositar(monto);
                        Console.WriteLine("Transferencia exitosa.");
                    }
                    catch (MontoinsuficienteExcepcion)
                    {
                        Console.WriteLine("Saldo insuficiente en la cuenta origen.");
                    }
                    break;

                // LISTAR CUENTAS

                case 7:
                    foreach (var cta in banco.TodasLasCuentas())
                        Console.WriteLine($"Cuenta Nº {cta.numero} | {cta.titular.apellido}, {cta.titular.nombre} | DNI: {cta.titular.dni} | Saldo: {cta.saldo}");
                    break;

                // LISTAR CLIENTES

                case 8:
                    foreach (var cli in banco.TodosLosClientes())
                        Console.WriteLine($"{cli.nombre} {cli.apellido} - DNI: {cli.dni} - Tel: {cli.telefono} - Mail: {cli.mail}");
                    break;
            }

        } while (opcion != 0);
    }
}

// CLASE BANCO

public class Banco
{
    public string nombre;
    public List<Cliente> clientes;
    public List<Cuenta> cuentas;

    public Banco(string nombre)
    {
        this.nombre = nombre;
        clientes = new List<Cliente>();
        cuentas = new List<Cuenta>();
    }

    // METODOS PARA CLIENTES
    public void AgregarCliente(Cliente c) 
    { 
        clientes.Add(c); 
    }
    public void EliminarCliente(Cliente c)
    {
        clientes.Remove(c);
    }

    public int CantidadClientes() 
    { 
        return clientes.Count; 
    }
    public Cliente VerCliente(int i) 
    { 
        return clientes[i]; 
    }
    public List<Cliente> TodosLosClientes() 
    { 
        return clientes; 
    }

    // METODOS PARA CUENTAS 
    public void AgregarCuenta(Cuenta c) 
    { 
        cuentas.Add(c); 
    }
    public void EliminarCuenta(Cuenta c) 
    { 
        cuentas.Remove(c); 
    }
    public int CantidadCuentas() 
    { 
        return cuentas.Count; 
    }
    public Cuenta VerCuenta(int i) 
    { 
        return cuentas[i]; 
    }
    public List<Cuenta> TodasLasCuentas() 
    { 
        return cuentas; 
    }
}

// CLASE CUENTA

public class Cuenta
{
    public int numero;
    public Cliente titular;
    public double saldo;

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
    public string nombre;
    public string apellido;
    public int dni;
    public string direccion;
    public int telefono;
    public string mail;

    public Cliente(string nombre, string apellido, int dni, string direccion, int telefono, string mail)
    {
        this.nombre = nombre;
        this.apellido = apellido;
        this.dni = dni;
        this.direccion = direccion;
        this.telefono = telefono;
        this.mail = mail;
    }

    public override string ToString()
    {
        return $"Nombre y Apellido: {nombre} {apellido}, DNI: {dni}";
    }
}

// EXCEPCIÓN
public class MontoinsuficienteExcepcion : Exception 
{

}
