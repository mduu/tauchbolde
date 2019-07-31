using Tauchbolde.Domain.Entities;
using Tauchbolde.Driver.DataAccessSql.DataEntities;

namespace Tauchbolde.Driver.DataAccessSql.Mappers
{
    internal static class CommentMapper
    {
        public static Comment MapTo(this CommentData dataEntity) =>
            dataEntity == null
                ? null
                : new Comment
                {
                    Id = dataEntity.Id,
                    Text = dataEntity.Text,
                    CreateDate = dataEntity.CreateDate,
                    ModifiedDate = dataEntity.ModifiedDate,
                    EventId = dataEntity.EventId,
                    AuthorId = dataEntity.AuthorId,
                    Author = dataEntity.Author.MapTo(),
                };

        public static CommentData MapTo(this Comment domainEntity) =>
            domainEntity == null
                ? null
                : new CommentData
                {
                    Id = domainEntity.Id,
                    Text = domainEntity.Text,
                    CreateDate = domainEntity.CreateDate,
                    ModifiedDate = domainEntity.ModifiedDate,
                    EventId = domainEntity.EventId,
                    AuthorId = domainEntity.AuthorId,
                    Author = domainEntity.Author.MapTo(),
                };
    }
}