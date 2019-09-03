using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ImportacaoGPX
{
    class Corrida
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Inicio { get; set; }

        [InverseProperty("Corrida")]
        public ICollection<Track> Tracks { get; set; }
    }
}
