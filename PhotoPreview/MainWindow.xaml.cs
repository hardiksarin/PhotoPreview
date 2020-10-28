using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Storage;

namespace PhotoPreview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void BtnLoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            //callFunc();
            CreateDir();
            /*OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                imgDynamic.Source = new BitmapImage(fileUri);
                imgDynamic2.Source = new BitmapImage(fileUri);

            }*/
        }
        private void BtnLoadFromResource_Click(object sender, RoutedEventArgs e)
        {
            Uri resourceUri = new Uri("C:\\Users\\sarin\\Documents\\Scented Candels\\FrontLogo.png", UriKind.Absolute);
            imgDynamic.Source = new BitmapImage(resourceUri);
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            MessageBox.Show(list1.SelectedItem.ToString());
        }

        private void callFunc()
        {
            var client = new RestClient($"http://localhost:3000/dev/private/FI/getPreSignedUrlForRetrieval?loan_type=Auto&profile_id=1000111000676767&__loan_id=auyghu&filename=loan.tgz");
            client.Authenticator = new JwtAuthenticator("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbmNyeXB0ZWRQYXlsb2FkIjoid3M3K2w2VTlSNHFRVzYzaTZGQm9JeEx6VWNSSFdYT09SYkxXSzl3czRUVW94d0VsNHZuRVU0M05EWVZ4RkU4YW9PM0RUbnpSRkV5bWl1NUxpMFQ4WGJyNkR3L2ZhbXRKQkxxTGhQM29EcGp0K1JDWnorQWVacFQzcWs5Q0Q0T0I1bDBjSzBaSDAyNmdJK2o1eUNhemtraUhXSzJ3RndpMEFCRXZyeWRoM1ROVVdGcWRlK3pSbjh3aDUwTmgwQWZ1eHlKTEppczNhTWs2Vm01TmtINXJuY0ZLQ3FHcEFBQWNEdmxpZExmQ1dsZz0iLCJpYXQiOjE2MDM3ODQxNjEsImV4cCI6MTYwMzg3MDU2MX0.RMGVzadbXtZLoZw71ClT2Sya_xEUIFlvMUirATr0lL0");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string con = response.Content;
                string a = con.Substring(1);
                string q = a.Split('\"').First();
                bool isDone = GetAttchments(q,"loan");
            }
        }

        public bool GetAttchments(string api, string filename)
        {
            var client = new RestClient(api);
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/octet-stream");
            byte[] response = client.DownloadData(request);
            Directory.CreateDirectory("Doc");
            File.WriteAllBytes($"x.tgz", response);

            int x = 0;

            ExtractTGZ("x.tgz", "Doc");

            /*Directory.CreateDirectory("extracted");
            ZipFile.ExtractToDirectory("x", "extracted");*/



            //ZipFile.ExtractToDirectory("x.zip", "extracted");

            /*using (ZipArchive archive = ZipFile.OpenRead("x.zip"))
            {
                Directory.CreateDirectory("extracted");
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    *//*if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        // Gets the full path to ensure that relative segments are removed.
                        //string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));

                        // Ordinal match is safest, case-sensitive volumes can be mounted within volumes that
                        // are case-insensitive.
                        //if (destinationPath.StartsWith(extractPath, StringComparison.Ordinal))
                        
                    }*//*
                    entry.ExtractToFile("extracted");
                }
            }*/

            return true;
        }

        private void CreateDir()
        {
            Directory.CreateDirectory("CreateZip");
            System.IO.File.Copy("C:\\Users\\ashis\\OneDrive\\Pictures\\try.jpeg", "CreateZip\\try.jpeg", true);
            //File.WriteAllText(System.IO.Path.Combine("CreateZip", "try.jpeg"), "C:\\Users\\ashis\\OneDrive\\Pictures\\try.JPEG");
            //System.IO.Path.Combine("CreateZip", "C:\\Users\\ashis\\OneDrive\\Pictures\\try.jpeg");
            string path = CreateTGZ("CreateZip", "loan", "Extracted");

            InsertFiAttachments();

            callFunc();
        }

        public void ExtractTGZ(String tarFileName, String destFolder)
        {
            Stream inStream = File.OpenRead(tarFileName);

            TarArchive tarArchive = TarArchive.CreateInputTarArchive(inStream, Encoding.ASCII);
            tarArchive.ExtractContents(destFolder);
            tarArchive.Close();

            inStream.Close();
        }

        public string CreateTGZ(string sourceDirectory, string tgzFileName, string targetDirectory, bool deleteSourceDirectoryUponCompletion = false)
        {
            if (!tgzFileName.EndsWith(".tgz"))
            {
                tgzFileName = tgzFileName + ".tgz";
            }
            using (var outStream = File.Create(System.IO.Path.Combine(targetDirectory, tgzFileName)))
            using (var gzoStream = new GZipOutputStream(outStream))
            {
                var tarArchive = TarArchive.CreateOutputTarArchive(gzoStream);

                // Note that the RootPath is currently case sensitive and must be forward slashes e.g. "c:/temp"
                // and must not end with a slash, otherwise cuts off first char of filename
                // This is scheduled for fix in next release
                tarArchive.RootPath = sourceDirectory.Replace('\\', '/');
                if (tarArchive.RootPath.EndsWith("/"))
                {
                    tarArchive.RootPath = tarArchive.RootPath.Remove(tarArchive.RootPath.Length - 1);
                }

                AddDirectoryFilesToTGZ(tarArchive, sourceDirectory);

                if (deleteSourceDirectoryUponCompletion)
                {
                    File.Delete(sourceDirectory);
                }

                var tgzPath = (tarArchive.RootPath + ".tgz").Replace('/', '\\');

                tarArchive.Close();
                return tgzPath;
            }
        }

        private void AddDirectoryFilesToTGZ(TarArchive tarArchive, string sourceDirectory)
        {
            AddDirectoryFilesToTGZ(tarArchive, sourceDirectory, string.Empty);
        }

        private void AddDirectoryFilesToTGZ(TarArchive tarArchive, string sourceDirectory, string currentDirectory)
        {
            var pathToCurrentDirectory = System.IO.Path.Combine(sourceDirectory, currentDirectory);

            // Write each file to the tgz.
            var filePaths = Directory.GetFiles(pathToCurrentDirectory);
            foreach (string filePath in filePaths)
            {
                var tarEntry = TarEntry.CreateEntryFromFile(filePath);

                // Name sets where the file is written. Write it in the same spot it exists in the source directory
                tarEntry.Name = filePath.Replace(sourceDirectory, "");

                // If the Name starts with '\' then an extra folder (with a blank name) will be created, we don't want that.
                if (tarEntry.Name.StartsWith("\\"))
                {
                    tarEntry.Name = tarEntry.Name.Substring(1);
                }
                tarArchive.WriteEntry(tarEntry, true);
            }

            // Write directories to tgz
            var directories = Directory.GetDirectories(pathToCurrentDirectory);
            foreach (string directory in directories)
            {
                AddDirectoryFilesToTGZ(tarArchive, sourceDirectory, directory);
            }
        }

        public bool InsertFiAttachments()
        {
            var client = new RestClient($"http://localhost:3000/dev/private/FI/getPreSignedUrl?loan_type=Auto&profile_id=1000111000676767&__loan_id=auyghu&filename=loan.tgz");
            client.Authenticator = new JwtAuthenticator("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbmNyeXB0ZWRQYXlsb2FkIjoid3M3K2w2VTlSNHFRVzYzaTZGQm9JeEx6VWNSSFdYT09SYkxXSzl3czRUVW94d0VsNHZuRVU0M05EWVZ4RkU4YW9PM0RUbnpSRkV5bWl1NUxpMFQ4WGJyNkR3L2ZhbXRKQkxxTGhQM29EcGp0K1JDWnorQWVacFQzcWs5Q0Q0T0I1bDBjSzBaSDAyNmdJK2o1eUNhemtraUhXSzJ3RndpMEFCRXZyeWRoM1ROVVdGcWRlK3pSbjh3aDUwTmgwQWZ1eHlKTEppczNhTWs2Vm01TmtINXJuY0ZLQ3FHcEFBQWNEdmxpZExmQ1dsZz0iLCJpYXQiOjE2MDM3ODQxNjEsImV4cCI6MTYwMzg3MDU2MX0.RMGVzadbXtZLoZw71ClT2Sya_xEUIFlvMUirATr0lL0");
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "text/plain");
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string con = response.Content;
                string a = con.Substring(1);
                string q = a.Split('\"').First();
                bool isDone = SendAttchments(q,"");
                if (isDone)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool SendAttchments(string api, string filename)
        {
            var client = new RestClient(api);
            var request = new RestRequest(Method.PUT);
            request.AddHeader("content-type", "text/plain");
            request.AddFile("loan", $"Extracted\\loan.tgz");
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
