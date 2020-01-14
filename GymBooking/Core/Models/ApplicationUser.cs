using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBooking.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        //virtual for partial?
        //public virtual ICollection<ApplicationUserGymClass> AttendedClasses { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public DateTime TimeOfRegistration { get; set; }
        public ICollection<ApplicationUserGymClass> AttendedClasses { get; set; }
    }
}
