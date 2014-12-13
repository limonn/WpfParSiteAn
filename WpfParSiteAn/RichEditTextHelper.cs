using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Text.RegularExpressions;

namespace WpfParSiteAn
{
    public static class RichEditTextHelper
    {
        public static IList<string> GetAllNonEmptyLines(this RichTextBox textBox) // RichTextBox extension method
        {
            var textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                textBox.Document.ContentStart, 
                // TextPointer to the end of content in the RichTextBox.
                textBox.Document.ContentEnd
            );

            // The Text property on a TextRange object returns a string
            // representing the plain text content of the TextRange.
            try
            {
                IList<string> res = Regex.Split(textRange.Text, "\r\n").Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                return res;
            } catch (Exception e)
            {
                return new List<string>();
            }
        }
    }
}
