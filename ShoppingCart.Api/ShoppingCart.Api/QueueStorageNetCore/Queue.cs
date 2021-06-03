using System;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Api.QueueStorageNetCore
{
    public class Queue
    {
        public Queue()
        {
        }
        
        static async Task send(string arg)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=rgqaarchitecturediag;AccountKey=Pq4IjNTdpC0CHXopxCJ84E9KdBNoCv1hLTiOFuBoAnB99/hgEUVd/sCDC5FFfJe8XucJmxwI6c7/9tSXITFB1w==;EndpointSuffix=core.windows.net";
           QueueClient queue = new QueueClient(connectionString, "Sandwich.com");

            if (arg != "0")
            {
                string value = arg;
                await InsertMessageAsync(queue, value);
             
            }
            else
            {
                string value = await RetrieveNextMessageAsync(queue);
           
            }
            
        }


        //A este método se le pasa una referencia de cola.Se crea una cola, en caso de que no exista, mediante una llamada a CreateIfNotExistsAsync.Luego, 
        //agrega newMessage a la cola mediante una llamada a SendMessageAsync.
        //Opcional: de forma predeterminada, el tiempo de vida máximo de un mensaje se establece en siete días.Puede especificar cualquier número positivo para el período de 
        // vida de un mensaje.El siguiente fragmento de código agrega un mensaje que nunca expira.

        static async Task InsertMessageAsync(QueueClient theQueue, string newMessage)
        {
            if (null != await theQueue.CreateIfNotExistsAsync())
            {
                Console.WriteLine("The queue was created.");
            }

            await theQueue.SendMessageAsync(newMessage);
        }


        //Este método recibe un mensaje de la cola mediante la realización de una llamada a ReceiveMessagesAsync, y usando1 en el primer
        // parámetro para recuperar solo el siguiente mensaje de la cola.Una vez recibido el mensaje, elimínelo de la cola mediante una llamada a DeleteMessageAsync.
        //Cuando se envía un mensaje a la cola con una versión del SDK anterior a la v12, se codifica automáticamente en Base64.A partir de la versión 12, se ha quitado esta 
        //funcionalidad.Cuando se recupera un mensaje mediante el SDK de la v12, no se descodifica automáticamente en Base64. Debe descodificar explícitamente en Base64 el contenido usted mismo.
        static async Task<string> RetrieveNextMessageAsync(QueueClient theQueue)
        {
            if (await theQueue.ExistsAsync())
            {
                QueueProperties properties = await theQueue.GetPropertiesAsync();

                if (properties.ApproximateMessagesCount > 0)
                {
                    QueueMessage[] retrievedMessage = await theQueue.ReceiveMessagesAsync(1);
                    string theMessage = retrievedMessage[0].MessageText;
                    await theQueue.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                    return theMessage;
                }
                else
                {
                    Console.Write("The queue is empty. Attempt to delete it? (Y/N) ");
                    string response = Console.ReadLine();

                    if (response.ToUpper() == "Y")
                    {
                        await theQueue.DeleteIfExistsAsync();
                        return "The queue was deleted.";
                    }
                    else
                    {
                        return "The queue was not deleted.";
                    }
                }
            }
            else
            {
                return "The queue does not exist. Add a message to the command line to create the queue and store the message.";
            }
        }
    }
}
