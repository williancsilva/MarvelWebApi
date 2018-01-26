using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarvelWebApi.Models
{
    public class Personagem
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string UrlImagem { get; set; }
        public string UrlWiki { get; set; }
        public string MsgErro { get; set; }
    }
}