using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Economy.BusinessObjects
{
    public class Good : ICloneable, IEquatable<Good>
    {
        public string InternalName { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }
        public int DaysToProduce { get; set; }

        public object Clone()
        {
            return new Good()
            {
                InternalName = this.InternalName,
                Value = this.Value,
                Name = this.Name,
                DaysToProduce = this.DaysToProduce
            };
        }

        public bool Equals(Good other)
        {
            return this.InternalName.Equals(other.InternalName, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
