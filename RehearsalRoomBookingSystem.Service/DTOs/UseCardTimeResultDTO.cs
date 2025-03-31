using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RehearsalRoomBookingSystem.Service.DTOs
{
    public class UseCardTimeResultDTO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UseCardTimeResultDTO"/> class.
        /// </summary>
        public UseCardTimeResultDTO()
        {
            Message = string.Empty;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public int RemainingHours { get; set; }
    }
}
