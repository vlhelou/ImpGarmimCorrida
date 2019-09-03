using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImportacaoGPX
{

    class Track
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int IdCorrida { get; set; }
        public DateTime Hora { get; set; }
        public float? Elevacao { get; set; }
        public int? Batimento { get; set; }
        public int? CadenciaPasso { get; set; }
        public float? lat { get; set; }
        public float? lon { get; set; }

        [ForeignKey("IdCorrida")]
        public Corrida Corrida { get; set; }
    }
}
