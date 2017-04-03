using System;

namespace ChipTuna
{

    [Serializable]
    public class VgmException : Exception
    {
        public VgmException() { }
        public VgmException(string message) : base(message) { }
        public VgmException(string message, Exception inner) : base(message, inner) { }
        protected VgmException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
