using System;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;


namespace Twilio
{


    public class Message
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
    }

    class Program
    {
        //        static void Main(string[] args)
        //        {
        //            //1
        //            var client = new RestClient("https://api.twilio.com/2010-04-01");
        //            //2
        //            var request = new RestRequest("Accounts/ACb7801c7dfea5c728f5c527cda72cfaee/Messages", Method.POST);
        //            //3
        //            request.AddParameter("To", "+12063905383");
        //            request.AddParameter("From", "+12069008523");
        //            request.AddParameter("Body", "Hello world!");
        //            //4
        //            client.Authenticator = new HttpBasicAuthenticator("ACb7801c7dfea5c728f5c527cda72cfaee", "888e04545f08af3379f9fbc51c019441");
        //            //5
        //            client.ExecuteAsync(request, response =>
        //            {
        //                Console.WriteLine(response);
        //            });
        //            Console.ReadLine();
        //        }
        //    }
        //}


        static void Main(string[] args)
        {
            var client = new RestClient("https://api.twilio.com/2010-04-01");
            //1
            var request = new RestRequest("Accounts/ACb7801c7dfea5c728f5c527cda72cfaee/Messages.json", Method.GET);
            client.Authenticator = new HttpBasicAuthenticator("ACb7801c7dfea5c728f5c527cda72cfaee", "888e04545f08af3379f9fbc51c019441");
            //2
            var response = new RestResponse();
            //3a
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            //4
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            List<Message> messageList = JsonConvert.DeserializeObject<List<Message>>(jsonResponse["messages"].ToString());
            foreach (Message message in messageList)
            {
                Console.WriteLine("To: {0}", message.To);
                Console.WriteLine("From: {0}", message.From);
                Console.WriteLine("Body: {0}", message.Body);
                Console.WriteLine("Status: {0}", message.Status);
            }
            Console.ReadLine();
        }


        //3b
        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    
    }

}