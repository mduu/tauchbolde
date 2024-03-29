using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.NewUseCase
{
    public class NewLogbookEntry : IRequest<UseCaseResult>
    {
        public NewLogbookEntry(
            [NotNull] string title,
            [NotNull] string teaser,
            [NotNull] string text,
            bool isFavorite,
            [CanBeNull] Stream teaserImage,
            [CanBeNull] string teaserImageFileName,
            [CanBeNull] string teaserImageContentType,
            [CanBeNull] string externalPhotoAlbumUrl,
            Guid? relatedEventId = null)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Teaser = teaser ?? throw new ArgumentNullException(nameof(teaser));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            IsFavorite = isFavorite;
            TeaserImage = teaserImage;
            TeaserImageFileName = teaserImageFileName;
            TeaserImageContentType = teaserImageContentType;
            ExternalPhotoAlbumUrl = externalPhotoAlbumUrl;
            RelatedEventId = relatedEventId;
        }

        [NotNull] public string Title { get; }
        [NotNull] public string Teaser { get; }
        [NotNull] public string Text { get; }
        public bool IsFavorite { get; }
        [CanBeNull] public Stream TeaserImage { get; }
        [CanBeNull] public string TeaserImageFileName { get; }
        [CanBeNull] public string TeaserImageContentType { get; }
        [CanBeNull] public string ExternalPhotoAlbumUrl { get; }
        public Guid? RelatedEventId { get; }
    }
}