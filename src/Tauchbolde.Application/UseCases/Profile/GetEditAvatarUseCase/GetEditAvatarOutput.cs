namespace Tauchbolde.Application.UseCases.Profile.GetEditAvatarUseCase
{
    public class GetEditAvatarOutput
    {
        public GetEditAvatarOutput(Guid userId, string realname)
        {
            UserId = userId;
            Realname = realname;
        }

        public Guid UserId { get; }
        public string Realname { get; }
    }
}