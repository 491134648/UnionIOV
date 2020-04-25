using System;
using System.Buffers;
using System.Text;
using System.Buffers.Text;

namespace SuperSocket.ProtoBase
{
    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// get string
        /// </summary>
        /// <param name="buffer">buffer</param>
        /// <param name="encoding">if encoding is null,use Encoding.ASCII</param>
        /// <returns></returns>
        public static string GetString(this ReadOnlySequence<byte> buffer, Encoding encoding=null)
        {
            if (encoding == null)
            {
                encoding = Encoding.ASCII;
            }
            if (buffer.IsSingleSegment)
            {
                return encoding.GetString(buffer.First.Span);
            }

            if (encoding.IsSingleByte)
            {
                return string.Create((int)buffer.Length, buffer, (span, sequence) =>
                {
                    foreach (var segment in sequence)
                    {
                        var count = encoding.GetChars(segment.Span, span);
                        span = span.Slice(count);
                    }
                });
            }

            var sb = new StringBuilder();
            var decoder = encoding.GetDecoder();

            foreach (var piece in buffer)
            {                
                var charBuff = (new char[piece.Length]).AsSpan();
                var len = decoder.GetChars(piece.Span, charBuff, false);                
                sb.Append(new string(len == charBuff.Length ? charBuff : charBuff.Slice(0, len)));
            }

            return sb.ToString();
        }
        /// <summary>
        /// write text 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="text"></param>
        /// <param name="encoding">if encoding is null,use Encoding.ASCII</param>
        /// <returns>total bytes</returns>
        public static int Write(this IBufferWriter<byte> writer, ReadOnlySpan<char> text, Encoding encoding=null)
        {
            if (encoding == null)
            {
                encoding = Encoding.ASCII;
            }
            var encoder = encoding.GetEncoder();
            var completed = false;
            var totalBytes = 0;

            var minSpanSizeHint = encoding.GetMaxByteCount(1);

            while (!completed)
            {
                var span = writer.GetSpan(minSpanSizeHint);

                encoder.Convert(text, span, false, out int charsUsed, out int bytesUsed, out completed);
                
                if (charsUsed > 0)
                    text = text.Slice(charsUsed);

                totalBytes += bytesUsed;
                writer.Advance(bytesUsed);
            }

            return totalBytes;
        }
    }
}