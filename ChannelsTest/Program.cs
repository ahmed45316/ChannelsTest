using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChannelsTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var obj = new ChannelsClass();
            await obj.ChannelSTest();
            Console.ReadLine();
        }
    }
    public class ChannelsClass
    {
        public async Task ChannelSTest()
        {
            var ch = Channel.CreateUnbounded<string>();

            var consumer = Task.Run(async () =>
            {
                while (await ch.Reader.WaitToReadAsync())
                    Console.WriteLine(await ch.Reader.ReadAsync());
            });
            var producer = Task.Run(async () =>
            {
                var address = new Address { NameAr = "ahmed", NameEn = "Mohamed" };
                var output = JsonConvert.SerializeObject(address);
                await ch.Writer.WriteAsync(output);
                ch.Writer.Complete();
            });

            await Task.WhenAll(producer, consumer);
        }
    }
    public class Address
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}
