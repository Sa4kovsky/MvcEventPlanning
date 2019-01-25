using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Activity activity, int quantity)
        {
            CartLine line = lineCollection
                .Where(g => g.Activity.ActivityId == activity.ActivityId)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Activity = activity,
                    Quantity = quantity
                });
            }
            else
            {
             line.Quantity += quantity;
            }
        }

        public void RemoveLine(Activity activity)
        {
            lineCollection.RemoveAll(l => l.Activity.ActivityId == activity.ActivityId);
        }

        //public decimal ComputeTotalValue()
        //{
        //   return lineCollection.Sum();
        //}

        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public Activity Activity { get; set; }
        public int Quantity { get; set; }
    }

}
