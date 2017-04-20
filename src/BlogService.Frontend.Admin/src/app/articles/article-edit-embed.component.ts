import { Article } from "./article.model";
import { EditorComponent } from "../shared";
import {  ArticleDelete, ArticleEdit, ArticleAdd } from "./article.actions";

const template = require("./article-edit-embed.component.html");
const styles = require("./article-edit-embed.component.scss");

export class ArticleEditEmbedComponent extends HTMLElement {
    constructor() {
        super();
        this.onSave = this.onSave.bind(this);
        this.onDelete = this.onDelete.bind(this);
        this.onCreate = this.onCreate.bind(this);
    }

    static get observedAttributes() {
        return [
            "article",
            "article-id"
        ];
    }
    
    connectedCallback() {        
        this.innerHTML = `<style>${styles}</style> ${template}`; 
        this._bind();
        this._setEventListeners();
    }
    
    private async _bind() {
        this._titleElement.textContent = this.article ? "Edit Article": "Create Article";

        if (this.article) {                
            this._nameInputElement.value = this.article.name;  
        } else {
            this._deleteButtonElement.style.display = "none";
        }     
    }

    private _setEventListeners() {
        this._saveButtonElement.addEventListener("click", this.onSave);
        this._deleteButtonElement.addEventListener("click", this.onDelete);
        this._createButtonElement.addEventListener("click", this.onCreate);
    }

    private disconnectedCallback() {
        this._saveButtonElement.removeEventListener("click", this.onSave);
        this._deleteButtonElement.removeEventListener("click", this.onDelete);
        this._createButtonElement.removeEventListener("click", this.onCreate);
    }

    public onSave() {
        const article = {
            id: this.article != null ? this.article.id : null,
            name: this._nameInputElement.value
        } as Article;
        
        this.dispatchEvent(new ArticleAdd(article));            
    }

    public onCreate() {        
        this.dispatchEvent(new ArticleEdit(new Article()));            
    }

    public onDelete() {        
        const article = {
            id: this.article != null ? this.article.id : null,
            name: this._nameInputElement.value
        } as Article;

        this.dispatchEvent(new ArticleDelete(article));         
    }

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "article-id":
                this.articleId = newValue;
                break;
            case "article":
                this.article = JSON.parse(newValue);
                if (this.parentNode) {
                    this.articleId = this.article.id;
                    this._nameInputElement.value = this.article.name != undefined ? this.article.name : "";
                    this._titleElement.textContent = this.articleId ? "Edit Article" : "Create Article";
                }
                break;
        }           
    }

    public articleId: any;
    
	public article: Article;
    
    private get _createButtonElement(): HTMLElement { return this.querySelector(".article-create") as HTMLElement; }
    
	private get _titleElement(): HTMLElement { return this.querySelector("h2") as HTMLElement; }
    
	private get _saveButtonElement(): HTMLElement { return this.querySelector(".save-button") as HTMLElement };
    
	private get _deleteButtonElement(): HTMLElement { return this.querySelector(".delete-button") as HTMLElement };
    
	private get _nameInputElement(): HTMLInputElement { return this.querySelector(".article-name") as HTMLInputElement;}
}

customElements.define(`ce-article-edit-embed`,ArticleEditEmbedComponent);
