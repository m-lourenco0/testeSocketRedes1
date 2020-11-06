using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SocketClienteSincrono
{

    public static void StartClient()
    {
        // Buffer de dados de entrada
        byte[] bytes = new byte[1024];

        // Conecta ao dispositivo remoto  
        try
        {
            // Estabelece o endpoint para o socket
            // Este exemplo utiliza a porta 11000 no computador local
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            // Cria um socket TCP/IP 
            Socket sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);


            // Conecta o socket ao endpoint remoto. Captura os erros.
            try
            {
                    sender.Connect(remoteEP);
                    
                    Console.WriteLine("Digite sua mensagem:");
                    // Codifica a string em uma array de bytes
                    byte[] msg = Encoding.ASCII.GetBytes(Console.ReadLine() + "<EOF>");

                    // Envia os dados atraves do socket 
                    int bitesEnviados = sender.Send(msg);

                    // Recebe a resposta do dispositivo remoto
                    int bytesRecuperados = sender.Receive(bytes);
                    Console.WriteLine("Mensagem retornada = {0}",
                    Encoding.ASCII.GetString(bytes, 0, bytesRecuperados));         
                    Console.WriteLine("\nAperte enter para continuar...");
                    Console.ReadKey();
                    // Libera o socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public static int Main(String[] args)
    {
        while (true)
        {
            Console.WriteLine("Deseja enviar uma mensagem? (S/N)");
            string mensagem = Console.ReadLine();
            if (mensagem.ToUpper() == "S")
            {
                StartClient();
            }
            else
            {
                break;
            }
        }
        
        return 0;
    }
}