﻿using OSharp.Beatmap.Configurable;
using OSharp.Beatmap.Sections;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OSharp.Beatmap
{
    public class OsuFile : Config
    {
        public int Version { get; set; }
        public General General { get; set; }
        public Editor Editor { get; set; }
        public Metadata Metadata { get; set; }
        public Difficulty Difficulty { get; set; }
        public Events Events { get; set; }
        public TimingPoints TimingPoints { get; set; }
        public Colours Colours { get; set; }
        public HitObjects HitObjects { get; set; }

        public static OsuFile ReadFromFile(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                return ConfigConvert.DeserializeObject<OsuFile>(sr);
            }
        }

        public override string ToString() => Path;

        //todo: not optimized
        public void WriteOsuFile(string path)
        {
            File.WriteAllText(path,
                string.Format("osu file format v{0}\r\n\r\n{1}{2}{3}{4}{5}{6}{7}{8}", Version,
                    General?.ToSerializedString(),
                    Editor?.ToSerializedString(),
                    Metadata?.ToSerializedString(),
                    Difficulty?.ToSerializedString(),
                    Events?.ToSerializedString(),
                    TimingPoints?.ToSerializedString(),
                    Colours?.ToSerializedString(),
                    HitObjects?.ToSerializedString()));
        }

        internal override void HandleCustom(string line)
        {
            const string verFlag = "osu file format v";

            if (line.StartsWith(verFlag))
            {
                var str = line.Replace(verFlag, "");
                if (!int.TryParse(str, out var verNum))
                    throw new BadOsuFormatException("未知的osu版本: " + str);
                if (verNum < 7)
                    throw new VersionNotSupportedException(verNum);
                Version = verNum;
            }
            else
            {
                throw new BadOsuFormatException("存在问题头声明: " + line);
            }
        }

        private OsuFile() { }

        private string Path => Common.IO.File.EscapeFileName(string.Format("{0} - {1} ({2}){3}.osu",
            Metadata.Artist,
            Metadata.Title,
            Metadata.Creator,
            Metadata.Version != "" ? $" [{Metadata.Version}]" : ""));

    }
}