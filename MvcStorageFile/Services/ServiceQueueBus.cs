using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvcStorage.Extensions;
using Microsoft.Extensions.Configuration;

namespace MvcStorage.Services
{
    public class ServiceQueueBus
    {
        private ServiceBusClient client;
        private IServiceProvider serviceprovider;
        private List<String> messages; 
        public ServiceQueueBus(IConfiguration configuration
            , IServiceProvider serviceprovider)
        {
            String keys = configuration["ServiceBusKey"];
            this.client = new ServiceBusClient(keys);
            this.serviceprovider = serviceprovider;
        }

        public async Task SendMessage(String data)
        {
            //NECESITAMOS UN SENDER QUE ES EL QUE VA
            //ASOCIADO A LA QUEUE
            ServiceBusSender sender = 
                this.client.CreateSender("programeitors");
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

        //LOS MENSAJES DE DESCARGAN Y SON PROCESADOS POR AZURE
        //NO PUEDO BUSCAR UN MENSAJE, ESTO ES UNA COLA
        //SI HAY 230, LOS VA DESCARGANDO UNO A UNO
        public async Task ReceiveMessages()
        {
            //NECESITAMOS UN PROCESADOR DE MENSAJES
            ServiceBusProcessor processor =
                this.client.CreateProcessor("programeitors");
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += Processor_ProcessErrorAsync;
            //INICIA EL PROCESO DE RECUPERAR LOS MENSAJES
            await processor.StartProcessingAsync();
            //Thread.Sleep(30000);
            //FINALIZA EL PROCESO
            //await processor.StopProcessingAsync();
        }

        private Task Processor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            return Task.CompletedTask;
        }

        private async Task MessageHandler(ProcessMessageEventArgs e)
        {
            String data = e.Message.Body.ToString();
            IHttpContextAccessor context = (IHttpContextAccessor)
                this.serviceprovider.GetService(typeof(IHttpContextAccessor));
            
            if (context.HttpContext.Session.GetObject<List<String>>("MESSAGES") != null)
            {
                this.messages =
                    context.HttpContext.Session.GetObject<List<String>>("MESSAGES");
            }
            else
            {
                this.messages = new List<string>();
            }
            this.messages.Add(data);
            context.HttpContext.Session
                .SetObject<List<String>>("MESSAGES", this.messages);
            //DEBEMOS IR ELIMINADO LOS MENSAJES DE LA COLA
            //COMO PROCESADOS
            await e.CompleteMessageAsync(e.Message);
        }
    }
}
