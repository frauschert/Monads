using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monads;

namespace MonadsTest
{
    internal interface IConfiguration
    {
        string Get(string key);
    }
    internal class ReaderTests
    {
        [Test]
        public void Test1()
        {
            Reader<Uri, string> reader = (uri) => uri.AbsolutePath;
            var result = reader(new Uri("https://example.com/path/to/resource"));
            Assert.That(result, Is.EqualTo("/path/to/resource")); // Check the result of the reader.
        }

        private static Reader<IConfiguration, FileInfo> DownloadHtml(Uri uri) =>
            configuration => default;

        private static Reader<IConfiguration, FileInfo> ConverToWord(FileInfo htmlDocument, FileInfo template) =>
            configuration => default;

        private static Reader<IConfiguration, Unit> UploadToOneDrive(FileInfo file) =>
            configuration => default;

        internal static void Workflow(IConfiguration configuration, Uri uri, FileInfo template)
        {
            Reader<IConfiguration, (FileInfo, FileInfo)> query =
                from htmlDocument in DownloadHtml(uri) // Reader<IConfiguration, FileInfo>.
                from wordDocument in ConverToWord(htmlDocument, template) // Reader<IConfiguration, FileInfo>.
                from unit in UploadToOneDrive(wordDocument) // Reader<IConfiguration, Unit>.
                select (htmlDocument, wordDocument); // Define query.
            (FileInfo, FileInfo) result = query(configuration); // Execute query.
        }

    }
}
