using Microsoft.Office.Interop.Word;
using System;
using Word = Microsoft.Office.Interop.Word;
using Spire.Doc;
using Spire.Doc.Documents;

namespace BKSOFT.SOCKET
{
    public class FileHelper
    {
        private static readonly string KEY_SPIRE_DOC = "CtOzJs2BlzPokWgBAKMfmNxjRwLa3eqzrAvKtn54UDB/dWjIyGokcs+UQuYuvMY03wX56Ox75KV+U1r5H0PR++c1zc6i8e0QIOVuhMp9Qbg5A9bJJA7e7KvC4KMINTr4jnJy/yTGFwT1aEusw144kml/6oAttwEUoXBkDPLWGOsvNgH1iTYkTGWMXEV8Or4p4t4doNsl0Z7V5qWDKwB6sD/ZiH7l/Jum27FWevOlKIa2VG1rEKjtURYukbWXeSH54IKtmn7nmr0wKwnRgdu3q60aC/PdkxC0zX75EnbU5M6fa3pplU40f3LGOWcgZ2f+8oI7qpPXJ8/s7LrsxBqpQ2YGKfKuqx5ex9ALrXgjnwjcslmXPYun7flHGIkbvBsCjCpo4Ed+M658sZTGATak6gLmftEqhJ1ZZJJKFgXE5qa/TyCY7wIq1ll+z1VNhnSBZUc1RA4TwSBcFKvrZEHlj9o1WFZ1+QqNAcnzh/n+tG48B0wHLCl6D4hroCfWMoaw/23DRxx1WuWqfkazuz2H8ga1RC2XPs83nB7CHPFNs0sT5lsKbfA3P9jgtza5CEhfjAN/3TiwEP/tvnTZY+VABK97veB77h4LEiVMfQXzKfhm9cNW4ft/ofVU2OfqZ8GjtntoZdPxp1bIwTvI98SnQi/H81w19aHwUqNECTeJBjqqHMxdVKVSBAKJL0TM7RyzoOPKS19OfURAxlEgRUqJF/BM8eU0R+UicIM2h36sTuBKO4g3H6woDMlnx0QG0nqthauTB7oK6QFTwk44UQ1kTAu8LeOJwM2xNu5MLsPmoWwDvmIaTuZIW6VUX8C285c9KkrYAf79YKA3e3yxx6SSQdN/jLbtR7MaeGpxRzX0iEbqL9sG1m5USuYVByvVKQ4ntvfCMlLmUN9UCvJ/m63K27Z2dm6fTXIe/g0smYmnvEQ3JQVnldWOi1TKOMK8RbuU5un5mQZ96pLq0Q7g0NLQZh50UMT+OjAzXHPxmXfV6/deHeE8Gbb3ZYJSg7UXW2sty86uXwkj89x5yJTaMNtm6Kh2QQugn/Vd9n8C8QReNewYxjF827FBpMp9yf+vLf2FSyA50wiA9o9luoXYgRmGuUh+g9+KMWgMK5fxQ2h3cHqADzPcwsDhVfG6HuAgt81vH/M5hFLdQztXdvRKVuYOyyTOnQz9K93LZ2EvbeWz0YByRkGxnve+K8UNo3pyNgaPGRQWr5RbeURNJ4PhmM3dB2oMkwE//+s39ccgADdEJS8s35cjRrVEGs8JicRu6mDNqJfdHUNfLmiySMjG/ePwhYkiB2WhJ9AqpY9N7eQ3TBsAMkr34olS6eSNpaE1BjgJsljB27GDnmMAXNZeifyIYpBcqu6H9SLN5pGBF9WHcPVivjdNpMUrKQ==";

        public static bool OfficeExportDocToPDF(string input, string output)
        {
            bool res = true;

            var wordApp = new Microsoft.Office.Interop.Word.Application();

            try
            {
                // Interop requires objects.
                object oMissing = System.Reflection.Missing.Value;
                object isVisible = true;
                object readOnly = true;     // Does not cause any word dialog to show up
                                            // object readOnly = false;  
                                            // Causes a word object dialog to show at the end of the conversion
                object oInput = input;
                object oOutput = output;

                var wordDocument = wordApp.Documents.Open(
                    ref oInput, ref oMissing, ref readOnly, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                    );

                wordDocument.ExportAsFixedFormat(output, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);

                wordDocument.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges,
                                   Microsoft.Office.Interop.Word.WdOriginalFormat.wdOriginalDocumentFormat,
                                   false);  //Close document
            }
            catch (Exception ex)
            {
                res = false;
                GPSFileLog.WriteLog(string.Empty, ex);
            }
            finally
            {
                wordApp.Quit();             //Important: When you forget this Word keeps running in the background}
            }

            return res;
        }

        public static bool OfficeSaveDocToPDF(string input, string output, WdSaveFormat format)
        {
            bool res = true;

            try
            {
                // Create an instance of Word.exe
                _Application oWord = new Word.Application
                {

                    // Make this instance of word invisible (Can still see it in the taskmgr).
                    Visible = false
                };

                // Interop requires objects.
                object oMissing = System.Reflection.Missing.Value;
                object isVisible = true;
                object readOnly = true;     // Does not cause any word dialog to show up
                                            //object readOnly = false;  // Causes a word object dialog to show at the end of the conversion
                object oInput = input;
                object oOutput = output;
                object oFormat = format;

                // Load a document into our instance of word.exe
                _Document oDoc = oWord.Documents.Open(
                    ref oInput, ref oMissing, ref readOnly, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                    );

                // Make this document the active document.
                oDoc.Activate();

                // Save this document using Word
                oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                    );

                // close word doc and word app.
                object saveChanges = WdSaveOptions.wdDoNotSaveChanges;

                oDoc.Close(ref saveChanges, ref oMissing, ref oMissing);

                oWord.Quit(ref oMissing, ref oMissing, ref oMissing);

                // Always close Word.exe.
                //oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            }
            catch (Exception)
            {
                res = false;
            }

            return res;
        }

        public static bool SpireDocToPDF(string input, string output)
        {
            bool res = true;
            try
            {
                Spire.License.LicenseProvider.SetLicenseKey(KEY_SPIRE_DOC);

                //Load Document
                Spire.Doc.Document document = new Spire.Doc.Document();
                document.LoadFromFile(input);

                //Convert Word to PDF
                document.SaveToFile(output, FileFormat.PDF);
            }
            catch (Exception ex)
            {
                res = false;
                GPSFileLog.WriteLog(string.Empty, ex);
            }

            return res;
        }
    }
}
