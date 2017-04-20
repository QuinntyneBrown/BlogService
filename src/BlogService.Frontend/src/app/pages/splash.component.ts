import { ApiService } from "../shared";
import { getCurrentPositionAsync } from "../utilities";

const template = require("./splash.component.html");
const styles = require("./splash.component.scss");

export class SplashComponent extends HTMLElement {
    constructor(
        private _apiService: ApiService = ApiService.Instance
    ) {
        super();

    }
    
    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
        this._setEventListeners();
    }

    private async _bind() {

    }


    private _setEventListeners() {

    }

    disconnectedCallback() {

    }

}

customElements.define(`ce-splash`,SplashComponent);