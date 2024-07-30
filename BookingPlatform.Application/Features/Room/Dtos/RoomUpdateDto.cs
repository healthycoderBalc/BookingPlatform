using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Application.Features.Room.Dtos
{
    public class RoomUpdateDto
    {
        public string RoomNumber { get; set; }
        public int AdultCapacity { get; set; }
        public int ChildrenCapacity { get; set; }
        public bool IsOperational { get; set; }
    }
}
