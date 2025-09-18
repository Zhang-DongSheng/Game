using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class TextManager : Singleton<TextManager>
    {
        private readonly TextTranslate translate = new TextTranslate();

        private readonly TextVoiceConvert convert = new TextVoiceConvert();

        public void Translate(string content, Action<string> callback)
        {
            translate.GetWorld(content, "zh", "en", callback);
        }

        //public void Translate(string source, Action<string> callback)
        //{
        //    translate.GetWorld(source, "zh", "en", callback);
        //}
    }
}