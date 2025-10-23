using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CasaDeCambio.Models;

public partial class CambiosDivisa
{
    [Key]
    public int IdCambio { get; set; }

    public int IdPersona { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal MontoQuetzales { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal MontoDolares { get; set; }

    [Precision(0)]
    public DateTime FechaHoraCambio { get; set; }

    [ForeignKey("IdPersona")]
    [InverseProperty("CambiosDivisas")]
    public virtual Persona IdPersonaNavigation { get; set; } = null!;
}
