using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class TCPServer
{
    static void Main()
    {
        // Define the IP address and port on which the server will listen.
        string ipAddress = "127.0.0.1"; // You can change this to your desired IP address.
        int port = 12345; // You can change this to your desired port number.

        // Create a TCP listener to listen for incoming connections.
        TcpListener listener = new TcpListener(IPAddress.Parse(ipAddress), port);

        try
        {
            // Start listening for incoming client requests.
            listener.Start();
            Console.WriteLine($"Server is listening on {ipAddress}:{port}");

            while (true)
            {
                // Accept a client connection when one comes in.
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine($"Accepted connection from {((IPEndPoint)client.Client.RemoteEndPoint).Address}:{((IPEndPoint)client.Client.RemoteEndPoint).Port}");

                // Create a new thread to handle the client communication.
                Thread clientThread = new Thread(HandleClient);
                clientThread.Start(client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            // Stop listening and close the listener when done.
            listener.Stop();
        }
    }

    static void HandleClient(object clientObj)
    {
        TcpClient client = (TcpClient)clientObj;
        NetworkStream stream = client.GetStream();

        try
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                // Convert the received data to a string.
                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received: {data}");

                // You can process the data here and send a response back to the client if needed.

                // Example response:
                string response = "Server received your message: " + data;
                byte[] responseData = Encoding.ASCII.GetBytes(response);
                stream.Write(responseData, 0, responseData.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            // Close the client connection when done.
            client.Close();
            Console.WriteLine($"Closed connection from {((IPEndPoint)client.Client.RemoteEndPoint).Address}:{((IPEndPoint)client.Client.RemoteEndPoint).Port}");
        }
    }
}
