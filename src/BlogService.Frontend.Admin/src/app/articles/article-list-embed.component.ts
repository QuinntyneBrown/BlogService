import { Article } from "./article.model";

const template = require("./article-list-embed.component.html");
const styles = require("./article-list-embed.component.scss");

export class ArticleListEmbedComponent extends HTMLElement {
    constructor(
        private _document: Document = document
    ) {
        super();
    }


    static get observedAttributes() {
        return [
            "articles"
        ];
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
    }

    private async _bind() {        
        for (let i = 0; i < this.articles.length; i++) {
            let el = this._document.createElement(`ce-article-item-embed`);
            el.setAttribute("entity", JSON.stringify(this.articles[i]));
            this.appendChild(el);
        }    
    }

    articles:Array<Article> = [];

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "articles":
                this.articles = JSON.parse(newValue);
                if (this.parentElement)
                    this.connectedCallback();
                break;
        }
    }
}

customElements.define("ce-article-list-embed", ArticleListEmbedComponent);
