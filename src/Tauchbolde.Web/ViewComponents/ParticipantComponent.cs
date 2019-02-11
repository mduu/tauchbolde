using System;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Common.Model;
using Tauchbolde.Web.Models.ViewComponentModels;

namespace Tauchbolde.Web.ViewComponents
{
    public class ParticipantComponent: ViewComponent
    {
        public IViewComponentResult Invoke(Participant participant)
        {
            return View(new ParticipantViewModel
            {
                Diver = participant.ParticipatingDiver,
                Name = participant.ParticipatingDiver.Firstname,
                CountPeople = participant.CountPeople,
                Notes = participant.Note,
                Status = participant.Status,
            });
        }
    }
}
