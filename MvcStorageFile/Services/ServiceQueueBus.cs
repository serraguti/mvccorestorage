using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStorage.Services
{
    public class ServiceQueueBus
    {
        private ServiceBusClient client;
        public ServiceQueueBus(String keys)
        {
            this.client = new ServiceBusClient(keys);
        }

        public async Task SendMessage(String data)
        {
            //NECESITAMOS UN SENDER QUE ES EL QUE VA
            //ASOCIADO A LA QUEUE
            ServiceBusSender sender = this.client.CreateSender("programeitors");
            //EL PROPIO MENSAJE
            ServiceBusMessage message =
                new ServiceBusMessage(data);
            //EL MENSAJE SE MANDA MEDIANTE EL SENDER
            await sender.SendMessageAsync(message);
        }

        private Queue<ServiceBusMessage> CreateMessages()
        {
            Queue<ServiceBusMessage> mensajes =
                new Queue<ServiceBusMessage>();
            mensajes.Enqueue(new ServiceBusMessage("Primer mensaje"));
            mensajes.Enqueue(new ServiceBusMessage("Segundo mensaje"));
            mensajes.Enqueue(new ServiceBusMessage("Tercer mensaje"));
            return mensajes;
        }
        
        public async Task SendBatchMessages()
        {
            //CREAMOS EL SENDER
            ServiceBusSender sender =
                this.client.CreateSender("programeitors");
            //RECUPERAMOS LOS MENSAJES EN BATCH
            Queue<ServiceBusMessage> colamensajes = this.CreateMessages();
            //LOS MENSAJES ESTA EN MODO ENQUEUE
            //A MEDIDA QUE LOS VAYAMOS PROCESANDO, SE IRAN
            //QUITANDO DE LA COLA Y PASANDO A PROCESO DEQUEUE
            //ESTE PROCESO NO SABEMOS CUANTO TARDARA
            //DEBEMOS HACER EL PROCESO DE BATCH EN UN BUCLE
            //MIENTRAS LOS MENSAJES ESTAN EN LA COLA
            while (colamensajes.Count > 0)
            {
                //TODOS LOS MENSAJES DE LA COLA SE PROCESAN CON BATCH
                ServiceBusMessageBatch batch =
                    await sender.CreateMessageBatchAsync();
                //LOS MENSAJES SE AGREGAN AL BATCH Y SE VAN
                //PROCESANDO
                if (batch.TryAddMessage(colamensajes.Peek()))
                {
                    colamensajes.Dequeue();
                }
                //SE VAN ENVIANDO LOS BATCH
                await sender.SendMessagesAsync(batch);
            }
        }
    }
}
