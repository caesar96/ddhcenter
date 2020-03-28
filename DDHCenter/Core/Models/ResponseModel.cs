using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDHCenter.Core.Models
{
    public class ResponseModel
    {
        public bool Error { get; set; }
        public string Message { get; set; }

        public string Token { get; set; }

        public Socio Socio { get; set; }

        public Categoria Categoria { get; set; }

        public Medicamento Medicamento { get; set; }

        public List<Categoria> Categorias { get; set; }

        public List<Medicamento> Medicamentos { get; set; }

        public Cliente Cliente { get; set; }
    }
}
