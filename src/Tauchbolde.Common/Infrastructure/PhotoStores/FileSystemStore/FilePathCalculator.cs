using System;
using System.IO;
using JetBrains.Annotations;
using Tauchbolde.Commom.Misc;
using Tauchbolde.Common.Domain.PhotoStorage;
using Tauchbolde.Entities;

namespace Tauchbolde.Common.Infrastructure.PhotoStores.FileSystemStore
{    
    public class FilePathCalculator : IFilePathCalculator
    {
        private const string ThumbSubPath = "thumbs";
        private readonly IMimeMapping mimeMapping;

        public FilePathCalculator(
            [NotNull] IMimeMapping mimeMapping)
        {
            this.mimeMapping = mimeMapping ?? throw new ArgumentNullException(nameof(mimeMapping));
        }
        
        public string CalculateUniqueFilePath(string rootPath,
            PhotoCategory category,
            string baseFileName,
            string contentType,
            bool isThumb)
        {
            string result;
            var counter = 0;
            
            do
            {
                result = CalculatePath(rootPath, category, baseFileName, contentType, counter, isThumb);
                counter++;
    
            } while (File.Exists(result));

            return result;
        }
        
        public string CalculatePath(
            string rootPath, 
            PhotoIdentifier photoIdentifier)
        {
            if (rootPath == null) throw new ArgumentNullException(nameof(rootPath));
            if (photoIdentifier == null) throw new ArgumentNullException(nameof(photoIdentifier));

            return CombinePath(
                rootPath,
                photoIdentifier.Category,
                photoIdentifier.IsThumb, 
                photoIdentifier.Filename);
        }


        
        internal string CalculatePath(
            string rootPath, 
            PhotoCategory category, 
            string baseFileName, 
            string contentType,
            int count,
            bool isThumb)
        {
            if (rootPath == null) throw new ArgumentNullException(nameof(rootPath));
            
            var fileName = CalculateFileName(baseFileName, contentType, count);

            return CombinePath(rootPath, category, isThumb, fileName);
        }

        private static string CombinePath(
            string rootPath,
            PhotoCategory category,
            bool isThumb,
            string fileName)
        {
            return Path.Combine(
                rootPath,
                category.ToString().ToLower(),
                isThumb ? ThumbSubPath : "",
                fileName);
        }

        private string CalculateFileName(string baseFileName, string contentType, int count)
        {
            var fileName = !string.IsNullOrWhiteSpace(baseFileName) ? baseFileName : "picture";

            if (count > 0)
            {
                fileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{count}{Path.GetExtension(fileName)}";
            }

            if (string.IsNullOrWhiteSpace(Path.GetExtension(fileName)) &&
                !string.IsNullOrWhiteSpace(contentType))
            {
                var mimeFileExt = mimeMapping.GetFileExtensionMapping(contentType);
                fileName = Path.ChangeExtension(fileName, mimeFileExt);
            }

            return fileName;
        }
    }
}