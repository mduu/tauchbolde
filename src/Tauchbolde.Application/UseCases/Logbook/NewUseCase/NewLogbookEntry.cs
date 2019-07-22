using System;
using System.IO;
using JetBrains.Annotations;
using MediatR;
using Tauchbolde.SharedKernel;

namespace Tauchbolde.Application.UseCases.Logbook.NewUseCase
{
    public class NewLogbookEntry : IRequest<UseCaseResult>
    {
        public NewLogbookEntry(
            Guid authorDiverId,
            [NotNull] string title,
            [NotNull] string text,
            [CanBeNull] string teaser,
            bool isFavorite,
            [CanBeNull] Stream teaserImage,
            [CanBeNull] string teaserImageFileName,
            [CanBeNull] string teaserImageContentType,
            [CanBeNull] string externalPhotoAlbumUrl)
        {
            IsFavorite = isFavorite;
            TeaserImage = teaserImage;
            TeaserImageFileName = teaserImageFileName;
            TeaserImageContentType = teaserImageContentType;
            Teaser = teaser;
            ExternalPhotoAlbumUrl = externalPhotoAlbumUrl;
            AuthorDiverId = authorDiverId;
            Title = title;
            Text = text;
        }

        [NotNull] public string Title { get; }
        [NotNull] public string Text { get; }
        public Guid AuthorDiverId { get; }
        public bool IsFavorite { get; }
        [CanBeNull] public Stream TeaserImage { get; }
        [CanBeNull] public string TeaserImageFileName { get; }
        [CanBeNull] public string TeaserImageContentType { get; }
        [CanBeNull] public string Teaser { get; }
        [CanBeNull] public string ExternalPhotoAlbumUrl { get; }
    }
}