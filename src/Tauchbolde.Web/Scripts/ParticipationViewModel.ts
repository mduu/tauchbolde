namespace Tauchbolde.Events {

    export class ParticipationViewModel {
        existingParticipantId: string;
        eventId: string;
        countPeople: number;
        note: string;
        status: number;
        buddyTeamName: string;

        constructor(private formElement: JQuery, private changeUrl: string) {
            this.changeUrl = undefined;
            this.formElement = undefined;
            if (!formElement) throw "'formElement' is required!";
            if (!changeUrl) throw "'changeUrl' is requried! ";

            this.formElement = formElement;
            this.changeUrl = changeUrl;

            this.formElement.submit((e) => {
                e.preventDefault();

                const participantData = {
                    eventid: this.formElement.find("#eventId").val(),
                    status: this.formElement.find("#status").val(),
                    note: this.formElement.find("#note").val(),
                    buddyteamname: this.formElement.find("#buddyTeam"),
                    countpeople: this.formElement.find("#CountPeople")
                };

                $.post({
                    url: changeUrl,
                    data: participantData
                });

                console.debug(formElement.serialize());
                alert("Submit");
            });
        }
    }
}