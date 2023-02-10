using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ella.Core.Entity
{
    public class Basket : Entity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool Published { get; set; }
        public List<BasketProduct> BasketProducts { get; set; }
    }
    
}
