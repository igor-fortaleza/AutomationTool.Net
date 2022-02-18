using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WebDriver.Options
{
    public class PreferenceOptions
    {
        /// <summary>
        /// Define onde iniciará o driver
        /// </summary>
        public String DriverPath { get; set; }
        /// <summary>
        /// Define o diretório de download do driver
        /// </summary>
        public String DownloadPath { get; set; }
        public Boolean OpenPdfExternally { get; set; }

        /// <summary>
        /// Não abrir pop up ao salvar pdf
        /// </summary>
        public Boolean NeverAskSavePdf { get; set; }
    }
}
