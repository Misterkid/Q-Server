using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QServer
{
    class QList<T>:List<T>
    {
        public void Remove(T item)
        {
            if(item == null)
            {
                RemoveAll(itm => itm == null);
                return;
            }
            base.Remove(item);
        }
    }
}
