using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Tauchbolde.Common.Domain.PhotoStorage
{
    /// <summary>
    /// Represents a unique photo identifier.
    /// </summary>
    public class PhotoIdentifier
    {
        private static readonly Regex ParseRegex =
            new Regex(@"^(?<category>.*)-(?<thumbnail>.*)-(?<filename>.*)$", RegexOptions.Compiled);

        public PhotoIdentifier(PhotoCategory category, bool isThumb, [NotNull] string filename)
        {
            Category = category;
            IsThumb = isThumb;
            Filename = filename ?? throw new ArgumentNullException(nameof(filename));
        }

        public PhotoIdentifier([NotNull] string serializedIdentifier)
        {
            if (string.IsNullOrWhiteSpace(serializedIdentifier))
                throw new ArgumentNullException(nameof(serializedIdentifier));

            Deserialize(serializedIdentifier);
        }

        public PhotoCategory Category { get; private set; }
        public bool IsThumb { get; private set; }
        [NotNull] public string Filename { get; private set; }

        private void Deserialize(string serializedIdentifier)
        {
            var match = ParseRegex.Match(serializedIdentifier);

            if (!match.Success)
            {
                throw new InvalidOperationException($"Invalid PhotoIdentifier: [${serializedIdentifier}]");
            }

            var categoryName = match.Groups["category"]?.Value ?? "";
            if (!Enum.TryParse(categoryName, out PhotoCategory category))
            {
                throw new InvalidOperationException($"Invalid category: [${categoryName}]");
            }

            var isThumbValue = match.Groups["thumbnail"]?.Value ?? "0";
            if (!bool.TryParse(isThumbValue, out var isThumb))
            {
                throw new InvalidOperationException($"Invalid thumbnail indicator: [${isThumbValue}]");
            }

            Category = category;
            IsThumb = isThumb;
            Filename = match.Groups["filename"]?.Value ??
                       throw new InvalidOperationException($"Filename must not be null in {nameof(PhotoIdentifier)}!");
        }

        public string Serialze() =>
            $"{Category.ToString()}-{(IsThumb ? "thumb" : "")}-{Filename}";
    }
}