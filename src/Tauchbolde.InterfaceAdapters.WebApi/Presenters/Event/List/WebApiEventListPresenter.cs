using System;
using System.Linq;
using JetBrains.Annotations;
using Tauchbolde.Application.UseCases.Event.GetEventListUseCase;

namespace Tauchbolde.InterfaceAdapters.WebApi.Presenters.Event.List
{
    public class WebApiEventListPresenter : IEventListOutputPort
    {
        private object jsonObject = new object();
        
        public void Output([NotNull] GetEventListOutput interactorOutput)
        {
            if (interactorOutput == null) throw new ArgumentNullException(nameof(interactorOutput));

            jsonObject = interactorOutput.Rows.Select(r => new
            {
                eventId = r.EventId,
                title = r.Title,
                location = r.Location,
                meetingPoint = r.MeetingPoint,
                startTime = r.StartTime.ToString("O"),
                endTime = r.EndTime?.ToString("O"),
            });
        }

        [NotNull] public object GetJsonObject() => jsonObject;
    }
}