var Tauchbolde;
(function (Tauchbolde) {
    var Events;
    (function (Events) {
        var ParticipationViewModel = /** @class */ (function () {
            function ParticipationViewModel(formElement, changeUrl) {
                var _this = this;
                this.formElement = formElement;
                this.changeUrl = changeUrl;
                this.changeUrl = undefined;
                this.formElement = undefined;
                if (!formElement)
                    throw "'formElement' is required!";
                if (!changeUrl)
                    throw "'changeUrl' is requried! ";
                this.formElement = formElement;
                this.changeUrl = changeUrl;
                this.formElement.submit(function (e) {
                    e.preventDefault();
                    var participantData = {
                        eventid: _this.formElement.find("#eventId").val(),
                        status: _this.formElement.find("#Participations_CurrentUserStatus").val(),
                        note: _this.formElement.find("#Participations_CurrentUserNote").val(),
                        buddyteamname: _this.formElement.find("#Participations_CurrentUserBuddyTeamName"),
                        countpeople: _this.formElement.find("#Participations_CurrentUserCountPeople")
                    };
                    $.post({
                        url: changeUrl,
                        data: participantData
                    });
                    console.debug(formElement.serialize());
                    alert("Submit");
                });
            }
            return ParticipationViewModel;
        }());
        Events.ParticipationViewModel = ParticipationViewModel;
    })(Events = Tauchbolde.Events || (Tauchbolde.Events = {}));
})(Tauchbolde || (Tauchbolde = {}));
