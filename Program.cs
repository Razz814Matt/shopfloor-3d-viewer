using System;
using System.Collections;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;

class Program
{
	static void Main(string[] args)
	{
		string prefix = "http://localhost:8000/";
		HttpListener listener = new HttpListener();
		listener.Prefix.Add(prefix);
		listener.Start();

		Console.WriteLine($"[ShopFloor3D] Local 3D Server Running At {prefix}");
		Console.WriteLine("[ShopFloor3D] Press Ctrl+C to stop");

		string wwwroot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot"));

		while (true)
		{	
			try
			{

				var context = listener.GetContext();
				var request = context.Request;
				var responce = context.Response;

				string rawPath = request.Url.AbsolutePath.TrimStart('/');
				if (string.IsNullOrEmpty(rawPath))
				{
					rawPath = "index.html";
				}

				string filePath = Path.GetFullPath(Path.Combine(wwwroot, rawPath));

				if (filePath.StartsWith(wwwroot, StringComparison.OrdinalIgnoreCase) && File.Exists(filePath))
				{
					byte[] buffer = File.ReadAllBytes(filePath);
					response.ContentLength64 = buffer.Length;
					response.ContentType = GetMimeType(filePath);
					responce.StatusCode = (int)HttpStatusCode.OK;
					response.OutputStream.Write(buffer, 0, buffer.Length);
				}
				else
				{
					responce.StatusCode = (int)HttpStatusCode.NotFound;
					byte[] errorBytes = Encoding.UTF8.GetBytes("<h1>404 - File Not Found</h1>");
					responce.ContentLength64 = errorBytes.Length;
					responce.OutputStream.Write(errorBytes, 0, errorBytes.Length);
				}
				response.OutputStream.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error Handeling Request: {ex.Message}");
			}
		}
		static string GetMimeType(string path)
		{
			string ext = Path.GetExtension(path).ToLowerInvariant();
			switch (ext) 
			{
				case ".html": 
					return "text/hmtl; charset=utf-8";
				case ".js":   
					return "application/javascript; charset=utf-8";
				case ".css":  
					return "text/css; charset=utf-8";
				case ".glb":  
					return "model/gltf-binary;";
				case ".gltf": 
					return "model/gltf+json";
				case ".png":  
					return "image/png";
				case ".jpg":  
					return "image/jpg";
				case ".json": 
					return "application/json";
				defualt:
			  	    return "application/octet-stream";	
			};
		}
	}
}	
