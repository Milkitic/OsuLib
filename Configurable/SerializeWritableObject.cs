﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OSharp.Beatmap.Configurable
{
    public abstract class SerializeWritableObject : ISerializeWritable
    {
        public string ToSerializedString()
        {
            using (var sb = new StringWriter())
            {
                AppendSerializedString(sb);
                return sb.ToString();
            }
        }

        public abstract void AppendSerializedString(TextWriter textWriter);
    }
}
