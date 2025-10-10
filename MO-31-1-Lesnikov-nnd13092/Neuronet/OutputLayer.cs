using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_31_1_Lesnikov_nnd13092.Neuronet
{
    class OutputLayer : Layer
    {
        public OutputLayer(int size, int prevSize, string layerName) 
            : base(size, prevSize, NeuronType.OUTPUT, layerName )
        {
        }
    }
}
