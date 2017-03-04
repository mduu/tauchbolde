var Tauchbolde;
(function (Tauchbolde) {
    var Events;
    (function (Events) {
        var ParticipationManager = (function () {
            function ParticipationManager(formElement) {
                if (!formElement)
                    throw "'formElement' is required!";
                this.formElement = formElement;
                this.formElement.submit(function (e) {
                    e.preventDefault();
                    alert("Submit");
                });
            }
            return ParticipationManager;
        }());
        Events.ParticipationManager = ParticipationManager;
    })(Events = Tauchbolde.Events || (Tauchbolde.Events = {}));
})(Tauchbolde || (Tauchbolde = {}));
//# sourceMappingURL=ParticipationManager.js.map