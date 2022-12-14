
using IDCH.Core;
using Spectre.Console;
using System.Drawing;
using System.Text;
using System.Text.Json;
using Color = System.Drawing.Color;
using Console = Colorful.Console;

class Program
{
    static CloudApi _api;
    static bool runForever = true;
    static void Main(string[] args)
    {

        RunCli();
        while (runForever)
        {
            Thread.Sleep(100);
        }
    }

    static async void RunCli()
    {
        Welcome();
        await InitializeClient();

        
        while (runForever)
        {
            string cmd = InputString("Command [? for help]:", null, false);
            switch (cmd)
            {
                case "?":
                    Menu();
                    break;
                case "q":
                    runForever = false;
                    
                    break;
                case "c":
                case "cls":
                case "clear":
                    Console.Clear();
                    break;

                case "auth-modify":
                     await ModifyProfile();
                    break;
                case "token-list":
                     await ListToken();
                    break;
                case "location-list":
                     await ListLocations();
                    break;
                case "vm-list":
                     await ListVM();
                    break;
                case "ms-list":
                    await ListPackage();
                    break;
                case "get":
                    //ReadBlob();
                    break;
                case "get stream":
                    //ReadBlobStream();
                    break;
                case "write":
                    //WriteBlob();
                    break;
                case "del":
                    //DeleteBlob();
                    break;
                case "upload":
                    //UploadBlob();
                    break;
                case "download":
                    //DownloadBlob();
                    break;
                case "exists":
                    //BlobExists();
                    break;
                case "md":
                    //BlobMetadata();
                    break;
                case "enum":
                    //Enumerate();
                    break;
                case "enumpfx":
                    //EnumeratePrefix();
                    break;
                case "url":
                    //GenerateUrl();
                    break;
            }
        }
    }

    static void Welcome()
    {
        int DA = 244;
        int V = 212;
        int ID = 255;
        Console.WriteAscii("Welcome to", Color.FromArgb(DA, V, ID));

        DA -= 18;
        V -= 36;

        Console.WriteAscii("IDCH CLI", Color.FromArgb(DA, V, ID));

        DA -= 18;
        V -= 36;
        
        Console.WriteAscii("Cloud", Color.FromArgb(DA, V, ID));       

    }
    static async Task InitializeClient()
    {
    reauth:
        Console.WriteLine("API-KEY, eg: dJMFzG4YmZv2wQxxxxx");
        string apikey = InputString("ApiKey   :", "", false);

        if (!String.IsNullOrEmpty(apikey))
        {
            _api = new CloudApi(apikey);
            var user = await _api.Auth.GetUserInfo();
            if (user == null)
            {
                Console.WriteLine("ApiKey is invalid, try again");
                goto reauth;

            }
            else
            {
                Console.WriteLine($"Welcome to IDCH Cloud CLI, {user.name}. Today is {DateTime.Now.ToString("dddd, dd MM yyyy HH:mm")}");
            }
        }
    }

    static string InputString(string question, string defaultAnswer, bool allowNull)
    {
        while (true)
        {
            Console.Write(question);

            if (!String.IsNullOrEmpty(defaultAnswer))
            {
                Console.Write(" [" + defaultAnswer + "]");
            }

            Console.Write(" ");

            string userInput = Console.ReadLine();

            if (String.IsNullOrEmpty(userInput))
            {
                if (!String.IsNullOrEmpty(defaultAnswer)) return defaultAnswer;
                if (allowNull) return null;
                else continue;
            }

            return userInput;
        }
    }

