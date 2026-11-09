using System;
using System.IO;
using System.Net;

class Program
{
	static void Main(string[] args)
	{
		string prefix = "http://localhost:8000/";
		HttpListener listener = new HttpListener();
		listener.Prefix.Add(prefix);
		listener.Start();
		Console.WriteLine($"Local 3D Server Running At {prefix} (Offline Ready)");
		
		while true
		{	
			HttpListenerContext context = listener.GetContext();
			HttpListenerRequest request = context.Request;
			HttpListenerResponse responce = context.Response;

			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "model", request.Url.AbsolutePath.TrimStart('/'));

			if (File.Exist(filePath))
			{
				byte[] buffer = File.ReadAllBytes(filePath);
				response.ContentLength64 = buffer.Length;
				response.ContentType = 'model/gltf-bianry";
				response.OutputStream.Write(buffer, 0, buffer.Length);
			}
			else
			{
				responce.StatusCode = (int)HttpStatusCode.NotFound;
			}
			response.OutputStream.Close();
		}
	}
}
