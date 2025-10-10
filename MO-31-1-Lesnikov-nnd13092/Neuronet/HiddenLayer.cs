using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_31_1_Lesnikov_nnd13092.Neuronet
{
    class HiddenLayer : Layer
    {
        public HiddenLayer(int size, int prevSize, string layerName)
            : base(size, prevSize, NeuronType.HIDDEN, layerName)
        {

        }
    }
}
