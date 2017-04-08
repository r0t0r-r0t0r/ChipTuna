using System;

namespace ChipTuna.IO
{
    public static class SequentialReaderUtils
    {
        public static void SkipToAbsoluteOffset(this ISequentialReader reader, uint offset)
        {
            if (offset < reader.BytesRead)
                throw new ArgumentOutOfRangeException(nameof(offset));

            reader.Skip(offset - reader.BytesRead);
        }
    }
}
