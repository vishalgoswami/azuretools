using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBloblUploader
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new BlobManager("CloudStorage");

            //var success = manager.UploadDirectory(@"C:\Users\devadmina\Desktop\outsideframeworks", "cdn","outsideframeworks","");
            //var success = manager.UploadDirectory(@"C:\Users\devadmina\Desktop\sourcesubscription", "cdn", "sourcesubscription", "");
            //var success = manager.UploadDirectory(@"C:\temp\cdn", "cdn", "trueiqresources", "");


            //var result = true;
            //No need to call everytime you upload blob. it is at container level.
            var result = manager.ConfigureCors();
            //Console.WriteLine(success + "|" + result);
            Console.ReadKey();
        }
    }
}
