using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CasaDeCambio.Models;

[Index("DPI", Name = "UQ_Personas_DPI", IsUnique = true)]
public partial class Persona
{
    [Key]
    public int IdPersona { get; set; }

    [StringLength(120)]
    public string Nombre { get; set; } = null!;

    public int DPI { get; set; }

    [InverseProperty("IdPersonaNavigation")]
    public virtual ICollection<CambiosDivisa> CambiosDivisas { get; set; } = new List<CambiosDivisa>();
}
