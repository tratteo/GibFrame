using System;
using System.Collections.Generic;

namespace GibFrame.SaveSystem
{
    [System.Serializable]
    internal class ByteBuffer
    {
        private readonly Queue<byte> buffer;

        public ByteBuffer()
        {
            buffer = new Queue<byte>();
        }

        public static implicit operator byte[](ByteBuffer buf) => buf.buffer.ToArray();

        public void PutUInt(int value)
        {
            int size = sizeof(int);
            for (int i = 0; i < size; i++)
            {
                buffer.Enqueue(Convert.ToByte((byte)(value >> (sizeof(byte) * 8 * i))));
            }
        }

        public int ReadUInt()
        {
            int res = 0x0;
            int size = sizeof(int);
            for (int i = 0; i < size; i++)
            {
                res |= buffer.Dequeue() << (sizeof(byte) * 8 * i);
            }

            return res;
        }

        public void PutString(string value)
        {
            int len = value.Length;
            PutUInt(len);
            for (int i = 0; i < len; i++)
            {
                buffer.Enqueue(Convert.ToByte(value[i]));
            }
        }

        public string ReadString()
        {
            string res = "";
            int len = ReadUInt();
            for (int i = 0; i < len; i++)
            {
                res += (char)buffer.Dequeue();
            }
            return res;
        }

        public void PutBool(bool value)
        {
            buffer.Enqueue(Convert.ToByte(value));
        }

        public bool ReadBool()
        {
            return Convert.ToBoolean(buffer.Dequeue());
        }

        public void PutFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            PutUInt(bytes.Length);
            foreach (byte b in bytes)
            {
                buffer.Enqueue(b);
            }
        }

        public float ReadFloat()
        {
            int len = ReadUInt();
            byte[] floatBuf = new byte[len];
            for (int i = 0; i < len; i++)
            {
                floatBuf[i] = buffer.Dequeue();
            }
            return (float)BitConverter.ToSingle(floatBuf, 0);
        }
    }
}
