// See https://aka.ms/new-console-template for more information
using DownloadTheHorny;
using System;
using System.Runtime.CompilerServices;

Console.WriteLine("Hello, World!");

things idk = new things();

while(true)
{
    var asdf = Console.ReadLine();

    switch(asdf.Split(' ')[0])
    {
        case "exit":
            Environment.Exit(0);
            break;
        case "get":
            string uri = null;
            try
            { //expand this out for URI checking etc 
                uri = asdf.Split(' ', 2)[1];
            } catch (Exception ex)
            {
                Console.WriteLine($"Could not find a URL to download...");
                continue;
            }
            //Console.WriteLine(uri); //dry run

            try
            {
                using HttpClient client = new HttpClient();
                byte[] fileBytes = await client.GetByteArrayAsync(uri);

                string fileName = GetFileNameFromUri(uri);
                string fileExtension = Path.GetExtension(fileName);

                string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileExtension.Substring(1));

                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                string filePath = Path.Combine(directoryPath, fileName);
                await File.WriteAllBytesAsync(filePath, fileBytes);

                Console.WriteLine($"File downloaded and saved to {filePath}");
            }
            catch (HttpRequestException ex)
            {
                if(ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    Console.WriteLine("Forbidden, we must find a way! The Horny musn't be stopped~ !");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
    break;
        default:
            Console.WriteLine("idk");
            break;
    }
}

static string GetFileNameFromUri(string uri)
{
    Uri uriObj = new Uri(uri);
    return Path.GetFileName(uriObj.LocalPath);
}