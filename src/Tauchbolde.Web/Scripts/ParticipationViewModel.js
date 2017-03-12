var Tauchbolde;
(function (Tauchbolde) {
    var Events;
    (function (Events) {
        var ParticipationViewModel = (function () {
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
                        eventid: _this.formElement.find("#evnetid").val(),
                        status: _this.formElement.find("#status").val(),
                        note: _this.formElement.find("#note").val(),
                        buddyteamname: _this.formElement.find("#buddyTeam"),
                        countpeople: _this.formElement.find("#CountPeople")
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
//# sourceMappingURL=ParticipationViewModel.js.map