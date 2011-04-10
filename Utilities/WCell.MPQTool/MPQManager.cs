using System;
using System.Collections.Generic;
using System.IO;
using WCell.MPQTool.StormLibWrapper;

//using MpqReader;


namespace WCell.MPQTool
{
    public class MpqManager
    {
        private readonly List<MpqArchive> MPQArchives;

        public MpqManager(string mpqPath)
        {
            MPQArchives = new List<MpqArchive>();

            var mpqNames = new List<string>
            {
                "Data\\patch-3.MPQ"
                ,"Data\\patch-2.MPQ"
                ,"Data\\patch.MPQ"
                ,"Data\\lichking.MPQ"
                ,"Data\\expansion.MPQ"
                ,"Data\\common-2.MPQ"
                ,"Data\\common.MPQ"
            };

            var dataDirectory = Path.Combine(mpqPath, "Data\\");
            var localeDirectorys = Directory.EnumerateDirectories(dataDirectory);

            foreach (var localeDir in localeDirectorys)
            {
                var locale = localeDir.Substring(localeDir.Length - 4, 4);
                switch(locale)
                {
                        //TODO: Add all locales here
                    case "enUS":
                    case "enGB":
                        break;
                    default:
                        continue;
                }
                mpqNames.Add(Path.Combine("Data\\", locale, "patch-" + locale + "-3.MPQ"));
                mpqNames.Add(Path.Combine("Data\\", locale, "patch-" + locale + "-2.MPQ"));
                mpqNames.Add(Path.Combine("Data\\", locale, "patch-" + locale + ".MPQ"));
                mpqNames.Add(Path.Combine("Data\\", locale, "lichking-locale-" + locale + ".MPQ"));
                mpqNames.Add(Path.Combine("Data\\", locale, "expansion-locale-" + locale + ".MPQ"));
                mpqNames.Add(Path.Combine("Data\\", locale, "locale-" + locale + ".MPQ"));
                mpqNames.Add(Path.Combine("Data\\", locale, "base-" + locale + ".MPQ"));
            }


            foreach (var mpqName in mpqNames)
            {
                try
                {
                    var path = Path.Combine(mpqPath, mpqName);
                    if(File.Exists(path))
                        MPQArchives.Add(new MpqArchive(path));
                    else
                        Console.WriteLine("File not found: {0}", path);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception thrown in MpqManager constructor." + e);
                }
            }
        }

        public MpqStream OpenFile(string fileName)
        {
            foreach (var archive in MPQArchives)
            {
                if (archive.FileExists(fileName))
                {
                    return archive.OpenFile(fileName).GetStream();
                }
            }
            throw new Exception(String.Format("Unable to load file {0}", fileName));
        }

        public bool FileExists(string fileName)
        {
            foreach (var archive in MPQArchives)
            {
                if (archive.FileExists(fileName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}