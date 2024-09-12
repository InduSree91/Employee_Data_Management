using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pet_Project_Backend.Utilities
{
    public class EntityData<T> : Responses
    {
        public T Data {  get; set; }
    }
}
