using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysShopDAO.Model
{
    public class BaseModel
    {
        public byte Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public byte idUsuario { get; set; }

        public BaseModel()
        {

        }

        public BaseModel(byte estado, DateTime fechaRegistro, DateTime fechaActualizacion, byte idUsuario)
        {
            Estado = estado;
            FechaRegistro = fechaRegistro;
            FechaActualizacion = fechaActualizacion;
            this.idUsuario = idUsuario;
        }

        public BaseModel(byte idUsuario )
        {
            this.idUsuario = idUsuario;
        }
    }
}
