using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace Topics.Util
{
    class TextBlockChanger
    {
        public static void Do(object textBlockObject)
        {
            TextBlock textBlock = textBlockObject as TextBlock;

            if(textBlock.Text.Contains("i"))
            {
                int index = textBlock.Text.IndexOf('i');
                textBlock.Text = textBlock.Text.Remove(index, 1);
                textBlock.Text = textBlock.Text.Insert(index, "!");

                string[] tmp = textBlock.Text.Split('!');

                textBlock.Inlines.Clear();

                Run run = new Run();
                run.Text = tmp[0];
                textBlock.Inlines.Add(run);

                run = new Run();
                run.Text = "!";
                run.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xf2, 0x68, 0x22));
                textBlock.Inlines.Add(run);

                run = new Run();
                run.Text = tmp[1];
                textBlock.Inlines.Add(run);
            }
        }
    }
}
