using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ella.Core.Entity
{
    public class BasketProduct : Entity
    {
        public int Id { get; set; }
        public bool Published { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int BasketId { get; set; }
        public Basket Basket { get; set; }
        public int Count { get; set; }  
    }
}
