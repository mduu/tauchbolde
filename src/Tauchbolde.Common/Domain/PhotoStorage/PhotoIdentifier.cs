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
        private const string ThumbSubdir = "thumbs";
        private static readonly Regex IdentifierRegex =
            new Regex(@"^(?<category>.*?)(-(?<thumbnail>.*)|)-(?<filename>.*)$", RegexOptions.Compiled);

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

        public string Serialze() =>
            $"{Category.ToString()}{(IsThumb ? $"-{ThumbSubdir}" : "")}-{Filename}";

        public override string ToString() => Serialze();

        private void Deserialize(string serializedIdentifier)
        {
            var match = IdentifierRegex.Match(serializedIdentifier);

            if (!match.Success)
            {
                throw new InvalidOperationException($"Invalid PhotoIdentifier: [{serializedIdentifier}]");
            }

            var categoryName = match.Groups["category"]?.Value ?? "";
            if (!Enum.TryParse(categoryName, out PhotoCategory category))
            {
                throw new InvalidOperationException($"Invalid category: [{categoryName}]");
            }

            var thumbIndicator = match.Groups["thumbnail"]?.Value ?? "";
            if (!string.IsNullOrEmpty(thumbIndicator) && thumbIndicator != ThumbSubdir)
            {
                throw new InvalidOperationException($"Invalid thumbnail indicator: [{thumbIndicator}]");
            }

            var isThumb = thumbIndicator == ThumbSubdir;

            Category = category;
            IsThumb = isThumb;
            Filename = match.Groups["filename"]?.Value ??
                       throw new InvalidOperationException($"Filename must not be null in {nameof(PhotoIdentifier)}!");
        }
    }
}