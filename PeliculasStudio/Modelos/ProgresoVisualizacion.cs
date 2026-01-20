using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ProgresoVisualizacion
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public int UsuarioId { get; set; }

    [Indexed]
    public int PeliculaId { get; set; }

    public long TiempoVistoTicks { get; set; } 
    public long DuracionTotalTicks { get; set; }
    public DateTime UltimaVisualizacion { get; set; }

    [Ignore]
    public TimeSpan TiempoVisto
    {
        get => TimeSpan.FromTicks(TiempoVistoTicks);
        set => TiempoVistoTicks = value.Ticks;
    }

    [Ignore]
    public double PorcentajeVisto
    {
        get
        {
            if (DuracionTotalTicks == 0) return 0;
            return (double)TiempoVistoTicks / DuracionTotalTicks;
        }
    }
}
