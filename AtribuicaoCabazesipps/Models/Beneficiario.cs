//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AtribuicaoCabazesipps.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Beneficiario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int NIF { get; set; }
        public int BI { get; set; }
        public int Telefone { get; set; }
        public int IdFamilia { get; set; }
    
        public virtual Familia Familia { get; set; }
    }
}
