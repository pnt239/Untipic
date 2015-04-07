namespace Untipic.Business
{
    internal class Data
    {
        public Data()
        {
        }

        public Data(byte[] buffer, int numByte)
        {
            Reserver = buffer;
            NumBytes = numByte;
        }

        public byte[] Reserver { get; set; }

        public int NumBytes { get; set; }
    }
}
