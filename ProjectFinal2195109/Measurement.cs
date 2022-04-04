using System;
using System.Collections.Generic;

namespace ProjectFinal2195109
{
    public partial class Measurement
    {
        public Measurement()
        {
            Quantities = new HashSet<Quantity>();
        }

        public int MeasurementId { get; set; }
        public string MeasurementUnit { get; set; } = null!;

        public virtual ICollection<Quantity> Quantities { get; set; }
    }
}
