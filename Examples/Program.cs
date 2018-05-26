﻿using System;
using System.Net;
using System.Threading;

//https://github.com/JamesDunne/aardwolf/blob/master/Aardwolf/HttpAsyncHost.cs#L107

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleListenerExample(new string[] { "http://localhost:8083/", "http://localhost:8084/"});
            Console.WriteLine("Hello World!");
        }

        //https://msdn.microsoft.com/en-us/library/system.net.httplistener(v=vs.110).aspx
        public static void SimpleListenerExample(string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            // for example "http://contoso.com:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            Console.WriteLine("Listening...");

            int _accepts = 0; ;

            _accepts = 4 * Environment.ProcessorCount;

            var sem = new Semaphore(_accepts, _accepts);
            while (true)
            {
                sem.WaitOne();

                listener.GetContextAsync().ContinueWith(async (t)=>
                {
                    string errMessage;
                    try
                    {
                        sem.Release();
                        // Note: The GetContext method blocks while waiting for a request. 
                        HttpListenerContext context = await t; //  listener.GetContext();

                        HttpListenerRequest request = context.Request;
                        // Obtain a response object.
                        HttpListenerResponse response = context.Response;
                        // Construct a response.
                        string responseString = $"<HTML><BODY> Hello world!{DateTime.Now}</BODY></HTML>";
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                        // Get a response stream and write the response to it.
                        response.ContentLength64 = buffer.Length;
                        System.IO.Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        // You must close the output stream.
                        output.Close();
                        return;
                    }
                    catch (Exception ex)
                    {
                        errMessage = ex.ToString();
                    }
                    await Console.Error.WriteLineAsync(errMessage);

                });

               
            }

            listener.Stop();
        }
    }
}
