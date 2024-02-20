using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotHome.Models
{
    public class Session
    {
        public string LanguageCode { get; set; }
        public bool IsCountingCharacters { get; set; }
        public bool IsCalculatingSum { get; set; }
    }
}
