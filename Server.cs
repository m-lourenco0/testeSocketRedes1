using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SocketServidorSincrono
{

    // Dados vindos do cliente
    public static string data = null;

    public static void IniciarServidor()
    {
        // Buffer de dados  
        byte[] bytes = new Byte[1024];

        // Estabelece a porta local do socket 
        // Dns.GetHostName retorna o nome do host rodando a aplicacao
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress enderecoIP = ipHostInfo.AddressList[0];
        IPEndPoint endPointLocal = new IPEndPoint(enderecoIP, 11000);

        // Cria um socket TCP/IP 
        Socket listener = new Socket(enderecoIP.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        // Vincular o socket ao endereço e esperando por conexoes
        try
        {
            listener.Bind(endPointLocal);
            listener.Listen(10);

            // Inicia a espera de conexoes
            while (true)
            {
                Console.WriteLine("Aguardando uma conexao...\n");
                // Programa fica suspenso ate iniciar uma conexao
                Socket handler = listener.Accept();
                data = null;

                // Processamento da conexao iniciada 
                while (true)
                {
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }
                data = data.Remove(data.IndexOf("<EOF>"));
                // Mostra os dados no console 
                Console.WriteLine("Texto recebido : {0}", data);

                // Ecoa os dados de volta para o cliente 
                byte[] msg = Encoding.ASCII.GetBytes(data);

                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nAperte ENTER para continuar...");
        Console.Read();

    }

    public static int Main(String[] args)
    {
        IniciarServidor();
        return 0;
    }
}