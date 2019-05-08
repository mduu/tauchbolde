using System;
using System.IO;
using JetBrains.Annotations;
using Tauchbolde.Commom.Misc;

namespace Tauchbolde.Common.DomainServices.PhotoStorage.Stores.FileSystemStore
{    
    public class FilePathCalculator : IFilePathCalculator
    {
        private readonly IMimeMapping mimeMapping;

        public FilePathCalculator(
            [NotNull] IMimeMapping mimeMapping)
        {
            this.mimeMapping = mimeMapping ?? throw new ArgumentNullException(nameof(mimeMapping));
        }
        
        public string CalculateUniqueFilePath(
            string rootPath, 
            PhotoCategory category, 
            string baseFileName, 
            string contentType,
            ThumbnailType thumbnailType = ThumbnailType.None)
        {
            string result;
            var counter = 0;
            
            do
            {
                result = CalculatePath(
                    rootPath,
                    category,
                    baseFileName,
                    contentType,
                    counter,
                    thumbnailType);
                
                counter++;
    
            } while (File.Exists(result));

            return result;
        }
        
        public string CalculatePath(
            string rootPath, 
            FilePhotoIdentifierInfos photoIdentifierInfos)
        {
            if (rootPath == null) throw new ArgumentNullException(nameof(rootPath));
            if (photoIdentifierInfos == null) throw new ArgumentNullException(nameof(photoIdentifierInfos));

            return CombinePath(
                rootPath,
                photoIdentifierInfos.Category,
                photoIdentifierInfos.ThumbnailType, 
                photoIdentifierInfos.Filename);
        }


        
        internal string CalculatePath(
            string rootPath, 
            PhotoCategory category, 
            string baseFileName, 
            string contentType,
            int count,
            ThumbnailType thumbnailType = ThumbnailType.None)
        {
            if (rootPath == null) throw new ArgumentNullException(nameof(rootPath));
            
            var fileName = CalculateFileName(baseFileName, contentType, count);

            return CombinePath(rootPath, category, thumbnailType, fileName);
        }

        private static string CombinePath(string rootPath, PhotoCategory category, ThumbnailType thumbnailType, string fileName)
        {
            return Path.Combine(
                rootPath,
                category.ToString().ToLower(),
                thumbnailType == ThumbnailType.None ? "" : "thumbs",
                fileName);
        }

        private string CalculateFileName(string baseFileName, string contentType, int count)
        {
            var fileName = !string.IsNullOrWhiteSpace(baseFileName) ? baseFileName : "picture";

            if (count > 0)
            {
                fileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{count}{Path.GetExtension(fileName)}";
            }

            if (string.IsNullOrWhiteSpace(Path.GetExtension(fileName)) && !string.IsNullOrWhiteSpace(contentType))
            {
                var mimeFileExt = mimeMapping.GetFileExtensionMapping(contentType);
                fileName = Path.ChangeExtension(fileName, mimeFileExt);
            }

            return fileName;
        }
    }
}