    static bool InputBoolean(string question, bool yesDefault)
    {
        Console.Write(question);

        if (yesDefault) Console.Write(" [Y/n]? ");
        else Console.Write(" [y/N]? ");

        string userInput = Console.ReadLine();

        if (String.IsNullOrEmpty(userInput))
        {
            if (yesDefault) return true;
            return false;
        }

        userInput = userInput.ToLower();

        if (yesDefault)
        {
            if (
                (String.Compare(userInput, "n") == 0)
                || (String.Compare(userInput, "no") == 0)
               )
            {
                return false;
            }

            return true;
        }
        else
        {
            if (
                (String.Compare(userInput, "y") == 0)
                || (String.Compare(userInput, "yes") == 0)
               )
            {
                return true;
            }

            return false;
        }
    }
    static async Task ListVM()
    {
        try
        {
            var slug = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
           .Title("Where is your vm location ?")
           .AddChoices(new string[] { "jkt01","jkt02","sgp01" }));
            Console.WriteLine("");
            AnsiConsole.WriteLine("List VM:");
            var vms = await _api.VM.GetVMList(slug);
            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn(new TableColumn("uuid").Centered());
            table.AddColumn(new TableColumn("hostname").Centered());
            table.AddColumn(new TableColumn("status").Centered());
            table.AddColumn(new TableColumn("memory").Centered());
            table.AddColumn(new TableColumn("storage").Centered());
            table.AddColumn(new TableColumn("os").Centered());

            foreach (var vm in vms)
            {
                var storageStr = string.Empty;
                vm.storage.ToList().ForEach(x => storageStr += $"uuid:{x.uuid}, size:{x.size.ToString("n0")}, name:{x.name}|");
                table.AddRow(vm.uuid, vm.hostname, vm.status, vm.memory.ToString("n0"), storageStr, $"{vm.os_name} {vm.os_version}");
            }


            // Render the table to the console
            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine($"Error: {ex.ToString()}");

        }


    }

    static async Task ListPackage()
    {
        try
        {
            Console.WriteLine("");
            AnsiConsole.WriteLine("List Service Package:");
            var package = await _api.ManagedServices.GetListPackage();
            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn(new TableColumn("uuid").Centered());
            table.AddColumn(new TableColumn("displayname").Centered());
            table.AddColumn(new TableColumn("service").Centered());
            table.AddColumn(new TableColumn("status").Centered());
            table.AddColumn(new TableColumn("version").Centered());
            table.AddColumn(new TableColumn("price").Centered());

            foreach (var item in package)
            {
                var priceStr = string.Empty;
                item.prices.ToList().ForEach(x => priceStr += $"{x.resourceType} price mult:{x.priceMultiplier.ToString("n0")}|");
                table.AddRow(item.uuid, item.display_name, item.service, item.status, item.version,priceStr);
            }


            // Render the table to the console
            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine($"Error: {ex.ToString()}");

        }


    }
    static async Task ListLocations()
    {
        try
        {
            Console.WriteLine("");
            AnsiConsole.WriteLine("List Location:");
            var tokens = await _api.Locations.GetLocations();
            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn(new TableColumn("display_name").Centered());
            table.AddColumn(new TableColumn("is_default").Centered());
            table.AddColumn(new TableColumn("is_preferred").Centered());
            table.AddColumn(new TableColumn("description").Centered());
            table.AddColumn(new TableColumn("order_nr").Centered());
            table.AddColumn(new TableColumn("slug").Centered());
            table.AddColumn(new TableColumn("country_code").Centered());

            foreach (var item in tokens)
            {
                table.AddRow(item.display_name, item.is_default.ToString(), item.is_preferred.ToString(), item.description, item.order_nr.ToString(),item.slug,item.country_code );
            }


            // Render the table to the console
            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine($"Error: {ex.ToString()}");

        }


    }
    static async Task ListToken()
    {
        try
        {
            Console.WriteLine("");
            AnsiConsole.WriteLine("List Token:");
            var tokens = await _api.Token.GetListToken();
            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn(new TableColumn("billingid").Centered());
            table.AddColumn(new TableColumn("consumerid").Centered());
            table.AddColumn(new TableColumn("created").Centered());
            table.AddColumn(new TableColumn("desc").Centered());
            table.AddColumn(new TableColumn("id").Centered());
            table.AddColumn(new TableColumn("kongid").Centered());
            table.AddColumn(new TableColumn("restricted").Centered());
            table.AddColumn(new TableColumn("token").Centered());
            table.AddColumn(new TableColumn("updated").Centered());

            foreach (var item in tokens)
            {
                table.AddRow(item.billing_account_id.ToString(),item.consumer_id,item.created_at,item.description,item.id.ToString(),item.kong_id,item.restricted.ToString(),item.token,item.updated_at??"-");
            }


            // Render the table to the console
            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine($"Error: {ex.ToString()}");

        }


    }

    static async Task ModifyProfile()
    {
        try
        {
            Console.WriteLine("");
            AnsiConsole.WriteLine("Modify User Profile:");
            var firstname = AnsiConsole.Ask<string>("What's your [green]first name[/]?");
            var lastname = AnsiConsole.Ask<string>("What's your [green]last name[/]?");
            var phonenumber = AnsiConsole.Ask<string>("What's your [green]phone number[/]?");
            var personalid = AnsiConsole.Ask<string>("What's your [green]personal id[/]?");
            var res = await _api.Auth.ModifyUserProfile(firstname,lastname,phonenumber,personalid);
            if(res!=null)
            // Render the table to the console
            AnsiConsole.WriteLine($"updated >> name: {res.first_name} {res.last_name}, ava: {res.avatar}, mail:{res.email}, id: {res.personal_id_number}, phone: {res.phone_number}");
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine($"Error: {ex.ToString()}");

        }


    }
    static void Menu()
    {
        Console.WriteLine("");
        AnsiConsole.WriteLine("Available commands:");

        // Create a table
        var table = new Table();

        // Add some columns
        table.AddColumn("No");
        table.AddColumn(new TableColumn("Category").Centered());
        table.AddColumn(new TableColumn("Command").Centered());
        table.AddColumn(new TableColumn("Description").Centered());

        // Add some rows
        table.AddRow("1", "global" ,"[green]?[/]","Help, this Menu");
        table.AddRow("2", "global", "[green]cls[/]", "Clear the screen");
        table.AddRow("3", "global", "[green]q[/]", "Quit");
        table.AddRow("4", "VM", "[green]vm-list[/]", "List of VMs");
        table.AddRow("5", "Managed Service", "[green]ms-list[/]", "List of Managed Service Package");
        table.AddRow("6", "Auth", "[green]auth-modify[/]", "Modify user profile");
        table.AddRow("7", "Token", "[green]token-list[/]", "List token");
        table.AddRow("8", "Locations", "[green]location-list[/]", "List DC location");

        // Render the table to the console
        AnsiConsole.Write(table);
        /*
        Console.WriteLine("");
        Console.WriteLine("Available commands:");
        Console.WriteLine("  ?            Help, this menu");
        Console.WriteLine("  cls          Clear the screen");
        Console.WriteLine("  q            Quit");
        Console.WriteLine("  get          Get a BLOB");
        Console.WriteLine("  get stream   Get a BLOB using stream");
        Console.WriteLine("  write        Write a BLOB");
        Console.WriteLine("  del          Delete a BLOB");
        Console.WriteLine("  upload       Upload a BLOB from a file");
        Console.WriteLine("  download     Download a BLOB from a file");
        Console.WriteLine("  exists       Check if a BLOB exists");
        Console.WriteLine("  md           Retrieve BLOB metadata");
        Console.WriteLine("  enum         Enumerate a bucket");
        Console.WriteLine("  enumpfx      Enumerate a bucket by object prefix");
        Console.WriteLine("  url          Generate a URL for an object by key");
        Console.WriteLine("");*/
    }
    /*
    static void WriteBlob()
    {
        try
        {
            string key = InputString("Key          :", null, false);
            string contentType = InputString("Content type :", "text/plain", true);
            string data = InputString("Data         :", null, true);

            byte[] bytes = new byte[0];
            if (!String.IsNullOrEmpty(data)) bytes = Encoding.UTF8.GetBytes(data);
            _Blobs.Write(key, contentType, bytes).Wait();

            Console.WriteLine("Success");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    static void ReadBlob()
    {
        byte[] data = _Blobs.Get(InputString("Key:", null, false)).Result;
        if (data != null && data.Length > 0)
        {
            Console.WriteLine(Encoding.UTF8.GetString(data));
        }
    }

    static void ReadBlobStream()
    {
        BlobData data = _Blobs.GetStream(InputString("Key:", null, false)).Result;
        if (data != null)
        {
            Console.WriteLine("Length: " + data.ContentLength);
            if (data.Data != null && data.Data.CanRead && data.ContentLength > 0)
            {
                byte[] bytes = ReadToEnd(data.Data);
                Console.WriteLine(Encoding.UTF8.GetString(bytes));
            }
        }
    }

    static void DeleteBlob()
    {
        _Blobs.Delete(InputString("Key:", null, false)).Wait();
    }

    static void BlobExists()
    {
        Console.WriteLine(_Blobs.Exists(InputString("Key:", null, false)).Result);
    }

    static void UploadBlob()
    {
        string filename = InputString("Filename     :", null, false);
        string key = InputString("Key          :", null, false);
        string contentType = InputString("Content type :", null, true);

        FileInfo fi = new FileInfo(filename);
        long contentLength = fi.Length;

        using (FileStream fs = new FileStream(filename, FileMode.Open))
        {
            _Blobs.Write(key, contentType, contentLength, fs).Wait();
        }

        Console.WriteLine("Success");
    }

    static void DownloadBlob()
    {
        string key = InputString("Key      :", null, false);
        string filename = InputString("Filename :", null, false);

        BlobData blob = _Blobs.GetStream(key).Result;
        using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
        {
            int bytesRead = 0;
            long bytesRemaining = blob.ContentLength;
            byte[] buffer = new byte[65536];

            while (bytesRemaining > 0)
            {
                bytesRead = blob.Data.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    fs.Write(buffer, 0, bytesRead);
                    bytesRemaining -= bytesRead;
                }
            }
        }

        Console.WriteLine("Success");
    }

    static void BlobMetadata()
    {
        BlobMetadata md = _Blobs.GetMetadata(InputString("Key:", null, false)).Result;
        Console.WriteLine("");
        Console.WriteLine(md.ToString());
    }

    static void Enumerate()
    {
        EnumerationResult result = _Blobs.Enumerate(InputString("Token (left empty):", null, true)).Result;

        Console.WriteLine("");
        if (result.Blobs != null && result.Blobs.Count > 0)
        {
            foreach (BlobMetadata curr in result.Blobs)
            {
                Console.WriteLine(
                    String.Format("{0,-27}", curr.Key) +
                    String.Format("{0,-18}", curr.ContentLength.ToString() + " bytes") +
                    String.Format("{0,-30}", curr.CreatedUtc.Value.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        else
        {
            Console.WriteLine("(none)");
        }

        if (!String.IsNullOrEmpty(result.NextContinuationToken))
            Console.WriteLine("Continuation token: " + result.NextContinuationToken);

        Console.WriteLine("");
        Console.WriteLine("Count: " + result.Count);
        Console.WriteLine("Bytes: " + result.Bytes);
        Console.WriteLine("");
    }

    static void EnumeratePrefix()
    {
        EnumerationResult result = _Blobs.Enumerate(
            InputString("Prefix :", null, true),
            InputString("Token  :", null, true)).Result;

        if (result.Blobs != null && result.Blobs.Count > 0)
        {
            foreach (BlobMetadata curr in result.Blobs)
            {
                Console.WriteLine(
                    String.Format("{0,-27}", curr.Key) +
                    String.Format("{0,-18}", curr.ContentLength.ToString() + " bytes") +
                    String.Format("{0,-30}", curr.CreatedUtc.Value.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        else
        {
            Console.WriteLine("(none)");
        }

        if (!String.IsNullOrEmpty(result.NextContinuationToken))
            Console.WriteLine("Continuation token: " + result.NextContinuationToken);

        Console.WriteLine("");
        Console.WriteLine("Count: " + result.Count);
        Console.WriteLine("Bytes: " + result.Bytes);
        Console.WriteLine("");
    }

    static void GenerateUrl()
    {
        Console.WriteLine(_Blobs.GenerateUrl(
            InputString("Key:", "hello.txt", false)));
    }

    private static byte[] ReadToEnd(Stream stream)
    {
        long originalPosition = 0;

        if (stream.CanSeek)
        {
            originalPosition = stream.Position;
            stream.Position = 0;
        }

        try
        {
            byte[] readBuffer = new byte[4096];

            int totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;

                if (totalBytesRead == readBuffer.Length)
                {
                    int nextByte = stream.ReadByte();
                    if (nextByte != -1)
                    {
                        byte[] temp = new byte[readBuffer.Length * 2];
                        Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                        Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                        readBuffer = temp;
                        totalBytesRead++;
                    }
                }
            }

            byte[] buffer = readBuffer;
            if (readBuffer.Length != totalBytesRead)
            {
                buffer = new byte[totalBytesRead];
                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
            }
            return buffer;
        }
        finally
        {
            if (stream.CanSeek)
            {
                stream.Position = originalPosition;
            }
        }
    }
    */
}