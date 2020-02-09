namespace GenC.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Agendamentos
    {
        private DateTime _setDate = DateTime.Now;
        public int Id { get; set; }

        public int? IdUsuario { get; set; }

        [Column(TypeName = "date")]
        [Required]
        [Display(Name= "Data do evento")]
        [DataType(DataType.DateTime)]
        public DateTime DtAgendamento { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.DateTime)]
        public DateTime DtCriacao
        {
            get { return _setDate; }
            set { _setDate = value; }
        }
        [StringLength(100)]
        public string Endereco { get; set; }

        [Required]
        [StringLength(10)]
        [MinLength(8)]
        public string CEP { get; set; }

        [StringLength(50)]
        public string Cidade { get; set; }

        [StringLength(50)]
        public string Estado { get; set; }

        [Required]
        [StringLength(30)]
        [MinLength(10)]
        public string Titulo { get; set; }

        [StringLength(255)]
        public string Descricao { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}
