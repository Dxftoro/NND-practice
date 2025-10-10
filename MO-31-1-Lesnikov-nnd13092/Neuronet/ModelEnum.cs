namespace MO_31_1_Lesnikov_nnd13092.Neuronet
{
    enum MemoryMode
    {
        GET,
        SET,
        INIT
    }

    enum NeuronType
    {
        INPUT,
        HIDDEN,
        OUTPUT
    }

    enum NetworkMode
    {
        TRAIN,      // learning mode
        TEST,       // testing mode
        DEMO        // recognising mode
    }
}
