namespace Tauchbolde.Events {
    export class ParticipationManager {
        private formElement: JQuery;

        constructor(formElement: JQuery) {
            if (!formElement) throw "'formElement' is required!";

            this.formElement = formElement;

            this.formElement.submit((e) => {
                e.preventDefault();
                alert("Submit");
            });
        }
    }
}