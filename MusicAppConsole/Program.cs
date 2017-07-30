using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using MusicApp.Domain.Infrastructure;
using MusicApp.Domain.Service;

namespace MusicAppConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                Program p = new Program();
                p.DoWork().Wait();
            }
            catch (DocumentClientException dce)
            {
                Exception baseException = dce.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", dce.StatusCode, dce.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }
        }

        private async Task DoWork()
        {
            DocumentClientFactory clientFactory = new DocumentClientFactory();
            MusicService musicService = new MusicService(clientFactory);
            await musicService.InitializeStorage();

            UserService userService = new UserService(clientFactory);

            var user1 = userService.CreateNew();
            user1.Id = new Guid("0e54b6da-caef-492f-bc95-b0669a8957d5").ToString();
            user1.UserName = "jack.black@gmail.com";
            user1.FirstName = "Jack";
            user1.LastName = "Blackie";

            await userService.AddOrUpdate(user1);

            var user2 = userService.CreateNew();
            user2.Id = new Guid("d13c91fd-f8df-4470-ae30-bb6ab5797438").ToString();
            user2.UserName = "frank.conrad@gmail.com";
            user2.FirstName = "Frank";
            user2.LastName = "Conrad";

            await userService.AddOrUpdate(user2);

            var users = await userService.GetByUsername("jack.black@gmail.com");

            foreach(var user in users)
            {
                Console.Write("\t Read {0}", user);
            }
            Console.ReadKey();

            user1.FirstName = "Jackie";
            await userService.Update(user1);

            users = await userService.GetByUsername("jack.black@gmail.com");

            foreach (var user in users)
            {
                Console.Write("\t Read {0}", user);
            }
            Console.ReadKey();

            await userService.Delete(user2);

            ArtistService artistService = new ArtistService(clientFactory);

            var artist = artistService.CreateNew(user1);
            artist.Id = "02ab6b39-4d53-45f4-8922-d118d8e27ad2";
            artist.Name = "Muse";

            await artistService.AddOrUpdate(artist);

            artist = await artistService.GetById(artist.Id);
            Console.Write("\t Read {0}", artist);
        }

        // ADD THIS PART TO YOUR CODE
        private void WriteToConsoleAndPromptToContinue(string format, params object[] args)
        {
            Console.WriteLine(format, args);
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }
    }
}
