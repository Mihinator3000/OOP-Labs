using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using BackupsExtra.Entities;
using BackupsExtra.Services;
using BackupsExtra.Tools;

namespace BackupsExtra.Algorithms.ConfigAlgorithms
{
    public class ConfigHandler
    {
        private readonly string _configPath;

        public ConfigHandler()
            : this(".config")
        {
        }

        public ConfigHandler(string path)
        {
            _configPath = path;
        }

        public void Save(BackupService backupService)
        {
            using var writer = XmlWriter.Create(
                _configPath,
                new XmlWriterSettings { Indent = true });

            new DataContractSerializer(
                    typeof(BackupService),
                    new DataContractSerializerSettings { KnownTypes = new List<Type> { typeof(RestorePoint) } })
                .WriteObject(writer, backupService);
        }

        public BackupService Load()
        {
            if (!File.Exists(_configPath))
                throw new BackupsExtraException("Config file does not exist");

            var fileStream = new FileStream(_configPath, FileMode.Open);
            var reader = XmlDictionaryReader
                .CreateTextReader(
                    fileStream,
                    new XmlDictionaryReaderQuotas());

            var serializer = new DataContractSerializer(
                typeof(BackupService),
                new DataContractSerializerSettings { KnownTypes = new List<Type> { typeof(RestorePoint) } });

            var backupService = (BackupService)serializer.ReadObject(reader, true);

            reader.Close();
            fileStream.Close();

            return backupService;
        }
    }
}